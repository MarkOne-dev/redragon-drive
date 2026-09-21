/// Vendor ID for Compx Technology (used by Redragon wireless mice and dongles)
pub const COMPX_VENDOR_ID: u16 = 0x3554;

/// Known Product IDs for Redragon M916-PRO 1K (extracted from Config.ini and DriverLib)
pub const KNOWN_PIDS: &[u16] = &[
    0xF55E, // M916-PRO Mouse (Wired / Direct connection)
    0xF55D, // M916-PRO Dongle 2.4G (Wireless receiver)
    0xF55F, // M916-PRO Alternate Dongle
    0xF501, // M916-PRO Dongle v1
    0x2635, // GamingPro2635 Standard
    0xF502,
    0xF503,
    0x0852,
];

/// Official USB Command IDs (DriverLib/UsbCommandID.cs)
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(u8)]
pub enum UsbCommandId {
    EncryptionData = 1,
    PCDriverStatus = 2,
    DeviceOnLine = 3,
    BatteryLevel = 4,
    DongleEnterPair = 5,
    GetPairState = 6,
    WriteFlashData = 7,
    ReadFlashData = 8,
    ClearSetting = 9,
    StatusChanged = 10,
    SetDeviceVidPid = 11,
    SetDeviceDescriptorString = 12,
    EnterUsbUpdateMode = 13,
    GetCurrentConfig = 14,
    SetCurrentConfig = 15,
    ReadCidMid = 16,
    EnterMTKMode = 17,
    ReadVersionId = 18,
    Set4KDongleRGB = 20,
    Get4KDongleRGBValue = 21,
    SetLongRangeMode = 22,
    GetLongRangeMode = 23,
}

/// Direct communication opcodes
pub mod opcodes {
    pub const CMD_PAIR_START: u8 = 0xA0;
    pub const CMD_CHECK_PID: u8 = 0x10;
    pub const CMD_GET_VERSION: u8 = 0x11;
    pub const CMD_SET_DPI: u8 = 0x20;
    pub const CMD_SET_POLLING_RATE: u8 = 0x21;
}
