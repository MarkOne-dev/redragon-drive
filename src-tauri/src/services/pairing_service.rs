use crate::core::error::{RedragonError, Result};
use crate::core::models::PairingState;
use crate::driver::HidTransport;
use crate::protocols::compx::pairing::PairingController;
use hidapi::HidDevice;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;

pub struct PairingService {
    state: Arc<Mutex<PairingState>>,
    cancel_flag: Arc<AtomicBool>,
}

impl PairingService {
    pub fn new() -> Self {
        Self {
            state: Arc::new(Mutex::new(PairingState::Idle)),
            cancel_flag: Arc::new(AtomicBool::new(false)),
        }
    }

    pub fn current_state(&self) -> PairingState {
        *self.state.lock().unwrap()
    }

    pub fn cancel(&self) {
        self.cancel_flag.store(true, Ordering::SeqCst);
        let mut s = self.state.lock().unwrap();
        *s = PairingState::Idle;
    }

    /// Starts the asynchronous wireless pairing sequence
    pub fn start_pairing(
        &self,
        device_handle: Arc<Mutex<Option<HidDevice>>>,
        cid: u8,
        pid: u8,
    ) -> Result<()> {
        let s = self.state.lock().unwrap();
        if matches!(*s, PairingState::Pairing { .. }) {
            return Err(RedragonError::UnexpectedResponse(
                "A pairing procedure is already in progress".into(),
            ));
        }

        self.cancel_flag.store(false, Ordering::SeqCst);
        let state = Arc::clone(&self.state);
        let cancel = Arc::clone(&self.cancel_flag);

        // Spawn background worker thread
        std::thread::spawn(move || {
            let mut controller = PairingController::new(cid, pid);
            let pair_bytes = controller.start();

            // 1. Send Feature Report 6 to initiate pairing on the dongle
            {
                let dev_guard = device_handle.lock().unwrap();
                if let Some(ref dev) = *dev_guard {
                    let transport = HidTransport::new(dev);
                    if let Err(_) = transport.send_feature_report(&pair_bytes) {
                        let mut s = state.lock().unwrap();
                        *s = PairingState::ReadFailed;
                        return;
                    }
                } else {
                    let mut s = state.lock().unwrap();
                    *s = PairingState::ReadFailed;
                    return;
                }
            }

            // 2. Polling loop (every 1000ms, up to 20 seconds timeout)
            while !cancel.load(Ordering::SeqCst) {
                std::thread::sleep(Duration::from_millis(1000));
                if cancel.load(Ordering::SeqCst) {
                    break;
                }

                let response_bytes = {
                    let dev_guard = device_handle.lock().unwrap();
                    if let Some(ref dev) = *dev_guard {
                        let transport = HidTransport::new(dev);
                        transport.get_feature_report(6, 9).ok()
                    } else {
                        None
                    }
                };

                let new_state = controller.update_tick(response_bytes.as_deref());
                {
                    let mut s = state.lock().unwrap();
                    *s = new_state;
                }

                match new_state {
                    PairingState::Success { .. }
                    | PairingState::Failed
                    | PairingState::Timeout
                    | PairingState::ReadFailed => {
                        break;
                    }
                    _ => {}
                }
            }
        });

        Ok(())
    }
}

impl Default for PairingService {
    fn default() -> Self {
        Self::new()
    }
}
