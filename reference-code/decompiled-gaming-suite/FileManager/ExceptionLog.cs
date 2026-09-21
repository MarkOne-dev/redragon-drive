using System;
using System.IO;
using Mouse_Drive_Beta.FileManager;

namespace FileManager;

public class ExceptionLog
{
	public void WriteExceptionLog(string str)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		string text = folderPath + "\\" + DriveConfig.GetCompany() + "\\" + DriveConfig.GetDriveName() + "\\Exception";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string text2 = "\\Exception_log_" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + " .txt";
		FileStream fileStream = new FileStream(text + text2, FileMode.Create, FileAccess.Write);
		DateTime now = DateTime.Now;
		fileStream.Position = fileStream.Length;
		StreamWriter streamWriter = new StreamWriter(fileStream);
		streamWriter.WriteLine();
		streamWriter.Write("-------------------");
		streamWriter.Write(now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
		streamWriter.Write("-------------------");
		streamWriter.WriteLine();
		streamWriter.WriteLine(str);
		streamWriter.Flush();
		fileStream.Flush();
		streamWriter.Close();
		fileStream.Close();
		streamWriter.Dispose();
		fileStream.Dispose();
	}

	public ExceptionLog(string str)
	{
		WriteExceptionLog(str);
	}
}
