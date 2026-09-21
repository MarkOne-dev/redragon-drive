use thiserror::Error;

#[derive(Error, Debug)]
pub enum RedragonError {
    #[error("HID device not found (VID: 0x{vid:04X}, PID: 0x{pid:04X})")]
    DeviceNotFound { vid: u16, pid: u16 },

    #[error("HID communication error: {0}")]
    HidError(#[from] hidapi::HidError),

    #[error("Invalid payload length: expected {expected}, received {actual}")]
    InvalidPayloadLength { expected: usize, actual: usize },

    #[error("Invalid checksum: expected 0x{expected:02X}, received 0x{actual:02X}")]
    ChecksumMismatch { expected: u8, actual: u8 },

    #[error("Timeout while communicating with device")]
    Timeout,

    #[error("Unsupported Report ID: 0x{0:02X}")]
    UnsupportedReportId(u8),

    #[error("Unexpected response from device: {0}")]
    UnexpectedResponse(String),
}

pub type Result<T> = std::result::Result<T, RedragonError>;
