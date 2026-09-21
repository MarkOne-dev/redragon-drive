import { useState, useRef } from 'react';
import { 
  Activity, 
  MousePointerClick, 
  RotateCcw, 
  Gauge, 
  Move
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { MouseStats } from '@/types/mouse';
import { mouseApi } from '@/api/mouseApi';

interface DiagnosticsViewProps {
  stats: MouseStats;
  onRefreshStats: () => void;
}

export const DiagnosticsView: React.FC<DiagnosticsViewProps> = ({
  stats,
  onRefreshStats,
}) => {
  const [localClicks, setLocalClicks] = useState({
    left: 0,
    right: 0,
    middle: 0,
    sideBack: 0,
    sideForward: 0,
  });
  const [activeBtn, setActiveBtn] = useState<string | null>(null);
  const [liveTestHz, setLiveTestHz] = useState<number>(0);
  const [peakTestHz, setPeakTestHz] = useState<number>(0);

  // Moving-window rate calculation on canvas movement
  const lastMoveTimestamp = useRef<number>(0);
  const intervalsRef = useRef<number[]>([]);

  const handleMouseMove = (_e: React.MouseEvent) => {
    const now = performance.now();
    if (lastMoveTimestamp.current > 0) {
      const deltaMs = now - lastMoveTimestamp.current;
      if (deltaMs > 0 && deltaMs < 100) {
        const hz = Math.round(1000 / deltaMs);
        intervalsRef.current.push(hz);
        if (intervalsRef.current.length > 10) intervalsRef.current.shift();

        const avgHz = Math.round(
          intervalsRef.current.reduce((a, b) => a + b, 0) / intervalsRef.current.length
        );
        setLiveTestHz(avgHz);
        setPeakTestHz((prev) => Math.max(prev, avgHz));
      }
    }
    lastMoveTimestamp.current = now;
  };

  const handleMouseDown = (e: React.MouseEvent) => {
    e.preventDefault();
    if (e.button === 0) {
      setLocalClicks((p) => ({ ...p, left: p.left + 1 }));
      setActiveBtn('left');
    } else if (e.button === 1) {
      setLocalClicks((p) => ({ ...p, middle: p.middle + 1 }));
      setActiveBtn('middle');
    } else if (e.button === 2) {
      setLocalClicks((p) => ({ ...p, right: p.right + 1 }));
      setActiveBtn('right');
    } else if (e.button === 3) {
      setLocalClicks((p) => ({ ...p, sideBack: p.sideBack + 1 }));
      setActiveBtn('sideBack');
    } else if (e.button === 4) {
      setLocalClicks((p) => ({ ...p, sideForward: p.sideForward + 1 }));
      setActiveBtn('sideForward');
    }
  };

  const handleMouseUp = () => {
    setActiveBtn(null);
  };

  const handleReset = async () => {
    await mouseApi.resetMouseStats();
    setLocalClicks({ left: 0, right: 0, middle: 0, sideBack: 0, sideForward: 0 });
    setLiveTestHz(0);
    setPeakTestHz(0);
    intervalsRef.current = [];
    onRefreshStats();
  };

  return (
    <div className="space-y-6 max-w-6xl mx-auto select-none">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <div className="flex items-center gap-2">
            <h2 className="text-base font-bold text-foreground flex items-center gap-2">
              <Activity className="size-4 text-red-500" />
              Hardware Diagnostics & Real-time Benchmark
            </h2>
            <Badge variant="outline" className="text-[10px] font-mono">
              Driver: {stats.current_polling_rate} Hz
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground mt-0.5">
            Test button actuation switches, scroll encoder ticks, and measure actual USB report rates in real time.
          </p>
        </div>
        <Button
          variant="outline"
          size="sm"
          onClick={handleReset}
          className="text-xs gap-1.5 hover:text-red-400"
        >
          <RotateCcw className="size-3.5" />
          Reset Counters
        </Button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Testing Box 1: Interactive Click Test Pad */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-xs font-bold uppercase tracking-wider text-foreground flex items-center gap-2">
              <MousePointerClick className="size-4 text-red-500" />
              Microswitch Actuation Pad
            </h3>
            <Badge variant="outline" className="text-[10px]">
              Click inside box
            </Badge>
          </div>

          <div
            onMouseDown={handleMouseDown}
            onMouseUp={handleMouseUp}
            onContextMenu={(e) => e.preventDefault()}
            className="h-44 rounded-xl border-2 border-dashed border-border/80 bg-background/50 hover:bg-background/80 transition-colors flex flex-col items-center justify-center cursor-pointer relative overflow-hidden"
          >
            <div className="text-center space-y-1 z-10 pointer-events-none">
              <MousePointerClick className={`size-8 mx-auto transition-transform ${activeBtn ? 'scale-125 text-red-500' : 'text-muted-foreground'}`} />
              <div className="text-xs font-bold text-foreground">Click Anywhere in this Box</div>
              <div className="text-[10px] text-muted-foreground">Tests Left, Right, Middle & Side Buttons</div>
            </div>

            {/* Ripple visual indicator */}
            {activeBtn && (
              <div className="absolute inset-0 bg-red-500/10 pointer-events-none animate-pulse" />
            )}
          </div>

          {/* Click Counters Grid */}
          <div className="grid grid-cols-3 sm:grid-cols-5 gap-2 text-xs">
            <div className={`p-2.5 rounded-lg border text-center transition-all ${activeBtn === 'left' ? 'border-red-500 bg-red-500/20' : 'border-border/50 bg-background/50'}`}>
              <div className="text-[10px] text-muted-foreground">Left Click</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">{localClicks.left}</div>
            </div>
            <div className={`p-2.5 rounded-lg border text-center transition-all ${activeBtn === 'middle' ? 'border-red-500 bg-red-500/20' : 'border-border/50 bg-background/50'}`}>
              <div className="text-[10px] text-muted-foreground">Wheel Click</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">{localClicks.middle}</div>
            </div>
            <div className={`p-2.5 rounded-lg border text-center transition-all ${activeBtn === 'right' ? 'border-red-500 bg-red-500/20' : 'border-border/50 bg-background/50'}`}>
              <div className="text-[10px] text-muted-foreground">Right Click</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">{localClicks.right}</div>
            </div>
            <div className={`p-2.5 rounded-lg border text-center transition-all ${activeBtn === 'sideBack' ? 'border-red-500 bg-red-500/20' : 'border-border/50 bg-background/50'}`}>
              <div className="text-[10px] text-muted-foreground">Back (Btn 4)</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">{localClicks.sideBack}</div>
            </div>
            <div className={`p-2.5 rounded-lg border text-center transition-all ${activeBtn === 'sideForward' ? 'border-red-500 bg-red-500/20' : 'border-border/50 bg-background/50'}`}>
              <div className="text-[10px] text-muted-foreground">Fwd (Btn 5)</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">{localClicks.sideForward}</div>
            </div>
          </div>
        </div>

        {/* Testing Box 2: Polling Rate Live Benchmark Area */}
        <div className="rounded-xl border border-border/60 bg-card p-5 space-y-4">
          <div className="flex items-center justify-between">
            <h3 className="text-xs font-bold uppercase tracking-wider text-foreground flex items-center gap-2">
              <Gauge className="size-4 text-red-500" />
              Live Polling Rate Benchmark
            </h3>
            <Badge variant="outline" className="text-[10px] font-mono border-red-500/30 text-red-400">
              Peak: {peakTestHz} Hz
            </Badge>
          </div>

          <div
            onMouseMove={handleMouseMove}
            className="h-44 rounded-xl border-2 border-dashed border-red-500/40 bg-red-950/10 hover:bg-red-950/20 transition-colors flex flex-col items-center justify-center cursor-crosshair relative"
          >
            <Move className="size-6 text-red-500/60 mb-2 animate-bounce" />
            <div className="text-3xl font-black font-mono text-foreground tracking-tight">
              {liveTestHz} <span className="text-xs font-sans text-muted-foreground">Hz</span>
            </div>
            <div className="text-[10px] text-muted-foreground mt-1">Move mouse rapidly inside this box to benchmark</div>
          </div>

          {/* Benchmark Results */}
          <div className="grid grid-cols-2 gap-3 text-xs">
            <div className="p-3 rounded-lg border border-border/50 bg-background/50">
              <div className="text-[10px] text-muted-foreground uppercase font-semibold">Real-time Latency</div>
              <div className="font-mono font-bold text-base text-foreground mt-0.5">
                {liveTestHz > 0 ? (1000 / liveTestHz).toFixed(2) : '0.00'} ms
              </div>
            </div>
            <div className="p-3 rounded-lg border border-border/50 bg-background/50">
              <div className="text-[10px] text-muted-foreground uppercase font-semibold">Peak Rate Achieved</div>
              <div className="font-mono font-bold text-base text-red-400 mt-0.5">
                {peakTestHz} Hz
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
