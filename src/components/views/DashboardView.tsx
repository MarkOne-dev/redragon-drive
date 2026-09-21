import React from 'react';
import { 
  Gauge, 
  Battery, 
  Cpu, 
  Zap, 
  Timer, 
  Crosshair, 
  Wifi, 
  Usb,
  ShieldCheck
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { DeviceInfo, BatteryInfo, MouseStats } from '@/types/mouse';

interface DashboardViewProps {
  device: DeviceInfo | null;
  battery: BatteryInfo;
  stats: MouseStats;
  currentDpi: number;
  dpiColor: string;
  pollingRateHz: number;
  onNavigate: (tab: any) => void;
  onRefreshBattery: () => void;
}

export const DashboardView: React.FC<DashboardViewProps> = ({
  device,
  battery,
  stats,
  currentDpi,
  dpiColor,
  pollingRateHz,
  onNavigate,
  onRefreshBattery,
}) => {
  const latencyMs = (1000 / (pollingRateHz || 1000)).toFixed(2);

  return (
    <div className="space-y-6 max-w-6xl mx-auto">
      {/* Top Banner Hero */}
      <div className="relative overflow-hidden rounded-2xl border border-red-500/20 bg-gradient-to-r from-red-950/40 via-background to-background p-6 shadow-lg">
        <div className="absolute top-0 right-0 w-96 h-96 bg-red-600/10 rounded-full blur-3xl pointer-events-none" />
        
        <div className="flex flex-col md:flex-row items-start md:items-center justify-between gap-6 relative z-10">
          <div>
            <div className="flex items-center gap-2 mb-1.5">
              <Badge className="bg-red-600 text-white font-semibold text-xs border-none px-2.5 py-0.5">
                FLAGSHIP WIRELESS
              </Badge>
              <span className="text-xs text-muted-foreground font-mono">
                {device ? `VID: 0x${device.vid.toString(16).toUpperCase()} · PID: 0x${device.pid.toString(16).toUpperCase()}` : 'DISCONNECTED'}
              </span>
            </div>
            <h2 className="text-2xl font-black tracking-tight text-foreground uppercase">
              Redragon M916-PRO 1K Gaming Mouse
            </h2>
            <p className="text-xs text-muted-foreground mt-1 max-w-xl">
              Equipped with PixArt PAW3395 ultra-high-precision optical sensor and Compx CX52850P microcontroller with FastConnect 2.4GHz ultra-low-latency RF protocol.
            </p>
          </div>

          <div className="flex items-center gap-3">
            <Button
              onClick={() => onNavigate('performance')}
              className="bg-red-600 hover:bg-red-700 text-white font-medium text-xs shadow-md shadow-red-600/30"
            >
              Configure DPI & Hz
            </Button>
            <Button
              variant="outline"
              onClick={() => onNavigate('pairing')}
              className="border-border text-xs"
            >
              Pairing Tool
            </Button>
          </div>
        </div>
      </div>

      {/* Grid of Key Metrics */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        {/* Metric 1: Polling Rate & Latency */}
        <div className="rounded-xl border border-border/60 bg-card p-4 flex flex-col justify-between hover:border-red-500/40 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-medium uppercase tracking-wider">Report Rate</span>
            <Gauge className="size-4 text-red-500" />
          </div>
          <div className="my-3">
            <div className="text-3xl font-black text-foreground tracking-tight font-mono">
              {pollingRateHz} <span className="text-sm font-sans font-normal text-muted-foreground">Hz</span>
            </div>
            <div className="text-[11px] text-muted-foreground mt-0.5 flex items-center gap-1">
              <Timer className="size-3 text-red-400" />
              <span>Input Latency: <strong className="text-foreground font-mono">{latencyMs} ms</strong></span>
            </div>
          </div>
          <Button 
            variant="ghost" 
            size="xs" 
            onClick={() => onNavigate('performance')}
            className="w-full justify-start text-[11px] text-muted-foreground hover:text-red-400 p-0 h-auto"
          >
            Adjust rate →
          </Button>
        </div>

        {/* Metric 2: Active DPI */}
        <div className="rounded-xl border border-border/60 bg-card p-4 flex flex-col justify-between hover:border-red-500/40 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-medium uppercase tracking-wider">Active DPI</span>
            <Crosshair className="size-4" style={{ color: dpiColor }} />
          </div>
          <div className="my-3">
            <div className="text-3xl font-black text-foreground tracking-tight font-mono flex items-baseline gap-2">
              {currentDpi}
              <span className="size-3 rounded-full border border-white/20 shadow-xs" style={{ backgroundColor: dpiColor }} />
            </div>
            <div className="text-[11px] text-muted-foreground mt-0.5">
              <span>PixArt PAW3395 (50 - 26,000 DPI)</span>
            </div>
          </div>
          <Button 
            variant="ghost" 
            size="xs" 
            onClick={() => onNavigate('performance')}
            className="w-full justify-start text-[11px] text-muted-foreground hover:text-red-400 p-0 h-auto"
          >
            Configure stages →
          </Button>
        </div>

        {/* Metric 3: Battery Telemetry */}
        <div className="rounded-xl border border-border/60 bg-card p-4 flex flex-col justify-between hover:border-red-500/40 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-medium uppercase tracking-wider">Battery Power</span>
            <Battery className="size-4 text-emerald-400" />
          </div>
          <div className="my-3">
            <div className="text-3xl font-black text-foreground tracking-tight font-mono">
              {battery.percentage}%
            </div>
            <div className="text-[11px] text-muted-foreground mt-0.5 flex items-center gap-1">
              <Zap className="size-3 text-amber-400" />
              <span>Voltage: <strong className="text-foreground font-mono">{(battery.voltage_mv / 1000).toFixed(2)} V</strong></span>
            </div>
          </div>
          <Button 
            variant="ghost" 
            size="xs" 
            onClick={onRefreshBattery}
            className="w-full justify-start text-[11px] text-muted-foreground hover:text-red-400 p-0 h-auto"
          >
            Poll battery status ↻
          </Button>
        </div>

        {/* Metric 4: Connection Link */}
        <div className="rounded-xl border border-border/60 bg-card p-4 flex flex-col justify-between hover:border-red-500/40 transition-colors">
          <div className="flex items-center justify-between text-muted-foreground">
            <span className="text-xs font-medium uppercase tracking-wider">Connection Link</span>
            {device?.is_wired ? <Usb className="size-4 text-blue-400" /> : <Wifi className="size-4 text-emerald-400" />}
          </div>
          <div className="my-3">
            <div className="text-base font-bold text-foreground">
              {device ? (device.is_wired ? 'USB-C High Speed' : '2.4G RF FastConnect') : 'Disconnected'}
            </div>
            <div className="text-[11px] text-muted-foreground mt-0.5">
              <span>{device ? 'Device synchronized & active' : 'Plug dongle or mouse'}</span>
            </div>
          </div>
          <Button 
            variant="ghost" 
            size="xs" 
            onClick={() => onNavigate('diagnostics')}
            className="w-full justify-start text-[11px] text-muted-foreground hover:text-red-400 p-0 h-auto"
          >
            View diagnostics →
          </Button>
        </div>
      </div>

      {/* Hardware Architecture & Live Monitor Strip */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Sensor Specifications */}
        <div className="md:col-span-2 rounded-xl border border-border/60 bg-card p-5">
          <div className="flex items-center justify-between mb-4">
            <h3 className="text-sm font-bold text-foreground flex items-center gap-2">
              <Cpu className="size-4 text-red-500" />
              PixArt PAW3395 Hardware Architecture
            </h3>
            <Badge variant="outline" className="text-[10px] font-mono">
              CX52850P MCU
            </Badge>
          </div>

          <div className="grid grid-cols-2 sm:grid-cols-4 gap-3 text-xs">
            <div className="p-3 rounded-lg bg-background/50 border border-border/50">
              <div className="text-muted-foreground text-[10px] uppercase font-semibold">Max Resolution</div>
              <div className="font-bold font-mono text-sm text-foreground mt-0.5">26,000 DPI</div>
              <div className="text-[10px] text-muted-foreground">50 DPI steps</div>
            </div>
            <div className="p-3 rounded-lg bg-background/50 border border-border/50">
              <div className="text-muted-foreground text-[10px] uppercase font-semibold">Tracking Speed</div>
              <div className="font-bold font-mono text-sm text-foreground mt-0.5">650 IPS</div>
              <div className="text-[10px] text-muted-foreground">Max velocity</div>
            </div>
            <div className="p-3 rounded-lg bg-background/50 border border-border/50">
              <div className="text-muted-foreground text-[10px] uppercase font-semibold">Acceleration</div>
              <div className="font-bold font-mono text-sm text-foreground mt-0.5">50 G</div>
              <div className="text-[10px] text-muted-foreground">Instant flick response</div>
            </div>
            <div className="p-3 rounded-lg bg-background/50 border border-border/50">
              <div className="text-muted-foreground text-[10px] uppercase font-semibold">Lift-off Dist</div>
              <div className="font-bold font-mono text-sm text-foreground mt-0.5">1mm / 2mm</div>
              <div className="text-[10px] text-muted-foreground">Programmable LOD</div>
            </div>
          </div>

          <div className="mt-4 pt-4 border-t border-border/40 flex items-center justify-between text-xs">
            <span className="text-muted-foreground flex items-center gap-1.5">
              <ShieldCheck className="size-4 text-emerald-400" />
              Memory-safe driver communication with microcontroller timing protection
            </span>
            <Button
              variant="outline"
              size="sm"
              onClick={() => onNavigate('sensor')}
              className="text-xs"
            >
              Sensor Tuning
            </Button>
          </div>
        </div>

        {/* Quick Clicks Counter */}
        <div className="rounded-xl border border-border/60 bg-card p-5 flex flex-col justify-between">
          <div>
            <h3 className="text-sm font-bold text-foreground mb-3 flex items-center gap-2">
              <Timer className="size-4 text-red-500" />
              Session Telemetry
            </h3>
            
            <div className="space-y-2 text-xs">
              <div className="flex justify-between py-1 border-b border-border/40">
                <span className="text-muted-foreground">Left Clicks</span>
                <span className="font-mono font-semibold text-foreground">{stats.left_clicks.toLocaleString()}</span>
              </div>
              <div className="flex justify-between py-1 border-b border-border/40">
                <span className="text-muted-foreground">Right Clicks</span>
                <span className="font-mono font-semibold text-foreground">{stats.right_clicks.toLocaleString()}</span>
              </div>
              <div className="flex justify-between py-1 border-b border-border/40">
                <span className="text-muted-foreground">Scroll Delta</span>
                <span className="font-mono font-semibold text-foreground">{stats.wheel_delta_y.toLocaleString()}</span>
              </div>
              <div className="flex justify-between py-1">
                <span className="text-muted-foreground">Peak Polling Rate</span>
                <span className="font-mono font-semibold text-red-400">{stats.max_polling_rate} Hz</span>
              </div>
            </div>
          </div>

          <Button
            variant="outline"
            size="sm"
            onClick={() => onNavigate('diagnostics')}
            className="w-full mt-4 text-xs"
          >
            Launch Diagnostics Monitor
          </Button>
        </div>
      </div>
    </div>
  );
};
