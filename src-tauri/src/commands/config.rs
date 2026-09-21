use crate::commands::AppState;
use crate::core::models::AppConfig;

#[tauri::command]
pub fn get_config(state: tauri::State<'_, AppState>) -> AppConfig {
    state.config_service.get_config()
}

#[tauri::command]
pub fn save_config(state: tauri::State<'_, AppState>, config: AppConfig) -> Result<(), String> {
    state.config_service.save_config(config).map_err(|e| e.to_string())
}

#[tauri::command]
pub fn add_custom_pid(state: tauri::State<'_, AppState>, vid_pid: String) -> Result<(), String> {
    state.config_service.add_custom_pid(&vid_pid).map_err(|e| e.to_string())
}
