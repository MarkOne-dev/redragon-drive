using System.Runtime.InteropServices;

namespace DriverLib;

public struct ShortCutKey
{
	public byte contextCount = 0;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public MacroContext[] context = new MacroContext[6];

	public ShortCutKey(int key)
	{
		for (int i = 0; i < 6; i++)
		{
			context[i] = new MacroContext(0);
		}
	}
}
