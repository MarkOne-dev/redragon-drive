/// Vendor ID and Product ID for Texas Instruments / Nordic FastConnect Dongle
pub const NRF_VENDOR_ID: u16 = 0x0451;
pub const NRF_PRODUCT_ID: u16 = 0x1AB4;

/// Generates the device check verification report for FastConnect NRF (Report ID 6, length 21)
pub fn create_nrf_check_device_report(cmd: u8) -> [u8; 21] {
    let mut data = [0u8; 21];
    data[0] = 6;
    data[1] = cmd;
    data
}

/// Parses the device name returned in Feature Report 6 (length 21)
pub fn parse_nrf_device_name(data: &[u8]) -> Option<String> {
    if data.len() < 4 || data[0] != 6 || data[1] != 1 {
        return None;
    }
    let name_len = data[2] as usize;
    if data.len() < 3 + name_len {
        return None;
    }
    String::from_utf8(data[3..3 + name_len].to_vec()).ok()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_nrf_check_report() {
        let report = create_nrf_check_device_report(2);
        assert_eq!(report.len(), 21);
        assert_eq!(report[0], 6);
        assert_eq!(report[1], 2);
    }

    #[test]
    fn test_nrf_parse_name() {
        let mut buffer = [0u8; 21];
        buffer[0] = 6;
        buffer[1] = 1;
        buffer[2] = 5;
        buffer[3..8].copy_from_slice(b"M916P");
        let name = parse_nrf_device_name(&buffer);
        assert_eq!(name, Some("M916P".to_string()));
    }
}
