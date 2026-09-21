use crate::core::error::{RedragonError, Result};
use hidapi::{HidApi, HidDevice};
use std::ffi::CString;

pub struct HidBackend {
    api: HidApi,
}

impl HidBackend {
    pub fn new() -> Result<Self> {
        let api = HidApi::new()?;
        Ok(Self { api })
    }

    /// Opens a device by its Vendor ID and Product ID
    pub fn open(&self, vid: u16, pid: u16) -> Result<HidDevice> {
        match self.api.open(vid, pid) {
            Ok(dev) => Ok(dev),
            Err(e) => {
                log_device_error(vid, pid, &e);
                Err(RedragonError::DeviceNotFound { vid, pid })
            }
        }
    }

    /// Opens a device by its OS hardware path
    pub fn open_path(&self, path: &str) -> Result<HidDevice> {
        let c_path = CString::new(path).map_err(|_| {
            RedragonError::UnexpectedResponse("Invalid device path string".into())
        })?;
        let dev = self.api.open_path(&c_path)?;
        Ok(dev)
    }
}

fn log_device_error(vid: u16, pid: u16, err: &hidapi::HidError) {
    eprintln!(
        "[HID] Failed to open device VID: 0x{:04X}, PID: 0x{:04X}. Reason: {}",
        vid, pid, err
    );
}
