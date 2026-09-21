using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using USBUpdateTool;

namespace Mouse_Drive_Beta;

public class FormRepair : Form
{
	private Thread pollingThread;

	private bool finding;

	private DriveConfig.DriveParam driveParam;

	private byte[] UpdateBuffer = new byte[1024];

	private List<byte> updateFialInfo = new List<byte>();

	private bool updating;

	private IContainer components;

	private CustomButton customButton_Close;

	private CustomButton customButton_Confirm;

	private Label label_UpdateTips;

	private Label label_Updating;

	public FormRepair(DriveConfig.DriveParam config)
	{
		InitializeComponent();
		ImageInit();
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)customButton_Confirm).Visible = false;
		((Control)label_Updating).Visible = false;
		driveParam = config;
		finding = true;
		pollingThread = new Thread(Polling);
		pollingThread.Start();
	}

	private void ImageInit()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\message_bg.png");
		customButton_Confirm.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_nr.png");
		customButton_Confirm.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_down.png");
		customButton_Confirm.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_enter.png");
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
	}

	private void Polling()
	{
		bool result = RegeditManager.GetUpdateFailInfo(out updateFialInfo);
		CXFILE_TYPE cXFILE_TYPE = (CXFILE_TYPE)updateFialInfo[0];
		DriveConfig.DeviceParam deviceParam = new DriveConfig.DeviceParam();
		for (int i = 0; i < driveParam.DeviceTotal; i++)
		{
			if (driveParam.DeviceParams[i].MID == updateFialInfo[3])
			{
				deviceParam = driveParam.DeviceParams[i];
				break;
			}
		}
		string mcu = deviceParam.MM;
		if (cXFILE_TYPE == CXFILE_TYPE.Dongle)
		{
			switch (updateFialInfo[1])
			{
			case 1:
				mcu = deviceParam.DM;
				break;
			case 2:
				mcu = deviceParam.D2M;
				break;
			case 4:
				mcu = deviceParam.D4M;
				break;
			}
		}
		result = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", mcu, updateFialInfo[2], (byte)deviceParam.MID, out UpdateBuffer);
		while (finding & result)
		{
			string[] array = UsbFinder.FindBootDevices(UpdateBuffer);
			if (array.Length == 1)
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58];
					result = UsbFinder.UsbUpgrade_Start(UpdateBuffer, UpgradeHandler, 10000);
					finding = false;
					((Control)label_Updating).Visible = true;
					((Control)customButton_Confirm).Visible = false;
					((Control)label_Updating).Text = "0%";
				});
			}
			else if (array.Length == 0)
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[6];
					((Control)customButton_Confirm).Visible = true;
				});
			}
			else
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[29];
					((Control)customButton_Confirm).Visible = true;
				});
			}
			Thread.Sleep(1000);
		}
	}

	private void UpgradeDataHandler(byte[] command)
	{
		if (command[0] != 7)
		{
			return;
		}
		switch ((UpgradeState)command[1])
		{
		case UpgradeState.DownLoadFile:
			if (!updating)
			{
				updating = true;
			}
			((Control)label_Updating).Text = command[2] + "%";
			break;
		case UpgradeState.UpgradeResult:
			if (command[2] == 1)
			{
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[56];
				((Control)label_Updating).Visible = false;
				RegeditManager.ClearUpdateFailInfo();
			}
			else
			{
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[57];
				((Control)label_Updating).Visible = false;
				SystemLog systemLog = new SystemLog("Update_Log.bin");
				string[] array = UsbUpgradeFile.UsbUpgrade_GetLogs();
				for (int i = 0; i < array.Length; i++)
				{
					systemLog.WriteLog(array[i]);
				}
				RegeditManager.SetUpdateFailInfo(updateFialInfo[0], updateFialInfo[1], updateFialInfo[2], updateFialInfo[3]);
			}
			updating = false;
			((Control)customButton_Confirm).Visible = true;
			break;
		case UpgradeState.UpgradeState:
			break;
		}
	}

	public void UpgradeHandler(byte[] command)
	{
		((Control)this).Invoke((Delegate)(Action)delegate
		{
			UpgradeDataHandler(command);
		});
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		if (!updating)
		{
			if (pollingThread != null)
			{
				pollingThread.Abort();
			}
			((Form)this).Close();
		}
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		if (!updating)
		{
			if (pollingThread != null)
			{
				pollingThread.Abort();
			}
			((Form)this).Close();
		}
	}

	private void FormRepair_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 27)
		{
			customButton_Confirm_Click(null, null);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		customButton_Close = new CustomButton();
		customButton_Confirm = new CustomButton();
		label_UpdateTips = new Label();
		label_Updating = new Label();
		((Control)this).SuspendLayout();
		((Control)customButton_Close).Location = new Point(292, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 27;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(122, 87);
		((Control)customButton_Confirm).Margin = new Padding(3, 4, 3, 4);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(78, 26);
		((Control)customButton_Confirm).TabIndex = 28;
		((Control)customButton_Confirm).Text = "确定";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)label_UpdateTips).BackColor = Color.Transparent;
		((Control)label_UpdateTips).Location = new Point(0, 30);
		((Control)label_UpdateTips).Name = "label_UpdateTips";
		((Control)label_UpdateTips).Size = new Size(322, 20);
		((Control)label_UpdateTips).TabIndex = 29;
		((Control)label_UpdateTips).Text = "label1";
		label_UpdateTips.TextAlign = (ContentAlignment)32;
		((Control)label_Updating).BackColor = Color.Transparent;
		((Control)label_Updating).Location = new Point(0, 53);
		((Control)label_Updating).Name = "label_Updating";
		((Control)label_Updating).Size = new Size(322, 20);
		((Control)label_Updating).TabIndex = 30;
		((Control)label_Updating).Text = "label1";
		label_Updating.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.消息窗背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(322, 126);
		((Control)this).Controls.Add((Control)(object)label_Updating);
		((Control)this).Controls.Add((Control)(object)label_UpdateTips);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormRepair";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormRepair";
		((Control)this).KeyDown += new KeyEventHandler(FormRepair_KeyDown);
		((Control)this).ResumeLayout(false);
	}
}
