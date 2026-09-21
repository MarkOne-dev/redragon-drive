using System.Runtime.InteropServices;

namespace DriverLib;

public struct MacroKey
{
	public byte nameLength = 0;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
	public byte[] name = new byte[30];

	public byte contextCount = 0;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 70)]
	public MacroContext[] context = new MacroContext[70];

	public MacroKey(int key)
	{
		for (int i = 0; i < 70; i++)
		{
			context[i] = new MacroContext(0);
		}
	}
}
