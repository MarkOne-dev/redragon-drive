use crate::core::calculator::PollingRateCalculator;
use crate::core::models::MouseStats;
use hidapi::HidDevice;
use std::sync::atomic::{AtomicBool, Ordering};
use std::sync::{Arc, Mutex};
use std::time::Duration;

pub struct HidInputReader {
    running: Arc<AtomicBool>,
    stats: Arc<Mutex<MouseStats>>,
    calculator: Arc<Mutex<PollingRateCalculator>>,
}

impl HidInputReader {
    pub fn new() -> Self {
        Self {
            running: Arc::new(AtomicBool::new(false)),
            stats: Arc::new(Mutex::new(MouseStats::default())),
            calculator: Arc::new(Mutex::new(PollingRateCalculator::new())),
        }
    }

    pub fn stats(&self) -> Arc<Mutex<MouseStats>> {
        Arc::clone(&self.stats)
    }

    /// Starts reading input reports from an opened device in a background worker thread
    pub fn start_reading(&self, device: Arc<Mutex<HidDevice>>) {
        if self.running.swap(true, Ordering::SeqCst) {
            return;
        }

        let running = Arc::clone(&self.running);
        let stats = Arc::clone(&self.stats);
        let calculator = Arc::clone(&self.calculator);

        std::thread::spawn(move || {
            let mut buffer = [0u8; 64];

            while running.load(Ordering::SeqCst) {
                let read_res = {
                    let dev_guard = device.lock();
                    match dev_guard {
                        Ok(dev) => dev.read_timeout(&mut buffer, 100),
                        Err(_) => break,
                    }
                };

                match read_res {
                    Ok(bytes_read) if bytes_read > 0 => {
                        // 1. Feed polling rate calculator
                        let mut calc = calculator.lock().unwrap();
                        let current_hz = calc.register_point();
                        let avg_hz = calc.average_rate();
                        let max_hz = calc.max_rate;

                        // 2. Parse buttons from standard mouse input report
                        let mut s = stats.lock().unwrap();
                        s.current_polling_rate = current_hz;
                        s.average_polling_rate = avg_hz;
                        s.max_polling_rate = max_hz;

                        if bytes_read >= 4 {
                            let buttons = buffer[1];
                            if buttons & 0x01 != 0 {
                                s.left_clicks += 1;
                            }
                            if buttons & 0x02 != 0 {
                                s.right_clicks += 1;
                            }
                            if buttons & 0x04 != 0 {
                                s.middle_clicks += 1;
                            }
                            if buttons & 0x08 != 0 {
                                s.button4_clicks += 1;
                            }
                            if buttons & 0x10 != 0 {
                                s.button5_clicks += 1;
                            }

                            // Scroll wheel (Y axis)
                            let wheel = buffer[3] as i8;
                            if wheel != 0 {
                                s.wheel_delta_y += wheel as i64;
                            }
                        }
                    }
                    Ok(_) => {
                        // Standard read timeout, continue loop
                    }
                    Err(_) => {
                        // Device might be disconnected or bus reset
                        std::thread::sleep(Duration::from_millis(50));
                    }
                }
            }
        });
    }

    pub fn stop(&self) {
        self.running.store(false, Ordering::SeqCst);
    }

    pub fn reset_stats(&self) {
        let mut s = self.stats.lock().unwrap();
        *s = MouseStats::default();
        let mut c = self.calculator.lock().unwrap();
        c.reset();
    }
}

impl Default for HidInputReader {
    fn default() -> Self {
        Self::new()
    }
}

impl Drop for HidInputReader {
    fn drop(&mut self) {
        self.stop();
    }
}
