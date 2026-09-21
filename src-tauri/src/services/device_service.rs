use crate::core::error::{RedragonError, Result};
use crate::core::models::{DeviceInfo, RfTestMode};
use crate::driver::{DeviceDetector, HidBackend, HidTransport};
use crate::protocols::compx::packet::{OutputReport8, RfTestReport6};
use hidapi::HidDevice;
use std::sync::{Arc, Mutex};

pub struct DeviceService {
    backend: HidBackend,
    active_device: Arc<Mutex<Option<HidDevice>>>,
    active_info: Arc<Mutex<Option<DeviceInfo>>>,
    active_battery: Arc<Mutex<crate::core::models::BatteryInfo>>,
}

impl DeviceService {
    pub fn new() -> Result<Self> {
        let backend = HidBackend::new()?;
        Ok(Self {
            backend,
            active_device: Arc::new(Mutex::new(None)),
            active_info: Arc::new(Mutex::new(None)),
            active_battery: Arc::new(Mutex::new(crate::core::models::BatteryInfo {
                percentage: 95,
                voltage_mv: 4120,
                is_charging: false,
            })),
        })
    }

    /// Gets a shared handle to the currently active device
    pub fn active_device_handle(&self) -> Arc<Mutex<Option<HidDevice>>> {
        Arc::clone(&self.active_device)
    }

    /// Connects to a device by its physical OS path
    pub fn connect_by_path(&self, path: &str) -> Result<DeviceInfo> {
        let dev = self.backend.open_path(path)?;

        let mut detector = DeviceDetector::new()?;
        let all = detector.scan_all_hid_devices()?;
        let info = all
            .into_iter()
            .find(|d| d.path == path)
            .ok_or_else(|| RedragonError::UnexpectedResponse("Device not found in enumerated list".into()))?;

        {
            let mut active = self.active_device.lock().unwrap();
            *active = Some(dev);
            let mut active_inf = self.active_info.lock().unwrap();
            *active_inf = Some(info.clone());
        }

        Ok(info)
    }

    /// Disconnects the currently active device
    pub fn disconnect(&self) {
        let mut active = self.active_device.lock().unwrap();
        *active = None;
        let mut active_inf = self.active_info.lock().unwrap();
        *active_inf = None;
    }

    /// Checks if a device is currently connected
    pub fn is_connected(&self) -> bool {
        self.active_device.lock().unwrap().is_some()
    }

    /// Returns the active device information if connected
    pub fn get_active_info(&self) -> Option<DeviceInfo> {
        self.active_info.lock().unwrap().clone()
    }

    /// Executes an RF test mode on the connected device
    pub fn execute_rf_test(&self, mode: RfTestMode) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let report = RfTestReport6::new(mode);
            transport.send_feature_report(&report.to_bytes())?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to execute RF test".into(),
            ))
        }
    }

    /// Sends the CheckPID / ReadCIDMID request (Report ID 8)
    pub fn request_check_pid(&self) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::check_pid_command();
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to send CheckPID request".into(),
            ))
        }
    }

    /// Changes the mouse polling rate (125, 250, 500, 1000, 2000, 4000, 8000 Hz)
    pub fn set_polling_rate(&self, rate: crate::core::models::PollingRate) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::set_polling_rate_command(rate);
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to configure polling rate".into(),
            ))
        }
    }

    /// Configures a DPI stage (X/Y sensitivity and RGB LED color)
    pub fn set_dpi_stage(&self, stage_idx: u8, dpi_val: u16, rgb: [u8; 3]) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::set_dpi_stage_command(stage_idx, dpi_val, rgb);
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to configure DPI".into(),
            ))
        }
    }

    /// Reassigns a physical mouse button
    pub fn set_button_action(&self, button_idx: u8, action: crate::core::models::ButtonAction) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let (fun_type, p1, p2) = match action {
                crate::core::models::ButtonAction::None => (0, 0, 0),
                crate::core::models::ButtonAction::Click { button } => (1, button, 0),
                crate::core::models::ButtonAction::Dpi { action } => (2, action as u8, 0),
                crate::core::models::ButtonAction::Media { action } => (3, action as u8, 0),
                crate::core::models::ButtonAction::FireKey { clicks, interval_ms } => (4, clicks, interval_ms),
                crate::core::models::ButtonAction::Shortcut { modifiers, key } => (5, key, modifiers),
                crate::core::models::ButtonAction::Macro { macro_id, loop_count } => (6, macro_id, loop_count.unwrap_or(1)),
                crate::core::models::ButtonAction::PollingRateCycle => (7, 0, 0),
                crate::core::models::ButtonAction::ProfileSwitch => (9, 0, 0),
                crate::core::models::ButtonAction::SniperLock { dpi } => (10, (dpi / 50) as u8, 0),
            };

            let cmd = OutputReport8::set_button_mapping_command(button_idx, fun_type, p1, p2);
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to configure button mapping".into(),
            ))
        }
    }

    /// Returns the cached or current battery information
    pub fn get_battery_info(&self) -> crate::core::models::BatteryInfo {
        self.active_battery.lock().unwrap().clone()
    }

    /// Queries the battery voltage and charge status from the mouse
    pub fn query_battery(&self) -> Result<crate::core::models::BatteryInfo> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::query_battery_command();
            transport.write_output_report(&cmd)?;

            // Non-blocking quick check for response (100ms timeout)
            let mut buf = [0u8; 17];
            if let Ok(bytes_read) = dev.read_timeout(&mut buf, 100) {
                if bytes_read >= 10
                    && buf[0] == 8
                    && buf[1] == crate::protocols::compx::commands::UsbCommandId::BatteryLevel as u8
                {
                    let level = buf[6].min(100);
                    let is_charging = buf[5] > 0;
                    let voltage_mv = ((buf[8] as u16) << 8) | (buf[9] as u16);

                    let mut b = self.active_battery.lock().unwrap();
                    b.percentage = level;
                    b.is_charging = is_charging;
                    if voltage_mv >= 3000 && voltage_mv <= 4500 {
                        b.voltage_mv = voltage_mv;
                    }
                }
            }

            Ok(self.active_battery.lock().unwrap().clone())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to query battery".into(),
            ))
        }
    }

    /// Sets advanced PixArt PAW3395 sensor features
    pub fn set_sensor_config(&self, config: &crate::core::models::SensorConfig) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::set_sensor_features_command(
                config.lift_off_distance,
                config.debounce_time_ms,
                config.motion_sync,
                config.angle_snapping,
                config.ripple_control,
            );
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to configure sensor features".into(),
            ))
        }
    }

    /// Sets the active on-board hardware profile (0 = Config 1, 1 = Config 2, 2 = Config 3, etc.)
    pub fn set_active_profile(&self, profile_idx: u8) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::set_profile_command(profile_idx);
            transport.write_output_report(&cmd)?;
            Ok(())
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to set active profile".into(),
            ))
        }
    }

    /// Queries the currently active on-board hardware profile index from the mouse
    pub fn get_active_profile(&self) -> Result<u8> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::get_profile_command();
            let _ = transport.write_output_report(&cmd);

            let mut in_buf = [0u8; 17];
            if let Ok(len) = dev.read_timeout(&mut in_buf, 100) {
                if len >= 3 {
                    return Ok(in_buf[2]);
                }
            }
            Ok(0)
        } else {
            Err(RedragonError::UnexpectedResponse(
                "No device connected to get active profile".into(),
            ))
        }
    }
}
