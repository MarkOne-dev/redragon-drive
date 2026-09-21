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

        // Prioritize official M916-PRO mouse and dongle PIDs, and vendor communication channel (Interface 1)
        devices.sort_by_key(|d| {
            let pid_priority = match d.pid {
                0xF55E => 0, // Wired M916-PRO
                0xF55D => 1, // Official 2.4G M916-PRO Dongle
                0xF55F => 2, // Alternate official dongle
                _ => 10,     // Other Compx devices (keyboards, generic receivers)
            };
            let iface_priority = if d.interface_number == 1 { 0 } else { 1 };
            (pid_priority, iface_priority)
        });

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

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_inspect_devices() {
        if let Ok(mut detector) = DeviceDetector::new() {
            if let Ok(devs) = detector.scan_compx_devices() {
                for d in &devs {
                    println!("FOUND COMPX: VID={:04x} PID={:04x} iface={} path={}", d.vid, d.pid, d.interface_number, d.path);
                }
            }
        }
    }
}

