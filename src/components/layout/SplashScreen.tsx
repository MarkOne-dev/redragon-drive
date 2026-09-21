import React from 'react';
import { useTranslation } from 'react-i18next';

interface SplashScreenProps {
  isVisible: boolean;
  statusText?: string;
}

export const SplashScreen: React.FC<SplashScreenProps> = ({ isVisible, statusText }) => {
  const { t } = useTranslation();

  return (
    <div
      className={`fixed inset-0 z-50 flex flex-col items-center justify-center bg-background text-foreground select-none transition-all duration-700 ease-out ${
        isVisible ? 'opacity-100 scale-100 pointer-events-auto' : 'opacity-0 scale-105 pointer-events-none'
      }`}
    >
      {/* Tactical Ambient Glow */}
      <div 
        className="absolute w-[600px] h-[600px] rounded-full pointer-events-none blur-3xl opacity-30 animate-pulse-glow"
        style={{
          background: 'radial-gradient(circle, rgba(239, 68, 68, 0.4) 0%, rgba(220, 38, 38, 0.1) 45%, transparent 70%)',
        }}
      />

      {/* Cyber Grid Background lines */}
      <div 
        className="absolute inset-0 opacity-[0.03] pointer-events-none"
        style={{
          backgroundImage: 'linear-gradient(to right, #ef4444 1px, transparent 1px), linear-gradient(to bottom, #ef4444 1px, transparent 1px)',
          backgroundSize: '40px 40px',
        }}
      />

      {/* Main Branding Card */}
      <div className="relative z-10 flex flex-col items-center text-center px-6">
        {/* Logo with Ambient Glow */}
        <div className="relative mb-6">
          <div className="absolute inset-0 blur-xl opacity-60 bg-red-600 rounded-full animate-pulse-glow" />
          <img
            src="/assets/logo.png"
            alt="Redragon Logo"
            className="relative h-20 w-auto object-contain dark:brightness-100 brightness-0 filter drop-shadow-[0_0_20px_rgba(239,68,68,0.7)]"
          />
        </div>

        {/* Title & Badge */}
        <h1 className="text-2xl sm:text-3xl font-black tracking-widest uppercase bg-gradient-to-r from-red-500 via-red-400 to-foreground bg-clip-text text-transparent">
          {t('splash.title')}
        </h1>
        <p className="text-xs font-mono text-muted-foreground uppercase tracking-widest mt-1">
          PixArt PAW3395 · Compx CX52850P Suite
        </p>

        {/* High-Tech Loader Bar */}
        <div className="w-64 sm:w-72 h-1.5 bg-red-950/60 rounded-full overflow-hidden border border-red-500/30 mt-8 relative shadow-[0_0_15px_rgba(239,68,68,0.2)]">
          <div className="absolute inset-y-0 bg-gradient-to-r from-red-600 via-red-400 to-red-500 rounded-full w-28 animate-[shimmer_1.5s_infinite] shadow-[0_0_10px_rgba(239,68,68,0.8)]" />
        </div>

        {/* Status indicator */}
        <div className="flex items-center gap-2 mt-4 text-xs font-mono text-red-400/90">
          <span className="size-2 rounded-full bg-red-500 animate-ping" />
          <span>{statusText || t('splash.scanning')}</span>
        </div>
      </div>
    </div>
  );
};
