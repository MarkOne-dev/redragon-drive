import React, { useState } from 'react';
import { 
  MousePointerClick, 
  Flame, 
  Target, 
  Volume2, 
  Keyboard, 
  RefreshCw,
  Info,
  ScrollText,
  Plus,
  Trash2,
  Sliders
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { ButtonAction } from '@/types/mouse';

interface MouseButtonDef {
  id: number;
  name: string;
  defaultAction: string;
  location: string;
  hotspot: { x: number; y: number }; // Percentage position on the mouse graphic
}

const MOUSE_BUTTONS: MouseButtonDef[] = [
  { id: 0, name: 'Button 1 (Left Click)', defaultAction: 'Primary Click', location: 'Main Left', hotspot: { x: 37.6, y: 78.6 } },
  { id: 1, name: 'Button 2 (Right Click)', defaultAction: 'Secondary Click', location: 'Main Right', hotspot: { x: 10.0, y: 66.4 } },
  { id: 2, name: 'Button 3 (Middle Click)', defaultAction: 'Scroll Click', location: 'Scroll Wheel', hotspot: { x: 30.5, y: 48.7 } },
  { id: 3, name: 'Button 4 (Forward)', defaultAction: 'Browser Forward', location: 'Side Front', hotspot: { x: 69.9, y: 57.5 } },
  { id: 4, name: 'Button 5 (Backward)', defaultAction: 'Browser Back', location: 'Side Rear', hotspot: { x: 79.2, y: 43.1 } },
  { id: 5, name: 'Button 6 (DPI Switch)', defaultAction: 'DPI Cycle', location: 'Underside (Base)', hotspot: { x: 63.1, y: 86.2 } },
];

const ACTION_CATEGORIES = [
  { id: 'standard', label: 'Standard Clicks', icon: MousePointerClick },
  { id: 'dpi', label: 'DPI Controls', icon: Target },
  { id: 'firekey', label: 'Rapid Fire (FireKey)', icon: Flame },
  { id: 'macro', label: 'Macro Sequence', icon: ScrollText },
  { id: 'media', label: 'Multimedia Controls', icon: Volume2 },
  { id: 'shortcuts', label: 'Keyboard Shortcuts', icon: Keyboard },
  { id: 'system', label: 'System & Hz', icon: RefreshCw },
];

interface MacroStep {
  key: string;
  delayMs: number;
}

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

  // Macro Editor State
  const [selectedMacroId, setSelectedMacroId] = useState<number>(1);
  const [macroLoopCount, setMacroLoopCount] = useState<number>(1);
  const [macroSteps, setMacroSteps] = useState<MacroStep[]>([
    { key: 'W', delayMs: 50 },
    { key: 'Shift', delayMs: 50 },
    { key: 'Space', delayMs: 100 },
  ]);
  const [newKey, setNewKey] = useState<string>('Q');
  const [newDelay, setNewDelay] = useState<number>(50);

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

  const handleAddMacroStep = () => {
    if (!newKey.trim()) return;
    setMacroSteps((prev) => [...prev, { key: newKey.trim().toUpperCase(), delayMs: newDelay }]);
  };

  const handleRemoveMacroStep = (index: number) => {
    setMacroSteps((prev) => prev.filter((_, i) => i !== index));
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
            Redragon M916-PRO Button Remapping Suite
          </h2>
          <p className="text-xs text-muted-foreground mt-0.5">
            Interactive hardware layout matching factory firmware. Click the mouse diagram or list to reassign opcodes.
          </p>
        </div>
      </div>

      {/* TOP: Interactive Mouse Diagram */}
      <div className="rounded-2xl border border-border/60 bg-gradient-to-b from-card/80 to-card/30 p-6 flex flex-col md:flex-row items-center justify-between gap-6 shadow-sm">
        <div className="space-y-2 text-center md:text-left">
          <Badge className="bg-red-600 text-white font-semibold text-xs border-none px-2.5 py-0.5">
            CHASSIS SCHEMATIC
          </Badge>
          <h3 className="text-lg font-black text-foreground uppercase tracking-tight">
            M916-PRO Microswitch Matrix
          </h3>
          <p className="text-xs text-muted-foreground max-w-sm">
            6 fully programmable physical switch locations with zero-bounce debounce filters and instant microcode dispatch.
          </p>
          <div className="flex items-center gap-2 pt-2 text-xs text-muted-foreground">
            <span className="size-3 rounded-full bg-red-500 animate-pulse" />
            <span>Active Target: <strong>{selectedBtn.name}</strong></span>
          </div>
        </div>

        {/* Visual Mouse Chassis with Hotspot Badges */}
        <div className="relative w-full max-w-md aspect-[442/318] flex items-center justify-center bg-background/40 rounded-xl border border-border/40 p-4">
          <img 
            src="/assets/mouse-m916.png" 
            alt="Redragon M916-PRO Mouse Chassis" 
            className="w-full h-full object-contain filter drop-shadow-[0_10px_20px_rgba(0,0,0,0.5)] select-none pointer-events-none"
          />

          {/* Interactive Button Hotspots */}
          {MOUSE_BUTTONS.map((btn) => {
            const isSelected = selectedButtonId === btn.id;
            return (
              <div
                key={btn.id}
                style={{ top: `${btn.hotspot.y}%`, left: `${btn.hotspot.x}%` }}
                className="absolute -translate-x-1/2 -translate-y-1/2 flex items-center gap-1.5 z-10"
              >
                <button
                  onClick={() => setSelectedButtonId(btn.id)}
                  title={`${btn.name} (${btn.location})`}
                  className={`size-7 rounded-full font-mono text-xs font-bold transition-all duration-200 flex items-center justify-center shadow-lg ${
                    isSelected
                      ? 'bg-red-600 text-white scale-125 ring-4 ring-red-500/40 z-20 shadow-[0_0_15px_rgba(239,68,68,0.8)]'
                      : 'bg-card/90 text-foreground border border-border/80 hover:bg-red-500/20 hover:text-red-400 hover:scale-110'
                  }`}
                >
                  {btn.id + 1}
                </button>
                {btn.id === 5 && (
                  <span className="text-[9px] font-mono font-bold bg-background/90 text-red-400 border border-red-500/30 px-1.5 py-0.5 rounded shadow-xs whitespace-nowrap">
                    Base DPI
                  </span>
                )}
              </div>
            );
          })}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-12 gap-6">
        {/* Left Column: Button List */}
        <div className="lg:col-span-5 rounded-xl border border-border/60 bg-card p-4 space-y-3">
          <div className="text-xs font-semibold uppercase tracking-wider text-muted-foreground px-2">
            Physical Switch List
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
                  <div className="flex items-center gap-2.5">
                    <span className={`size-5 rounded-full flex items-center justify-center font-mono text-[10px] font-bold ${
                      isSelected ? 'bg-red-600 text-white' : 'bg-muted text-muted-foreground'
                    }`}>
                      {btn.id + 1}
                    </span>
                    <div>
                      <div className="text-xs font-bold text-foreground">{btn.name}</div>
                      <div className="text-[10px] text-muted-foreground">{btn.location}</div>
                    </div>
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
            <span>Click any numbered hotspot or switch item to reassign.</span>
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
              {isSaving ? 'Saving...' : `Switch #${selectedBtn.id + 1}`}
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
                      ? 'border-red-500/80 bg-red-500/15 text-red-400 font-semibold shadow-xs'
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

            {selectedCategory === 'macro' && (
              <div className="space-y-4">
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <ScrollText className="size-4 text-red-500" />
                    <span className="text-xs font-bold text-foreground">Hardware Macro Sequencer</span>
                  </div>
                  <div className="flex items-center gap-2">
                    <span className="text-[11px] text-muted-foreground">Select Macro:</span>
                    <select
                      value={selectedMacroId}
                      onChange={(e) => setSelectedMacroId(Number(e.target.value))}
                      className="bg-background border border-border rounded px-2 py-1 text-xs font-mono text-foreground"
                    >
                      {[1, 2, 3, 4, 5, 6, 7, 8].map((id) => (
                        <option key={id} value={id}>Macro #{id}</option>
                      ))}
                    </select>
                  </div>
                </div>

                {/* Macro Steps Sequence */}
                <div className="space-y-1.5">
                  <div className="text-[10px] text-muted-foreground uppercase font-semibold">Key Sequence Pipeline</div>
                  <div className="space-y-1 max-h-36 overflow-y-auto p-2 rounded-lg bg-background border border-border/60">
                    {macroSteps.map((step, idx) => (
                      <div key={idx} className="flex items-center justify-between py-1 px-2 rounded bg-card/60 border border-border/40 text-xs">
                        <div className="flex items-center gap-2">
                          <Badge variant="outline" className="font-mono text-[10px]">Step {idx + 1}</Badge>
                          <span className="font-bold text-foreground font-mono">[{step.key}]</span>
                          <span className="text-muted-foreground text-[10px]">Delay: {step.delayMs}ms</span>
                        </div>
                        <button
                          onClick={() => handleRemoveMacroStep(idx)}
                          className="text-muted-foreground hover:text-red-400 p-0.5"
                          title="Remove Step"
                        >
                          <Trash2 className="size-3" />
                        </button>
                      </div>
                    ))}
                  </div>
                </div>

                {/* Add Step Controls */}
                <div className="grid grid-cols-12 gap-2 pt-1 text-xs items-center">
                  <div className="col-span-5">
                    <input
                      type="text"
                      placeholder="Key (e.g. W, Space, F)"
                      value={newKey}
                      maxLength={10}
                      onChange={(e) => setNewKey(e.target.value)}
                      className="w-full px-2 py-1 bg-background border border-border rounded text-xs font-mono text-foreground"
                    />
                  </div>
                  <div className="col-span-4 flex items-center gap-1">
                    <input
                      type="number"
                      min={10}
                      max={1000}
                      step={10}
                      value={newDelay}
                      onChange={(e) => setNewDelay(Number(e.target.value))}
                      className="w-full px-2 py-1 bg-background border border-border rounded text-xs font-mono text-foreground"
                    />
                    <span className="text-[10px] text-muted-foreground font-mono">ms</span>
                  </div>
                  <div className="col-span-3">
                    <Button size="sm" variant="outline" onClick={handleAddMacroStep} className="w-full text-xs gap-1">
                      <Plus className="size-3" /> Add
                    </Button>
                  </div>
                </div>

                {/* Loop Count & Apply */}
                <div className="flex items-center justify-between pt-2 border-t border-border/40">
                  <div className="flex items-center gap-2 text-xs">
                    <Sliders className="size-3.5 text-muted-foreground" />
                    <span className="text-muted-foreground">Execution Loop:</span>
                    <input
                      type="number"
                      min={1}
                      max={99}
                      value={macroLoopCount}
                      onChange={(e) => setMacroLoopCount(Number(e.target.value))}
                      className="w-14 px-1.5 py-0.5 bg-background border border-border rounded font-mono text-center text-xs"
                    />
                    <span className="text-[11px] text-muted-foreground">times</span>
                  </div>

                  <Button
                    size="sm"
                    className="bg-red-600 hover:bg-red-700 text-white text-xs"
                    onClick={() => handleApplyAction({ type: 'Macro', macro_id: selectedMacroId, loop_count: macroLoopCount }, `Macro #${selectedMacroId} (${macroLoopCount}x)`)}
                  >
                    Assign Macro #{selectedMacroId}
                  </Button>
                </div>
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
                <Button 
                  variant="outline" 
                  className="w-full justify-start"
                  onClick={() => handleApplyAction({ type: 'ProfileSwitch' }, 'Toggle On-Board Profile')}
                >
                  Toggle On-Board Profile (Profile 1 ↔ Profile 2 ↔ Profile 3)
                </Button>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ButtonsView;
