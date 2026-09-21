use crate::commands::AppState;
use crate::core::models::MouseStats;

#[tauri::command]
pub fn get_mouse_stats(state: tauri::State<'_, AppState>) -> MouseStats {
    state.diagnostics_service.get_stats()
}

#[tauri::command]
pub fn reset_mouse_stats(state: tauri::State<'_, AppState>) {
    state.diagnostics_service.reset_stats();
}
