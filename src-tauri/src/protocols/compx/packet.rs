use crate::core::error::{RedragonError, Result};

/// Calculates the command checksum used by the Compx protocol: (0x55 - sum) & 0xFF
pub fn calculate_command_crc(data: &[u8]) -> u8 {
    let sum: u32 = data.iter().map(|&b| b as u32).sum();
    let b = (sum & 0xFF) as u8;
    0x55u8.wrapping_sub(b)
}

/// Validates whether a full packet (including trailing CRC byte) is mathematically valid.
/// If valid: (0x55 - (sum + crc)) & 0xFF == 0.
pub fn is_packet_valid(full_packet: &[u8]) -> bool {
    if full_packet.is_empty() {
        return false;
    }
    calculate_command_crc(full_packet) == 0
}

/// Validates if an incoming packet matches an expected checksum value
pub fn verify_command_crc(data_without_crc: &[u8], expected_crc: u8) -> bool {
    let calculated = calculate_command_crc(data_without_crc);
    calculated == expected_crc
}

/// Safe packet builder for standard 17-byte output reports (Report ID 8)
#[derive(Debug, Clone, PartialEq, Eq)]
pub struct OutputReport8 {
    pub opcode: u8,
    pub payload: [u8; 14],
}

impl OutputReport8 {
    pub const REPORT_ID: u8 = 8;
    pub const PACKET_LEN: usize = 17;

    pub fn new(opcode: u8, payload: [u8; 14]) -> Self {
        Self { opcode, payload }
    }

    /// Builds the 17-byte buffer with the computed CRC placed at index 16
    pub fn to_bytes(&self) -> [u8; Self::PACKET_LEN] {
        let mut buffer = [0u8; Self::PACKET_LEN];
        buffer[0] = Self::REPORT_ID;
        buffer[1] = self.opcode;
        buffer[2..16].copy_from_slice(&self.payload);
        buffer[16] = 0; // Initialize as 0 for sum calculation

        buffer[16] = calculate_command_crc(&buffer);
        buffer
    }

    /// Creates the CheckPid command (used during device pairing and model verification)
    pub fn check_pid_command() -> [u8; Self::PACKET_LEN] {
        let cmd = Self::new(16, [0u8; 14]); // Opcode 16 (0x10 / ReadCidMid)
        cmd.to_bytes()
    }

    /// Command to set the active hardware profile on the mouse (0..3)
    pub fn set_profile_command(profile_idx: u8) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = profile_idx; // 0=Config1, 1=Config2, 2=Config3, 3=Config4
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::SetCurrentConfig as u8, payload);
        cmd.to_bytes()
    }

    /// Command to query the active hardware profile from the mouse
    pub fn get_profile_command() -> [u8; Self::PACKET_LEN] {
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::GetCurrentConfig as u8, [0u8; 14]);
        cmd.to_bytes()
    }

    /// Command to set the polling rate (Hz)
    pub fn set_polling_rate_command(rate: crate::core::models::PollingRate) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = rate.mask(); // 1=1000Hz, 2=500Hz, 4=250Hz, 8=125Hz, 16=2000Hz, 32=4000Hz
        let cmd = Self::new(crate::protocols::compx::commands::opcodes::CMD_SET_POLLING_RATE, payload);
        cmd.to_bytes()
    }

    /// Command to set a DPI stage sensitivity and indicator LED color
    pub fn set_dpi_stage_command(stage_idx: u8, dpi_val: u16, rgb: [u8; 3]) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = stage_idx; // Stage index (0..7)
        // PixArt PAW3395 DPI steps are mapped in increments of 50 DPI
        let dpi_step = (dpi_val / 50) as u8;
        payload[1] = dpi_step; // X DPI
        payload[2] = dpi_step; // Y DPI
        payload[3] = rgb[0];   // R
        payload[4] = rgb[1];   // G
        payload[5] = rgb[2];   // B
        let cmd = Self::new(crate::protocols::compx::commands::opcodes::CMD_SET_DPI, payload);
        cmd.to_bytes()
    }

    /// Command to reassign a physical mouse button (index 0 to 15)
    pub fn set_button_mapping_command(btn_idx: u8, fun_type: u8, param1: u8, param2: u8) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = btn_idx;
        payload[1] = fun_type; // 0=None, 1=Click, 2=DPI, 3=Media, 4=Fire, 5=Shortcut, 6=Macro, 7=PollingCycle, 10=Sniper
        payload[2] = param1;
        payload[3] = param2;
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::WriteFlashData as u8, payload);
        cmd.to_bytes()
    }

    /// Command to query battery level and status from the mouse
    pub fn query_battery_command() -> [u8; Self::PACKET_LEN] {
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::BatteryLevel as u8, [0u8; 14]);
        cmd.to_bytes()
    }

    /// Command to configure PixArt PAW3395 sensor features
    pub fn set_sensor_features_command(
        lod: u8,
        debounce_ms: u8,
        motion_sync: bool,
        angle_snapping: bool,
        ripple: bool,
    ) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = lod; // 1 = 1mm, 2 = 2mm
        payload[1] = debounce_ms; // 4, 8, 12 ms
        payload[2] = if motion_sync { 1 } else { 0 };
        payload[3] = if angle_snapping { 1 } else { 0 };
        payload[4] = if ripple { 1 } else { 0 };
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::SetCurrentConfig as u8, payload);
        cmd.to_bytes()
    }
}

/// Pairing Feature Report structure (Report ID 6)
#[derive(Debug, Clone, PartialEq, Eq)]
pub struct PairFeatureReport6 {
    pub report_id: u8,
    pub command: u8,
    pub cid: u8,
    pub pid: u8,
}

impl PairFeatureReport6 {
    pub const REPORT_ID: u8 = 6;
    pub const PACKET_LEN: usize = 8;

    pub fn new(cid: u8, pid: u8) -> Self {
        Self {
            report_id: Self::REPORT_ID,
            command: 0xA0, // Decimal 160
            cid,
            pid,
        }
    }

    pub fn to_bytes(&self) -> [u8; Self::PACKET_LEN] {
        [
            self.report_id,
            self.command,
            self.cid,
            self.pid,
            0,
            0,
            0,
            0,
        ]
    }

    pub fn parse_response(data: &[u8]) -> Result<PairResponse> {
        if data.len() < 9 {
            return Err(RedragonError::InvalidPayloadLength {
                expected: 9,
                actual: data.len(),
            });
        }

        if data[1] != 0xA0 {
            return Err(RedragonError::UnexpectedResponse(format!(
                "Invalid response command byte: 0x{:02X}",
                data[1]
            )));
        }

        let state_byte = data[2];
        match state_byte {
            3 | 11 => {
                let vid = data[5];
                let pid = data[6];
                Ok(PairResponse::Success { vid, pid })
            }
            2 => Ok(PairResponse::Failed),
            _ => Ok(PairResponse::InProgress),
        }
    }
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum PairResponse {
    InProgress,
    Success { vid: u8, pid: u8 },
    Failed,
}

/// Radio Frequency test report structure (Report ID 6, commands 163 / 161)
#[derive(Debug, Clone, PartialEq, Eq)]
pub struct RfTestReport6 {
    pub report_id: u8,
    pub command: u8,
    pub mode: u8,
}

impl RfTestReport6 {
    pub const REPORT_ID: u8 = 6;
    pub const PACKET_LEN: usize = 8;

    pub fn new(mode: crate::core::models::RfTestMode) -> Self {
        let (command, mode_val) = match mode {
            crate::core::models::RfTestMode::MtkMode => (161, 0),
            other => (163, other as u8),
        };

        Self {
            report_id: Self::REPORT_ID,
            command,
            mode: mode_val,
        }
    }

    pub fn to_bytes(&self) -> [u8; Self::PACKET_LEN] {
        [
            self.report_id,
            self.command,
            self.mode,
            0,
            0,
            0,
            0,
            0,
        ]
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_calculate_command_crc() {
        let mut buffer = [0u8; 17];
        buffer[0] = 8;
        buffer[1] = 16;
        let crc = calculate_command_crc(&buffer);
        // sum = 8 + 16 = 24 (0x18). 0x55 - 0x18 = 0x3D (61)
        assert_eq!(crc, 0x55 - 24);
        assert_eq!(crc, 61);
    }

    #[test]
    fn test_output_report8_build() {
        let bytes = OutputReport8::check_pid_command();
        assert_eq!(bytes.len(), 17);
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 16);
        assert_eq!(bytes[16], 61);
    }

    #[test]
    fn test_pair_feature_report6_build() {
        let report = PairFeatureReport6::new(0x01, 0x05);
        let bytes = report.to_bytes();
        assert_eq!(bytes, [6, 160, 1, 5, 0, 0, 0, 0]);
    }

    #[test]
    fn test_set_polling_rate_command_valid() {
        let bytes = OutputReport8::set_polling_rate_command(crate::core::models::PollingRate::Hz1000);
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 0x21); // CMD_SET_POLLING_RATE
        assert_eq!(bytes[2], 1);    // 1000Hz mask
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_set_profile_command_valid() {
        let bytes = OutputReport8::set_profile_command(1); // Profile 2 (index 1)
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 15); // SetCurrentConfig (opcode 15)
        assert_eq!(bytes[2], 1);  // Profile index 1
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_get_profile_command_valid() {
        let bytes = OutputReport8::get_profile_command();
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 14); // GetCurrentConfig (opcode 14)
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_set_dpi_stage_command_valid() {
        let bytes = OutputReport8::set_dpi_stage_command(1, 1600, [255, 0, 0]);
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[2], 1);  // stage 1
        assert_eq!(bytes[3], 32); // 1600 / 50 = 32 steps
        assert!(is_packet_valid(&bytes));
    }
}
