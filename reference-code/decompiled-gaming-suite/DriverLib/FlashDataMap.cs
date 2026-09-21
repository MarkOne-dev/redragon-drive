using System.Runtime.InteropServices;

namespace DriverLib;

public struct FlashDataMap(int key)
{
	public MouseConfig mouseConfig = default(MouseConfig);

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public DPIConfig[] dpiConfig = new DPIConfig[8];

	public DPILed dpiLed = default(DPILed);

	public LedBar ledBar = default(LedBar);

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public KeyFunMap[] keys = new KeyFunMap[16];

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public ShortCutKey[] shortCutKey = new ShortCutKey[16];

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public MacroKey[] macroKey = new MacroKey[16];
}
