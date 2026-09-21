use crate::core::models::MouseStats;
use crate::driver::HidInputReader;
use hidapi::HidDevice;
use std::sync::{Arc, Mutex};

pub struct DiagnosticsService {
    reader: HidInputReader,
}

impl DiagnosticsService {
    pub fn new() -> Self {
        Self {
            reader: HidInputReader::new(),
        }
    }

    /// Returns the current diagnostic statistics
    pub fn get_stats(&self) -> MouseStats {
        let stats_lock = self.reader.stats();
        let s = stats_lock.lock().unwrap();
        s.clone()
    }

    /// Resets the click counters and polling rate calculator
    pub fn reset_stats(&self) {
        self.reader.reset_stats();
    }

    /// Starts monitoring mouse events if a device handle is available
    pub fn start_monitoring(&self, device: Arc<Mutex<HidDevice>>) {
        self.reader.start_reading(device);
    }

    /// Stops monitoring mouse events
    pub fn stop_monitoring(&self) {
        self.reader.stop();
    }
}

impl Default for DiagnosticsService {
    fn default() -> Self {
        Self::new()
    }
}
