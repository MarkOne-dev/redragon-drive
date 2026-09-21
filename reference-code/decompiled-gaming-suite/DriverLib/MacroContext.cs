using System.Runtime.InteropServices;

namespace DriverLib;

public struct MacroContext
{
	public byte keyState = 1;

	public byte type = 1;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
	public byte[] value = new byte[2];

	public uint delay;

	public MacroContext(int key)
	{
		value[0] = 0;
		value[1] = 0;
		delay = 0u;
	}
}
