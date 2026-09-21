use crate::core::models::PairingState;
use crate::protocols::compx::packet::{PairFeatureReport6, PairResponse};

/// State machine controller for wireless pairing (FastConnect)
pub struct PairingController {
    cid: u8,
    pid: u8,
    elapsed_seconds: u32,
    max_timeout_seconds: u32,
    state: PairingState,
}

impl PairingController {
    pub fn new(cid: u8, pid: u8) -> Self {
        Self {
            cid,
            pid,
            elapsed_seconds: 0,
            max_timeout_seconds: 20,
            state: PairingState::Idle,
        }
    }

    pub fn start(&mut self) -> [u8; 8] {
        self.elapsed_seconds = 0;
        self.state = PairingState::Pairing { seconds_elapsed: 0 };
        let report = PairFeatureReport6::new(self.cid, self.pid);
        report.to_bytes()
    }

    pub fn update_tick(&mut self, feature_response: Option<&[u8]>) -> PairingState {
        self.elapsed_seconds += 1;

        if let Some(data) = feature_response {
            match PairFeatureReport6::parse_response(data) {
                Ok(PairResponse::Success { vid, pid }) => {
                    self.state = PairingState::Success { vid, pid };
                    return self.state;
                }
                Ok(PairResponse::Failed) => {
                    self.state = PairingState::Failed;
                    return self.state;
                }
                Ok(PairResponse::InProgress) => {
                    self.state = PairingState::Pairing {
                        seconds_elapsed: self.elapsed_seconds,
                    };
                }
                Err(_) => {
                    self.state = PairingState::ReadFailed;
                }
            }
        } else {
            self.state = PairingState::ReadFailed;
        }

        if self.elapsed_seconds >= self.max_timeout_seconds {
            self.state = PairingState::Timeout;
        }

        self.state
    }

    pub fn current_state(&self) -> PairingState {
        self.state
    }
}
