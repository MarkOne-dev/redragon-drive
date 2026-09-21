pub mod config;
pub mod diagnostics;
pub mod mouse;
pub mod pairing;

use crate::driver::DeviceMonitor;
use crate::services::{ConfigService, DeviceService, DiagnosticsService, PairingService};
use std::sync::Arc;

pub struct AppState {
    pub device_service: Arc<DeviceService>,
    pub pairing_service: Arc<PairingService>,
    pub diagnostics_service: Arc<DiagnosticsService>,
    pub config_service: Arc<ConfigService>,
    pub monitor: Arc<DeviceMonitor>,
}

impl AppState {
    pub fn new() -> crate::core::error::Result<Self> {
        let (monitor, _) = DeviceMonitor::new();
        monitor.start();

        let config_path = dirs_or_local_config_path();

        Ok(Self {
            device_service: Arc::new(DeviceService::new()?),
            pairing_service: Arc::new(PairingService::new()),
            diagnostics_service: Arc::new(DiagnosticsService::new()),
            config_service: Arc::new(ConfigService::new(config_path)),
            monitor: Arc::new(monitor),
        })
    }
}

fn dirs_or_local_config_path() -> std::path::PathBuf {
    std::path::PathBuf::from("config.json")
}
