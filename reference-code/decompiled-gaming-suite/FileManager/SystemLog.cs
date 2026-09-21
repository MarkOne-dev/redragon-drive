using System;
using System.IO;
using Mouse_Drive_Beta.FileManager;

namespace FileManager;

public class SystemLog
{
	public string LogPath = "";

	public void CreateLog(string logPath)
	{
		if (DriveConfig.GetDebug() == 1)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string text = folderPath + "\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName();
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			LogPath = text + "\\" + logPath + ".txt";
			FileStream fileStream = new FileStream(LogPath, FileMode.Create, FileAccess.Write);
			fileStream.Flush();
			fileStream.Close();
			fileStream.Dispose();
		}
		WriteLog("System Running");
	}

	public void WriteLog(string str)
	{
		if (DriveConfig.GetDebug() == 1)
		{
			string value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + str;
			if (!File.Exists(LogPath))
			{
				FileStream fileStream = new FileStream(LogPath, FileMode.Create, FileAccess.Write);
				fileStream.Flush();
				fileStream.Close();
				fileStream.Dispose();
			}
			FileStream fileStream2 = new FileStream(LogPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			fileStream2.Position = fileStream2.Length;
			StreamWriter streamWriter = new StreamWriter(fileStream2);
			streamWriter.WriteLine(value);
			streamWriter.Flush();
			fileStream2.Flush();
			streamWriter.Close();
			fileStream2.Close();
			streamWriter.Dispose();
			fileStream2.Dispose();
		}
	}

	public SystemLog(string logPath)
	{
		CreateLog(logPath);
	}
}
