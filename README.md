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

## Installation

Download the latest release for your Linux distribution from [GitHub Releases](https://github.com/MarkOne-dev/redragon-drive/releases/latest).

### 1. Arch Linux / CachyOS / Manjaro / EndeavourOS

#### Method A: AUR Helper (paru / yay)
Install directly using your preferred AUR package manager:
```bash
# Using paru
paru -S redragon-m916-suite-bin

# Using yay
yay -S redragon-m916-suite-bin
```

#### Method B: Standalone AppImage
Download the `.AppImage` from GitHub Releases, make it executable, and run:
```bash
chmod +x Redragon*.AppImage
./Redragon*.AppImage
```
If your system lacks FUSE2, run with:
```bash
./Redragon*.AppImage --appimage-extract-and-run
```

#### Method C: Convert from deb via debtap
```bash
# Update debtap database if needed
sudo debtap -u
debtap Redragon*.deb
sudo pacman -U redragon-m916-suite-*.pkg.tar.zst
```

---

### 2. Ubuntu / Debian / Linux Mint / Pop!_OS

Download the `.deb` package from GitHub Releases and install with `apt`:
```bash
sudo apt install ./Redragon*.deb
```

---

### 3. Fedora / RHEL / openSUSE

Download the `.rpm` package from GitHub Releases and install with your package manager:
```bash
# Fedora / RHEL
sudo dnf install ./Redragon*.rpm

# openSUSE
sudo zypper install ./Redragon*.rpm
```

---

### 4. Universal Linux (AppImage)

Compatible with any modern Linux distribution:
```bash
chmod +x Redragon*.AppImage
./Redragon*.AppImage
```

---

## USB Permissions (udev rules)

To allow the application to configure the mouse and 2.4GHz wireless dongle via `/dev/hidraw*` without requiring root permissions, install the udev rule:

```bash
echo 'KERNEL=="hidraw*", ATTRS{idVendor}=="3554", MODE="0666"' | sudo tee /etc/udev/rules.d/99-redragon.rules
sudo udevadm control --reload-rules && sudo udevadm trigger
```

---

## License
MIT License

