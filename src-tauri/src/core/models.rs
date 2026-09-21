use serde::{Deserialize, Serialize};

/// Basic information of a detected HID device
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct DeviceInfo {
    pub vid: u16,
    pub pid: u16,
    pub path: String,
    pub manufacturer: Option<String>,
    pub product: Option<String>,
    pub serial_number: Option<String>,
    pub interface_number: i32,
}

/// State of the wireless pairing process (FastConnect / Pairing)
#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
#[serde(tag = "state")]
pub enum PairingState {
    Idle,
    #[serde(alias = "Searching")]
    Pairing {
        #[serde(alias = "elapsed_secs", default)]
        seconds_elapsed: u32,
    },
    Synchronizing,
    Success {
        #[serde(default)]
        vid: u8,
        #[serde(default)]
        pid: u8,
    },
    Failed,
    Timeout,
    ReadFailed,
    Cancelled,
}

/// Radio frequency test modes extracted from the factory test protocol
#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum RfTestMode {
    LowCarrier = 0,
    MidCarrier = 1,
    HighCarrier = 2,
    MtkMode = 161,
    LowData = 3,
    MidData = 4,
    HighData = 5,
    AllData = 6,
    LowReceived = 7,
    MidReceived = 8,
    HighReceived = 9,
    AllReceived = 10,
}

/// Polling rates with their frequency in Hz and bitmask for the Compx protocol
#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum PollingRate {
    Hz125 = 8,
    Hz250 = 4,
    Hz500 = 2,
    Hz1000 = 1,
    Hz2000 = 16,
    Hz4000 = 32,
    Hz8000 = 64,
}

impl PollingRate {
    pub fn to_hz(&self) -> u32 {
        match self {
            PollingRate::Hz125 => 125,
            PollingRate::Hz250 => 250,
            PollingRate::Hz500 => 500,
            PollingRate::Hz1000 => 1000,
            PollingRate::Hz2000 => 2000,
            PollingRate::Hz4000 => 4000,
            PollingRate::Hz8000 => 8000,
        }
    }

    pub fn from_hz(hz: u32) -> Option<Self> {
        match hz {
            125 => Some(PollingRate::Hz125),
            250 => Some(PollingRate::Hz250),
            500 => Some(PollingRate::Hz500),
            1000 => Some(PollingRate::Hz1000),
            2000 => Some(PollingRate::Hz2000),
            4000 => Some(PollingRate::Hz4000),
            8000 => Some(PollingRate::Hz8000),
            _ => None,
        }
    }

    pub fn mask(&self) -> u8 {
        *self as u8
    }
}

/// Configuration for a single DPI stage (up to 8 levels)
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct DpiStageConfig {
    pub stage: u8,
    pub dpi_x: u16,
    pub dpi_y: u16,
    pub color_rgb: [u8; 3],
    pub enabled: bool,
}

/// Programmable actions for mouse buttons
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
#[serde(tag = "type")]
pub enum ButtonAction {
    #[serde(alias = "Disabled")]
    None,
    #[serde(alias = "MouseClick")]
    Click { button: u8 },
    #[serde(alias = "DpiSwitch")]
    Dpi { action: DpiAction },
    #[serde(alias = "MediaControl")]
    Media { action: MediaAction },
    #[serde(alias = "RapidFire")]
    FireKey {
        #[serde(default = "default_burst_clicks")]
        clicks: u8,
        #[serde(default = "default_burst_interval")]
        interval_ms: u8,
    },
    #[serde(alias = "KeyboardShortcut")]
    Shortcut {
        #[serde(default)]
        modifiers: u8,
        #[serde(default)]
        key: u8,
    },
    Macro {
        macro_id: u8,
        #[serde(default)]
        loop_count: Option<u8>,
    },
    PollingRateCycle,
    ProfileSwitch,
    SniperLock {
        #[serde(alias = "target_dpi")]
        dpi: u16,
    },
}

#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum DpiAction {
    Cycle = 1,
    Up = 2,
    Down = 3,
    Stage1 = 4,
    Stage2 = 5,
    Stage3 = 6,
    Stage4 = 7,
    Stage5 = 8,
}

#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum MediaAction {
    VolumeUp = 1,
    VolumeDown = 2,
    Mute = 3,
    PlayPause = 4,
    Next = 5,
    Previous = 6,
}

fn default_burst_clicks() -> u8 {
    3
}

fn default_burst_interval() -> u8 {
    20
}

#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum MouseButtonType {
    Left = 1,
    Right = 2,
    Middle = 4,
    Backward = 8,
    Forward = 16,
}

#[derive(Debug, Clone, Copy, Serialize, Deserialize, PartialEq, Eq)]
pub enum DpiSwitchMode {
    Cycle = 1,
    Increase = 2,
    Decrease = 3,
}

/// Button mapping definition (index 0 to 15)
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct ButtonMapping {
    pub button_index: u8,
    pub action: ButtonAction,
}

/// Advanced sensor configuration for PixArt PAW3395
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct SensorConfig {
    #[serde(alias = "lod_height_mm", alias = "lodHeightMm")]
    pub lift_off_distance: u8, // 1 = 1mm, 2 = 2mm
    #[serde(default)]
    pub motion_sync: bool,
    #[serde(default)]
    pub angle_snapping: bool,
    #[serde(default)]
    pub ripple_control: bool,
    #[serde(alias = "debounce_ms", alias = "debounceMs")]
    pub debounce_time_ms: u8,  // 4ms, 8ms, 12ms, etc.
}

impl Default for SensorConfig {
    fn default() -> Self {
        Self {
            lift_off_distance: 1,
            motion_sync: true,
            angle_snapping: false,
            ripple_control: false,
            debounce_time_ms: 8,
        }
    }
}

/// Device battery state information
#[derive(Debug, Default, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct BatteryInfo {
    pub percentage: u8,
    pub voltage_mv: u16,
    pub is_charging: bool,
}

/// Real-time mouse statistics and diagnostic counters
#[derive(Debug, Default, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct MouseStats {
    pub left_clicks: u64,
    pub middle_clicks: u64,
    pub right_clicks: u64,
    pub button4_clicks: u64,
    pub button5_clicks: u64,
    pub wheel_delta_y: i64,
    pub wheel_delta_x: i64,
    pub current_polling_rate: u32,
    pub average_polling_rate: u32,
    pub max_polling_rate: u32,
}

/// Persistent application configuration
#[derive(Debug, Clone, Serialize, Deserialize, PartialEq, Eq)]
pub struct AppConfig {
    #[serde(default)]
    pub auto_pair: bool,
    #[serde(default = "default_true")]
    pub auto_pair_on_insert: bool,
    #[serde(default)]
    pub custom_pids: Vec<String>,
    #[serde(default = "default_polling_rate")]
    pub default_polling_rate: u32,
    #[serde(default = "default_dpi_stage")]
    pub default_dpi_stage: u8,
    #[serde(default)]
    pub polling_rate_super_font: bool,
    #[serde(default)]
    pub zoom_step: u32,
    #[serde(default = "default_language")]
    pub language: String,
    #[serde(default = "default_true")]
    pub dark_mode: bool,
}

fn default_true() -> bool {
    true
}

fn default_polling_rate() -> u32 {
    1000
}

fn default_dpi_stage() -> u8 {
    2
}

fn default_language() -> String {
    "en".to_string()
}

impl Default for AppConfig {
    fn default() -> Self {
        Self {
            auto_pair: false,
            auto_pair_on_insert: true,
            custom_pids: vec![
                "3554F55E".to_string(), // M916-PRO Wired / Direct
                "3554F55D".to_string(), // M916-PRO Dongle 2.4G
                "3554F55F".to_string(), // M916-PRO Dongle Alt
                "3554F501".to_string(), // M916-PRO Dongle v1
                "35542635".to_string(), // GamingPro2635 Standard
                "3554F502".to_string(),
            ],
            default_polling_rate: 1000,
            default_dpi_stage: 2,
            polling_rate_super_font: false,
            zoom_step: 0,
            language: "en".to_string(),
            dark_mode: true,
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_sensor_config_json_compatibility() {
        // Test with TS names (lod_height_mm, debounce_ms)
        let ts_json = r#"{
            "lod_height_mm": 2,
            "motion_sync": true,
            "debounce_ms": 12,
            "ripple_control": true,
            "angle_snapping": false
        }"#;

        let cfg: SensorConfig = serde_json::from_str(ts_json).expect("Failed to deserialize SensorConfig with TS field names");
        assert_eq!(cfg.lift_off_distance, 2);
        assert_eq!(cfg.debounce_time_ms, 12);
        assert!(cfg.motion_sync);
        assert!(cfg.ripple_control);
        assert!(!cfg.angle_snapping);
    }

    #[test]
    fn test_button_action_json_compatibility() {
        // 1. Click
        let json_click = r#"{"type": "Click", "button": 1}"#;
        let action: ButtonAction = serde_json::from_str(json_click).expect("Click deserialization failed");
        assert_eq!(action, ButtonAction::Click { button: 1 });

        // 2. FireKey
        let json_fire = r#"{"type": "FireKey", "clicks": 5, "interval_ms": 30}"#;
        let action: ButtonAction = serde_json::from_str(json_fire).expect("FireKey deserialization failed");
        assert_eq!(action, ButtonAction::FireKey { clicks: 5, interval_ms: 30 });

        // 3. SniperLock
        let json_sniper = r#"{"type": "SniperLock", "dpi": 400}"#;
        let action: ButtonAction = serde_json::from_str(json_sniper).expect("SniperLock deserialization failed");
        assert_eq!(action, ButtonAction::SniperLock { dpi: 400 });

        // 4. Dpi
        let json_dpi = r#"{"type": "Dpi", "action": "Cycle"}"#;
        let action: ButtonAction = serde_json::from_str(json_dpi).expect("Dpi deserialization failed");
        assert_eq!(action, ButtonAction::Dpi { action: DpiAction::Cycle });

        // 5. Media
        let json_media = r#"{"type": "Media", "action": "VolumeUp"}"#;
        let action: ButtonAction = serde_json::from_str(json_media).expect("Media deserialization failed");
        assert_eq!(action, ButtonAction::Media { action: MediaAction::VolumeUp });

        // 6. Shortcut
        let json_shortcut = r#"{"type": "Shortcut", "modifiers": 1, "key": 67}"#;
        let action: ButtonAction = serde_json::from_str(json_shortcut).expect("Shortcut deserialization failed");
        assert_eq!(action, ButtonAction::Shortcut { modifiers: 1, key: 67 });

        // 7. None / Disabled
        let json_none = r#"{"type": "None"}"#;
        let action: ButtonAction = serde_json::from_str(json_none).expect("None deserialization failed");
        assert_eq!(action, ButtonAction::None);
    }

    #[test]
    fn test_pairing_state_json_compatibility() {
        let json_searching = r#"{"state": "Searching", "elapsed_secs": 5}"#;
        let state: PairingState = serde_json::from_str(json_searching).expect("PairingState Searching failed");
        assert_eq!(state, PairingState::Pairing { seconds_elapsed: 5 });

        let json_idle = r#"{"state": "Idle"}"#;
        let state: PairingState = serde_json::from_str(json_idle).expect("PairingState Idle failed");
        assert_eq!(state, PairingState::Idle);

        let json_success = r#"{"state": "Success", "vid": 53, "pid": 84}"#;
        let state: PairingState = serde_json::from_str(json_success).expect("PairingState Success failed");
        assert_eq!(state, PairingState::Success { vid: 53, pid: 84 });
    }

    #[test]
    fn test_app_config_json_compatibility() {
        let json_cfg = r#"{
            "auto_pair_on_insert": true,
            "custom_pids": ["3554F55D"],
            "default_polling_rate": 1000,
            "default_dpi_stage": 2,
            "language": "en"
        }"#;

        let cfg: AppConfig = serde_json::from_str(json_cfg).expect("AppConfig deserialization failed");
        assert_eq!(cfg.default_polling_rate, 1000);
        assert_eq!(cfg.default_dpi_stage, 2);
        assert_eq!(cfg.custom_pids, vec!["3554F55D".to_string()]);
    }
}

