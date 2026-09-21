namespace DriverLib;

public class ShortCutKeyDriver
{
	public ShortCutKey shortCutKey = new ShortCutKey(0);

	public void Clear()
	{
		shortCutKey.contextCount = 0;
	}

	public void AddShortCutKey(KEY_STATE keyState, HID_CODE_TYPE keyType, int hidCode, uint delay)
	{
		if (shortCutKey.contextCount < 6)
		{
			int num = shortCutKey.contextCount++;
			shortCutKey.context[num].keyState = (byte)keyState;
			shortCutKey.context[num].type = (byte)keyType;
			shortCutKey.context[num].value[0] = (byte)hidCode;
			shortCutKey.context[num].value[1] = (byte)(hidCode >> 8);
			shortCutKey.context[num].delay = 0u;
		}
	}

	public void GetShortCutKeyData(ref ShortCutKey outShortCutKey)
	{
		outShortCutKey.contextCount = shortCutKey.contextCount;
		for (int i = 0; i < shortCutKey.contextCount; i++)
		{
			outShortCutKey.context[i].keyState = shortCutKey.context[i].keyState;
			outShortCutKey.context[i].type = shortCutKey.context[i].type;
			outShortCutKey.context[i].value[0] = shortCutKey.context[i].value[0];
			outShortCutKey.context[i].value[1] = shortCutKey.context[i].value[1];
			outShortCutKey.context[i].delay = 0u;
		}
	}
}
