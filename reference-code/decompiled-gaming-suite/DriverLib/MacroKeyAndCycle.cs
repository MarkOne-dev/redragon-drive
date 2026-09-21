namespace DriverLib;

public struct MacroKeyAndCycle(int key)
{
	public MacroKey macroKey = new MacroKey(0);

	public byte CycleTimes = 1;

	public string name = "";
}
