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

## Getting Started

### Prerequisites
- [Bun](https://bun.sh) (v1.4+) or Node.js (18+)
- [Rust toolchain](https://www.rust-lang.org/) (1.80+)
- Linux HID development headers (`libudev-dev`) or Windows / macOS platform dependencies

### Installation from Source

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

### Building Packages for Linux

```bash
# Compile and generate Linux release bundles (.AppImage, .deb, .rpm)
NO_STRIP=true bun run tauri build
```

---

## Linux Installation by Distribution

### 1. Arch Linux / CachyOS / Manjaro / EndeavourOS

#### Option A: Standalone AppImage (Fastest)
```bash
chmod +x "Redragon M916 Suite_0.3.0_amd64.AppImage"
./"Redragon M916 Suite_0.3.0_amd64.AppImage"
```
If your system environment lacks FUSE2, run with the extraction flag:
```bash
./"Redragon M916 Suite_0.3.0_amd64.AppImage" --appimage-extract-and-run
```

#### Option B: Convert and Install via debtap
```bash
# Install debtap if not present (available in AUR: yay -S debtap)
sudo debtap -u
debtap "Redragon M916 Suite_0.3.0_amd64.deb"
sudo pacman -U redragon-m916-suite-*.pkg.tar.zst
```

#### Option C: Native Build on Arch / CachyOS
```bash
sudo pacman -S --needed base-devel webkit2gtk-4.1 libsoup3 openssl libappindicator-gtk3
bun install
NO_STRIP=true bun run tauri build
```

---

### 2. Ubuntu / Debian / Linux Mint / Pop!_OS

Install the generated `.deb` package:
```bash
sudo apt install ./"Redragon M916 Suite_0.3.0_amd64.deb"
```
Or with `dpkg`:
```bash
sudo dpkg -i ./"Redragon M916 Suite_0.3.0_amd64.deb"
sudo apt-get install -f
```

---

### 3. Fedora / RHEL / openSUSE

Install the generated `.rpm` package:
```bash
# Fedora / RHEL
sudo dnf install ./"Redragon M916 Suite-0.3.0-1.x86_64.rpm"

# openSUSE
sudo zypper install ./"Redragon M916 Suite-0.3.0-1.x86_64.rpm"
```

---

### 4. Universal Linux (AppImage)

Compatible with any Linux distribution with modern glibc:
```bash
chmod +x "Redragon M916 Suite_0.3.0_amd64.AppImage"
./"Redragon M916 Suite_0.3.0_amd64.AppImage"
```

---

## USB Permissions (udev rules)

To allow the application to communicate with the mouse and 2.4GHz wireless dongle via `/dev/hidraw*` without requiring superuser (root) privileges, add the following udev rule:

```bash
echo 'KERNEL=="hidraw*", ATTRS{idVendor}=="3554", MODE="0666"' | sudo tee /etc/udev/rules.d/99-redragon.rules
sudo udevadm control --reload-rules && sudo udevadm trigger
```

---

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

