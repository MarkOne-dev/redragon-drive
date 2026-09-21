using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;

namespace Mouse_Drive_Beta;

public class ValueConvert
{
	public static int StringToInt(string str)
	{
		int num = 0;
		if (str.Contains("0x") || str.Contains("0X"))
		{
			return Convert.ToInt32(str, 16);
		}
		return Convert.ToInt32(str);
	}

	public static string IntToVersion(int version)
	{
		return (version >> 8).ToString("x") + "." + (version & 0xFF).ToString("x2");
	}

	public static Version StringToVersion(uint version)
	{
		new Version();
		return new Version((version >> 8).ToString("x") + "." + (version & 0xFF).ToString("x02"));
	}

	public static int DPIIndexToBrightness(int index)
	{
		int num = 0;
		switch (index)
		{
		case 1:
			return 16;
		case 2:
		case 3:
		case 4:
		case 6:
		case 7:
		case 8:
			return 30 * (index - 1);
		case 5:
			return 128;
		case 9:
			return 230;
		case 10:
			return 255;
		default:
			return 128;
		}
	}

	public static int DPIBrightnessToIndex(int value)
	{
		int num = 0;
		if (value % 30 == 0)
		{
			return value / 30 + 1;
		}
		return value switch
		{
			16 => 1, 
			128 => 5, 
			230 => 9, 
			255 => 10, 
			_ => 5, 
		};
	}

	public static bool[] SetDPIEffect(int index)
	{
		bool[] array = new bool[2];
		bool flag = true;
		bool flag2 = true;
		switch (index)
		{
		case 0:
			flag = false;
			flag2 = false;
			break;
		case 1:
			flag2 = false;
			break;
		case 2:
			flag = false;
			break;
		}
		array[0] = flag;
		array[1] = flag2;
		return array;
	}

	public static bool[] SetLEDMode(int index)
	{
		bool[] array = new bool[3];
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		switch (index)
		{
		case 0:
			flag = false;
			flag2 = false;
			flag3 = false;
			break;
		case 1:
		case 4:
		case 5:
			flag = false;
			break;
		case 6:
			flag = false;
			flag3 = false;
			break;
		case 3:
			flag3 = false;
			break;
		}
		array[0] = flag2;
		array[1] = flag3;
		array[2] = flag;
		return array;
	}

	public static int[] StructToInt<T>(T obj)
	{
		int[] array = new int[4];
		int num = Marshal.SizeOf(typeof(T));
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.StructureToPtr(obj, intPtr, fDeleteOld: true);
		array = new int[num];
		Marshal.Copy(intPtr, array, 0, num);
		Marshal.FreeHGlobal(intPtr);
		return array;
	}

	public static T IntToStruct<T>(int[] data)
	{
		byte[] array = new byte[4 * data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			array[i * 4 + 3] = (byte)(data[i] >> 24);
			array[i * 4 + 2] = (byte)(data[i] >> 16);
			array[i * 4 + 1] = (byte)(data[i] >> 8);
			array[i * 4] = (byte)data[i];
		}
		int num = Marshal.SizeOf(typeof(T));
		IntPtr intPtr = Marshal.AllocHGlobal(num);
		Marshal.Copy(array, 0, intPtr, num);
		object obj = (T)Marshal.PtrToStructure(intPtr, typeof(T));
		Marshal.FreeHGlobal(intPtr);
		return (T)obj;
	}

	public static void ClassSetValue<T>(T obj, FieldInfo fieldInfo, string str)
	{
		if (str == null || str == "")
		{
			return;
		}
		if (fieldInfo.FieldType.Name == "Boolean")
		{
			fieldInfo.SetValue(obj, Convert.ToInt32(str) > 0);
		}
		else if (fieldInfo.FieldType.Name == "Int32")
		{
			fieldInfo.SetValue(obj, Convert.ToInt32(str));
		}
		else if (fieldInfo.FieldType.Name == "String")
		{
			fieldInfo.SetValue(obj, str);
		}
		else if (fieldInfo.FieldType.Name == "Color")
		{
			string[] array = str.Split(new char[1] { ',' });
			fieldInfo.SetValue(obj, Color.FromArgb(Convert.ToInt32(array[0].ToString()), Convert.ToInt32(array[1].ToString()), Convert.ToInt32(array[2].ToString())));
		}
		else if (fieldInfo.FieldType.Name.Contains("List"))
		{
			string[] array = str.Split(new char[1] { ',' });
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
			fieldInfo.SetValue(obj, list);
		}
		else if (fieldInfo.FieldType.Name == "Int32[]")
		{
			string[] array = str.Split(new char[1] { ',' });
			int[] array2 = new int[array.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = StringToInt(array[j]);
			}
			fieldInfo.SetValue(obj, array2);
		}
		else if (fieldInfo.FieldType.Name == "Color[]")
		{
			string[] array = str.Split(new char[1] { ',' });
			Color[] array3 = new Color[array.Length / 3];
			for (int k = 0; k < array.Length / 3; k++)
			{
				array3[k] = Color.FromArgb(Convert.ToInt32(array[3 * k].ToString()), Convert.ToInt32(array[3 * k + 1].ToString()), Convert.ToInt32(array[3 * k + 2].ToString()));
			}
			fieldInfo.SetValue(obj, array3);
		}
	}

	public static string ClassGetValue<T>(T obj, FieldInfo fieldInfo)
	{
		string text = "";
		if (fieldInfo.FieldType.Name == "Boolean")
		{
			text = ((!(bool)fieldInfo.GetValue(obj)) ? "0" : "1");
		}
		else if (fieldInfo.FieldType.Name == "Int32")
		{
			text = ((int)fieldInfo.GetValue(obj)).ToString();
		}
		else if (fieldInfo.FieldType.Name == "String")
		{
			text = (string)fieldInfo.GetValue(obj);
		}
		else if (fieldInfo.FieldType.Name == "Color")
		{
			Color color = (Color)fieldInfo.GetValue(obj);
			text = color.R + "," + color.G + "," + color.B;
		}
		else if (fieldInfo.FieldType.Name.Contains("List"))
		{
			List<string> list = (List<string>)fieldInfo.GetValue(obj);
			text = list[0];
			for (int i = 1; i < list.Count; i++)
			{
				text = text + "," + list[i];
			}
		}
		else if (fieldInfo.FieldType.Name == "Int32[]")
		{
			int[] array = (int[])fieldInfo.GetValue(obj);
			text = array[0].ToString();
			for (int j = 1; j < array.Length; j++)
			{
				text = text + "," + array[j];
			}
		}
		else if (fieldInfo.FieldType.Name == "Color[]")
		{
			Color[] array2 = (Color[])fieldInfo.GetValue(obj);
			text = array2[0].R + "," + array2[0].G + "," + array2[0].B;
			for (int k = 1; k < array2.Length; k++)
			{
				text = text + "," + array2[k].R + "," + array2[k].G + "," + array2[k].B;
			}
		}
		return text;
	}

	public static List<MacroContext> App_AddMultiMedia(byte[] value)
	{
		List<MacroContext> list = new List<MacroContext>();
		list.Clear();
		MacroContext item = new MacroContext(0)
		{
			type = 2,
			keyState = 0
		};
		item.value[0] = value[0];
		item.value[1] = value[1];
		list.Add(item);
		item = new MacroContext(0)
		{
			type = 2,
			keyState = 1
		};
		item.value[0] = value[0];
		item.value[1] = value[1];
		list.Add(item);
		return list;
	}

	public static void App_AddShortCutKey(ref FlashDataMap flashDataMap, int index, List<byte> value)
	{
		new List<MacroContext>().Clear();
		int num = 0;
		flashDataMap.shortCutKey[index].contextCount = (byte)(value.Count / 3 * 2);
		for (int i = 0; i < value.Count / 3; i++)
		{
			MacroContext macroContext = new MacroContext(0);
			macroContext.type = value[i * 3];
			macroContext.keyState = 0;
			macroContext.value[0] = value[i * 3 + 1];
			macroContext.value[1] = value[i * 3 + 2];
			flashDataMap.shortCutKey[index].context[num++] = macroContext;
		}
		for (int num2 = value.Count / 3 - 1; num2 >= 0; num2--)
		{
			MacroContext macroContext = new MacroContext(0);
			macroContext.type = value[num2 * 3];
			macroContext.keyState = 1;
			macroContext.value[0] = value[num2 * 3 + 1];
			macroContext.value[1] = value[num2 * 3 + 2];
			flashDataMap.shortCutKey[index].context[num++] = macroContext;
		}
	}

	public static MacroContext[] App_StringToShortcutKey(string str)
	{
		string[] array = str.Split(new char[1] { '+' });
		MacroContext[] array2 = new MacroContext[array.Length * 2];
		for (int i = 0; i < array.Length; i++)
		{
			MacroContext macroContext = new MacroContext(0);
			KeyboardCode keyboardCode = KeyboardCodes.FindKeyboardCode(array[i]);
			macroContext.type = (byte)keyboardCode.hidCodeType;
			macroContext.keyState = 0;
			macroContext.value[0] = (byte)keyboardCode.hidCode;
			macroContext.value[1] = 0;
			array2[i] = macroContext;
			macroContext = new MacroContext(0);
			macroContext.type = (byte)keyboardCode.hidCodeType;
			macroContext.keyState = 1;
			macroContext.value[0] = (byte)keyboardCode.hidCode;
			macroContext.value[1] = 0;
			array2[array.Length + i] = macroContext;
		}
		return array2;
	}

	public static int ComboxIndexToValue(CustomComboBox comboBox)
	{
		int result = 0;
		for (int i = 0; i < LanguageFile.ComboBoxStructs.Count; i++)
		{
			if (((Control)comboBox).Name == LanguageFile.ComboBoxStructs[i].Name)
			{
				result = LanguageFile.ComboBoxStructs[i].Values[comboBox.SelectIndex];
				break;
			}
		}
		return result;
	}

	public static int ValueToComboxIndex(CustomComboBox comboBox, int value)
	{
		bool flag = false;
		int i = 0;
		for (int j = 0; j < LanguageFile.ComboBoxStructs.Count; j++)
		{
			if (((Control)comboBox).Name == LanguageFile.ComboBoxStructs[j].Name)
			{
				for (i = 0; i < LanguageFile.ComboBoxStructs[j].Values.Count; i++)
				{
					if (value == LanguageFile.ComboBoxStructs[j].Values[i])
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		if (!flag)
		{
			i = 0;
		}
		return i;
	}
}
