use crate::core::models::DeviceInfo;
use crate::driver::DeviceDetector;
use std::collections::HashSet;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::Arc;
use std::time::Duration;
use tokio::sync::broadcast;

#[derive(Debug, Clone, serde::Serialize, serde::Deserialize)]
#[serde(tag = "type", content = "payload")]
pub enum DeviceEvent {
    Connected(DeviceInfo),
    Disconnected(String), // Path of the disconnected device
}

pub struct DeviceMonitor {
    running: Arc<AtomicBool>,
    sender: broadcast::Sender<DeviceEvent>,
}

impl DeviceMonitor {
    pub fn new() -> (Self, broadcast::Receiver<DeviceEvent>) {
        let (sender, receiver) = broadcast::channel(32);
        let running = Arc::new(AtomicBool::new(false));

        (
            Self { running, sender },
            receiver,
        )
    }

    /// Subscribes to device hotplug events
    pub fn subscribe(&self) -> broadcast::Receiver<DeviceEvent> {
        self.sender.subscribe()
    }

    /// Starts the background scan loop (checking every 500ms)
    pub fn start(&self) {
        if self.running.swap(true, Ordering::SeqCst) {
            return; // Already running
        }

        let running = Arc::clone(&self.running);
        let sender = self.sender.clone();

        std::thread::spawn(move || {
            let mut known_paths: HashSet<String> = HashSet::new();

            while running.load(Ordering::SeqCst) {
                if let Ok(mut detector) = DeviceDetector::new() {
                    if let Ok(current_devices) = detector.scan_compx_devices() {
                        let current_paths: HashSet<String> =
                            current_devices.iter().map(|d| d.path.clone()).collect();

                        // Detect disconnected devices
                        for path in known_paths.difference(&current_paths) {
                            let _ = sender.send(DeviceEvent::Disconnected(path.clone()));
                        }

                        // Detect newly connected devices
                        for dev in current_devices {
                            if !known_paths.contains(&dev.path) {
                                let _ = sender.send(DeviceEvent::Connected(dev));
                            }
                        }

                        known_paths = current_paths;
                    }
                }

                std::thread::sleep(Duration::from_millis(500));
            }
        });
    }

    /// Stops the background monitor
    pub fn stop(&self) {
        self.running.store(false, Ordering::SeqCst);
    }
}

impl Drop for DeviceMonitor {
    fn drop(&mut self) {
        self.stop();
    }
}
