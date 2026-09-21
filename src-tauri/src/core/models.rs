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
pub enum PairingState {
    Idle,
    Pairing { seconds_elapsed: u32 },
    Success { vid: u8, pid: u8 },
    Failed,
    Timeout,
    ReadFailed,
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
pub enum ButtonAction {
    Disabled,
    MouseClick { button: MouseButtonType },
    DpiSwitch { mode: DpiSwitchMode },
    MediaControl { command: u8 },
    RapidFire { speed: u8, count: u8 },
    KeyboardShortcut { key_code: u8, modifiers: u8 },
    Macro { macro_id: u8, loop_count: u8 },
    PollingRateCycle,
    ProfileSwitch,
    SniperLock { target_dpi: u16 },
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
    pub lift_off_distance: u8, // 1 = 1mm, 2 = 2mm
    pub motion_sync: bool,
    pub angle_snapping: bool,
    pub ripple_control: bool,
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
    pub auto_pair: bool,
    pub auto_pair_on_insert: bool,
    pub custom_pids: Vec<String>,
    pub polling_rate_super_font: bool,
    pub zoom_step: u32,
    pub language: String,
    pub dark_mode: bool,
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
            polling_rate_super_font: false,
            zoom_step: 0,
            language: "en".to_string(),
            dark_mode: true,
        }
    }
}
