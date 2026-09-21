export interface DeviceInfo {
  path: string;
  vid: number;
  pid: number;
  serial_number: string | null;
  manufacturer: string | null;
  product: string | null;
  interface_number: number;
  is_dongle: boolean;
  is_wired: boolean;
}

export type PollingRateHz = 125 | 250 | 500 | 1000 | 2000 | 4000 | 8000;

export interface DpiStageConfig {
  stage: number;
  dpi_x: number;
  dpi_y: number;
  rgb: [number, number, number];
  enabled: boolean;
}

export type ButtonActionType =
  | 'None'
  | 'Click'
  | 'Dpi'
  | 'Media'
  | 'FireKey'
  | 'Shortcut'
  | 'Macro'
  | 'PollingRateCycle'
  | 'SniperLock';

export type ButtonAction =
  | { type: 'None' }
  | { type: 'Click'; button: number }
  | { type: 'Dpi'; action: 'Up' | 'Down' | 'Cycle' | 'Stage1' | 'Stage2' | 'Stage3' | 'Stage4' | 'Stage5' }
  | { type: 'Media'; action: 'VolumeUp' | 'VolumeDown' | 'Mute' | 'PlayPause' | 'Next' | 'Previous' }
  | { type: 'FireKey'; clicks: number; interval_ms: number }
  | { type: 'Shortcut'; modifiers: number; key: number }
  | { type: 'Macro'; macro_id: number }
  | { type: 'PollingRateCycle' }
  | { type: 'SniperLock'; dpi: number };

export interface SensorConfig {
  lod_height_mm: number; // 1 or 2 mm
  motion_sync: boolean;
  debounce_ms: number; // 4 - 20 ms
  ripple_control: boolean;
  angle_snapping: boolean;
}

export interface BatteryInfo {
  percentage: number;
  voltage_mv: number;
  is_charging: boolean;
}

export interface MouseStats {
  left_clicks: number;
  middle_clicks: number;
  right_clicks: number;
  button4_clicks: number;
  button5_clicks: number;
  wheel_delta_y: number;
  wheel_delta_x: number;
  current_polling_rate: number;
  max_polling_rate: number;
}

export type PairingState =
  | { state: 'Idle' }
  | { state: 'Searching'; elapsed_secs: number }
  | { state: 'Synchronizing' }
  | { state: 'Success'; pid: number }
  | { state: 'Failed'; error: string }
  | { state: 'Cancelled' };

export interface AppConfig {
  auto_pair_on_insert: boolean;
  custom_pids: string[];
  default_polling_rate: number;
  default_dpi_stage: number;
  language: string;
}

export type RfTestMode = 'LowCarrier' | 'MtkMode' | 'AllReceived';
