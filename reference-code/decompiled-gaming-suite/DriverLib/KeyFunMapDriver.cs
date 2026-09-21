namespace DriverLib;

public class KeyFunMapDriver
{
	public void CloseMouseKey(ref KeyFunMap keyFunMap)
	{
		keyFunMap.type = 0;
		keyFunMap.param1 = 0;
		keyFunMap.param2 = 0;
	}

	public void SetMouseKey(ref KeyFunMap keyFunMap, MouseKey mouseKey)
	{
		keyFunMap.type = 1;
		keyFunMap.param1 = (byte)mouseKey;
		keyFunMap.param2 = 0;
	}

	public void SetChangeDPIKey(ref KeyFunMap keyFunMap, ChangeDPIKey dpiKey)
	{
		keyFunMap.type = 2;
		keyFunMap.param1 = (byte)dpiKey;
		keyFunMap.param2 = 0;
	}

	public void SetAcPanKey(ref KeyFunMap keyFunMap, ACPAN acPan)
	{
		keyFunMap.type = 3;
		keyFunMap.param1 = (byte)acPan;
		keyFunMap.param2 = 0;
	}

	public void SetFireKey(ref KeyFunMap keyFunMap, byte speed, byte count)
	{
		keyFunMap.type = 4;
		keyFunMap.param1 = speed;
		keyFunMap.param2 = count;
	}

	public void SetShortCutKey(ref KeyFunMap keyFunMap)
	{
		keyFunMap.type = 5;
		keyFunMap.param1 = 0;
		keyFunMap.param2 = 0;
	}

	public void SetMacroKey(ref KeyFunMap keyFunMap, byte macroId, byte repeatCount)
	{
		keyFunMap.type = 6;
		keyFunMap.param1 = macroId;
		keyFunMap.param2 = repeatCount;
	}

	public void SetChangeReportRateKey(ref KeyFunMap keyFunMap)
	{
		keyFunMap.type = 7;
		keyFunMap.param1 = 0;
		keyFunMap.param2 = 0;
	}

	public void SetChangeConfigKey(ref KeyFunMap keyFunMap)
	{
		keyFunMap.type = 9;
		keyFunMap.param1 = 0;
		keyFunMap.param2 = 0;
	}

	public void SetDPILockKey(ref KeyFunMap keyFunMap, byte dpiValue)
	{
		keyFunMap.type = 10;
		keyFunMap.param1 = dpiValue;
		keyFunMap.param2 = 0;
	}
}
