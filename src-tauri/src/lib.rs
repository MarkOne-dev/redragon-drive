pub mod commands;
pub mod core;
pub mod driver;
pub mod protocols;
pub mod services;

pub use commands::*;
pub use core::*;
pub use driver::*;
pub use protocols::*;
pub use services::*;

use tauri::{Emitter, Manager};
use tauri_plugin_log::{Target, TargetKind};

pub fn run() {
    let app_state = match AppState::new() {
        Ok(state) => state,
        Err(e) => {
            eprintln!("Failed to initialize AppState: {}", e);
            std::process::exit(1);
        }
    };

    tauri::Builder::default()
        .plugin(
            tauri_plugin_log::Builder::new()
                .targets([
                    Target::new(TargetKind::Stdout),
                    Target::new(TargetKind::LogDir { file_name: None }),
                    Target::new(TargetKind::Webview),
                ])
                .build(),
        )
        .plugin(tauri_plugin_opener::init())
        .manage(app_state)
        .setup(|app| {
            let app_handle = app.handle().clone();
            let state = app.state::<AppState>();
            let mut rx = state.monitor.subscribe();

            tauri::async_runtime::spawn(async move {
                while let Ok(event) = rx.recv().await {
                    let _ = app_handle.emit("device-changed", &event);
                }
            });

            Ok(())
        })
        .invoke_handler(tauri::generate_handler![
            commands::mouse::scan_devices,
            commands::mouse::scan_all_devices,
            commands::mouse::connect_device,
            commands::mouse::disconnect_device,
            commands::mouse::get_active_device,
            commands::mouse::get_battery_info,
            commands::mouse::get_device_full_state,
            commands::mouse::trigger_check_pid,
            commands::mouse::run_rf_test,
            commands::mouse::set_mouse_polling_rate,
            commands::mouse::set_mouse_dpi,
            commands::mouse::set_active_dpi_stage,
            commands::mouse::set_mouse_button,
            commands::mouse::query_mouse_battery,
            commands::mouse::set_mouse_sensor,
            commands::mouse::set_active_profile,
            commands::mouse::get_active_profile,
            commands::pairing::start_pairing,
            commands::pairing::cancel_pairing,
            commands::pairing::get_pairing_status,
            commands::diagnostics::get_mouse_stats,
            commands::diagnostics::reset_mouse_stats,
            commands::config::get_config,
            commands::config::save_config,
            commands::config::add_custom_pid,
        ])
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
