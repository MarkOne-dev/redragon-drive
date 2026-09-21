use crate::core::error::{RedragonError, Result};
use hidapi::HidDevice;
use std::time::Duration;

/// Safe transport layer for sending and receiving HID packets without saturating the device MCU
pub struct HidTransport<'a> {
    device: &'a HidDevice,
}

impl<'a> HidTransport<'a> {
    pub fn new(device: &'a HidDevice) -> Self {
        Self { device }
    }

    /// Sends a Feature Report safely with validation
    pub fn send_feature_report(&self, buffer: &[u8]) -> Result<()> {
        if buffer.is_empty() {
            return Err(RedragonError::InvalidPayloadLength {
                expected: 1,
                actual: 0,
            });
        }

        self.device.send_feature_report(buffer)?;
        // Short safety guard (10ms) to allow the microcontroller to process the feature command
        std::thread::sleep(Duration::from_millis(10));
        Ok(())
    }

    /// Reads a Feature Report from the device / dongle
    pub fn get_feature_report(&self, report_id: u8, buffer_len: usize) -> Result<Vec<u8>> {
        let mut buffer = vec![0u8; buffer_len];
        buffer[0] = report_id;

        let bytes_read = self.device.get_feature_report(&mut buffer)?;
        if bytes_read == 0 {
            return Err(RedragonError::Timeout);
        }

        buffer.truncate(bytes_read);
        Ok(buffer)
    }

    /// Writes a Report (e.g. Report ID 8) to the Compx MCU.
    /// Compx receiver firmwares require control/configuration commands to be submitted
    /// as HID Feature Reports (HidD_SetFeature / HIDIOCSFEATURE).
    /// If send_feature_report fails, falls back to standard interrupt out write.
    pub fn write_output_report(&self, buffer: &[u8]) -> Result<usize> {
        match self.device.send_feature_report(buffer) {
            Ok(_) => {
                std::thread::sleep(Duration::from_millis(10));
                Ok(buffer.len())
            }
            Err(_) => {
                let written = self.device.write(buffer)?;
                std::thread::sleep(Duration::from_millis(10));
                Ok(written)
            }
        }
    }

    /// Reads an incoming report with configurable timeout in milliseconds
    pub fn read_with_timeout(&self, buffer: &mut [u8], timeout_ms: i32) -> Result<usize> {
        let bytes_read = self.device.read_timeout(buffer, timeout_ms)?;
        Ok(bytes_read)
    }
}
