import React from 'react';
import { 
  Wifi, 
  Usb, 
  Battery, 
  BatteryCharging, 
  RefreshCw, 
  PowerOff,
  Sun,
  Moon
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import type { DeviceInfo, BatteryInfo } from '@/types/mouse';
import { useTheme } from '@/components/theme-provider';

interface HeaderProps {
  device: DeviceInfo | null;
  battery: BatteryInfo;
  isScanning: boolean;
  onScan: () => void;
  onDisconnect: () => void;
  activeProfile: number;
  onSelectProfile: (profile: number) => void;
}

export const Header: React.FC<HeaderProps> = ({
  device,
  battery,
  isScanning,
  onScan,
  onDisconnect,
  activeProfile,
  onSelectProfile,
}) => {
  const { theme, setTheme } = useTheme();

  return (
    <header className="h-16 border-b border-border/50 bg-card/60 backdrop-blur-md px-6 flex items-center justify-between sticky top-0 z-40">
      {/* Brand & Model */}
      <div className="flex items-center gap-3">
        <div className="size-9 rounded-lg bg-red-600/20 border border-red-500/40 flex items-center justify-center shadow-[0_0_15px_rgba(239,68,68,0.3)]">
          <span className="text-red-500 font-black text-lg tracking-tighter">RD</span>
        </div>
        <div>
          <div className="flex items-center gap-2">
            <h1 className="font-bold tracking-wide text-foreground text-sm uppercase">
              Redragon M916-PRO
            </h1>
            <Badge variant="outline" className="text-[10px] px-1.5 py-0 border-red-500/40 text-red-400 bg-red-500/10">
              1K PRO
            </Badge>
          </div>
          <p className="text-xs text-muted-foreground">PixArt PAW3395 · CX52850P MCU</p>
        </div>
      </div>

      {/* Center Device Status */}
      <div className="flex items-center gap-4">
        {device ? (
          <div className="flex items-center gap-2.5 px-3 py-1.5 rounded-full bg-emerald-500/10 border border-emerald-500/30 text-xs">
            <span className="relative flex size-2">
              <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span>
              <span className="relative inline-flex rounded-full size-2 bg-emerald-500"></span>
            </span>
            <span className="font-medium text-emerald-400 flex items-center gap-1.5">
              {device.is_wired ? <Usb className="size-3.5" /> : <Wifi className="size-3.5" />}
              {device.is_wired ? 'USB-C Wired Mode' : '2.4GHz Wireless Dongle'}
            </span>
            <span className="text-muted-foreground">|</span>
            <span className="text-xs text-muted-foreground font-mono">
              PID: 0x{device.pid.toString(16).toUpperCase()}
            </span>
          </div>
        ) : (
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-full bg-muted/40 border border-border text-xs text-muted-foreground">
            <span className="size-2 rounded-full bg-muted-foreground/40" />
            <span>No Device Connected</span>
          </div>
        )}

        {/* Battery Telemetry */}
        {device && (
          <div className="flex items-center gap-2 px-3 py-1.5 rounded-lg bg-card border border-border/80 text-xs">
            {battery.is_charging ? (
              <BatteryCharging className="size-4 text-amber-400 animate-pulse" />
            ) : (
              <Battery className={`size-4 ${battery.percentage <= 20 ? 'text-red-400' : 'text-emerald-400'}`} />
            )}
            <div className="flex items-center gap-1.5">
              <span className="font-semibold text-foreground">{battery.percentage}%</span>
              {battery.voltage_mv > 0 && (
                <span className="text-[10px] text-muted-foreground font-mono">
                  ({(battery.voltage_mv / 1000).toFixed(2)}V)
                </span>
              )}
            </div>
            {/* Battery bar indicator */}
            <div className="w-12 h-1.5 rounded-full bg-muted overflow-hidden ml-1">
              <div 
                className={`h-full transition-all duration-300 ${battery.percentage <= 20 ? 'bg-red-500' : 'bg-emerald-500'}`}
                style={{ width: `${Math.min(100, Math.max(5, battery.percentage))}%` }}
              />
            </div>
          </div>
        )}
      </div>

      {/* Quick Controls & Profile Switcher */}
      <div className="flex items-center gap-3">
        {/* Profile Pill Switcher */}
        <div className="flex items-center gap-1 bg-background/60 border border-border/80 rounded-lg p-0.5 text-xs">
          <span className="text-[10px] text-muted-foreground uppercase font-semibold px-1.5 font-mono">Profile</span>
          {[1, 2, 3].map((p) => (
            <button
              key={p}
              onClick={() => onSelectProfile(p)}
              className={`px-2 py-0.5 rounded text-[11px] font-mono font-bold transition-all ${
                activeProfile === p
                  ? 'bg-red-600 text-white shadow-xs'
                  : 'text-muted-foreground hover:text-foreground hover:bg-muted/50'
              }`}
            >
              P{p}
            </button>
          ))}
        </div>

        <Button
          variant="outline"
          size="sm"
          onClick={onScan}
          disabled={isScanning}
          className="gap-1.5 text-xs border-border/60 hover:border-red-500/40"
        >
          <RefreshCw className={`size-3.5 ${isScanning ? 'animate-spin text-red-500' : ''}`} />
          {isScanning ? 'Scanning...' : 'Scan USB'}
        </Button>

        {device && (
          <Button
            variant="ghost"
            size="icon-sm"
            onClick={onDisconnect}
            title="Disconnect device"
            className="text-muted-foreground hover:text-destructive"
          >
            <PowerOff className="size-3.5" />
          </Button>
        )}

        <Button
          variant="ghost"
          size="icon-sm"
          onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}
          className="text-muted-foreground hover:text-foreground"
        >
          {theme === 'dark' ? <Sun className="size-3.5" /> : <Moon className="size-3.5" />}
        </Button>
      </div>
    </header>
  );
};
