import { useState } from 'react';
import { 
  MousePointerClick, 
  Flame, 
  Target, 
  Volume2, 
  Keyboard, 
  RefreshCw,
  Info
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { ButtonAction } from '@/types/mouse';

interface MouseButtonDef {
  id: number;
  name: string;
  defaultAction: string;
  location: string;
}

const MOUSE_BUTTONS: MouseButtonDef[] = [
  { id: 0, name: 'Button 1 (Left Click)', defaultAction: 'Primary Click', location: 'Top Left' },
  { id: 1, name: 'Button 2 (Right Click)', defaultAction: 'Secondary Click', location: 'Top Right' },
  { id: 2, name: 'Button 3 (Middle Click)', defaultAction: 'Scroll Click', location: 'Wheel' },
  { id: 3, name: 'Button 4 (Backward)', defaultAction: 'Browser Back', location: 'Side Lower' },
  { id: 4, name: 'Button 5 (Forward)', defaultAction: 'Browser Forward', location: 'Side Upper' },
  { id: 5, name: 'Button 6 (DPI Switch)', defaultAction: 'DPI Cycle', location: 'Top Center' },
  { id: 6, name: 'Button 7 (FireKey / Rapid)', defaultAction: 'Burst 3x Click', location: 'Auxiliary' },
  { id: 7, name: 'Button 8 (Sniper Lock)', defaultAction: '400 DPI Precision Lock', location: 'Thumb' },
];

const ACTION_CATEGORIES = [
  { id: 'standard', label: 'Standard Clicks', icon: MousePointerClick },
  { id: 'dpi', label: 'DPI Controls', icon: Target },
  { id: 'firekey', label: 'Rapid Fire (FireKey)', icon: Flame },
  { id: 'media', label: 'Multimedia Controls', icon: Volume2 },
  { id: 'shortcuts', label: 'Keyboard Shortcuts', icon: Keyboard },
  { id: 'system', label: 'System & Hz', icon: RefreshCw },
];

interface ButtonsViewProps {
  buttonMappings: Record<number, ButtonAction>;
  onSaveButtonMapping: (buttonIdx: number, action: ButtonAction) => Promise<void>;
}

export const ButtonsView: React.FC<ButtonsViewProps> = ({
  buttonMappings,
  onSaveButtonMapping,
}) => {
  const [selectedButtonId, setSelectedButtonId] = useState<number>(0);
  const [selectedCategory, setSelectedCategory] = useState<string>('standard');
  const [burstClicks, setBurstClicks] = useState<number>(3);
  const [burstInterval, setBurstInterval] = useState<number>(20);
  const [sniperDpi, setSniperDpi] = useState<number>(400);
  const [isSaving, setIsSaving] = useState<boolean>(false);
  const [statusMessage, setStatusMessage] = useState<string | null>(null);

  const selectedBtn = MOUSE_BUTTONS.find((b) => b.id === selectedButtonId) || MOUSE_BUTTONS[0];

  const handleApplyAction = async (action: ButtonAction, description: string) => {
    setIsSaving(true);
    try {
      await onSaveButtonMapping(selectedButtonId, action);
      setStatusMessage(`Button ${selectedButtonId + 1} remapped to: ${description}`);
      setTimeout(() => setStatusMessage(null), 3000);
    } catch (err: any) {
      setStatusMessage(`Error remapping button: ${err?.message || err}`);
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="space-y-6 max-w-6xl mx-auto select-none">
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
            <MousePointerClick className="size-4 text-red-500" />
            Redragon M916-PRO Button Remapping (0-15 Keys)
          </h2>
          <p className="text-xs text-muted-foreground mt-0.5">
            Customize physical buttons, assign rapid-fire bursts, sniper precision mode, or media shortcuts.
          </p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
        {/* Left Column: Button List & Wireframe diagram */}
        <div className="lg:col-span-5 rounded-xl border border-border/60 bg-card p-4 space-y-3">
          <div className="text-xs font-semibold uppercase tracking-wider text-muted-foreground px-2">
            Select Physical Button
          </div>

          <div className="space-y-1.5">
            {MOUSE_BUTTONS.map((btn) => {
              const isSelected = selectedButtonId === btn.id;
              const currentAction = buttonMappings[btn.id];

              return (
                <button
                  key={btn.id}
                  onClick={() => setSelectedButtonId(btn.id)}
                  className={`w-full p-3 rounded-lg border text-left flex items-center justify-between transition-all ${
                    isSelected
                      ? 'border-red-500 bg-red-500/15 text-foreground shadow-xs'
                      : 'border-border/50 bg-background/50 hover:bg-muted/60 text-muted-foreground'
                  }`}
                >
                  <div>
                    <div className="text-xs font-bold text-foreground">{btn.name}</div>
                    <div className="text-[10px] text-muted-foreground">{btn.location}</div>
                  </div>
                  <Badge variant="outline" className="text-[10px] font-mono">
                    {currentAction ? currentAction.type : btn.defaultAction}
                  </Badge>
                </button>
              );
            })}
          </div>

          <div className="p-3 rounded-lg bg-background/40 border border-border/40 text-[11px] text-muted-foreground flex items-center gap-2">
            <Info className="size-4 text-blue-400 shrink-0" />
            <span>Click any button above to customize its hardware opcode.</span>
          </div>
        </div>

        {/* Right Column: Action Selector */}
        <div className="lg:col-span-7 rounded-xl border border-border/60 bg-card p-5 space-y-5">
          <div className="flex items-center justify-between border-b border-border/40 pb-3">
            <div>
              <span className="text-xs text-muted-foreground uppercase font-semibold">Configuring Target</span>
              <h3 className="text-sm font-black text-foreground">{selectedBtn.name}</h3>
            </div>
            <Badge className="bg-red-600 text-white font-mono text-xs border-none">
              {isSaving ? 'Saving...' : `Index: #${selectedBtn.id}`}
            </Badge>
          </div>

          {/* Action Category Tabs */}
          <div className="flex flex-wrap gap-1.5">
            {ACTION_CATEGORIES.map((cat) => {
              const Icon = cat.icon;
              const isSelected = selectedCategory === cat.id;

              return (
                <button
                  key={cat.id}
                  onClick={() => setSelectedCategory(cat.id)}
                  className={`px-3 py-1.5 rounded-lg border text-xs font-medium flex items-center gap-1.5 transition-all ${
                    isSelected
                      ? 'border-red-500/80 bg-red-500/15 text-red-400 font-semibold'
                      : 'border-border/50 bg-background/50 hover:bg-muted text-muted-foreground'
                  }`}
                >
                  <Icon className="size-3.5" />
                  <span>{cat.label}</span>
                </button>
              );
            })}
          </div>

          {/* Category Details & Actions */}
          <div className="rounded-xl border border-border/50 bg-background/50 p-4 space-y-4">
            {selectedCategory === 'standard' && (
              <div className="grid grid-cols-2 gap-2 text-xs">
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Click', button: 1 }, 'Left Click')}>
                  Left Click (Button 1)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Click', button: 2 }, 'Right Click')}>
                  Right Click (Button 2)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Click', button: 4 }, 'Middle Click')}>
                  Middle Click (Wheel)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Click', button: 8 }, 'Browser Backward')}>
                  Backward (Button 4)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Click', button: 16 }, 'Browser Forward')}>
                  Forward (Button 5)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'None' }, 'Disabled (None)')} className="text-muted-foreground">
                  Disable Button
                </Button>
              </div>
            )}

            {selectedCategory === 'dpi' && (
              <div className="space-y-3">
                <div className="grid grid-cols-3 gap-2 text-xs">
                  <Button variant="outline" onClick={() => handleApplyAction({ type: 'Dpi', action: 'Cycle' }, 'DPI Loop')}>
                    DPI Cycle Loop
                  </Button>
                  <Button variant="outline" onClick={() => handleApplyAction({ type: 'Dpi', action: 'Up' }, 'DPI Up')}>
                    DPI Increase (+)
                  </Button>
                  <Button variant="outline" onClick={() => handleApplyAction({ type: 'Dpi', action: 'Down' }, 'DPI Down')}>
                    DPI Decrease (-)
                  </Button>
                </div>

                <div className="p-3 rounded-lg border border-border/60 bg-card space-y-2">
                  <div className="flex items-center justify-between text-xs">
                    <span className="font-semibold text-foreground flex items-center gap-1.5">
                      <Target className="size-3.5 text-red-500" />
                      Sniper Mode (Momentary Precision DPI Lock)
                    </span>
                    <span className="font-mono text-red-400 font-bold">{sniperDpi} DPI</span>
                  </div>
                  <input
                    type="range"
                    min={100}
                    max={3200}
                    step={50}
                    value={sniperDpi}
                    onChange={(e) => setSniperDpi(Number(e.target.value))}
                    className="w-full accent-red-600 cursor-pointer h-1.5 bg-muted rounded-lg"
                  />
                  <Button
                    size="sm"
                    className="w-full bg-red-600 hover:bg-red-700 text-white text-xs mt-1"
                    onClick={() => handleApplyAction({ type: 'SniperLock', dpi: sniperDpi }, `Sniper Lock (${sniperDpi} DPI)`)}
                  >
                    Assign Sniper Precision DPI
                  </Button>
                </div>
              </div>
            )}

            {selectedCategory === 'firekey' && (
              <div className="p-4 rounded-lg border border-red-500/30 bg-red-950/20 space-y-4">
                <div className="flex items-center gap-2 text-xs font-bold text-red-400">
                  <Flame className="size-4" />
                  Rapid FireKey Burst Settings
                </div>
                <div className="grid grid-cols-2 gap-4 text-xs">
                  <div>
                    <label className="text-muted-foreground block mb-1">Clicks per Burst</label>
                    <input
                      type="number"
                      min={2}
                      max={10}
                      value={burstClicks}
                      onChange={(e) => setBurstClicks(Number(e.target.value))}
                      className="w-full px-2 py-1 bg-background border border-border rounded font-mono font-bold text-foreground"
                    />
                  </div>
                  <div>
                    <label className="text-muted-foreground block mb-1">Interval (ms)</label>
                    <input
                      type="number"
                      min={5}
                      max={100}
                      step={5}
                      value={burstInterval}
                      onChange={(e) => setBurstInterval(Number(e.target.value))}
                      className="w-full px-2 py-1 bg-background border border-border rounded font-mono font-bold text-foreground"
                    />
                  </div>
                </div>
                <Button
                  size="sm"
                  className="w-full bg-red-600 hover:bg-red-700 text-white text-xs"
                  onClick={() => handleApplyAction({ type: 'FireKey', clicks: burstClicks, interval_ms: burstInterval }, `Rapid Fire ${burstClicks}x (${burstInterval}ms)`)}
                >
                  Assign Rapid FireKey
                </Button>
              </div>
            )}

            {selectedCategory === 'media' && (
              <div className="grid grid-cols-2 sm:grid-cols-3 gap-2 text-xs">
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'PlayPause' }, 'Play / Pause')}>
                  Play / Pause
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'VolumeUp' }, 'Volume Up')}>
                  Volume +
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'VolumeDown' }, 'Volume Down')}>
                  Volume -
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'Mute' }, 'Mute Audio')}>
                  Mute Audio
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'Next' }, 'Next Track')}>
                  Next Track
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Media', action: 'Previous' }, 'Previous Track')}>
                  Previous Track
                </Button>
              </div>
            )}

            {selectedCategory === 'shortcuts' && (
              <div className="grid grid-cols-2 gap-2 text-xs">
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Shortcut', modifiers: 1, key: 67 }, 'Copy (Ctrl+C)')}>
                  Copy (Ctrl+C)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Shortcut', modifiers: 1, key: 86 }, 'Paste (Ctrl+V)')}>
                  Paste (Ctrl+V)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Shortcut', modifiers: 1, key: 90 }, 'Undo (Ctrl+Z)')}>
                  Undo (Ctrl+Z)
                </Button>
                <Button variant="outline" onClick={() => handleApplyAction({ type: 'Shortcut', modifiers: 1, key: 83 }, 'Save (Ctrl+S)')}>
                  Save (Ctrl+S)
                </Button>
              </div>
            )}

            {selectedCategory === 'system' && (
              <div className="space-y-2 text-xs">
                <Button 
                  variant="outline" 
                  className="w-full justify-start"
                  onClick={() => handleApplyAction({ type: 'PollingRateCycle' }, 'Cycle Polling Rate')}
                >
                  Cycle Polling Rate (125Hz → 250Hz → 500Hz → 1000Hz)
                </Button>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
