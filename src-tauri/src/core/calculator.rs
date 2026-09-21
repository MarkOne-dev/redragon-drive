use std::time::Instant;

/// Polling rate calculator based on the original `MouseReportRate` algorithm
pub struct PollingRateCalculator {
    start_time: Instant,
    point_elapsed_times: Vec<u64>,
    point_deltas: Vec<u64>,
    pub current_rate: u32,
    pub max_rate: u32,
    rate_history: Vec<u32>,
    is_starting: bool,
}

impl PollingRateCalculator {
    pub fn new() -> Self {
        Self {
            start_time: Instant::now(),
            point_elapsed_times: Vec::with_capacity(256),
            point_deltas: Vec::with_capacity(128),
            current_rate: 0,
            max_rate: 0,
            rate_history: Vec::with_capacity(64),
            is_starting: true,
        }
    }

    /// Resets the calculation history and timestamps
    pub fn reset(&mut self) {
        self.point_elapsed_times.clear();
        self.point_deltas.clear();
        self.rate_history.clear();
        self.current_rate = 0;
        self.max_rate = 0;
        self.is_starting = true;
        self.start_time = Instant::now();
    }

    /// Records the arrival timestamp of an incoming HID event report
    pub fn register_point(&mut self) -> u32 {
        let elapsed_us = self.start_time.elapsed().as_micros() as u64;
        self.point_elapsed_times.push(elapsed_us);

        if self.calc_rate_avg(128, 128, true) {
            self.is_starting = false;
        } else if self.is_starting {
            self.calc_rate_avg(32, 16, false);
        }

        if self.current_rate > self.max_rate {
            self.max_rate = self.current_rate;
        }

        if self.current_rate > 0 {
            if self.rate_history.len() >= 64 {
                self.rate_history.remove(0);
            }
            self.rate_history.push(self.current_rate);
        }

        self.current_rate
    }

    /// Moving window averaging algorithm with interquartile outlier rejection
    fn calc_rate_avg(&mut self, start_count: usize, calc_count: usize, remove_first: bool) -> bool {
        if self.point_elapsed_times.len() >= start_count {
            let last = *self.point_elapsed_times.last().unwrap();
            let first = *self.point_elapsed_times.first().unwrap();
            let delta = last.saturating_sub(first);

            if delta > 0 {
                self.point_deltas.push(delta);
            }

            if remove_first {
                self.point_elapsed_times.remove(0);
            }

            if self.point_deltas.len() >= calc_count {
                // Sort to filter out extreme quartiles (lower 25% and upper 25%)
                self.point_deltas.sort_unstable();
                let q1 = calc_count / 4;
                let q3 = calc_count - (calc_count / 4);

                let mut sum_deltas: u64 = 0;
                for i in q1..q3 {
                    sum_deltas += self.point_deltas[i];
                }

                self.point_deltas.clear();

                if sum_deltas > 0 {
                    let effective_points = (start_count * (q3 - q1)) as u64;
                    // delta is in microseconds, multiply by 1_000_000 to convert to Hz
                    let calculated_hz = (effective_points * 1_000_000) / sum_deltas;
                    self.current_rate = calculated_hz as u32;
                    return true;
                }
            }
        }
        false
    }

    /// Returns the accumulated moving average polling rate
    pub fn average_rate(&self) -> u32 {
        if self.rate_history.is_empty() {
            return self.current_rate;
        }
        let sum: u64 = self.rate_history.iter().map(|&r| r as u64).sum();
        (sum / self.rate_history.len() as u64) as u32
    }
}

impl Default for PollingRateCalculator {
    fn default() -> Self {
        Self::new()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_polling_rate_calculator_initial() {
        let mut calc = PollingRateCalculator::new();
        assert_eq!(calc.current_rate, 0);
        assert_eq!(calc.max_rate, 0);
        calc.reset();
        assert_eq!(calc.average_rate(), 0);
    }
}
