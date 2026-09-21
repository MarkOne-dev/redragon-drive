# Redragon M916-PRO 1K Gaming Suite

High-performance, modern, memory-safe gaming mouse management driver and desktop suite for the **Redragon M916-PRO 1K** wireless mouse, engineered in **Rust (Tauri v2)** and **React 19 / Vite / Tailwind CSS v4**.

![Architecture](https://img.shields.io/badge/Backend-Rust%20%7C%20Tauri%20v2-orange)
![Frontend](https://img.shields.io/badge/Frontend-React%2019%20%7C%20Vite%208%20%7C%20Tailwind%20v4-blue)
![Sensor](https://img.shields.io/badge/Sensor-PixArt%20PAW3395%20(26K%20DPI)-red)
![MCU](https://img.shields.io/badge/MCU-Compx%20CX52850P%20%7C%20CX52650N-purple)

---

## Key Capabilities

### 1. Hardware Configuration Suite (PixArt PAW3395 & Compx CX52850P)
- **Polling Rate (Hz) Switching**: 125 Hz, 250 Hz, 500 Hz, 1000 Hz, 2000 Hz, 4000 Hz, 8000 Hz.
- **Granular DPI Stages & Decoupled X/Y Axes**: 50 to 26,000 DPI in precise 50 DPI steps with independent X and Y axis tuning and per-stage RGB LED indicator assignment.
- **Interactive Button Remapping (0-15)**: Visual chassis schematic matching factory hardware coordinates, Rapid Fire (*FireKey* burst sequences), multimedia shortcuts, and Sniper Precision DPI lock.
- **Hardware Macro Sequencer**: Record key press sequences with custom millisecond delays and execution loop counts.
- **On-Board Memory Profile Switcher**: Toggle between Profiles 1, 2, and 3 with instant configuration persistence.
- **Advanced Sensor Tuning**:
  - **Lift-off Distance (LOD)**: 1.0 mm (esports/low profile) or 2.0 mm (cloth pads).
  - **Hardware Motion Sync**: Zero-jitter frame alignment with USB poll timing.
  - **Debounce Filter Delay**: Adjustable from 4 ms to 20 ms.
  - **Ripple Control & Angle Snapping**: Dynamic jitter suppression and linear trajectory correction.
- **Real-Time Battery & Hotplug Telemetry**: Live battery percentage, ADC voltage telemetry (mV), and instant USB hotplug detection via asynchronous Tauri events.

### 2. 2.4GHz FastConnect Pairing Wizard
- Seamless wireless pairing state machine replacing legacy factory pairing utilities (`CompxTester` / `max v1.5`).
- Visual countdown and RF carrier test mode validation (`LowCarrier`, `MtkMode`, `AllReceived`).

### 3. Diagnostics & Telemetry
- Interactive microswitch actuation test pad (Left, Right, Wheel, Side buttons 4 & 5).
- Live real-time USB report rate benchmarking and peak rate tracking.

---

## Architecture Overview

```
redragon-drive/
├── src/                          # Modern React 19 / Vite / Tailwind CSS v4 frontend
│   ├── api/                      # Strongly typed Tauri IPC bridge (mouseApi)
│   ├── components/
│   │   ├── layout/               # Header (telemetry, battery, status), Sidebar navigation
│   │   ├── views/                # Dashboard, Performance, Buttons, Sensor, Pairing, Diagnostics, Settings
│   │   └── ui/                   # shadcn / Base UI components
│   └── types/                    # Domain models matching the Rust backend
│
└── src-tauri/                    # Native Rust backend (Tauri v2)
    ├── src/
    │   ├── core/                 # Typed errors (thiserror), domain models, IQR moving-window calculator
    │   ├── driver/               # Safe HIDAPI transport with microcontroller delay guards, device detector & monitor
    │   ├── protocols/            # Byte-exact Compx (GamingPro2635) & Nordic RF packet serialization and CRC
    │   ├── services/             # Business coordinators (Device, Pairing, Diagnostics, Config)
    │   └── commands/             # Tauri IPC invoke handlers
    ├── capabilities/             # Tauri security capabilities
    └── Cargo.toml                # Native dependencies
```

---

## Getting Started

### Prerequisites
- [Bun](https://bun.sh) (v1.4+) or Node.js (18+)
- [Rust toolchain](https://www.rust-lang.org/) (1.80+)
- Linux HID development headers (`libudev-dev`) or Windows / macOS platform dependencies

### Installation

```bash
# Clone the repository
git clone git@github.com:MarkOne-dev/redragon-drive.git
cd redragon-drive

# Install frontend dependencies
bun install
```

### Running Development Server

```bash
# Run desktop app via Tauri v2
bun run tauri dev

# Or run frontend in browser preview mode
bun run dev
```

### Running Tests

```bash
# Backend unit tests
cargo test --manifest-path src-tauri/Cargo.toml

# Frontend typecheck & production build
bun run typecheck
bun run build
```

---

## License
MIT License
