use crate::commands::AppState;
use crate::core::models::{DeviceInfo, RfTestMode};
use crate::driver::DeviceDetector;

#[tauri::command]
pub fn scan_devices() -> Result<Vec<DeviceInfo>, String> {
    let mut detector = DeviceDetector::new().map_err(|e| e.to_string())?;
    detector.scan_compx_devices().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn scan_all_devices() -> Result<Vec<DeviceInfo>, String> {
    let mut detector = DeviceDetector::new().map_err(|e| e.to_string())?;
    detector.scan_all_hid_devices().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn connect_device(state: tauri::State<'_, AppState>, path: String) -> Result<DeviceInfo, String> {
    state.device_service.connect_by_path(&path).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn disconnect_device(state: tauri::State<'_, AppState>) -> Result<(), String> {
    state.device_service.disconnect();
    state.diagnostics_service.stop_monitoring();
    Ok(())
}

#[tauri::command]
pub fn get_active_device(state: tauri::State<'_, AppState>) -> Option<DeviceInfo> {
    state.device_service.get_active_info()
}

#[tauri::command]
pub fn trigger_check_pid(state: tauri::State<'_, AppState>) -> Result<(), String> {
    state.device_service.request_check_pid().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn run_rf_test(state: tauri::State<'_, AppState>, mode: RfTestMode) -> Result<(), String> {
    state.device_service.execute_rf_test(mode).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn set_mouse_polling_rate(state: tauri::State<'_, AppState>, hz: u32) -> Result<(), String> {
    let rate = crate::core::models::PollingRate::from_hz(hz)
        .ok_or_else(|| format!("Unsupported polling rate: {} Hz. Supported options: 125, 250, 500, 1000, 2000, 4000, 8000 Hz", hz))?;
    state.device_service.set_polling_rate(rate).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn set_mouse_dpi(state: tauri::State<'_, AppState>, stage_idx: u8, dpi_val: u16, r: u8, g: u8, b: u8) -> Result<(), String> {
    if dpi_val < 50 || dpi_val > 26000 {
        return Err("Invalid DPI range for PixArt PAW3395 (must be between 50 and 26,000)".into());
    }
    state.device_service.set_dpi_stage(stage_idx, dpi_val, [r, g, b]).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn set_mouse_button(state: tauri::State<'_, AppState>, button_idx: u8, action: crate::core::models::ButtonAction) -> Result<(), String> {
    if button_idx > 15 {
        return Err("Button index out of range (must be between 0 and 15)".into());
    }
    state.device_service.set_button_action(button_idx, action).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn query_mouse_battery(state: tauri::State<'_, AppState>) -> Result<crate::core::models::BatteryInfo, String> {
    state.device_service.query_battery().map_err(|e| e.to_string())
}

#[tauri::command]
pub fn get_battery_info(state: tauri::State<'_, AppState>) -> crate::core::models::BatteryInfo {
    state.device_service.get_battery_info()
}

#[tauri::command]
pub fn set_mouse_sensor(state: tauri::State<'_, AppState>, config: crate::core::models::SensorConfig) -> Result<(), String> {
    state.device_service.set_sensor_config(&config).map_err(|e| e.to_string())
}

#[derive(Debug, Clone, serde::Serialize, serde::Deserialize)]
pub struct DeviceFullState {
    pub device: Option<DeviceInfo>,
    pub battery: crate::core::models::BatteryInfo,
    pub polling_rate: u32,
    pub current_dpi_stage: u8,
    pub sensor: crate::core::models::SensorConfig,
}

#[tauri::command]
pub fn get_device_full_state(state: tauri::State<'_, AppState>) -> DeviceFullState {
    let device = state.device_service.get_active_info();
    let battery = state.device_service.get_battery_info();
    let stats = state.diagnostics_service.get_stats();
    let polling_rate = if stats.current_polling_rate > 0 { stats.current_polling_rate } else { 1000 };

    DeviceFullState {
        device,
        battery,
        polling_rate,
        current_dpi_stage: 2,
        sensor: crate::core::models::SensorConfig::default(),
    }
}

