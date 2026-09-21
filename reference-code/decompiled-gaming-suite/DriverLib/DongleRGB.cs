using System.Runtime.InteropServices;

namespace DriverLib;

public struct DongleRGB(int key)
{
	public byte mode = 0;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] color1 = new byte[3];

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] color2 = new byte[3];

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
	public byte[] color3 = new byte[3];
}
