import React from 'react';
import { 
  Usb, 
  Wifi, 
  RefreshCw, 
  Lock, 
  Cpu, 
  Zap, 
  Moon, 
  Sun, 
  Languages,
  CheckCircle2
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { useTranslation } from 'react-i18next';
import { useTheme } from '@/components/theme-provider';

interface DeviceLockScreenProps {
  isScanning: boolean;
  onScan: () => void;
  detectedName?: string | null;
}

export const DeviceLockScreen: React.FC<DeviceLockScreenProps> = ({
  isScanning,
  onScan,
  detectedName,
}) => {
  const { t: translate, i18n } = useTranslation();
  const t = (key: string) => (translate as any)(key);
  const { theme, setTheme } = useTheme();

  const toggleLanguage = () => {
    const nextLang = i18n.language.startsWith('es') ? 'en' : 'es';
    i18n.changeLanguage(nextLang);
  };

  const isDark = theme === 'dark' || (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);

  return (
    <div className="relative min-h-screen w-screen overflow-x-hidden overflow-y-auto bg-background text-foreground flex flex-col justify-between select-none">
      {/* Background Cyber Ambient Lights */}
      <div 
        className="fixed top-1/4 left-1/2 -translate-x-1/2 -translate-y-1/2 w-[700px] h-[500px] rounded-full pointer-events-none blur-3xl opacity-20 dark:opacity-30 animate-pulse-glow"
        style={{
          background: 'radial-gradient(circle, rgba(239, 68, 68, 0.5) 0%, rgba(185, 28, 28, 0.15) 50%, transparent 75%)',
        }}
      />

      {/* Cyber Grid Pattern */}
      <div 
        className="fixed inset-0 opacity-[0.025] dark:opacity-[0.04] pointer-events-none"
        style={{
          backgroundImage: 'linear-gradient(to right, #ef4444 1px, transparent 1px), linear-gradient(to bottom, #ef4444 1px, transparent 1px)',
          backgroundSize: '48px 48px',
        }}
      />

      {/* Top Navigation Bar */}
      <header className="relative z-20 h-16 border-b border-border/40 bg-card/40 backdrop-blur-md px-6 sm:px-10 flex items-center justify-between">
        <div className="flex items-center gap-3.5">
          <img
            src="/assets/logo.png"
            alt="Redragon Logo"
            className="h-10 w-auto object-contain dark:brightness-100 brightness-0 dark:drop-shadow-[0_0_10px_rgba(239,68,68,0.4)]"
          />
          <Badge variant="outline" className="border-red-500/40 text-red-400 bg-red-500/10 text-xs px-2 py-0.5 font-mono">
            M916-PRO 1K
          </Badge>
        </div>

        {/* Quick Settings: Theme & Language */}
        <div className="flex items-center gap-2">
          <Button
            variant="ghost"
            size="sm"
            onClick={toggleLanguage}
            className="h-8 px-2.5 text-xs font-mono gap-1.5 border border-border/40 hover:border-red-500/40 text-muted-foreground hover:text-foreground"
            title={t('header.language')}
          >
            <Languages className="size-3.5 text-red-500" />
            <span className="uppercase font-semibold">{i18n.language.substring(0, 2)}</span>
          </Button>

          <Button
            variant="ghost"
            size="icon"
            onClick={() => setTheme(isDark ? 'light' : 'dark')}
            className="size-8 border border-border/40 hover:border-red-500/40 text-muted-foreground hover:text-foreground"
          >
            {isDark ? <Sun className="size-4 text-amber-400" /> : <Moon className="size-4 text-slate-700" />}
          </Button>
        </div>
      </header>

      {/* Center Gaming Lock View */}
      <main className="relative z-10 flex-1 flex flex-col items-center justify-center px-4 py-8 max-w-4xl mx-auto w-full text-center">
        
        {/* 3D Mouse Visual Container with Radar Scanning */}
        <div className="relative flex items-center justify-center my-4 w-72 h-72 sm:w-80 sm:h-80">
          
          {/* Concentric Radar Rings */}
          <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
            <span className="absolute size-48 sm:size-56 rounded-full border border-red-500/30 animate-radar-ripple" />
            <span className="absolute size-64 sm:size-72 rounded-full border border-red-500/20 animate-radar-ripple" style={{ animationDelay: '0.8s' }} />
            <span className="absolute size-80 sm:size-88 rounded-full border border-red-500/10 animate-radar-ripple" style={{ animationDelay: '1.6s' }} />
          </div>

          {/* Underglow Aura */}
          <div 
            className="absolute size-44 rounded-full pointer-events-none blur-2xl opacity-40 dark:opacity-60 animate-pulse-glow"
            style={{
              background: detectedName 
                ? 'radial-gradient(circle, rgba(16, 185, 129, 0.8) 0%, transparent 70%)'
                : 'radial-gradient(circle, rgba(239, 68, 68, 0.8) 0%, rgba(220, 38, 38, 0.3) 50%, transparent 75%)',
            }}
          />

          {/* Cyber Scan Line */}
          <div className="absolute inset-x-4 h-24 overflow-hidden pointer-events-none z-10 opacity-70">
            <div className="w-full h-0.5 bg-gradient-to-r from-transparent via-red-500 to-transparent shadow-[0_0_12px_#ef4444] animate-cyber-scan" />
          </div>

          {/* Floating Levitating Mouse Render */}
          <img
            src="/assets/mouse-m916.png"
            alt="Redragon M916-PRO"
            className={`relative z-10 w-64 sm:w-72 h-auto object-contain select-none animate-float transition-all duration-500 ${
              detectedName
                ? 'filter drop-shadow-[0_0_35px_rgba(16,185,129,0.7)] scale-105'
                : 'filter drop-shadow-[0_0_30px_rgba(239,68,68,0.5)]'
            }`}
          />
        </div>

        {/* Lock Status Header */}
        <div className="space-y-2 mt-2">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full border text-xs font-mono font-semibold uppercase tracking-wider bg-card/60 backdrop-blur-md shadow-sm transition-colors duration-300">
            {detectedName ? (
              <>
                <CheckCircle2 className="size-3.5 text-emerald-400 animate-bounce" />
                <span className="text-emerald-400">{t('lockScreen.deviceFound')}</span>
              </>
            ) : (
              <>
                <span className="size-2 rounded-full bg-red-500 animate-ping" />
                <Lock className="size-3 text-red-400" />
                <span className="text-red-400">{t('lockScreen.title')}</span>
              </>
            )}
          </div>

          <h2 className="text-2xl sm:text-3xl font-black tracking-tight text-foreground uppercase">
            {detectedName ? detectedName : t('lockScreen.subtitle')}
          </h2>
          <p className="text-xs text-muted-foreground font-mono max-w-lg mx-auto">
            {t('lockScreen.statusWaiting')}
          </p>
        </div>

        {/* Dual Mode Connection Cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mt-8 w-full max-w-2xl text-left">
          {/* Card 1: Wired USB-C */}
          <div className="p-4 rounded-xl border border-border/50 bg-card/50 backdrop-blur-sm hover:border-red-500/40 transition-all group relative overflow-hidden">
            <div className="absolute top-0 right-0 w-24 h-24 bg-red-500/5 rounded-full blur-xl pointer-events-none group-hover:bg-red-500/10 transition-colors" />
            <div className="flex items-start gap-3 relative z-10">
              <div className="size-10 rounded-lg bg-red-500/10 border border-red-500/30 flex items-center justify-center shrink-0 group-hover:scale-110 transition-transform">
                <Usb className="size-5 text-red-500" />
              </div>
              <div className="space-y-1">
                <div className="flex items-center gap-2">
                  <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
                    {t('lockScreen.wiredTitle')}
                  </h3>
                  <Badge variant="outline" className="text-[9px] px-1.5 py-0 border-red-500/30 text-red-400 bg-red-500/5">
                    1000Hz
                  </Badge>
                </div>
                <p className="text-[11px] text-muted-foreground leading-relaxed">
                  {t('lockScreen.wiredDesc')}
                </p>
              </div>
            </div>
          </div>

          {/* Card 2: Wireless 2.4G */}
          <div className="p-4 rounded-xl border border-border/50 bg-card/50 backdrop-blur-sm hover:border-red-500/40 transition-all group relative overflow-hidden">
            <div className="absolute top-0 right-0 w-24 h-24 bg-red-500/5 rounded-full blur-xl pointer-events-none group-hover:bg-red-500/10 transition-colors" />
            <div className="flex items-start gap-3 relative z-10">
              <div className="size-10 rounded-lg bg-red-500/10 border border-red-500/30 flex items-center justify-center shrink-0 group-hover:scale-110 transition-transform">
                <Wifi className="size-5 text-red-500" />
              </div>
              <div className="space-y-1">
                <div className="flex items-center gap-2">
                  <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
                    {t('lockScreen.wirelessTitle')}
                  </h3>
                  <Badge variant="outline" className="text-[9px] px-1.5 py-0 border-red-500/30 text-red-400 bg-red-500/5">
                    2.4G
                  </Badge>
                </div>
                <p className="text-[11px] text-muted-foreground leading-relaxed">
                  {t('lockScreen.wirelessDesc')}
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Action Button: Scan Now */}
        <div className="mt-8 flex items-center justify-center">
          <Button
            onClick={onScan}
            disabled={isScanning}
            className="h-11 px-6 rounded-xl bg-red-600 hover:bg-red-700 text-white font-semibold text-xs tracking-wider uppercase gap-2.5 shadow-[0_0_20px_rgba(239,68,68,0.4)] hover:shadow-[0_0_30px_rgba(239,68,68,0.6)] transition-all hover:scale-[1.02] active:scale-[0.98]"
          >
            <RefreshCw className={`size-4 ${isScanning ? 'animate-spin' : ''}`} />
            <span>{isScanning ? t('lockScreen.scanning') : t('lockScreen.scanNow')}</span>
          </Button>
        </div>
      </main>

      {/* Footer Specs Bar */}
      <footer className="relative z-20 border-t border-border/40 bg-card/30 backdrop-blur-md px-6 py-3 flex flex-col sm:flex-row items-center justify-between gap-2 text-[11px] font-mono text-muted-foreground">
        <div className="flex items-center gap-4">
          <span className="flex items-center gap-1.5">
            <Cpu className="size-3.5 text-red-500" />
            PixArt PAW3395 (26K DPI)
          </span>
          <span className="hidden sm:inline text-border">•</span>
          <span className="flex items-center gap-1.5">
            <Zap className="size-3.5 text-amber-500" />
            Compx CX52850P Dual-Mode
          </span>
        </div>
        <div>
          <span>Redragon M916-PRO 1K Suite · Driver v0.2.0</span>
        </div>
      </footer>
    </div>
  );
};
