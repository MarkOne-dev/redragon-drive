using System.ComponentModel;

namespace DriverLib;

[TypeConverter(typeof(RangeConverter))]
public class KeyboardCode
{
	public int vkCode;

	public int scanCode;

	public int flags;

	public int hidCode;

	public HID_CODE_TYPE hidCodeType;

	public string keyChar = "";

	public KeysEnum keyCode;

	public KeyboardCode()
	{
		vkCode = 0;
		scanCode = 0;
		flags = 0;
		hidCode = 0;
		hidCodeType = HID_CODE_TYPE.Normal;
		keyChar = "";
		keyCode = KeysEnum.None;
	}

	public KeyboardCode(int vkCode, int scanCode, int flags, int hidCode, string keyChar, KeysEnum keycode)
	{
		this.vkCode = vkCode;
		this.scanCode = scanCode;
		this.flags = flags;
		this.hidCode = hidCode;
		hidCodeType = HID_CODE_TYPE.Normal;
		this.keyChar = keyChar;
		keyCode = keycode;
	}

	public KeyboardCode(int vkCode, int scanCode, int flags, int hidCode, HID_CODE_TYPE hidCodeType, string keyChar, KeysEnum keycode)
	{
		this.vkCode = vkCode;
		this.scanCode = scanCode;
		this.flags = flags;
		this.hidCode = hidCode;
		this.hidCodeType = hidCodeType;
		this.keyChar = keyChar;
		keyCode = keycode;
	}
}
