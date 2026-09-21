import { trackedInvoke, isTauri } from '@/lib/tauri';
import type {
  DeviceInfo,
  PollingRateHz,
  ButtonAction,
  SensorConfig,
  MouseStats,
  PairingState,
  AppConfig,
  RfTestMode,
} from '@/types/mouse';

// Mock state for browser development & preview
const mockDeviceInfo: DeviceInfo = {
  path: 'USB#VID_3554&PID_F55D#Dongle01',
  vid: 0x3554,
  pid: 0xf55d,
  serial_number: 'RD-M916-24G01',
  manufacturer: 'Redragon / Compx',
  product: 'Redragon M916-PRO 1K (2.4G)',
  interface_number: 1,
  is_dongle: true,
  is_wired: false,
};

let mockStats: MouseStats = {
  left_clicks: 142,
  middle_clicks: 29,
  right_clicks: 86,
  button4_clicks: 14,
  button5_clicks: 21,
  wheel_delta_y: 450,
  wheel_delta_x: 0,
  current_polling_rate: 1000,
  max_polling_rate: 1000,
};

let mockConfig: AppConfig = {
  auto_pair_on_insert: true,
  custom_pids: ['3554F55E', '3554F55D', '3554F55F', '3554F501', '35542635'],
  default_polling_rate: 1000,
  default_dpi_stage: 2,
  language: 'en',
};

export const mouseApi = {
  async scanDevices(): Promise<DeviceInfo[]> {
    if (!isTauri()) return [mockDeviceInfo];
    return trackedInvoke<DeviceInfo[]>('scan_devices');
  },

  async scanAllDevices(): Promise<DeviceInfo[]> {
    if (!isTauri()) return [mockDeviceInfo];
    return trackedInvoke<DeviceInfo[]>('scan_all_devices');
  },

  async connectDevice(path: string): Promise<DeviceInfo> {
    if (!isTauri()) return { ...mockDeviceInfo, path };
    return trackedInvoke<DeviceInfo>('connect_device', { path });
  },

  async disconnectDevice(): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('disconnect_device');
  },

  async getActiveDevice(): Promise<DeviceInfo | null> {
    if (!isTauri()) return mockDeviceInfo;
    return trackedInvoke<DeviceInfo | null>('get_active_device');
  },

  async triggerCheckPid(): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('trigger_check_pid');
  },

  async setPollingRate(hz: PollingRateHz): Promise<void> {
    if (!isTauri()) {
      mockStats.current_polling_rate = hz;
      return;
    }
    return trackedInvoke<void>('set_mouse_polling_rate', { hz });
  },

  async setDpi(stageIdx: number, dpiVal: number, r: number, g: number, b: number): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('set_mouse_dpi', { stageIdx, dpiVal, r, g, b });
  },

  async setButton(buttonIdx: number, action: ButtonAction): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('set_mouse_button', { buttonIdx, action });
  },

  async queryBattery(): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('query_mouse_battery');
  },

  async setSensor(config: SensorConfig): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('set_mouse_sensor', { config });
  },

  async startPairing(cid: number, pid: number): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('start_pairing', { cid, pid });
  },

  async cancelPairing(): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('cancel_pairing');
  },

  async getPairingStatus(): Promise<PairingState> {
    if (!isTauri()) return { state: 'Idle' };
    return trackedInvoke<PairingState>('get_pairing_status');
  },

  async getMouseStats(): Promise<MouseStats> {
    if (!isTauri()) return { ...mockStats };
    return trackedInvoke<MouseStats>('get_mouse_stats');
  },

  async resetMouseStats(): Promise<void> {
    if (!isTauri()) {
      mockStats = {
        left_clicks: 0,
        middle_clicks: 0,
        right_clicks: 0,
        button4_clicks: 0,
        button5_clicks: 0,
        wheel_delta_y: 0,
        wheel_delta_x: 0,
        current_polling_rate: 0,
        max_polling_rate: 0,
      };
      return;
    }
    return trackedInvoke<void>('reset_mouse_stats');
  },

  async getConfig(): Promise<AppConfig> {
    if (!isTauri()) return mockConfig;
    return trackedInvoke<AppConfig>('get_config');
  },

  async saveConfig(config: AppConfig): Promise<void> {
    if (!isTauri()) {
      mockConfig = config;
      return;
    }
    return trackedInvoke<void>('save_config', { config });
  },

  async addCustomPid(vidPid: string): Promise<void> {
    if (!isTauri()) {
      mockConfig.custom_pids.push(vidPid);
      return;
    }
    return trackedInvoke<void>('add_custom_pid', { vidPid });
  },

  async runRfTest(mode: RfTestMode): Promise<void> {
    if (!isTauri()) return;
    return trackedInvoke<void>('run_rf_test', { mode });
  },
};
