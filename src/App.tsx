import { useState, useEffect, useCallback } from 'react';
import { Header } from '@/components/layout/Header';
import { Sidebar, type NavTab } from '@/components/layout/Sidebar';
import { DashboardView } from '@/components/views/DashboardView';
import { PerformanceView } from '@/components/views/PerformanceView';
import { ButtonsView } from '@/components/views/ButtonsView';
import { SensorView } from '@/components/views/SensorView';
import { PairingView } from '@/components/views/PairingView';
import { DiagnosticsView } from '@/components/views/DiagnosticsView';
import { SettingsView } from '@/components/views/SettingsView';
import { mouseApi } from '@/api/mouseApi';
import type { 
  DeviceInfo, 
  BatteryInfo, 
  MouseStats, 
  PollingRateHz, 
  ButtonAction, 
  SensorConfig, 
  PairingState, 
  AppConfig,
  MouseProfile 
} from '@/types/mouse';

const DEFAULT_SENSOR_CONFIG: SensorConfig = {
  lod_height_mm: 1,
  motion_sync: true,
  debounce_ms: 8,
  ripple_control: false,
  angle_snapping: false,
};

const createDefaultProfile = (id: number): MouseProfile => ({
  id,
  name: `Profile ${id}`,
  dpi_stages: [
    { stage: 0, dpi_x: 400, dpi_y: 400, rgb: [255, 42, 77], enabled: true },
    { stage: 1, dpi_x: 800, dpi_y: 800, rgb: [0, 240, 255], enabled: true },
    { stage: 2, dpi_x: 1600, dpi_y: 1600, rgb: [50, 255, 126], enabled: true },
    { stage: 3, dpi_x: 3200, dpi_y: 3200, rgb: [255, 211, 42], enabled: true },
    { stage: 4, dpi_x: 6400, dpi_y: 6400, rgb: [156, 39, 176], enabled: true },
  ],
  active_stage_index: 2,
  polling_rate: 1000,
  button_mappings: {},
  sensor_config: DEFAULT_SENSOR_CONFIG,
});

const loadInitialProfiles = (): Record<number, MouseProfile> => {
  const result: Record<number, MouseProfile> = {
    1: createDefaultProfile(1),
    2: createDefaultProfile(2),
    3: createDefaultProfile(3),
  };

  for (let p = 1; p <= 3; p++) {
    const raw = localStorage.getItem(`rd_profile_${p}_data`);
    if (raw) {
      try {
        result[p] = { ...result[p], ...JSON.parse(raw) };
      } catch {}
    } else {
      const legacyDpi = localStorage.getItem(`rd_profile_${p}_dpi`);
      if (legacyDpi) {
        try {
          result[p].dpi_stages = JSON.parse(legacyDpi);
        } catch {}
      }
    }
  }
  return result;
};

export function App() {
  const [activeTab, setActiveTab] = useState<NavTab>('dashboard');
  const [device, setDevice] = useState<DeviceInfo | null>(null);
  const [isScanning, setIsScanning] = useState<boolean>(false);
  const [battery, setBattery] = useState<BatteryInfo>({ percentage: 95, voltage_mv: 4120, is_charging: false });
  const [stats, setStats] = useState<MouseStats>({
    left_clicks: 0,
    middle_clicks: 0,
    right_clicks: 0,
    button4_clicks: 0,
    button5_clicks: 0,
    wheel_delta_y: 0,
    wheel_delta_x: 0,
    current_polling_rate: 1000,
    max_polling_rate: 1000,
  });

  const [profiles, setProfiles] = useState<Record<number, MouseProfile>>(loadInitialProfiles);
  const [activeProfile, setActiveProfile] = useState<number>(() => {
    const saved = localStorage.getItem('rd_active_profile');
    return saved ? Math.min(3, Math.max(1, Number(saved))) : 1;
  });

  const [pairingStatus, setPairingStatus] = useState<PairingState>({ state: 'Idle' });
  const [config, setConfig] = useState<AppConfig>({
    auto_pair_on_insert: true,
    custom_pids: ['3554F55E', '3554F55D', '3554F55F', '3554F501', '35542635'],
    default_polling_rate: 1000,
    default_dpi_stage: 2,
    language: 'en',
  });

  const currentProfile = profiles[activeProfile] || profiles[1];
  const dpiStages = currentProfile.dpi_stages;
  const activeStageIndex = currentProfile.active_stage_index;
  const pollingRate = currentProfile.polling_rate;
  const buttonMappings = currentProfile.button_mappings;
  const sensorConfig = currentProfile.sensor_config;

  const handleSelectProfile = async (p: number) => {
    setActiveProfile(p);
    localStorage.setItem('rd_active_profile', String(p));

    // 1. Tell mouse hardware to switch on-board profile (0-indexed: 0=P1, 1=P2, 2=P3)
    try {
      await mouseApi.setActiveProfile(p - 1);
    } catch (err) {
      console.warn('Failed to switch hardware profile:', err);
    }

    // 2. Synchronize active profile parameters to the hardware
    const target = profiles[p] || profiles[1];
    try {
      await mouseApi.setPollingRate(target.polling_rate);
      const activeStage = target.dpi_stages[target.active_stage_index] || target.dpi_stages[0];
      await mouseApi.setDpi(
        target.active_stage_index,
        activeStage.dpi_x,
        activeStage.rgb[0],
        activeStage.rgb[1],
        activeStage.rgb[2]
      );
      await mouseApi.setSensor(target.sensor_config);
      for (const [btnIdx, action] of Object.entries(target.button_mappings)) {
        await mouseApi.setButton(Number(btnIdx), action);
      }
    } catch (syncErr) {
      console.warn('Failed to synchronize profile settings to mouse hardware:', syncErr);
    }
  };

  const handleSelectActiveStage = (idx: number) => {
    setProfiles((prev) => {
      const updated = {
        ...prev,
        [activeProfile]: {
          ...prev[activeProfile],
          active_stage_index: idx,
        },
      };
      localStorage.setItem(`rd_profile_${activeProfile}_data`, JSON.stringify(updated[activeProfile]));
      return updated;
    });
  };

  // Scan for connected Compx / Redragon mice and hydrate full state
  const scanDevices = useCallback(async () => {
    setIsScanning(true);
    try {
      const devices = await mouseApi.scanDevices();
      if (devices && devices.length > 0) {
        const primary = devices.find((d) => d.interface_number === 1) || devices[0];
        setDevice(primary);
        try {
          await mouseApi.connectDevice(primary.path);
          const fullState = await mouseApi.getDeviceFullState();
          if (fullState.battery) setBattery(fullState.battery);
          if (fullState.active_profile !== undefined && fullState.active_profile !== null) {
            const hwProfile = fullState.active_profile + 1;
            if (hwProfile >= 1 && hwProfile <= 3) {
              setActiveProfile(hwProfile);
              localStorage.setItem('rd_active_profile', String(hwProfile));
            }
          }
        } catch (connErr) {
          console.warn('Device detected but could not establish HID control session:', connErr);
        }
      } else {
        setDevice(null);
      }
    } catch (err) {
      console.error('Failed to scan devices:', err);
    } finally {
      setIsScanning(false);
    }
  }, []);

  const handleDisconnect = async () => {
    await mouseApi.disconnectDevice();
    setDevice(null);
  };

  const handleSetPollingRate = async (hz: PollingRateHz) => {
    await mouseApi.setPollingRate(hz);
    setProfiles((prev) => {
      const updated = {
        ...prev,
        [activeProfile]: {
          ...prev[activeProfile],
          polling_rate: hz,
        },
      };
      localStorage.setItem(`rd_profile_${activeProfile}_data`, JSON.stringify(updated[activeProfile]));
      return updated;
    });
  };

  const handleUpdateDpiStage = async (stageIdx: number, dpiX: number, dpiY: number, rgb: [number, number, number]) => {
    await mouseApi.setDpi(stageIdx, dpiX, rgb[0], rgb[1], rgb[2]);
    setProfiles((prev) => {
      const updatedDpi = prev[activeProfile].dpi_stages.map((s, idx) =>
        idx === stageIdx ? { ...s, dpi_x: dpiX, dpi_y: dpiY, rgb } : s
      );
      const updated = {
        ...prev,
        [activeProfile]: {
          ...prev[activeProfile],
          dpi_stages: updatedDpi,
        },
      };
      localStorage.setItem(`rd_profile_${activeProfile}_data`, JSON.stringify(updated[activeProfile]));
      return updated;
    });
  };

  const handleSaveButton = async (buttonIdx: number, action: ButtonAction) => {
    await mouseApi.setButton(buttonIdx, action);
    setProfiles((prev) => {
      const updated = {
        ...prev,
        [activeProfile]: {
          ...prev[activeProfile],
          button_mappings: {
            ...prev[activeProfile].button_mappings,
            [buttonIdx]: action,
          },
        },
      };
      localStorage.setItem(`rd_profile_${activeProfile}_data`, JSON.stringify(updated[activeProfile]));
      return updated;
    });
  };

  const handleSaveSensor = async (newConfig: SensorConfig) => {
    await mouseApi.setSensor(newConfig);
    setProfiles((prev) => {
      const updated = {
        ...prev,
        [activeProfile]: {
          ...prev[activeProfile],
          sensor_config: newConfig,
        },
      };
      localStorage.setItem(`rd_profile_${activeProfile}_data`, JSON.stringify(updated[activeProfile]));
      return updated;
    });
  };

  const handleStartPairing = async () => {
    setPairingStatus({ state: 'Searching', elapsed_secs: 0 });
    try {
      await mouseApi.startPairing(0x35, 0x54);
    } catch (err: any) {
      setPairingStatus({ state: 'Failed', error: err?.message || String(err) });
    }
  };

  const handleCancelPairing = async () => {
    await mouseApi.cancelPairing();
    setPairingStatus({ state: 'Cancelled' });
  };

  const handleRefreshStats = async () => {
    try {
      const currentStats = await mouseApi.getMouseStats();
      setStats(currentStats);
    } catch (err) {
      console.error('Failed to refresh stats:', err);
    }
  };

  const handleRefreshBattery = async () => {
    try {
      const bat = await mouseApi.queryBattery();
      if (bat && bat.percentage > 0) {
        setBattery(bat);
      }
    } catch (err) {
      console.error('Failed to query battery:', err);
    }
  };

  const handleSaveConfig = async (newCfg: AppConfig) => {
    await mouseApi.saveConfig(newCfg);
    setConfig(newCfg);
  };

  // Initial load and hotplug event subscription
  useEffect(() => {
    scanDevices();
    mouseApi.getConfig().then(setConfig).catch(console.error);

    // Subscribe to hotplug USB events
    let unlistenFn: (() => void) | null = null;
    mouseApi.onDeviceChanged((event) => {
      if (event.type === 'Connected') {
        scanDevices();
      } else if (event.type === 'Disconnected') {
        setDevice(null);
      }
    }).then((unlisten) => {
      unlistenFn = unlisten;
    });

    // Periodic telemetry refresh
    const timer = setInterval(() => {
      handleRefreshStats();
      handleRefreshBattery();
      mouseApi.getPairingStatus().then(setPairingStatus).catch(() => {});
    }, 3000);

    return () => {
      clearInterval(timer);
      if (unlistenFn) unlistenFn();
    };
  }, [scanDevices]);

  const activeStage = dpiStages[activeStageIndex] || dpiStages[0];
  const dpiColor = `rgb(${activeStage.rgb[0]}, ${activeStage.rgb[1]}, ${activeStage.rgb[2]})`;

  return (
    <div className="flex flex-col h-screen w-screen overflow-hidden bg-background text-foreground font-sans antialiased">
      {/* Top Application Header */}
      <Header
        device={device}
        battery={battery}
        isScanning={isScanning}
        onScan={scanDevices}
        onDisconnect={handleDisconnect}
        activeProfile={activeProfile}
        onSelectProfile={handleSelectProfile}
      />

      {/* Main App Workspace */}
      <div className="flex flex-1 overflow-hidden">
        {/* Navigation Sidebar */}
        <Sidebar
          activeTab={activeTab}
          onSelectTab={setActiveTab}
          pollingRateHz={pollingRate}
        />

        {/* Dynamic Content View */}
        <main className="flex-1 overflow-y-auto p-6 bg-gradient-to-b from-background to-card/30">
          {activeTab === 'dashboard' && (
            <DashboardView
              device={device}
              battery={battery}
              stats={stats}
              currentDpi={activeStage.dpi_x}
              dpiColor={dpiColor}
              pollingRateHz={pollingRate}
              onNavigate={setActiveTab}
              onRefreshBattery={handleRefreshBattery}
            />
          )}

          {activeTab === 'performance' && (
            <PerformanceView
              currentPollingRate={pollingRate}
              onSetPollingRate={handleSetPollingRate}
              dpiStages={dpiStages}
              activeStageIndex={activeStageIndex}
              onSelectActiveStage={handleSelectActiveStage}
              onUpdateDpiStage={handleUpdateDpiStage}
            />
          )}

          {activeTab === 'buttons' && (
            <ButtonsView
              buttonMappings={buttonMappings}
              onSaveButtonMapping={handleSaveButton}
            />
          )}

          {activeTab === 'sensor' && (
            <SensorView
              sensorConfig={sensorConfig}
              onSaveSensorConfig={handleSaveSensor}
            />
          )}

          {activeTab === 'pairing' && (
            <PairingView
              pairingStatus={pairingStatus}
              onStartPairing={handleStartPairing}
              onCancelPairing={handleCancelPairing}
            />
          )}

          {activeTab === 'diagnostics' && (
            <DiagnosticsView
              stats={stats}
              onRefreshStats={handleRefreshStats}
            />
          )}

          {activeTab === 'settings' && (
            <SettingsView
              config={config}
              onSaveConfig={handleSaveConfig}
            />
          )}
        </main>
      </div>
    </div>
  );
}

export default App;
