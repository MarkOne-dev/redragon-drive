import React, { useState } from 'react';
import { 
  Gauge, 
  Crosshair, 
  Check, 
  Palette, 
  SlidersHorizontal,
  Info,
  Link,
  Unlink
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { PollingRateHz, DpiStageConfig } from '@/types/mouse';
import { useTranslation } from 'react-i18next';

interface PerformanceViewProps {
  currentPollingRate: PollingRateHz;
  onSetPollingRate: (hz: PollingRateHz) => Promise<void>;
  dpiStages: DpiStageConfig[];
  activeStageIndex: number;
  onSelectActiveStage: (index: number) => void;
  onUpdateDpiStage: (stageIndex: number, dpiX: number, dpiY: number, rgb: [number, number, number]) => Promise<void>;
}

const POLLING_RATES: { hz: PollingRateHz; latency: string; label: string }[] = [
  { hz: 125, latency: '8.0 ms', label: '125 Hz' },
  { hz: 250, latency: '4.0 ms', label: '250 Hz' },
  { hz: 500, latency: '2.0 ms', label: '500 Hz' },
  { hz: 1000, latency: '1.0 ms', label: '1000 Hz' },
  { hz: 2000, latency: '0.5 ms', label: '2000 Hz (4K/8K)' },
  { hz: 4000, latency: '0.25 ms', label: '4000 Hz (4K/8K)' },
  { hz: 8000, latency: '0.125 ms', label: '8000 Hz (8K)' },
];

const PRESET_COLORS: [number, number, number][] = [
  [255, 42, 77],   // Redragon Crimson
  [0, 240, 255],   // Cyan
  [50, 255, 126],  // Neon Green
  [255, 211, 42],  // Gold Yellow
  [156, 39, 176],  // Purple
  [255, 121, 63],  // Orange
  [255, 255, 255], // Pure White
];

export const PerformanceView: React.FC<PerformanceViewProps> = ({
  currentPollingRate,
  onSetPollingRate,
  dpiStages,
  activeStageIndex,
  onSelectActiveStage,
  onUpdateDpiStage,
}) => {
  const { t } = useTranslation();
  const [isApplyingRate, setIsApplyingRate] = useState(false);
  const [isApplyingDpi, setIsApplyingDpi] = useState(false);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  const activeStage = dpiStages[activeStageIndex] || dpiStages[0];
  const [isDecoupledXY, setIsDecoupledXY] = useState<boolean>(
    activeStage?.dpi_x !== activeStage?.dpi_y
  );
  const [localDpiX, setLocalDpiX] = useState<number>(activeStage?.dpi_x || 1600);
  const [localDpiY, setLocalDpiY] = useState<number>(activeStage?.dpi_y || 1600);
  const [localRgb, setLocalRgb] = useState<[number, number, number]>(activeStage?.rgb || [255, 42, 77]);

  // Sync with active stage when switching
  const handleStageSelect = (index: number) => {
    onSelectActiveStage(index);
    const stage = dpiStages[index];
    if (stage) {
      setLocalDpiX(stage.dpi_x);
      setLocalDpiY(stage.dpi_y);
      setIsDecoupledXY(stage.dpi_x !== stage.dpi_y);
      setLocalRgb(stage.rgb);
    }
  };

  const handleApplyPollingRate = async (hz: PollingRateHz) => {
    setIsApplyingRate(true);
    try {
      await onSetPollingRate(hz);
      setStatusMessage(`${t('performance.pollingRateTitle')}: ${hz} Hz`);
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Error: ${err?.message || err}`);
    } finally {
      setIsApplyingRate(false);
    }
  };

  const handleApplyDpi = async () => {
    setIsApplyingDpi(true);
    try {
      await onUpdateDpiStage(activeStageIndex, localDpiX, localDpiY, localRgb);
      setStatusMessage(`${t('performance.stage')} ${activeStageIndex + 1} (${localDpiX} DPI) - ${t('common.saved')}`);
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Error: ${err?.message || err}`);
    } finally {
      setIsApplyingDpi(false);
    }
  };

  const handleToggleDecouple = () => {
    const nextDecoupled = !isDecoupledXY;
    setIsDecoupledXY(nextDecoupled);
    if (!nextDecoupled) {
      setLocalDpiY(localDpiX);
    }
  };

  const handleDpiXChange = (val: number) => {
    setLocalDpiX(val);
    if (!isDecoupledXY) {
      setLocalDpiY(val);
    }
  };

  const handleSetDpiPreset = (preset: number) => {
    setLocalDpiX(preset);
    if (!isDecoupledXY) {
      setLocalDpiY(preset);
    }
  };

  return (
    <div className="space-y-6 max-w-5xl mx-auto select-none">
      {/* Status Alert Toast */}
      {statusMessage && (
        <div className="rounded-lg p-3 bg-red-500/10 border border-red-500/30 text-xs text-red-400 flex items-center justify-between animate-in fade-in">
          <span>{statusMessage}</span>
          <Button variant="ghost" size="xs" onClick={() => setStatusMessage(null)}>{t('common.dismiss')}</Button>
        </div>
      )}

      {/* SECTION 1: Polling Rate (Hz) */}
      <div className="rounded-xl border border-border/60 bg-card p-6 shadow-sm">
        <div className="flex items-center justify-between mb-4">
          <div>
            <h2 className="text-base font-bold text-foreground flex items-center gap-2">
              <Gauge className="size-4 text-red-500" />
              {t('performance.pollingRateTitle')}
            </h2>
            <p className="text-xs text-muted-foreground mt-0.5">
              {t('performance.pollingRateSubtitle')}
            </p>
          </div>
          <Badge variant="outline" className="text-xs font-mono border-red-500/40 text-red-400 bg-red-500/10">
            {t('performance.currentRate')}: {currentPollingRate} Hz
          </Badge>
        </div>

        <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-7 gap-2.5 mt-4">
          {POLLING_RATES.map((item) => {
            const isSelected = currentPollingRate === item.hz;
            return (
              <button
                key={item.hz}
                onClick={() => handleApplyPollingRate(item.hz)}
                disabled={isApplyingRate}
                className={`p-3 rounded-xl border flex flex-col items-center justify-center transition-all duration-150 relative ${
                  isSelected
                    ? 'border-red-500 bg-red-500/15 text-foreground shadow-[0_0_15px_rgba(239,68,68,0.25)]'
                    : 'border-border/60 bg-background/50 hover:bg-muted/60 text-muted-foreground hover:text-foreground'
                }`}
              >
                {isSelected && (
                  <span className="absolute top-1.5 right-1.5 size-2 bg-red-500 rounded-full" />
                )}
                <span className="font-bold font-mono text-sm tracking-tight">{item.label}</span>
                <span className="text-[10px] text-muted-foreground mt-0.5">{item.latency}</span>
              </button>
            );
          })}
        </div>

        <div className="mt-4 p-3 rounded-lg bg-background/40 border border-border/40 flex items-center gap-2 text-xs text-muted-foreground">
          <Info className="size-3.5 text-blue-400 shrink-0" />
          <span>
            <strong>1000 Hz (1.0ms)</strong> {t('performance.optimalRateNote')}
          </span>
        </div>
      </div>

      {/* SECTION 2: DPI Stages & PixArt PAW3395 Tuning */}
      <div className="rounded-xl border border-border/60 bg-card p-6 shadow-sm">
        <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4 mb-4">
          <div>
            <h2 className="text-base font-bold text-foreground flex items-center gap-2">
              <Crosshair className="size-4 text-red-500" />
              {t('performance.dpiStages')}
            </h2>
            <p className="text-xs text-muted-foreground mt-0.5">
              {t('performance.dpiStagesSubtitle')}
            </p>
          </div>

          <div className="flex items-center gap-2">
            <Button
              variant="outline"
              size="sm"
              onClick={handleToggleDecouple}
              className={`text-xs gap-1.5 ${isDecoupledXY ? 'border-red-500 text-red-400 bg-red-500/10' : ''}`}
            >
              {isDecoupledXY ? <Unlink className="size-3.5" /> : <Link className="size-3.5" />}
              {isDecoupledXY ? t('performance.sync') : t('performance.decouple')}
            </Button>

            <Button
              onClick={handleApplyDpi}
              disabled={isApplyingDpi}
              className="bg-red-600 hover:bg-red-700 text-white text-xs gap-1.5 shadow-sm shadow-red-600/30"
            >
              <Check className="size-3.5" />
              {isApplyingDpi ? t('common.saving') : t('common.save')}
            </Button>
          </div>
        </div>

        {/* Stage Selector Tabs */}
        <div className="flex items-center gap-2 overflow-x-auto pb-2 border-b border-border/40">
          {dpiStages.map((stage, idx) => {
            const isSelected = activeStageIndex === idx;
            const rgbColor = `rgb(${stage.rgb[0]}, ${stage.rgb[1]}, ${stage.rgb[2]})`;
            const isSplit = stage.dpi_x !== stage.dpi_y;

            return (
              <button
                key={idx}
                onClick={() => handleStageSelect(idx)}
                className={`px-3.5 py-2 rounded-lg border text-xs font-medium flex items-center gap-2 transition-all ${
                  isSelected
                    ? 'border-red-500/80 bg-red-500/10 text-foreground font-semibold shadow-xs'
                    : 'border-border/60 bg-background/50 hover:bg-muted text-muted-foreground'
                }`}
              >
                <span className="size-2.5 rounded-full border border-white/20" style={{ backgroundColor: rgbColor }} />
                <span>{t('performance.stage')} {idx + 1}</span>
                <span className="font-mono text-[11px] opacity-75">
                  {isSplit ? `(X:${stage.dpi_x}/Y:${stage.dpi_y})` : `(${stage.dpi_x})`}
                </span>
              </button>
            );
          })}
        </div>

        {/* Active Stage Details Editor */}
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6">
          {/* DPI Sliders & Values */}
          <div className="lg:col-span-2 space-y-5">
            {/* Axis X Slider */}
            <div className="space-y-2">
              <div className="flex items-center justify-between">
                <label className="text-xs font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                  <SlidersHorizontal className="size-3.5 text-red-400" />
                  {isDecoupledXY ? 'Axis X Sensitivity (Horizontal)' : `Stage ${activeStageIndex + 1} Sensitivity`}
                </label>
                <div className="flex items-center gap-2">
                  <input
                    type="number"
                    min={50}
                    max={26000}
                    step={50}
                    value={localDpiX}
                    onChange={(e) => handleDpiXChange(Number(e.target.value))}
                    className="w-24 px-2 py-1 bg-background border border-border rounded text-center font-mono font-bold text-sm text-foreground focus:outline-none focus:border-red-500"
                  />
                  <span className="text-xs font-mono text-muted-foreground">DPI</span>
                </div>
              </div>

              <input
                type="range"
                min={50}
                max={26000}
                step={50}
                value={localDpiX}
                onChange={(e) => handleDpiXChange(Number(e.target.value))}
                className="w-full accent-red-600 cursor-pointer h-2 bg-muted rounded-lg appearance-none"
              />
              <div className="flex justify-between text-[10px] text-muted-foreground font-mono">
                <span>50</span>
                <span>800</span>
                <span>1,600</span>
                <span>3,200</span>
                <span>6,400</span>
                <span>12,800</span>
                <span>26,000 DPI</span>
              </div>
            </div>

            {/* Axis Y Slider (when decoupled) */}
            {isDecoupledXY && (
              <div className="space-y-2 pt-2 border-t border-border/40">
                <div className="flex items-center justify-between">
                  <label className="text-xs font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
                    <SlidersHorizontal className="size-3.5 text-blue-400" />
                    Axis Y Sensitivity (Vertical)
                  </label>
                  <div className="flex items-center gap-2">
                    <input
                      type="number"
                      min={50}
                      max={26000}
                      step={50}
                      value={localDpiY}
                      onChange={(e) => setLocalDpiY(Number(e.target.value))}
                      className="w-24 px-2 py-1 bg-background border border-border rounded text-center font-mono font-bold text-sm text-foreground focus:outline-none focus:border-blue-500"
                    />
                    <span className="text-xs font-mono text-muted-foreground">DPI</span>
                  </div>
                </div>

                <input
                  type="range"
                  min={50}
                  max={26000}
                  step={50}
                  value={localDpiY}
                  onChange={(e) => setLocalDpiY(Number(e.target.value))}
                  className="w-full accent-blue-500 cursor-pointer h-2 bg-muted rounded-lg appearance-none"
                />
                <div className="flex justify-between text-[10px] text-muted-foreground font-mono">
                  <span>50</span>
                  <span>800</span>
                  <span>1,600</span>
                  <span>3,200</span>
                  <span>6,400</span>
                  <span>12,800</span>
                  <span>26,000 DPI</span>
                </div>
              </div>
            )}

            {/* Quick DPI Presets */}
            <div className="pt-2">
              <span className="text-[11px] text-muted-foreground mb-1.5 block">Quick Sensitivity Presets:</span>
              <div className="flex flex-wrap gap-1.5">
                {[400, 800, 1200, 1600, 2400, 3200, 6400, 12000, 26000].map((preset) => (
                  <Button
                    key={preset}
                    variant="outline"
                    size="xs"
                    onClick={() => handleSetDpiPreset(preset)}
                    className={`font-mono text-[11px] ${localDpiX === preset && (!isDecoupledXY || localDpiY === preset) ? 'border-red-500 text-red-400 bg-red-500/10' : ''}`}
                  >
                    {preset}
                  </Button>
                ))}
              </div>
            </div>
          </div>

          {/* RGB Color Selection for DPI Indicator */}
          <div className="rounded-xl border border-border/50 bg-background/50 p-4 space-y-3">
            <label className="text-xs font-semibold uppercase tracking-wider text-muted-foreground flex items-center gap-2">
              <Palette className="size-3.5 text-red-400" />
              Stage Indicator LED Color
            </label>

            <div className="flex items-center gap-3">
              <div 
                className="size-10 rounded-lg border border-white/20 shadow-md shrink-0 transition-colors"
                style={{ backgroundColor: `rgb(${localRgb[0]}, ${localRgb[1]}, ${localRgb[2]})` }}
              />
              <div className="font-mono text-xs text-foreground">
                RGB({localRgb[0]}, {localRgb[1]}, {localRgb[2]})
              </div>
            </div>

            <div className="pt-2">
              <span className="text-[11px] text-muted-foreground mb-2 block">Preset Swatches:</span>
              <div className="flex flex-wrap gap-2">
                {PRESET_COLORS.map((color, i) => (
                  <button
                    key={i}
                    onClick={() => setLocalRgb(color)}
                    className={`size-6 rounded-full border transition-transform hover:scale-110 ${
                      localRgb[0] === color[0] && localRgb[1] === color[1] && localRgb[2] === color[2]
                        ? 'ring-2 ring-white ring-offset-2 ring-offset-background scale-110'
                        : 'border-white/20'
                    }`}
                    style={{ backgroundColor: `rgb(${color[0]}, ${color[1]}, ${color[2]})` }}
                  />
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PerformanceView;
