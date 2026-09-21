use crate::core::error::Result;
use crate::core::models::AppConfig;
use std::fs;
use std::path::{Path, PathBuf};
use std::sync::{Arc, Mutex};

pub struct ConfigService {
    config: Arc<Mutex<AppConfig>>,
    config_path: PathBuf,
}

impl ConfigService {
    pub fn new<P: AsRef<Path>>(path: P) -> Self {
        let config_path = path.as_ref().to_path_buf();
        let config = Self::load_from_disk(&config_path).unwrap_or_default();

        Self {
            config: Arc::new(Mutex::new(config)),
            config_path,
        }
    }

    fn load_from_disk(path: &Path) -> Option<AppConfig> {
        if path.exists() {
            if let Ok(content) = fs::read_to_string(path) {
                if let Ok(cfg) = serde_json::from_str::<AppConfig>(&content) {
                    return Some(cfg);
                }
            }
        }
        None
    }

    pub fn get_config(&self) -> AppConfig {
        self.config.lock().unwrap().clone()
    }

    pub fn save_config(&self, new_config: AppConfig) -> Result<()> {
        let json = serde_json::to_string_pretty(&new_config)
            .map_err(|e| crate::core::error::RedragonError::UnexpectedResponse(e.to_string()))?;

        if let Some(parent) = self.config_path.parent() {
            let _ = fs::create_dir_all(parent);
        }

        fs::write(&self.config_path, json)
            .map_err(|e| crate::core::error::RedragonError::UnexpectedResponse(e.to_string()))?;

        let mut cfg = self.config.lock().unwrap();
        *cfg = new_config;
        Ok(())
    }

    pub fn add_custom_pid(&self, vid_pid: &str) -> Result<()> {
        let mut cfg = self.get_config();
        let clean = vid_pid.trim().to_uppercase();
        if !cfg.custom_pids.contains(&clean) {
            cfg.custom_pids.push(clean);
            self.save_config(cfg)?;
        }
        Ok(())
    }
}
