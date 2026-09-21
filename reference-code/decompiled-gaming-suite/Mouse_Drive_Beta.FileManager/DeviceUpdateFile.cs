using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using USBUpdateTool;

namespace Mouse_Drive_Beta.FileManager;

public class DeviceUpdateFile
{
	public static bool HasDeviceUpdateFile(byte type, string currentVersion, string mcu, byte cid, byte mid, out byte[] buffers)
	{
		bool result = false;
		new Version((currentVersion == "") ? "0.0" : currentVersion.Replace("v", ""));
		Version version = new Version((currentVersion == "") ? "0.0" : currentVersion.Replace("v", ""));
		buffers = new byte[10];
		string path = Environment.CurrentDirectory + "\\bin";
		if (!Directory.Exists(path))
		{
			return result;
		}
		string[] files = Directory.GetFiles(path, "*.bin");
		new List<UpgradeFileHeader>();
		new List<string>();
		for (int i = 0; i < files.Length; i++)
		{
			byte[] array = File.ReadAllBytes(files[i]);
			UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(array);
			string text = Encoding.UTF8.GetString(upgradeFileHeader.icName).Replace("\0", "");
			if (type == upgradeFileHeader.DeciveType && cid == upgradeFileHeader.Cid && mid == upgradeFileHeader.Mid && mcu == text)
			{
				Version version2 = ValueConvert.StringToVersion(upgradeFileHeader.version);
				if (version2 > version || currentVersion == "")
				{
					result = true;
					buffers = new byte[array.Length];
					Array.Copy(array, 0, buffers, 0, array.Length);
				}
			}
		}
		return result;
	}

	public static bool HasNewVersion(byte type, string currentVersion, string mcu, byte cid, byte mid)
	{
		bool result = false;
		Version version = new Version((currentVersion == "") ? "0.0" : currentVersion.Replace("v", ""));
		string path = Environment.CurrentDirectory + "\\bin";
		if (!Directory.Exists(path))
		{
			return result;
		}
		string[] files = Directory.GetFiles(path, "*.bin");
		new List<UpgradeFileHeader>();
		new List<string>();
		for (int i = 0; i < files.Length; i++)
		{
			UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(File.ReadAllBytes(files[i]));
			string text = Encoding.UTF8.GetString(upgradeFileHeader.icName).Replace("\0", "");
			if (type == upgradeFileHeader.DeciveType && cid == upgradeFileHeader.Cid && mid == upgradeFileHeader.Mid && mcu == text && (ValueConvert.StringToVersion(upgradeFileHeader.version) > version || currentVersion == ""))
			{
				result = true;
				break;
			}
		}
		return result;
	}
}
