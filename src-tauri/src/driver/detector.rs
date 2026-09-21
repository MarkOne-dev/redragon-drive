use crate::core::error::Result;
use crate::core::models::DeviceInfo;
use crate::protocols::compx::commands::COMPX_VENDOR_ID;
use hidapi::HidApi;

pub struct DeviceDetector {
    api: HidApi,
}

impl DeviceDetector {
    pub fn new() -> Result<Self> {
        let api = HidApi::new()?;
        Ok(Self { api })
    }

    /// Scans and returns all connected Redragon / Compx devices
    pub fn scan_compx_devices(&mut self) -> Result<Vec<DeviceInfo>> {
        self.api.refresh_devices()?;
        let mut devices = Vec::new();

        for dev in self.api.device_list() {
            if dev.vendor_id() == COMPX_VENDOR_ID {
                let is_wired = dev.product_id() == 0xF55E;
                let is_dongle = !is_wired;
                devices.push(DeviceInfo {
                    vid: dev.vendor_id(),
                    pid: dev.product_id(),
                    path: dev.path().to_string_lossy().to_string(),
                    manufacturer: dev.manufacturer_string().map(|s| s.to_string()),
                    product: dev.product_string().map(|s| s.to_string()),
                    serial_number: dev.serial_number().map(|s| s.to_string()),
                    interface_number: dev.interface_number(),
                    is_wired,
                    is_dongle,
                });
            }
        }

        // Prioritize vendor communication channel (Interface 1) first
        devices.sort_by_key(|d| if d.interface_number == 1 { 0 } else { 1 });

        Ok(devices)
    }

    /// Enumerates all system HID devices (for general diagnostics and fallback)
    pub fn scan_all_hid_devices(&mut self) -> Result<Vec<DeviceInfo>> {
        self.api.refresh_devices()?;
        let mut devices = Vec::new();

        for dev in self.api.device_list() {
            let is_wired = dev.vendor_id() == COMPX_VENDOR_ID && dev.product_id() == 0xF55E;
            let is_dongle = dev.vendor_id() == COMPX_VENDOR_ID && !is_wired;
            devices.push(DeviceInfo {
                vid: dev.vendor_id(),
                pid: dev.product_id(),
                path: dev.path().to_string_lossy().to_string(),
                manufacturer: dev.manufacturer_string().map(|s| s.to_string()),
                product: dev.product_string().map(|s| s.to_string()),
                serial_number: dev.serial_number().map(|s| s.to_string()),
                interface_number: dev.interface_number(),
                is_wired,
                is_dongle,
            });
        }

        Ok(devices)
    }
}

