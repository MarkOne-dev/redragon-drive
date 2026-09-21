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

    /// Authorizes and unlocks PC driver mode on the Compx mouse MCU / dongle (UsbCommandId::PCDriverStatus = 2)
    pub fn set_driver_status_command(is_active: bool) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[3] = 1; // length 1 byte
        payload[4] = if is_active { 1 } else { 0 };
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::PCDriverStatus as u8, payload);
        cmd.to_bytes()
    }

    /// Command to set the polling rate (Hz)
    pub fn set_polling_rate_command(rate: crate::core::models::PollingRate) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = rate.mask(); // 1=1000Hz, 2=500Hz, 4=250Hz, 8=125Hz, 16=2000Hz, 32=4000Hz
        let cmd = Self::new(crate::protocols::compx::commands::opcodes::CMD_SET_POLLING_RATE, payload);
        cmd.to_bytes()
    }

    /// Universal command to write raw bytes into Compx MCU Flash memory registers
    pub fn write_flash_command(address: u16, data: &[u8]) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = 0x00; // Sub-id / reserved
        payload[1] = (address >> 8) as u8; // Address High
        payload[2] = (address & 0xFF) as u8; // Address Low
        let len = data.len().min(10);
        payload[3] = len as u8; // Byte count (max 10 bytes per packet)
        payload[4..4 + len].copy_from_slice(&data[..len]);
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::WriteFlashData as u8, payload);
        cmd.to_bytes()
    }

    /// Universal command to read raw bytes from Compx MCU Flash memory registers
    pub fn read_flash_command(address: u16, len: u8) -> [u8; Self::PACKET_LEN] {
        let mut payload = [0u8; 14];
        payload[0] = 0x00;
        payload[1] = (address >> 8) as u8;
        payload[2] = (address & 0xFF) as u8;
        payload[3] = len.min(10);
        let cmd = Self::new(crate::protocols::compx::commands::UsbCommandId::ReadFlashData as u8, payload);
        cmd.to_bytes()
    }

    /// Sets the active DPI stage index directly in MCU Flash (Address 0x0004 / currentDPI)
    /// Compx flash protocol requires a checksum byte: [stage_idx, 0x55 - stage_idx]
    pub fn set_active_dpi_stage_command(stage_idx: u8) -> [u8; Self::PACKET_LEN] {
        let chk = 0x55u8.wrapping_sub(stage_idx);
        Self::write_flash_command(0x0004, &[stage_idx, chk])
    }

    /// Configures the DPI resolution for a stage in Compx Flash (Address 0x000C + stage * 4)
    /// Encodes xDPI, yDPI, DPIex, and Compx checksum (0x55 - (x + y + ex)) for PixArt PAW3395 (50-26,000 DPI)
    pub fn set_dpi_stage_command(stage_idx: u8, dpi_x: u16, dpi_y: u16) -> [u8; Self::PACKET_LEN] {
        let x_step = (dpi_x / 50).saturating_sub(1);
        let y_step = (dpi_y / 50).saturating_sub(1);
        let x_hi = (x_step >> 8) as u8;
        let y_hi = (y_step >> 8) as u8;
        let dpiex = (x_hi << 2) | (y_hi << 6);
        let b0 = x_step as u8;
        let b1 = y_step as u8;
        let b2 = dpiex;
        let chk = 0x55u8.wrapping_sub(b0.wrapping_add(b1).wrapping_add(b2));
        let data = [b0, b1, b2, chk];
        Self::write_flash_command(0x000C + (stage_idx as u16) * 4, &data)
    }

    /// Configures the RGB LED indicator color for a DPI stage (Address 0x002C + stage * 4)
    /// Compx flash protocol requires a checksum byte: [R, G, B, 0x55 - (R + G + B)]
    pub fn set_dpi_color_command(stage_idx: u8, rgb: [u8; 3]) -> [u8; Self::PACKET_LEN] {
        let chk = 0x55u8.wrapping_sub(rgb[0].wrapping_add(rgb[1]).wrapping_add(rgb[2]));
        let data = [rgb[0], rgb[1], rgb[2], chk];
        Self::write_flash_command(0x002C + (stage_idx as u16) * 4, &data)
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
    fn test_set_active_dpi_stage_command_valid() {
        let bytes = OutputReport8::set_active_dpi_stage_command(2); // Stage 3 (index 2)
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 7); // WriteFlashData
        assert_eq!(bytes[3], 0); // Addr High
        assert_eq!(bytes[4], 4); // Addr Low: 0x0004 (currentDPI)
        assert_eq!(bytes[5], 2); // Length: 2 bytes
        assert_eq!(bytes[6], 2); // Stage index: 2
        assert_eq!(bytes[7], 0x55 - 2); // Checksum byte: 0x53
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_set_dpi_stage_command_valid() {
        let bytes = OutputReport8::set_dpi_stage_command(1, 1600, 1600); // Stage index 1
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 7);  // WriteFlashData
        assert_eq!(bytes[3], 0);  // Addr High
        assert_eq!(bytes[4], 16); // Addr Low: 0x000C + 1*4 = 0x0010 = 16
        assert_eq!(bytes[5], 4);  // Length: 4 bytes
        assert_eq!(bytes[6], 31); // (1600 / 50) - 1 = 31
        assert_eq!(bytes[7], 31); // yDPI = 31
        assert_eq!(bytes[8], 0);  // DPIex = 0
        assert_eq!(bytes[9], 0x55u8.wrapping_sub(31 + 31 + 0)); // Compx checksum
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_set_dpi_color_command_valid() {
        let bytes = OutputReport8::set_dpi_color_command(0, [255, 128, 0]);
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 7);  // WriteFlashData
        assert_eq!(bytes[3], 0);  // Addr High
        assert_eq!(bytes[4], 44); // Addr Low: 0x002C + 0*4 = 0x002C = 44
        assert_eq!(bytes[5], 4);  // Length: 4 bytes
        assert_eq!(bytes[6], 255);
        assert_eq!(bytes[7], 128);
        assert_eq!(bytes[8], 0);
        assert_eq!(bytes[9], 0x55u8.wrapping_sub(255u8.wrapping_add(128))); // Compx checksum
        assert!(is_packet_valid(&bytes));
    }

    #[test]
    fn test_set_driver_status_command_valid() {
        let bytes = OutputReport8::set_driver_status_command(true);
        assert_eq!(bytes[0], 8);
        assert_eq!(bytes[1], 2); // PCDriverStatus
        assert_eq!(bytes[5], 1); // length
        assert_eq!(bytes[6], 1); // is_active
        assert!(is_packet_valid(&bytes));
    }
}
