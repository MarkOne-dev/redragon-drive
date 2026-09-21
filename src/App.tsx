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
  DpiStageConfig, 
  ButtonAction, 
  SensorConfig, 
  PairingState, 
  AppConfig 
} from '@/types/mouse';

const DEFAULT_DPI_STAGES: DpiStageConfig[] = [
  { stage: 0, dpi_x: 400, dpi_y: 400, rgb: [255, 42, 77], enabled: true },
  { stage: 1, dpi_x: 800, dpi_y: 800, rgb: [0, 240, 255], enabled: true },
  { stage: 2, dpi_x: 1600, dpi_y: 1600, rgb: [50, 255, 126], enabled: true },
  { stage: 3, dpi_x: 3200, dpi_y: 3200, rgb: [255, 211, 42], enabled: true },
  { stage: 4, dpi_x: 6400, dpi_y: 6400, rgb: [156, 39, 176], enabled: true },
];

const DEFAULT_SENSOR_CONFIG: SensorConfig = {
  lod_height_mm: 1,
  motion_sync: true,
  debounce_ms: 8,
  ripple_control: false,
  angle_snapping: false,
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

  const [activeProfile, setActiveProfile] = useState<number>(() => {
    const saved = localStorage.getItem('rd_active_profile');
    return saved ? Number(saved) : 1;
  });

  const [pollingRate, setPollingRate] = useState<PollingRateHz>(1000);
  const [dpiStages, setDpiStages] = useState<DpiStageConfig[]>(() => {
    const saved = localStorage.getItem('rd_profile_1_dpi');
    if (saved) {
      try { return JSON.parse(saved); } catch {}
    }
    return DEFAULT_DPI_STAGES;
  });
  const [activeStageIndex, setActiveStageIndex] = useState<number>(2); // Default to 1600 DPI
  const [sensorConfig, setSensorConfig] = useState<SensorConfig>(DEFAULT_SENSOR_CONFIG);
  const [buttonMappings, setButtonMappings] = useState<Record<number, ButtonAction>>({});
  const [pairingStatus, setPairingStatus] = useState<PairingState>({ state: 'Idle' });
  const [config, setConfig] = useState<AppConfig>({
    auto_pair_on_insert: true,
    custom_pids: ['3554F55E', '3554F55D', '3554F55F', '3554F501', '35542635'],
    default_polling_rate: 1000,
    default_dpi_stage: 2,
    language: 'en',
  });

  const handleSelectProfile = (p: number) => {
    setActiveProfile(p);
    localStorage.setItem('rd_active_profile', String(p));
    const savedDpi = localStorage.getItem(`rd_profile_${p}_dpi`);
    if (savedDpi) {
      try {
        setDpiStages(JSON.parse(savedDpi));
      } catch {}
    }
  };

  // Scan for connected Compx / Redragon mice and hydrate full state
  const scanDevices = useCallback(async () => {
    setIsScanning(true);
    try {
      const devices = await mouseApi.scanDevices();
      if (devices && devices.length > 0) {
        const primary = devices[0];
        setDevice(primary);
        await mouseApi.connectDevice(primary.path);
        const fullState = await mouseApi.getDeviceFullState();
        if (fullState.battery) setBattery(fullState.battery);
        if (fullState.polling_rate) setPollingRate(fullState.polling_rate as PollingRateHz);
        if (fullState.sensor) setSensorConfig(fullState.sensor);
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
    setPollingRate(hz);
  };

  const handleUpdateDpiStage = async (stageIdx: number, dpiX: number, dpiY: number, rgb: [number, number, number]) => {
    await mouseApi.setDpi(stageIdx, dpiX, rgb[0], rgb[1], rgb[2]);
    setDpiStages((prev) => {
      const updated = prev.map((s, idx) => (idx === stageIdx ? { ...s, dpi_x: dpiX, dpi_y: dpiY, rgb } : s));
      localStorage.setItem(`rd_profile_${activeProfile}_dpi`, JSON.stringify(updated));
      return updated;
    });
  };

  const handleSaveButton = async (buttonIdx: number, action: ButtonAction) => {
    await mouseApi.setButton(buttonIdx, action);
    setButtonMappings((prev) => ({ ...prev, [buttonIdx]: action }));
  };

  const handleSaveSensor = async (newConfig: SensorConfig) => {
    await mouseApi.setSensor(newConfig);
    setSensorConfig(newConfig);
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
              onSelectActiveStage={setActiveStageIndex}
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
