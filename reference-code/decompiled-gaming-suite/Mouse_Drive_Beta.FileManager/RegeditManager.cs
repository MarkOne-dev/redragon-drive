using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Mouse_Drive_Beta.FileManager;

public class RegeditManager
{
	public static BatteryHandleParam GetBatteryLevel(byte CID, byte MID, bool haveRollingCode, byte[] rollingCode)
	{
		BatteryHandleParam batteryHandleParam = new BatteryHandleParam();
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		if (registryKey != null)
		{
			string text = (string)registryKey.GetValue("BatteryHandleParam_" + CID + "_" + MID);
			if (text == null)
			{
				registryKey.Close();
				return null;
			}
			string[] array = text.Split(new char[1] { '_' });
			batteryHandleParam.level = Convert.ToByte(array[0]);
			batteryHandleParam.haveRollingCode = Convert.ToByte(array[2]);
			batteryHandleParam.rollingCode = new List<byte>();
			TimeSpan timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(array[1])).Duration();
			if (timeSpan.Days > 1 || timeSpan.Hours > 1)
			{
				batteryHandleParam.sec = 2000;
			}
			else
			{
				batteryHandleParam.sec = timeSpan.Seconds + timeSpan.Minutes * 60;
			}
			for (int i = 3; i < array.Length; i++)
			{
				batteryHandleParam.rollingCode.Add(Convert.ToByte(array[i], 16));
			}
			if (haveRollingCode && batteryHandleParam.haveRollingCode > 0)
			{
				for (int j = 0; j < batteryHandleParam.rollingCode.Count; j++)
				{
					if (batteryHandleParam.rollingCode[j] != rollingCode[j])
					{
						registryKey.Close();
						return null;
					}
				}
			}
			registryKey.Close();
			return batteryHandleParam;
		}
		return null;
	}

	public static void SaveBatteryLevel(byte DisplayPercentage, byte CID, byte MID, byte HaveRollingCode, byte[] RollingCode)
	{
		DateTime now = DateTime.Now;
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		string text = "";
		text += DisplayPercentage.ToString("d2");
		text = text + "_" + now;
		text = text + "_" + HaveRollingCode.ToString("x2");
		for (int i = 0; i < RollingCode.Length; i++)
		{
			text = text + "_" + RollingCode[i].ToString("x2");
		}
		registryKey.SetValue("BatteryHandleParam_" + CID + "_" + MID, text);
		registryKey.Close();
	}

	public static void SaveLanguageIndex(int index)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		}
		string value = index.ToString("d2");
		registryKey.SetValue("LanguageIndex", value);
		registryKey.Close();
	}

	public static bool GetLanguageIndex(out int index)
	{
		bool flag = false;
		index = 0;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		if (registryKey != null)
		{
			string text = (string)registryKey.GetValue("LanguageIndex");
			if (text == null)
			{
				return false;
			}
			index = Convert.ToInt32(text, 10);
			registryKey.Close();
			return true;
		}
		return false;
	}

	public static void SetUpdateFailInfo(byte type, byte is4K, byte cid, byte mid)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		}
		string text = type.ToString("x2");
		text = text + "," + is4K.ToString("x2");
		text = text + "," + cid.ToString("x2");
		text = text + "," + mid.ToString("x2");
		registryKey.SetValue("UpdateFailInfo", text);
		registryKey.Close();
	}

	public static bool GetUpdateFailInfo(out List<byte> data)
	{
		bool flag = false;
		data = new List<byte>();
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		if (registryKey != null)
		{
			string text = (string)registryKey.GetValue("UpdateFailInfo");
			if (text == null)
			{
				return false;
			}
			string[] array = text.Split(new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				data.Add((byte)Convert.ToInt32(array[i], 16));
			}
			registryKey.Close();
			return true;
		}
		return false;
	}

	public static void ClearUpdateFailInfo()
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		}
		if (registryKey.GetValue("UpdateFailInfo") != null)
		{
			registryKey.DeleteValue("UpdateFailInfo", throwOnMissingValue: true);
			registryKey.Close();
		}
	}

	public static void SetRunBoot(bool enable, string name, string path)
	{
		RegistryKey currentUser = Registry.CurrentUser;
		RegistryKey registryKey = currentUser.CreateSubKey("SOFTWARE\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Run");
		if (enable)
		{
			registryKey.SetValue(name, path);
			currentUser.Close();
		}
		else
		{
			registryKey.DeleteValue(name);
			currentUser.Close();
		}
	}

	public static bool GetRunBoot(string name, string path)
	{
		bool result = false;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Run");
		if (registryKey != null && registryKey.GetValue(name) != null && (string)registryKey.GetValue(name) == path)
		{
			result = true;
		}
		return result;
	}

	public static bool GetDotNetRelease(int release)
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\");
		if (registryKey != null && registryKey.GetValue("Release") != null)
		{
			return ((int)registryKey.GetValue("Release") >= release) ? true : false;
		}
		return false;
	}

	public static bool GetDotNetVersion(string version)
	{
		string text = "0";
		using (RegistryKey registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\"))
		{
			string[] subKeyNames = registryKey.GetSubKeyNames();
			foreach (string text2 in subKeyNames)
			{
				if (!text2.StartsWith("v"))
				{
					continue;
				}
				RegistryKey registryKey2 = registryKey.OpenSubKey(text2);
				string text3 = (string)registryKey2.GetValue("Version", "");
				if (string.Compare(text3, text) > 0)
				{
					text = text3;
				}
				if (text3 != "")
				{
					continue;
				}
				string[] subKeyNames2 = registryKey2.GetSubKeyNames();
				foreach (string name in subKeyNames2)
				{
					text3 = (string)registryKey2.OpenSubKey(name).GetValue("Version", "");
					if (string.Compare(text3, text) > 0)
					{
						text = text3;
					}
				}
			}
		}
		if (string.Compare(text, version) <= 0)
		{
			return false;
		}
		return true;
	}

	public static void SetMouseVersion(byte CID, byte MID, string version)
	{
		bool flag = false;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
			flag = true;
		}
		else if ((string)registryKey.GetValue("MouseVer" + CID.ToString("X02") + MID.ToString("X02")) != version)
		{
			flag = true;
		}
		if (flag)
		{
			registryKey.SetValue("MouseVer" + CID.ToString("X02") + MID.ToString("X02"), version);
		}
		registryKey.Close();
	}

	public static string GetMouseVersion(byte CID, byte MID)
	{
		string result = null;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		if (registryKey != null)
		{
			result = (string)registryKey.GetValue("MouseVer" + CID.ToString("X02") + MID.ToString("X02"));
			registryKey.Close();
		}
		return result;
	}

	public static void SetFirmwareUpdateTips()
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		if (registryKey == null)
		{
			registryKey = Registry.CurrentUser.CreateSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
			registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName(), writable: true);
		}
		registryKey.SetValue("FirmwareUpdateTips", "01");
		registryKey.Close();
	}

	public static bool GetFirmwareUpdateTips()
	{
		bool result = false;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Compx\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName());
		if (registryKey != null)
		{
			if ((string)registryKey.GetValue("FirmwareUpdateTips") != null)
			{
				result = true;
			}
			registryKey.Close();
		}
		return result;
	}
}
