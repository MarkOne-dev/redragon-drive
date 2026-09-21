use crate::commands::AppState;
use crate::core::models::PairingState;

#[tauri::command]
pub fn start_pairing(state: tauri::State<'_, AppState>, cid: u8, pid: u8) -> Result<(), String> {
    let handle = state.device_service.active_device_handle();
    state
        .pairing_service
        .start_pairing(handle, cid, pid)
        .map_err(|e| e.to_string())
}

#[tauri::command]
pub fn cancel_pairing(state: tauri::State<'_, AppState>) {
    state.pairing_service.cancel();
}

#[tauri::command]
pub fn get_pairing_status(state: tauri::State<'_, AppState>) -> PairingState {
    state.pairing_service.current_state()
}
