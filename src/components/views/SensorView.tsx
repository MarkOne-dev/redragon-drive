import { useState } from 'react';
import { 
  Cpu, 
  Check, 
  Layers, 
  Zap, 
  Clock, 
  Activity
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { SensorConfig } from '@/types/mouse';
import { useTranslation } from 'react-i18next';

interface SensorViewProps {
  sensorConfig: SensorConfig;
  onSaveSensorConfig: (config: SensorConfig) => Promise<void>;
}

export const SensorView: React.FC<SensorViewProps> = ({
  sensorConfig,
  onSaveSensorConfig,
}) => {
  const { t } = useTranslation();
  const [config, setConfig] = useState<SensorConfig>(sensorConfig);
  const [isSaving, setIsSaving] = useState(false);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  const handleApply = async () => {
    setIsSaving(true);
    try {
      await onSaveSensorConfig(config);
      setStatusMessage(t('sensor.saveSuccess'));
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Error: ${err?.message || err}`);
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="space-y-6 max-w-5xl mx-auto select-none">
      {statusMessage && (
        <div className="rounded-lg p-3 bg-red-500/10 border border-red-500/30 text-xs text-red-400 flex items-center justify-between">
          <span>{statusMessage}</span>
          <Button variant="ghost" size="xs" onClick={() => setStatusMessage(null)}>{t('common.dismiss')}</Button>
        </div>
      )}

      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h2 className="text-base font-bold text-foreground flex items-center gap-2">
            <Cpu className="size-4 text-red-500" />
            {t('sensor.title')}
          </h2>
          <p className="text-xs text-muted-foreground mt-0.5">
            {t('sensor.subtitle')}
          </p>
        </div>
        <Button
          onClick={handleApply}
          disabled={isSaving}
          className="bg-red-600 hover:bg-red-700 text-white text-xs gap-1.5 shadow-sm shadow-red-600/30"
        >
          <Check className="size-3.5" />
          {isSaving ? t('common.saving') : t('sensor.saveButton')}
        </Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* LOD (Lift-off Distance) */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Layers className="size-4 text-red-500" />
              <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
                Lift-off Distance (LOD)
              </h3>
            </div>
            <Badge variant="outline" className="text-[10px] font-mono">
              {config.lod_height_mm} mm
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground">
            The height above the mouse pad at which the sensor stops tracking to prevent erratic crosshair shifts when repositioning.
          </p>
          <div className="grid grid-cols-2 gap-3 pt-2">
            <button
              onClick={() => setConfig({ ...config, lod_height_mm: 1 })}
              className={`p-3 rounded-lg border text-xs font-semibold flex flex-col items-center gap-1 transition-all ${
                config.lod_height_mm === 1
                  ? 'border-red-500 bg-red-500/15 text-foreground'
                  : 'border-border/60 bg-background/50 text-muted-foreground hover:bg-muted'
              }`}
            >
              <span>1.0 mm (Low)</span>
              <span className="text-[10px] text-muted-foreground font-normal">Fastest reset / esports</span>
            </button>
            <button
              onClick={() => setConfig({ ...config, lod_height_mm: 2 })}
              className={`p-3 rounded-lg border text-xs font-semibold flex flex-col items-center gap-1 transition-all ${
                config.lod_height_mm === 2
                  ? 'border-red-500 bg-red-500/15 text-foreground'
                  : 'border-border/60 bg-background/50 text-muted-foreground hover:bg-muted'
              }`}
            >
              <span>2.0 mm (High)</span>
              <span className="text-[10px] text-muted-foreground font-normal">Thick cloth pads</span>
            </button>
          </div>
        </div>

        {/* Motion Sync */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Zap className="size-4 text-red-500" />
              <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
                Motion Sync
              </h3>
            </div>
            <Badge variant="outline" className={`text-[10px] font-mono ${config.motion_sync ? 'text-emerald-400 border-emerald-500/40' : 'text-muted-foreground'}`}>
              {config.motion_sync ? 'ENABLED' : 'DISABLED'}
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground">
            Synchronizes sensor frames with USB polling cycles to provide seamless trajectory tracking with minimum variance.
          </p>
          <div className="flex items-center justify-between pt-3">
            <span className="text-xs font-medium text-foreground">Enable Hardware Motion Sync</span>
            <button
              onClick={() => setConfig({ ...config, motion_sync: !config.motion_sync })}
              className={`w-11 h-6 rounded-full transition-colors relative ${config.motion_sync ? 'bg-red-600' : 'bg-muted'}`}
            >
              <span 
                className={`absolute top-1 left-1 size-4 rounded-full bg-white transition-transform ${config.motion_sync ? 'translate-x-5' : ''}`}
              />
            </button>
          </div>
        </div>

        {/* Debounce Time */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Clock className="size-4 text-red-500" />
              <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
                Debounce Time (Anti-Bounce)
              </h3>
            </div>
            <Badge variant="outline" className="text-[10px] font-mono">
              {config.debounce_ms} ms
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground">
            Switch filter delay. Lower values yield instant click actuation, while higher values prevent unintentional double-clicking.
          </p>
          <div className="space-y-2 pt-2">
            <input
              type="range"
              min={4}
              max={20}
              step={2}
              value={config.debounce_ms}
              onChange={(e) => setConfig({ ...config, debounce_ms: Number(e.target.value) })}
              className="w-full accent-red-600 cursor-pointer h-2 bg-muted rounded-lg"
            />
            <div className="flex justify-between text-[10px] text-muted-foreground font-mono">
              <span>4 ms (Instant)</span>
              <span>8 ms (Standard)</span>
              <span>12 ms</span>
              <span>20 ms (Safe)</span>
            </div>
          </div>
        </div>

        {/* Ripple Control & Angle Snapping */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center gap-2">
            <Activity className="size-4 text-red-500" />
            <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
              Jitter & Angle Processing
            </h3>
          </div>

          <div className="space-y-3 pt-1">
            <div className="flex items-center justify-between">
              <div>
                <div className="text-xs font-medium text-foreground">Ripple Control</div>
                <div className="text-[10px] text-muted-foreground">Smoothes sensor noise at ultra-high DPI (&gt;10K)</div>
              </div>
              <button
                onClick={() => setConfig({ ...config, ripple_control: !config.ripple_control })}
                className={`w-11 h-6 rounded-full transition-colors relative ${config.ripple_control ? 'bg-red-600' : 'bg-muted'}`}
              >
                <span className={`absolute top-1 left-1 size-4 rounded-full bg-white transition-transform ${config.ripple_control ? 'translate-x-5' : ''}`} />
              </button>
            </div>

            <div className="flex items-center justify-between border-t border-border/40 pt-3">
              <div>
                <div className="text-xs font-medium text-foreground">Angle Snapping</div>
                <div className="text-[10px] text-muted-foreground">Linear trajectory correction (straight line snap)</div>
              </div>
              <button
                onClick={() => setConfig({ ...config, angle_snapping: !config.angle_snapping })}
                className={`w-11 h-6 rounded-full transition-colors relative ${config.angle_snapping ? 'bg-red-600' : 'bg-muted'}`}
              >
                <span className={`absolute top-1 left-1 size-4 rounded-full bg-white transition-transform ${config.angle_snapping ? 'translate-x-5' : ''}`} />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
