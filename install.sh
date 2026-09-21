#!/usr/bin/env bash
# Redragon M916 Suite - Linux Installer
set -e

REPO="MarkOne-dev/redragon-drive"
APP_NAME="redragon-m916-suite"
DISPLAY_NAME="Redragon M916 Suite"
ICON_NAME="redragon-m916-suite"
BIN_DEST="/usr/local/bin/${APP_NAME}"
DESKTOP_DIR="/usr/share/applications"
ICON_DIR="/usr/share/icons/hicolor/512x512/apps"
UDEV_RULE_FILE="/etc/udev/rules.d/99-redragon.rules"

echo "=== Redragon M916 Suite Installer ==="

# Check architecture
ARCH=$(uname -m)
if [ "$ARCH" != "x86_64" ]; then
    echo "Error: Only x86_64 architecture is supported at this time." >&2
    exit 1
fi

# Detect sudo/root privileges
SUDO=""
if [ "$(id -u)" -ne 0 ]; then
    if command -v sudo >/dev/null 2>&1; then
        SUDO="sudo"
    else
        echo "Error: This installer requires root privileges or sudo to install system-wide." >&2
        exit 1
    fi
fi

TEMP_DIR=$(mktemp -d)
cleanup() {
    rm -rf "$TEMP_DIR"
}
trap cleanup EXIT

# 1. Obtain AppImage binary
APPIMAGE_SRC=""
# Check if running locally within the repository
LOCAL_APPIMAGE=$(ls -t src-tauri/target/release/bundle/appimage/Redragon*.AppImage 2>/dev/null | head -n1 || true)

if [ -n "$LOCAL_APPIMAGE" ] && [ -f "$LOCAL_APPIMAGE" ]; then
    echo "Found local compiled AppImage: $LOCAL_APPIMAGE"
    APPIMAGE_SRC="$LOCAL_APPIMAGE"
else
    echo "Fetching latest release from GitHub (${REPO})..."
    DOWNLOAD_URL=$(curl -s "https://api.github.com/repos/${REPO}/releases/latest" \
        | grep -o 'https://[^"]*Redragon[^"]*\.AppImage' \
        | head -n1 || true)

    if [ -z "$DOWNLOAD_URL" ]; then
        # Fallback to direct tag download URL if API is rate-limited
        DOWNLOAD_URL="https://github.com/${REPO}/releases/latest/download/Redragon%20M916%20Suite_0.3.0_amd64.AppImage"
    fi

    echo "Downloading installer from ${DOWNLOAD_URL}..."
    curl -fSL "$DOWNLOAD_URL" -o "${TEMP_DIR}/${APP_NAME}.AppImage"
    APPIMAGE_SRC="${TEMP_DIR}/${APP_NAME}.AppImage"
fi

# 2. Install binary to /usr/local/bin
echo "Installing binary to ${BIN_DEST}..."
$SUDO install -Dm755 "$APPIMAGE_SRC" "$BIN_DEST"

# 3. Install application icon
echo "Installing icon to ${ICON_DIR}..."
$SUDO mkdir -p "$ICON_DIR"
if [ -f "src-tauri/icons/128x128@2x.png" ]; then
    $SUDO cp "src-tauri/icons/128x128@2x.png" "${ICON_DIR}/${ICON_NAME}.png"
elif [ -f "public/assets/logo.png" ]; then
    $SUDO cp "public/assets/logo.png" "${ICON_DIR}/${ICON_NAME}.png"
else
    curl -fsSL "https://raw.githubusercontent.com/${REPO}/main/public/app-icon.png" -o "${TEMP_DIR}/app-icon.png" || true
    if [ -f "${TEMP_DIR}/app-icon.png" ]; then
        $SUDO install -Dm644 "${TEMP_DIR}/app-icon.png" "${ICON_DIR}/${ICON_NAME}.png"
    fi
fi

# 4. Create desktop entry
echo "Creating desktop entry in ${DESKTOP_DIR}..."
$SUDO mkdir -p "$DESKTOP_DIR"
cat <<EOF | $SUDO tee "${DESKTOP_DIR}/${APP_NAME}.desktop" > /dev/null
[Desktop Entry]
Name=${DISPLAY_NAME}
Comment=High-performance gaming mouse management suite for Redragon M916-PRO
Exec=${BIN_DEST} %U
Icon=${ICON_NAME}
Terminal=false
Type=Application
Categories=Utility;Settings;HardwareSettings;
Keywords=redragon;mouse;gaming;dpi;paw3395;
StartupWMClass=redragon-app
EOF

# Update desktop database if available
if command -v update-desktop-database >/dev/null 2>&1; then
    $SUDO update-desktop-database "$DESKTOP_DIR" >/dev/null 2>&1 || true
fi

# 5. Configure udev rules for non-root access
echo "Configuring udev rules for Compx / Redragon USB devices..."
cat <<EOF | $SUDO tee "$UDEV_RULE_FILE" > /dev/null
# Redragon M916-PRO / Compx HID raw device permissions
KERNEL=="hidraw*", ATTRS{idVendor}=="3554", MODE="0666", TAG+="uaccess"
SUBSYSTEM=="usb", ATTRS{idVendor}=="3554", MODE="0666", TAG+="uaccess"
EOF

if command -v udevadm >/dev/null 2>&1; then
    $SUDO udevadm control --reload-rules || true
    $SUDO udevadm trigger || true
fi

echo ""
echo "Installation completed successfully."
echo "You can now launch '${DISPLAY_NAME}' from your application menu or run '${APP_NAME}' in terminal."
