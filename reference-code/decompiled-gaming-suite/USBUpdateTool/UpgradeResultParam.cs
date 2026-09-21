namespace USBUpdateTool;

internal enum UpgradeResultParam
{
	Success = 1,
	TimeOutError,
	DeviceNotMatch,
	MultiNormalDeviceError,
	MultiBootDeviceError,
	NotBootFoundDeviceError,
	NormalUsbError,
	BootUsbError,
	BootDeviceError
}
