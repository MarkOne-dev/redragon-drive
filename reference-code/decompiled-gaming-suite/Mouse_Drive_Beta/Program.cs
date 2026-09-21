using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using FileManager;
using Mouse_Drive_Beta.FileManager;

namespace Mouse_Drive_Beta;

internal static class Program
{
	private static Mutex mutex;

	public const int SW_RESTORE = 9;

	public static IntPtr formhwnd;

	[DllImport("user32.dll")]
	public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

	[DllImport("user32.dll ", SetLastError = true)]
	private static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

	[STAThread]
	private static void Main()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		Application.SetUnhandledExceptionMode((UnhandledExceptionMode)2);
		Application.ThreadException += Application_ThreadException;
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		if (Thread.CurrentThread.CurrentUICulture.Name != "zh-CN" && Thread.CurrentThread.CurrentUICulture.Name != "en-US")
		{
			CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
		}
		if (!RegeditManager.GetDotNetVersion("4.8"))
		{
			string text = AppDomain.CurrentDomain.FriendlyName + " - This application could not be started.";
			if ((int)MessageBox.Show("This application requires one of the following versions of the NET Framework:\r\n.NETFramework.Version=v4.8\r\n\r\nDo you want to install this .NET Framewok version now?", text, (MessageBoxButtons)4, (MessageBoxIcon)16) == 7)
			{
				Application.Exit();
				return;
			}
			try
			{
				Process.Start("https://dotnet.microsoft.com/get-dotnet/dotnet-framework?tfm=.NETFramework%2CVersion%3Dv4.8");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			Application.Exit();
			return;
		}
		LanguageFile languageFile = new LanguageFile();
		languageFile.GetLanguageFileCount();
		if (languageFile.LanguageCount == 0)
		{
			MessageBox.Show("No Language,please recheck!");
			Application.Exit();
			return;
		}
		languageFile.SetLanguage(languageFile.LanguageIndex, null);
		if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\res"))
		{
			MessageBox.Show(LanguageFile.Dialogs[1]);
			Application.Exit();
			return;
		}
		DriveConfig driveConfig = new DriveConfig();
		if (!driveConfig.GetDriveConfig())
		{
			((Form)new FormDialog(LanguageFile.Dialogs[2], DialogButtons.OK)).ShowDialog();
			Application.Exit();
			return;
		}
		bool flag = false;
		mutex = new Mutex(initiallyOwned: true, AppDomain.CurrentDomain.FriendlyName + driveConfig.gDriveParam.DriveName);
		if (!mutex.WaitOne(10, exitContext: false))
		{
			FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[3]);
			((Form)formDialog).StartPosition = (FormStartPosition)1;
			((Form)formDialog).ShowDialog();
			Application.Exit();
		}
		else
		{
			Application.Run((Form)(object)new FormHomePage(driveConfig.gDriveParam, formPro: true));
		}
	}

	private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
	{
		new ExceptionLog(e.Exception.ToString());
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		new ExceptionLog(e.ExceptionObject.ToString());
	}
}
