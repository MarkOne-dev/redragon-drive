import { useState } from 'react';
import { 
  Settings, 
  Plus, 
  Trash2, 
  ShieldCheck, 
  Save
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import type { AppConfig } from '@/types/mouse';
import { mouseApi } from '@/api/mouseApi';

interface SettingsViewProps {
  config: AppConfig;
  onSaveConfig: (config: AppConfig) => Promise<void>;
}

export const SettingsView: React.FC<SettingsViewProps> = ({
  config,
  onSaveConfig,
}) => {
  const [localConfig, setLocalConfig] = useState<AppConfig>(config);
  const [newPid, setNewPid] = useState('');
  const [isSaving, setIsSaving] = useState(false);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  const handleAddPid = async () => {
    const trimmed = newPid.trim().toUpperCase();
    if (!trimmed || trimmed.length !== 8) {
      setStatusMessage('PID must be an 8-character hex string (e.g. 3554F55E)');
      return;
    }

    if (localConfig.custom_pids.includes(trimmed)) {
      setStatusMessage('PID is already registered.');
      return;
    }

    const updated = {
      ...localConfig,
      custom_pids: [...localConfig.custom_pids, trimmed],
    };
    setLocalConfig(updated);
    setNewPid('');
    try {
      await mouseApi.addCustomPid(trimmed);
      setStatusMessage(`Custom PID ${trimmed} added successfully.`);
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Error adding PID: ${err?.message || err}`);
    }
  };

  const handleRemovePid = (pidToRemove: string) => {
    setLocalConfig({
      ...localConfig,
      custom_pids: localConfig.custom_pids.filter((p) => p !== pidToRemove),
    });
  };

  const handleSave = async () => {
    setIsSaving(true);
    try {
      await onSaveConfig(localConfig);
      setStatusMessage('Configuration preferences saved successfully.');
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Failed to save settings: ${err?.message || err}`);
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto select-none">
      {statusMessage && (
        <div className="rounded-lg p-3 bg-red-500/10 border border-red-500/30 text-xs text-red-400 flex items-center justify-between">
          <span>{statusMessage}</span>
          <Button variant="ghost" size="xs" onClick={() => setStatusMessage(null)}>Dismiss</Button>
        </div>
      )}

      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-base font-bold text-foreground flex items-center gap-2">
            <Settings className="size-4 text-red-500" />
            Application Settings & Hardware PIDs
          </h2>
          <p className="text-xs text-muted-foreground mt-0.5">
            Manage device identification numbers, auto-connection policies, and persistence.
          </p>
        </div>
        <Button
          onClick={handleSave}
          disabled={isSaving}
          className="bg-red-600 hover:bg-red-700 text-white text-xs gap-1.5 shadow-sm shadow-red-600/30"
        >
          <Save className="size-3.5" />
          {isSaving ? 'Saving...' : 'Save Settings'}
        </Button>
      </div>

      {/* General Settings Card */}
      <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
        <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
          General Preferences
        </h3>

        <div className="space-y-3">
          <div className="flex items-center justify-between py-2 border-b border-border/40">
            <div>
              <div className="text-xs font-medium text-foreground">Auto-Pair on Dongle Insertion</div>
              <div className="text-[10px] text-muted-foreground">Automatically trigger 2.4G sync when receiver is detected</div>
            </div>
            <button
              onClick={() => setLocalConfig({ ...localConfig, auto_pair_on_insert: !localConfig.auto_pair_on_insert })}
              className={`w-11 h-6 rounded-full transition-colors relative ${localConfig.auto_pair_on_insert ? 'bg-red-600' : 'bg-muted'}`}
            >
              <span className={`absolute top-1 left-1 size-4 rounded-full bg-white transition-transform ${localConfig.auto_pair_on_insert ? 'translate-x-5' : ''}`} />
            </button>
          </div>

          <div className="flex items-center justify-between py-2">
            <div>
              <div className="text-xs font-medium text-foreground">Default Startup Polling Rate</div>
              <div className="text-[10px] text-muted-foreground">Applied automatically on device initialization</div>
            </div>
            <select
              value={localConfig.default_polling_rate}
              onChange={(e) => setLocalConfig({ ...localConfig, default_polling_rate: Number(e.target.value) })}
              className="bg-background border border-border rounded px-2 py-1 text-xs font-mono text-foreground"
            >
              <option value={125}>125 Hz</option>
              <option value={250}>250 Hz</option>
              <option value={500}>500 Hz</option>
              <option value={1000}>1000 Hz</option>
              <option value={2000}>2000 Hz</option>
              <option value={4000}>4000 Hz</option>
              <option value={8000}>8000 Hz</option>
            </select>
          </div>
        </div>
      </div>

      {/* Known / Custom PIDs Card */}
      <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
        <div className="flex items-center justify-between">
          <div>
            <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
              Registered Hardware PIDs (VID: 0x3554)
            </h3>
            <p className="text-[11px] text-muted-foreground mt-0.5">
              USB Vendor/Product identifiers recognized by the Compx detector engine.
            </p>
          </div>
        </div>

        {/* Input to add PID */}
        <div className="flex items-center gap-2">
          <input
            type="text"
            placeholder="e.g. 3554F55E"
            value={newPid}
            maxLength={8}
            onChange={(e) => setNewPid(e.target.value)}
            className="flex-1 px-3 py-1.5 bg-background border border-border rounded-lg text-xs font-mono uppercase text-foreground focus:outline-none focus:border-red-500"
          />
          <Button size="sm" onClick={handleAddPid} className="text-xs gap-1 bg-red-600 hover:bg-red-700 text-white">
            <Plus className="size-3.5" />
            Add PID
          </Button>
        </div>

        {/* List of Registered PIDs */}
        <div className="grid grid-cols-2 sm:grid-cols-3 gap-2 pt-2">
          {localConfig.custom_pids.map((pid) => (
            <div
              key={pid}
              className="p-2.5 rounded-lg border border-border/50 bg-background/50 flex items-center justify-between text-xs"
            >
              <span className="font-mono font-semibold text-foreground">{pid}</span>
              <button
                onClick={() => handleRemovePid(pid)}
                className="text-muted-foreground hover:text-red-400 p-1"
                title="Remove PID"
              >
                <Trash2 className="size-3.5" />
              </button>
            </div>
          ))}
        </div>
      </div>

      {/* System & Architecture Info */}
      <div className="rounded-xl border border-border/50 bg-background/40 p-4 text-xs text-muted-foreground space-y-1">
        <div className="flex items-center gap-2 text-foreground font-semibold">
          <ShieldCheck className="size-4 text-emerald-400" />
          Redragon M916-PRO Native Backend
        </div>
        <p className="text-[11px]">
          Rust Backend (Tauri v2) · Memory-safe HIDAPI with report buffer length enforcement and non-blocking multi-threaded device monitoring.
        </p>
      </div>
    </div>
  );
};
