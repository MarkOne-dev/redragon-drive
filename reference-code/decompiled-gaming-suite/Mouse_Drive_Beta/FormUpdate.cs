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
using Mouse_Drive_Beta.SkinFormLib;
using USBUpdateTool;

namespace Mouse_Drive_Beta;

public class FormUpdate : Form
{
	private bool updating;

	public bool updateSuccess;

	private Thread pollingThread;

	private bool finding;

	private DriveConfig.DriveParam driveParam;

	private byte[] UpdateBuffer;

	private List<byte> updateFialInfo;

	private Timer UpdateTimerOut;

	private int UpdateTimerOutCnt;

	private FormResize formResize;

	private LanguageFile languageFile;

	private IContainer components;

	private Label label_UpdateTips;

	private CustomButton customButton_Confirm;

	private CustomButton customButton_Close;

	private Panel panel_Background;

	private Panel panel_Silder;

	public FormUpdate(DriveConfig.DriveParam param)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		updating = true;
		UpdateBuffer = new byte[1024];
		updateFialInfo = new List<byte>();
		formResize = new FormResize();
		languageFile = new LanguageFile();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		driveParam = param;
		updating = false;
		finding = true;
		if (UpdateTimerOut == null)
		{
			UpdateTimerOut = new Timer();
			UpdateTimerOut.Interval = 1000;
			UpdateTimerOut.Tick += UpdateTimerOut_Tick;
		}
		pollingThread = new Thread(RepairProcess);
		pollingThread.Start();
	}

	private void UpdateTimerOut_Tick(object sender, EventArgs e)
	{
		UpdateTimerOutCnt++;
		if (UpdateTimerOutCnt >= 15)
		{
			if (updating)
			{
				updating = false;
				UpdateTimerOut.Enabled = false;
				((Control)customButton_Confirm).Visible = true;
			}
			else
			{
				UpdateTimerOutCnt = 0;
			}
		}
	}

	public FormUpdate(byte[] buff, byte report)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		updating = true;
		UpdateBuffer = new byte[1024];
		updateFialInfo = new List<byte>();
		formResize = new FormResize();
		languageFile = new LanguageFile();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		driveParam = FormMain.driveParam;
		UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(buff);
		updateFialInfo.Add(upgradeFileHeader.DeciveType);
		updateFialInfo.Add(report);
		updateFialInfo.Add(upgradeFileHeader.Cid);
		updateFialInfo.Add(upgradeFileHeader.Mid);
		UpdateBuffer = buff;
		if (UpdateTimerOut == null)
		{
			UpdateTimerOut = new Timer();
			UpdateTimerOut.Interval = 1000;
			UpdateTimerOut.Tick += UpdateTimerOut_Tick;
		}
		pollingThread = new Thread(UpdatingProcess);
		pollingThread.Start();
	}

	private void ImageInit()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\message_bg.png");
		((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
		customButton_Confirm.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_nr.png");
		customButton_Confirm.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_down.png");
		customButton_Confirm.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_enter.png");
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
		((Control)panel_Background).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\process_bg.png");
		((Control)panel_Silder).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\process.png");
		SetProcessVisible(visible: false);
		((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58];
		((Control)customButton_Confirm).Visible = false;
	}

	private void SetProcessVisible(bool visible)
	{
		((Control)panel_Background).Visible = visible;
		((Control)panel_Silder).Visible = visible;
	}

	private void SetProcessValue(int value)
	{
		if (!((Control)panel_Silder).Visible)
		{
			SetProcessVisible(visible: true);
		}
		((Control)panel_Silder).Width = ((Control)panel_Background).Width * value / 100;
	}

	private void UpdatingProcess()
	{
		bool result = false;
		while (updating && !result)
		{
			if (((Control)this).IsHandleCreated)
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					UpdateTimerOut.Enabled = true;
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58];
					result = UsbFinder.UsbUpgrade_Start(UpdateBuffer, UpgradeHandler, 10000);
					finding = false;
					((Control)customButton_Confirm).Visible = false;
					SetProcessValue(0);
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58] + "：0%";
				});
			}
			Thread.Sleep(500);
		}
		pollingThread.Abort();
	}

	private void RepairProcess()
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
					UpdateTimerOut.Enabled = true;
					updating = true;
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58];
					result = UsbFinder.UsbUpgrade_Start(UpdateBuffer, UpgradeHandler, 10000);
					finding = false;
					((Control)customButton_Confirm).Visible = false;
					SetProcessValue(0);
				});
			}
			else if (array.Length == 0)
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					updating = false;
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[6];
					((Control)customButton_Confirm).Visible = true;
					SetProcessVisible(visible: false);
				});
			}
			else
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					updating = false;
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[29];
					((Control)customButton_Confirm).Visible = true;
					SetProcessVisible(visible: false);
				});
			}
			Thread.Sleep(1000);
		}
		pollingThread.Abort();
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
			UpdateTimerOutCnt = 0;
			SetProcessValue(command[2]);
			((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58] + "：" + command[2] + "%";
			break;
		case UpgradeState.UpgradeResult:
			if (command[2] == 1)
			{
				SetProcessValue(100);
				updateSuccess = true;
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[56];
				RegeditManager.ClearUpdateFailInfo();
			}
			else
			{
				updateSuccess = false;
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[57];
				SystemLog systemLog = new SystemLog("Update_Log.bin");
				string[] array = UsbUpgradeFile.UsbUpgrade_GetLogs();
				for (int i = 0; i < array.Length; i++)
				{
					systemLog.WriteLog(array[i]);
				}
				RegeditManager.SetUpdateFailInfo(updateFialInfo[0], updateFialInfo[1], updateFialInfo[2], updateFialInfo[3]);
			}
			UpdateTimerOutCnt = 0;
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
			pollingThread.Abort();
			((Form)this).Close();
		}
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		if (!updating)
		{
			pollingThread.Abort();
			((Form)this).Close();
		}
	}

	private void FormUpdate_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13 && !updating)
		{
			pollingThread.Abort();
			((Form)this).Close();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		label_UpdateTips = new Label();
		panel_Background = new Panel();
		panel_Silder = new Panel();
		customButton_Confirm = new CustomButton();
		customButton_Close = new CustomButton();
		((Control)panel_Background).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)label_UpdateTips).BackColor = Color.Transparent;
		((Control)label_UpdateTips).Location = new Point(0, 35);
		((Control)label_UpdateTips).Name = "label_UpdateTips";
		((Control)label_UpdateTips).Size = new Size(322, 20);
		((Control)label_UpdateTips).TabIndex = 33;
		((Control)label_UpdateTips).Text = "label1";
		label_UpdateTips.TextAlign = (ContentAlignment)32;
		((Control)panel_Background).BackColor = Color.White;
		((Control)panel_Background).BackgroundImage = (Image)(object)Resources.进度条背景;
		((Control)panel_Background).BackgroundImageLayout = (ImageLayout)0;
		((Control)panel_Background).Controls.Add((Control)(object)panel_Silder);
		((Control)panel_Background).Location = new Point(27, 68);
		((Control)panel_Background).Name = "panel_Background";
		((Control)panel_Background).Size = new Size(270, 23);
		((Control)panel_Background).TabIndex = 36;
		((Control)panel_Silder).BackColor = Color.Transparent;
		((Control)panel_Silder).BackgroundImage = (Image)(object)Resources.进度条;
		((Control)panel_Silder).BackgroundImageLayout = (ImageLayout)0;
		((Control)panel_Silder).Location = new Point(0, 0);
		((Control)panel_Silder).Name = "panel_Silder";
		((Control)panel_Silder).Size = new Size(35, 23);
		((Control)panel_Silder).TabIndex = 37;
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(119, 101);
		((Control)customButton_Confirm).Margin = new Padding(3, 4, 3, 4);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(78, 26);
		((Control)customButton_Confirm).TabIndex = 32;
		((Control)customButton_Confirm).Text = "确定";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)customButton_Close).Location = new Point(294, 7);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 31;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.消息窗背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(320, 136);
		((Control)this).Controls.Add((Control)(object)panel_Background);
		((Control)this).Controls.Add((Control)(object)label_UpdateTips);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormUpdate";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormUpdate";
		((Control)this).KeyDown += new KeyEventHandler(FormUpdate_KeyDown);
		((Control)panel_Background).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
