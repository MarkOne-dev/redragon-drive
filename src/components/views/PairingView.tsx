import { useState } from 'react';
import { 
  Radio, 
  Wifi, 
  AlertCircle, 
  CheckCircle2, 
  Loader2, 
  Antenna
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import type { PairingState, RfTestMode } from '@/types/mouse';
import { mouseApi } from '@/api/mouseApi';

interface PairingViewProps {
  pairingStatus: PairingState;
  onStartPairing: () => Promise<void>;
  onCancelPairing: () => Promise<void>;
}

export const PairingView: React.FC<PairingViewProps> = ({
  pairingStatus,
  onStartPairing,
  onCancelPairing,
}) => {
  const [rfStatus, setRfStatus] = useState<string | null>(null);

  const isPairing = pairingStatus.state === 'Searching' || pairingStatus.state === 'Synchronizing';

  const handleRfTest = async (mode: RfTestMode) => {
    try {
      await mouseApi.runRfTest(mode);
      setRfStatus(`RF Mode '${mode}' sent to wireless dongle`);
      setTimeout(() => setRfStatus(null), 3000);
    } catch (err: any) {
      setRfStatus(`RF test failed: ${err?.message || err}`);
    }
  };

  return (
    <div className="space-y-6 max-w-4xl mx-auto select-none">
      {rfStatus && (
        <div className="rounded-lg p-3 bg-red-500/10 border border-red-500/30 text-xs text-red-400 flex items-center justify-between">
          <span>{rfStatus}</span>
          <Button variant="ghost" size="xs" onClick={() => setRfStatus(null)}>Dismiss</Button>
        </div>
      )}

      {/* Header */}
      <div>
        <h2 className="text-base font-bold text-foreground flex items-center gap-2">
          <Radio className="size-4 text-red-500" />
          2.4GHz FastConnect Pairing Suite
        </h2>
        <p className="text-xs text-muted-foreground mt-0.5">
          Pair and synchronize your Redragon M916-PRO wireless mouse with the Compx USB dongle receiver.
        </p>
      </div>

      {/* Pairing Wizard Card */}
      <div className="rounded-2xl border border-border/60 bg-card p-6 shadow-sm space-y-6">
        <div className="flex flex-col md:flex-row items-start md:items-center justify-between gap-4 pb-4 border-b border-border/40">
          <div>
            <span className="text-xs uppercase font-semibold text-muted-foreground tracking-wider">
              Pairing State Machine
            </span>
            <div className="flex items-center gap-2 mt-1">
              <span className="text-xl font-black text-foreground">
                Status: {pairingStatus.state}
              </span>
              {isPairing && (
                <Loader2 className="size-4 text-red-500 animate-spin" />
              )}
            </div>
          </div>

          <div className="flex items-center gap-2">
            {!isPairing ? (
              <Button
                onClick={onStartPairing}
                className="bg-red-600 hover:bg-red-700 text-white text-xs gap-1.5 shadow-md shadow-red-600/30"
              >
                <Wifi className="size-3.5" />
                Start 2.4G Pairing
              </Button>
            ) : (
              <Button
                variant="destructive"
                onClick={onCancelPairing}
                className="text-xs gap-1.5"
              >
                Cancel Pairing
              </Button>
            )}
          </div>
        </div>

        {/* Pairing Instructions / Steps */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-xs">
          <div className="p-4 rounded-xl border border-border/50 bg-background/50 space-y-2">
            <div className="size-6 rounded-full bg-red-500/20 text-red-400 font-bold flex items-center justify-center font-mono">
              1
            </div>
            <div className="font-bold text-foreground">Insert USB Dongle</div>
            <p className="text-muted-foreground text-[11px]">
              Plug the Compx 2.4G wireless receiver dongle directly into an open USB port on your computer.
            </p>
          </div>

          <div className="p-4 rounded-xl border border-border/50 bg-background/50 space-y-2">
            <div className="size-6 rounded-full bg-red-500/20 text-red-400 font-bold flex items-center justify-center font-mono">
              2
            </div>
            <div className="font-bold text-foreground">Put Mouse in Pair Mode</div>
            <p className="text-muted-foreground text-[11px]">
              Turn mouse ON in 2.4G mode. Press & hold <strong>Left + Scroll + Right Click</strong> for 3 seconds until the indicator LED flashes rapidly.
            </p>
          </div>

          <div className="p-4 rounded-xl border border-border/50 bg-background/50 space-y-2">
            <div className="size-6 rounded-full bg-red-500/20 text-red-400 font-bold flex items-center justify-center font-mono">
              3
            </div>
            <div className="font-bold text-foreground">Bring Mouse Close</div>
            <p className="text-muted-foreground text-[11px]">
              Place the mouse within 10 cm of the receiver dongle. The FastConnect protocol will lock and bind automatically.
            </p>
          </div>
        </div>

        {/* Status Outcome Banner */}
        {pairingStatus.state === 'Success' && (
          <div className="rounded-xl p-4 bg-emerald-500/10 border border-emerald-500/30 flex items-center gap-3 text-xs text-emerald-400">
            <CheckCircle2 className="size-5 shrink-0" />
            <div>
              <div className="font-bold text-sm">Pairing Successful!</div>
              <div className="text-[11px] text-emerald-500/80">
                Mouse paired and bound with PID 0x{pairingStatus.pid.toString(16).toUpperCase()}. Receiver is now receiving tracking packets.
              </div>
            </div>
          </div>
        )}

        {pairingStatus.state === 'Failed' && (
          <div className="rounded-xl p-4 bg-red-500/10 border border-red-500/30 flex items-center gap-3 text-xs text-red-400">
            <AlertCircle className="size-5 shrink-0" />
            <div>
              <div className="font-bold text-sm">Pairing Timed Out / Failed</div>
              <div className="text-[11px] text-red-400/80">
                {pairingStatus.error || 'Ensure the mouse is turned on and close to the USB dongle, then retry.'}
              </div>
            </div>
          </div>
        )}
      </div>

      {/* Hardware RF Calibration Modes */}
      <div className="rounded-xl border border-border/60 bg-card p-5 space-y-3">
        <div className="flex items-center gap-2">
          <Antenna className="size-4 text-red-500" />
          <h3 className="text-xs font-bold uppercase tracking-wider text-foreground">
            Factory RF Test Modes (Diagnostic)
          </h3>
        </div>
        <p className="text-xs text-muted-foreground">
          Commands matching the factory tester (`max v1.5`) for antenna frequency carrier verification.
        </p>

        <div className="flex flex-wrap gap-2 pt-1">
          <Button variant="outline" size="sm" onClick={() => handleRfTest('LowCarrier')} className="text-xs">
            Test LowCarrier Mode
          </Button>
          <Button variant="outline" size="sm" onClick={() => handleRfTest('MtkMode')} className="text-xs">
            Test MTK Carrier Mode
          </Button>
          <Button variant="outline" size="sm" onClick={() => handleRfTest('AllReceived')} className="text-xs">
            Test AllReceived Mode
          </Button>
        </div>
      </div>
    </div>
  );
};
