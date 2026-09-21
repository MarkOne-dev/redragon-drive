import React from 'react';
import { 
  Gauge, 
  Sliders, 
  MousePointerClick, 
  Cpu, 
  Radio, 
  Activity, 
  Settings,
  Flame
} from 'lucide-react';
import { cn } from '@/lib/utils';
import { useTranslation } from 'react-i18next';

export type NavTab = 
  | 'dashboard'
  | 'performance'
  | 'buttons'
  | 'sensor'
  | 'pairing'
  | 'diagnostics'
  | 'settings';

interface SidebarProps {
  activeTab: NavTab;
  onSelectTab: (tab: NavTab) => void;
  pollingRateHz: number;
}

interface NavItem {
  id: NavTab;
  label: string;
  icon: React.ElementType;
  badge?: string;
}

export const Sidebar: React.FC<SidebarProps> = ({
  activeTab,
  onSelectTab,
  pollingRateHz,
}) => {
  const { t } = useTranslation();

  const navItems: NavItem[] = [
    { id: 'dashboard', label: t('nav.dashboard'), icon: Gauge },
    { id: 'performance', label: t('nav.performance'), icon: Sliders },
    { id: 'buttons', label: t('nav.buttons'), icon: MousePointerClick },
    { id: 'sensor', label: t('nav.sensor'), icon: Cpu },
    { id: 'pairing', label: t('nav.pairing'), icon: Radio },
    { id: 'diagnostics', label: t('nav.diagnostics'), icon: Activity },
    { id: 'settings', label: t('nav.settings'), icon: Settings },
  ];

  return (
    <aside className="w-64 border-r border-border/50 bg-card/40 flex flex-col justify-between shrink-0 select-none">
      <div className="p-4 space-y-1">
        <div className="px-3 py-2 text-[11px] font-semibold text-muted-foreground uppercase tracking-wider font-mono">
          {t('header.appName')}
        </div>

        {navItems.map((item) => {
          const Icon = item.icon;
          const isActive = activeTab === item.id;

          return (
            <button
              key={item.id}
              onClick={() => onSelectTab(item.id)}
              className={cn(
                'w-full flex items-center gap-3 px-3.5 py-2.5 rounded-lg text-xs font-medium transition-all duration-150 relative text-left',
                isActive
                  ? 'bg-red-500/15 text-red-500 font-semibold shadow-xs border border-red-500/30'
                  : 'text-muted-foreground hover:bg-muted/60 hover:text-foreground'
              )}
            >
              {isActive && (
                <div className="absolute left-0 top-1/2 -translate-y-1/2 w-1 h-5 bg-red-500 rounded-r-full shadow-[0_0_8px_rgba(239,68,68,0.8)]" />
              )}
              <Icon className={cn('size-4 shrink-0', isActive ? 'text-red-500' : 'text-muted-foreground')} />
              <span className="flex-1">{item.label}</span>
              {item.badge && (
                <span className="text-[10px] px-1.5 py-0.5 rounded-full bg-red-500/10 text-red-400 font-mono">
                  {item.badge}
                </span>
              )}
            </button>
          );
        })}
      </div>

      {/* Live Polling Rate Quick Indicator Widget at bottom */}
      <div className="p-4 border-t border-border/50 bg-card/60">
        <div className="rounded-xl p-3 bg-background/60 border border-border/60">
          <div className="flex items-center justify-between text-[11px] text-muted-foreground mb-1">
            <span className="flex items-center gap-1">
              <Flame className="size-3 text-red-400" />
              {t('dashboard.pollingRate')}
            </span>
            <span className="font-mono text-foreground font-semibold">
              {pollingRateHz} Hz
            </span>
          </div>
          <div className="w-full bg-muted/60 h-1.5 rounded-full overflow-hidden">
            <div 
              className="h-full bg-red-500 rounded-full transition-all duration-500"
              style={{ width: `${Math.min(100, (pollingRateHz / 1000) * 100)}%` }}
            />
          </div>
          <div className="flex justify-between text-[9px] text-muted-foreground mt-1 font-mono">
            <span>125Hz</span>
            <span>500Hz</span>
            <span>1000Hz</span>
          </div>
        </div>
      </div>
    </aside>
  );
};
