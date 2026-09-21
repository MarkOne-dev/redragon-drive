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
}

impl DeviceService {
    pub fn new() -> Result<Self> {
        let backend = HidBackend::new()?;
        Ok(Self {
            backend,
            active_device: Arc::new(Mutex::new(None)),
            active_info: Arc::new(Mutex::new(None)),
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
                crate::core::models::ButtonAction::Disabled => (0, 0, 0),
                crate::core::models::ButtonAction::MouseClick { button } => (1, button as u8, 0),
                crate::core::models::ButtonAction::DpiSwitch { mode } => (2, mode as u8, 0),
                crate::core::models::ButtonAction::MediaControl { command } => (3, command, 0),
                crate::core::models::ButtonAction::RapidFire { speed, count } => (4, speed, count),
                crate::core::models::ButtonAction::KeyboardShortcut { key_code, modifiers } => (5, key_code, modifiers),
                crate::core::models::ButtonAction::Macro { macro_id, loop_count } => (6, macro_id, loop_count),
                crate::core::models::ButtonAction::PollingRateCycle => (7, 0, 0),
                crate::core::models::ButtonAction::ProfileSwitch => (9, 0, 0),
                crate::core::models::ButtonAction::SniperLock { target_dpi } => (10, (target_dpi / 50) as u8, 0),
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

    /// Queries the battery voltage and charge status from the mouse
    pub fn query_battery(&self) -> Result<()> {
        let active = self.active_device.lock().unwrap();
        if let Some(ref dev) = *active {
            let transport = HidTransport::new(dev);
            let cmd = OutputReport8::query_battery_command();
            transport.write_output_report(&cmd)?;
            Ok(())
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
}
