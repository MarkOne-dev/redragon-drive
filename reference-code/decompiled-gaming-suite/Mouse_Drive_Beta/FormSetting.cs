using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;
using USBUpdateTool;
using WindControls;

namespace Mouse_Drive_Beta;

public class FormSetting : Form
{
	public delegate void LanguageChangeEventHandler(object sender, CustomEventArgs e);

	private FormMover formMover;

	private FormResize formResize;

	private LanguageFile languageFile;

	private string[] Version;

	private bool DongleLatestVersion;

	private bool MouseLatestVersion;

	private ButtonStateEnum DongleUpdate;

	private ButtonStateEnum MouseUpdate;

	private Timer DongleCheckTimer;

	private Timer MouseCheckTimer;

	private Timer DeviceCheckTimer;

	private Timer AfterUpdateTimer;

	private bool DongleCheckTimerEnable;

	private bool MouseCheckTimerEnable;

	private int UpdateStep;

	private byte[] UpdateBuffer;

	private string[] boot1KDongle;

	private string[] boot2KDongle;

	private string[] boot4KDongle;

	private List<string> Dongle;

	private DriveConfig.DeviceParam deviceParam;

	private int reportRate;

	public bool isUpdating;

	private int CID;

	private CXFILE_TYPE gType;

	private IContainer components;

	private Label label_Web;

	private Label label_MouseVersion;

	private Label label_DongleVersion;

	private Label label_DriveVersion;

	private LinkLabel linkLabel_Web;

	private CustomComboBox customComboBox1;

	private Label label_DongleVersionValue;

	private Label label_MouseVersionValue;

	private CustomButton customButton_Pairing;

	private CustomButton customButton_Close;

	private Label label_DriveVersionValue;

	private CustomStateButton customStateButton_DongleUpdate;

	private CustomStateButton customStateButton_MouseUpdate;

	private Label label_UpdateTips;

	private PictureBox pictureBox2;

	public event LanguageChangeEventHandler LanguageChange;

	public FormSetting(LanguageFile language, string[] version, int index, string[] str, int cid, DriveConfig.DeviceParam param, bool update, int reportrate)
	{
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Expected O, but got Unknown
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		formMover = new FormMover();
		formResize = new FormResize();
		languageFile = new LanguageFile();
		UpdateBuffer = new byte[1024];
		deviceParam = new DriveConfig.DeviceParam();
		reportRate = 1;
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		SetStyles();
		formResize.Resize((Form)(object)this);
		formMover.AddForm((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		deviceParam = param;
		CID = cid;
		reportRate = reportrate;
		DriveConfig.DriveParam driveParam = DriveConfig.GetDriveParam();
		((Control)label_UpdateTips).ForeColor = driveParam.TipsClr;
		int right = ((Control)linkLabel_Web).Right;
		((Control)linkLabel_Web).Text = DriveConfig.GetDescription("Web");
		((Control)linkLabel_Web).Location = new Point(right - ((Control)linkLabel_Web).Width, ((Control)linkLabel_Web).Location.Y);
		DongleCheckTimer = new Timer();
		DongleCheckTimer.Interval = 1000;
		DongleCheckTimer.Enabled = false;
		DongleCheckTimer.Tick += DongleCheckTimer_Tick;
		MouseCheckTimer = new Timer();
		MouseCheckTimer.Interval = 1000;
		MouseCheckTimer.Enabled = false;
		MouseCheckTimer.Tick += MouseCheckTimer_Tick;
		DeviceCheckTimer = new Timer();
		DeviceCheckTimer.Interval = 300;
		DeviceCheckTimer.Enabled = true;
		DeviceCheckTimer.Tick += DeviceCheckTimer_Tick;
		AfterUpdateTimer = new Timer();
		AfterUpdateTimer.Interval = 1000;
		AfterUpdateTimer.Enabled = false;
		AfterUpdateTimer.Tick += AfterUpdateTimer_Tick;
		languageFile = language;
		SetComboBox(language.LanguageType);
		SetText(str, version);
		Version = new string[version.Length];
		for (int i = 0; i < version.Length; i++)
		{
			Version[i] = version[i];
		}
		((Control)customStateButton_DongleUpdate).Visible = update;
		((Control)customStateButton_MouseUpdate).Visible = update;
		((Control)customStateButton_DongleUpdate).Font = new Font("微软雅黑", LanguageFile.GetFontSize(languageFile.LanguageIndex));
		((Control)customStateButton_MouseUpdate).Font = new Font("微软雅黑", LanguageFile.GetFontSize(languageFile.LanguageIndex));
		customComboBox1.SelectIndex = index;
		((Control)customButton_Pairing).Font = new Font("微软雅黑", LanguageFile.GetFontSize(languageFile.LanguageIndex));
		((Control)customButton_Pairing).Text = LanguageFile.Dialogs[39];
		SetLabWeb();
	}

	private void AfterUpdateTimer_Tick(object sender, EventArgs e)
	{
		AfterUpdateTimer.Enabled = false;
		if (isUpdating)
		{
			ExitUpdating();
		}
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
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00b0: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_00d9: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0102: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_012b: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\setting_bg.png");
		customButton_Pairing.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		customButton_Pairing.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		customButton_Pairing.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		((Control)pictureBox2).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\tips.png");
		CustomStateButton customStateButton = customStateButton_MouseUpdate;
		CustomStateButton customStateButton2 = customStateButton_DongleUpdate;
		Bitmap val = new Bitmap(baseDirectory + "\\res\\6Setting\\check_update.png");
		Image normalImage = (Image)val;
		customStateButton2.NormalImage = (Image)val;
		customStateButton.NormalImage = normalImage;
		CustomStateButton customStateButton3 = customStateButton_MouseUpdate;
		CustomStateButton customStateButton4 = customStateButton_DongleUpdate;
		Bitmap val2 = new Bitmap(baseDirectory + "\\res\\6Setting\\check_update.png");
		normalImage = (Image)val2;
		customStateButton4.FailImage = (Image)val2;
		customStateButton3.FailImage = normalImage;
		CustomStateButton customStateButton5 = customStateButton_MouseUpdate;
		CustomStateButton customStateButton6 = customStateButton_DongleUpdate;
		Bitmap val3 = new Bitmap(baseDirectory + "\\res\\6Setting\\check_update.png");
		normalImage = (Image)val3;
		customStateButton6.SuccessImage = (Image)val3;
		customStateButton5.SuccessImage = normalImage;
		CustomStateButton customStateButton7 = customStateButton_MouseUpdate;
		CustomStateButton customStateButton8 = customStateButton_DongleUpdate;
		Bitmap val4 = new Bitmap(baseDirectory + "\\res\\6Setting\\updating.png");
		normalImage = (Image)val4;
		customStateButton8.UpdatedImage = (Image)val4;
		customStateButton7.UpdatedImage = normalImage;
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void SetText(string[] str, string[] ver)
	{
		((Control)label_DriveVersion).Text = str[0];
		((Control)label_DongleVersion).Text = str[1];
		((Control)label_MouseVersion).Text = str[2];
		((Control)label_DongleVersionValue).Text = ver[0];
		((Control)label_MouseVersionValue).Text = ver[1];
		int num = ((Control)label_DongleVersionValue).Left - 3 - ((Control)label_DongleVersion).Width;
		if (num < 0)
		{
			num = 0;
		}
		((Control)label_DongleVersion).Location = new Point(num, ((Control)label_DongleVersion).Location.Y);
		((Control)label_MouseVersion).Location = new Point(num, ((Control)label_MouseVersion).Location.Y);
		((Control)label_DriveVersionValue).Text = DriveConfig.GetDriveVersion();
		((Control)label_DriveVersionValue).Location = new Point(((Control)label_DriveVersion).Right, ((Control)label_DriveVersionValue).Location.Y);
		customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[52];
		customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[52];
		customStateButton_DongleUpdate.DoingText = LanguageFile.Dialogs[53];
		customStateButton_MouseUpdate.DoingText = LanguageFile.Dialogs[53];
		((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[52];
		((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[52];
	}

	private void SetLabWeb()
	{
		((Control)label_Web).Text = DriveConfig.GetDescription(customComboBox1.Item[customComboBox1.SelectIndex].ToString());
		((Control)label_Web).Location = new Point(((Control)linkLabel_Web).Right - ((Control)label_Web).Width, ((Control)label_Web).Location.Y);
	}

	private void SetComboBox(string[] language)
	{
		customComboBox1.Item.Clear();
		for (int i = 0; i < language.Length; i++)
		{
			customComboBox1.Item.Add((object)language[i]);
		}
	}

	private void linkLabel_Web_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start(((Control)linkLabel_Web).Text);
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		if (!isUpdating)
		{
			((Form)this).Close();
		}
	}

	public void SetMouseVersion(string ver)
	{
		((Control)label_MouseVersionValue).Text = ver;
	}

	private void customComboBox1_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		if (customComboBox1.SelectIndex != FormMain.languageFile.LanguageIndex)
		{
			languageFile.SetSettingLanguage(customComboBox1.SelectIndex);
			SetText(languageFile.FromSettingString, Version);
			((Control)customButton_Pairing).Font = new Font("微软雅黑", LanguageFile.GetFontSize(customComboBox1.SelectIndex));
			((Control)customStateButton_DongleUpdate).Font = new Font("微软雅黑", LanguageFile.GetFontSize(customComboBox1.SelectIndex));
			((Control)customStateButton_MouseUpdate).Font = new Font("微软雅黑", LanguageFile.GetFontSize(customComboBox1.SelectIndex));
			((Control)customButton_Pairing).Text = LanguageFile.Dialogs[39];
			SetLabWeb();
			if (MouseLatestVersion)
			{
				customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[54];
				((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[54];
			}
			if (DongleLatestVersion)
			{
				customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[54];
				((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[54];
			}
			RegeditManager.SaveLanguageIndex(customComboBox1.SelectIndex);
			CustomEventArgs e2 = new CustomEventArgs(customComboBox1.SelectIndex);
			LanguageChange?.Invoke(this, e2);
		}
	}

	private void customButton_Pairing_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (!IsUpdating())
		{
			((Form)new FormPair(CID)).ShowDialog();
		}
	}

	private void customStateButton_DongleUpdate_Click(object sender, EventArgs e)
	{
		if (!IsUpdating() && !DongleLatestVersion && !MouseCheckTimerEnable)
		{
			if (!isUpdating)
			{
				EnterUpdating();
			}
			UpdateStep = 0;
			DongleCheckTimer.Enabled = true;
			DongleCheckTimerEnable = true;
			customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Doing;
		}
	}

	private void customStateButton_MouseUpdate_Click(object sender, EventArgs e)
	{
		if (!IsUpdating() && !MouseLatestVersion && !DongleCheckTimerEnable)
		{
			if (!isUpdating)
			{
				EnterUpdating();
			}
			UpdateStep = 0;
			MouseCheckTimer.Enabled = true;
			MouseCheckTimerEnable = true;
			customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Doing;
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
			if (IsDongleUpdating())
			{
				customStateButton_DongleUpdate.Percent = command[2];
			}
			if (IsMouseUpdating())
			{
				customStateButton_MouseUpdate.Percent = command[2];
			}
			break;
		case UpgradeState.UpgradeResult:
			if (command[2] == 1)
			{
				if (IsDongleUpdating())
				{
					DongleUpdate = ButtonStateEnum.Success;
					UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(UpdateBuffer);
					((Control)label_DongleVersionValue).Text = "v" + ValueConvert.IntToVersion((int)upgradeFileHeader.version);
				}
				if (IsMouseUpdating())
				{
					MouseUpdate = ButtonStateEnum.Success;
					UpgradeFileHeader upgradeFileHeader2 = UsbUpgradeFile.ByteToStruct(UpdateBuffer);
					((Control)label_MouseVersionValue).Text = "v" + ValueConvert.IntToVersion((int)upgradeFileHeader2.version);
				}
				RegeditManager.ClearUpdateFailInfo();
			}
			else
			{
				if (IsDongleUpdating())
				{
					DongleUpdate = ButtonStateEnum.Fail;
				}
				if (IsMouseUpdating())
				{
					MouseUpdate = ButtonStateEnum.Fail;
				}
				SystemLog systemLog = new SystemLog("Update_Log.bin");
				string[] array = UsbUpgradeFile.UsbUpgrade_GetLogs();
				for (int i = 0; i < array.Length; i++)
				{
					systemLog.WriteLog(array[i]);
				}
				RegeditManager.SetUpdateFailInfo((byte)gType, (byte)reportRate, (byte)CID, (byte)deviceParam.MID);
			}
			AfterUpdateTimer.Enabled = true;
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

	private void EnterUpdating()
	{
		((Control)customButton_Pairing).Enabled = false;
		((Control)customComboBox1).Enabled = false;
		((Control)linkLabel_Web).Enabled = false;
		isUpdating = true;
	}

	private void ExitUpdating()
	{
		isUpdating = false;
		((Control)customButton_Pairing).Enabled = true;
		((Control)customComboBox1).Enabled = true;
		((Control)linkLabel_Web).Enabled = true;
	}

	private void DongleCheckTimer_Tick(object sender, EventArgs e)
	{
		CheckUpdatePolling(UpdateStep, CXFILE_TYPE.Dongle, ((Control)label_DongleVersionValue).Text);
	}

	private void MouseCheckTimer_Tick(object sender, EventArgs e)
	{
		CheckUpdatePolling(UpdateStep, CXFILE_TYPE.Mouse, ((Control)label_MouseVersionValue).Text);
	}

	private void DeviceCheckTimer_Tick(object sender, EventArgs e)
	{
	}

	private bool IsUpdating()
	{
		if (!IsDongleUpdating())
		{
			return IsMouseUpdating();
		}
		return true;
	}

	private bool IsDongleUpdating()
	{
		return DongleUpdate == ButtonStateEnum.Updating;
	}

	private bool IsMouseUpdating()
	{
		return MouseUpdate == ButtonStateEnum.Updating;
	}

	private void CheckUpdatePolling(int step, CXFILE_TYPE type, string version)
	{
		switch (step)
		{
		case 0:
		{
			string text = "";
			bool flag2 = false;
			if (type == CXFILE_TYPE.Mouse)
			{
				text = deviceParam.MM;
				flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, version, text, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
			}
			else
			{
				Dongle = new List<string>();
				byte[] buffers;
				if (FormMain.ConnectedDeviceInfo.isUSB)
				{
					if (deviceParam.D4M != null)
					{
						text = deviceParam.D4M;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot4KDongle = UsbFinder.FindBootDevices(buffers);
					}
					if (deviceParam.D2M != null)
					{
						text = deviceParam.D2M;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot2KDongle = UsbFinder.FindBootDevices(buffers);
					}
					if (deviceParam.DM != null)
					{
						text = deviceParam.DM;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot1KDongle = UsbFinder.FindBootDevices(buffers);
					}
					Dongle = new List<string>();
					Dongle.Clear();
					for (int j = 0; j < FormMain.AllDeviceInfoList.Count; j++)
					{
						if (!FormMain.AllDeviceInfoList[j].isUSB)
						{
							Dongle.Add(FormMain.AllDeviceInfoList[j].deviceString);
						}
					}
				}
				else
				{
					if (reportRate == 4)
					{
						text = deviceParam.D4M;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot4KDongle = UsbFinder.FindBootDevices(buffers);
					}
					else if (reportRate == 2)
					{
						text = deviceParam.D4M;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot2KDongle = UsbFinder.FindBootDevices(buffers);
					}
					else
					{
						text = deviceParam.DM;
						flag2 = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
						boot1KDongle = UsbFinder.FindBootDevices(buffers);
					}
					for (int k = 0; k < FormMain.AllDeviceInfoList.Count; k++)
					{
						if (!FormMain.AllDeviceInfoList[k].isUSB && FormMain.AllDeviceInfoList[k].deviceString == FormMain.SelectedDeviceInfo.deviceString)
						{
							Dongle.Add(FormMain.SelectedDeviceInfo.deviceString);
						}
					}
				}
			}
			if (flag2)
			{
				UpdateStep = 1;
				gType = type;
				break;
			}
			if (type == CXFILE_TYPE.Dongle)
			{
				DongleUpdate = ButtonStateEnum.LastVersion;
				DongleUpdateResult();
				DongleLatestVersion = true;
			}
			else
			{
				MouseUpdate = ButtonStateEnum.LastVersion;
				MouseUpdateResult();
				MouseLatestVersion = true;
			}
			UpdateStep = 4;
			break;
		}
		case 1:
		{
			new List<string[]>();
			int num = 0;
			new List<byte[]>();
			List<string> list = new List<string>();
			if (type == CXFILE_TYPE.Mouse)
			{
				UsbFinder.FindBootDevices(UpdateBuffer);
				for (int i = 0; i < FormMain.AllDeviceInfoList.Count; i++)
				{
					if (FormMain.AllDeviceInfoList[i].isUSB)
					{
						list.Add(FormMain.AllDeviceInfoList[i].deviceString);
					}
				}
			}
			else
			{
				num = Dongle.Count + ((boot1KDongle != null) ? boot1KDongle.Length : 0) + ((boot2KDongle != null) ? boot2KDongle.Length : 0) + ((boot4KDongle != null) ? boot4KDongle.Length : 0);
			}
			num += list.Count;
			bool flag = false;
			if (num == 1)
			{
				if (type == CXFILE_TYPE.Mouse)
				{
					flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, version, deviceParam.MM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				}
				else
				{
					if (boot1KDongle != null && boot1KDongle.Length == 1)
					{
						flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", deviceParam.DM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						flag = true;
					}
					if (boot2KDongle != null && !flag && boot2KDongle.Length == 1)
					{
						flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", deviceParam.D2M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						flag = true;
					}
					if (boot4KDongle != null && !flag && boot4KDongle.Length == 1)
					{
						flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, "", deviceParam.D4M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						flag = true;
					}
					if (!flag)
					{
						if (reportRate == 4)
						{
							int slaveVersion = 0;
							string currentVersion = "";
							if (UsbFinder.GetSlaveVersion(Dongle[0], out slaveVersion))
							{
								currentVersion = "v" + ValueConvert.IntToVersion(slaveVersion);
							}
							flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, currentVersion, deviceParam.D4M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						}
						else if (reportRate == 2)
						{
							int slaveVersion2 = 0;
							string currentVersion2 = "";
							if (UsbFinder.GetSlaveVersion(Dongle[0], out slaveVersion2))
							{
								currentVersion2 = "v" + ValueConvert.IntToVersion(slaveVersion2);
							}
							flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, currentVersion2, deviceParam.D2M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						}
						else
						{
							flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)type, version, deviceParam.DM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
						}
					}
				}
				if (flag)
				{
					flag = UsbFinder.UsbUpgrade_Start(UpdateBuffer, UpgradeHandler, 10000);
					UpdateStep = 2;
					((Control)pictureBox2).Visible = true;
					((Control)label_UpdateTips).Visible = true;
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[58];
					break;
				}
				if (type == CXFILE_TYPE.Dongle)
				{
					DongleUpdate = ButtonStateEnum.LastVersion;
					DongleUpdateResult();
					DongleLatestVersion = true;
				}
				else
				{
					MouseUpdate = ButtonStateEnum.LastVersion;
					MouseUpdateResult();
					MouseLatestVersion = true;
				}
				UpdateStep = 4;
				break;
			}
			switch (type)
			{
			case CXFILE_TYPE.Dongle:
				DongleUpdate = ButtonStateEnum.Normal;
				DongleUpdateResult();
				break;
			case CXFILE_TYPE.Mouse:
				MouseUpdate = ButtonStateEnum.Normal;
				MouseUpdateResult();
				break;
			}
			((Control)pictureBox2).Visible = true;
			((Control)label_UpdateTips).Visible = true;
			if (num == 0)
			{
				if (type == CXFILE_TYPE.Mouse)
				{
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[47];
				}
				else
				{
					((Control)label_UpdateTips).Text = LanguageFile.Dialogs[6];
				}
			}
			else
			{
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[29];
			}
			UpdateStep = 4;
			break;
		}
		case 2:
			switch (type)
			{
			case CXFILE_TYPE.Dongle:
				DongleUpdate = ButtonStateEnum.Updating;
				customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Updating;
				customStateButton_DongleUpdate.DoingText = LanguageFile.Dialogs[55];
				break;
			case CXFILE_TYPE.Mouse:
				MouseUpdate = ButtonStateEnum.Updating;
				customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Updating;
				customStateButton_MouseUpdate.DoingText = LanguageFile.Dialogs[55];
				break;
			}
			UpdateStep = 3;
			break;
		case 3:
			switch (type)
			{
			case CXFILE_TYPE.Dongle:
				if (DongleUpdate != ButtonStateEnum.Updating)
				{
					UpdateStep = 4;
				}
				break;
			case CXFILE_TYPE.Mouse:
				if (!IsMouseUpdating())
				{
					UpdateStep = 4;
				}
				break;
			}
			break;
		case 4:
			switch (type)
			{
			case CXFILE_TYPE.Dongle:
				DongleCheckTimer.Enabled = false;
				DongleCheckTimerEnable = false;
				DongleUpdateResult();
				break;
			case CXFILE_TYPE.Mouse:
				MouseCheckTimer.Enabled = false;
				MouseCheckTimerEnable = false;
				MouseUpdateResult();
				break;
			}
			ExitUpdating();
			break;
		}
	}

	private void DongleUpdateResult()
	{
		if (DongleUpdate == ButtonStateEnum.LastVersion)
		{
			customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[54];
			((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[54];
			((Control)pictureBox2).Visible = false;
			((Control)label_UpdateTips).Visible = false;
		}
		else if (DongleUpdate == ButtonStateEnum.Success)
		{
			customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[54];
			((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[56];
			FormMain.ConnectedDeviceInfo = default(DeviceAllInfo);
			((Control)pictureBox2).Visible = false;
			((Control)label_UpdateTips).Visible = false;
		}
		else if (DongleUpdate == ButtonStateEnum.Fail)
		{
			customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Fail;
			customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[52];
			((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[57];
		}
		else if (DongleUpdate == ButtonStateEnum.Normal)
		{
			customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_DongleUpdate.NormalText = LanguageFile.Dialogs[52];
			((Control)customStateButton_DongleUpdate).Text = LanguageFile.Dialogs[52];
		}
		UpdateStep = 4;
	}

	private void MouseUpdateResult()
	{
		if (MouseUpdate == ButtonStateEnum.LastVersion)
		{
			customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[54];
			((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[54];
			((Control)pictureBox2).Visible = false;
			((Control)label_UpdateTips).Visible = false;
		}
		else if (MouseUpdate == ButtonStateEnum.Success)
		{
			customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[54];
			((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[56];
			FormMain.ConnectedDeviceInfo = default(DeviceAllInfo);
			((Control)pictureBox2).Visible = false;
			((Control)label_UpdateTips).Visible = false;
		}
		else if (MouseUpdate == ButtonStateEnum.Fail)
		{
			customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Fail;
			customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[52];
			((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[57];
		}
		else if (MouseUpdate == ButtonStateEnum.Normal)
		{
			customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Success;
			customStateButton_MouseUpdate.NormalText = LanguageFile.Dialogs[52];
			((Control)customStateButton_MouseUpdate).Text = LanguageFile.Dialogs[52];
		}
		UpdateStep = 4;
	}

	private void FormSetting_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32)
		{
			customButton_Pairing_Click(null, null);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_0a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Expected O, but got Unknown
		//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c09: Expected O, but got Unknown
		//IL_0c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormSetting));
		label_Web = new Label();
		label_MouseVersion = new Label();
		label_DongleVersion = new Label();
		label_DriveVersion = new Label();
		linkLabel_Web = new LinkLabel();
		label_DongleVersionValue = new Label();
		label_MouseVersionValue = new Label();
		label_DriveVersionValue = new Label();
		label_UpdateTips = new Label();
		pictureBox2 = new PictureBox();
		customStateButton_MouseUpdate = new CustomStateButton();
		customStateButton_DongleUpdate = new CustomStateButton();
		customButton_Close = new CustomButton();
		customButton_Pairing = new CustomButton();
		customComboBox1 = new CustomComboBox();
		((ISupportInitialize)pictureBox2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_Web).AutoSize = true;
		((Control)label_Web).BackColor = Color.Transparent;
		((Control)label_Web).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Web).ForeColor = Color.White;
		((Control)label_Web).Location = new Point(327, 262);
		((Control)label_Web).Name = "label_Web";
		((Control)label_Web).Size = new Size(199, 20);
		((Control)label_Web).TabIndex = 15;
		((Control)label_Web).Text = "版权所有Compx.保留所有权利";
		((Control)label_MouseVersion).BackColor = Color.Transparent;
		((Control)label_MouseVersion).ForeColor = Color.White;
		((Control)label_MouseVersion).Location = new Point(4, 156);
		((Control)label_MouseVersion).Name = "label_MouseVersion";
		((Control)label_MouseVersion).Size = new Size(262, 20);
		((Control)label_MouseVersion).TabIndex = 14;
		((Control)label_MouseVersion).Text = "鼠标固件版本：";
		label_MouseVersion.TextAlign = (ContentAlignment)64;
		((Control)label_DongleVersion).BackColor = Color.Transparent;
		((Control)label_DongleVersion).ForeColor = Color.White;
		((Control)label_DongleVersion).Location = new Point(4, 102);
		((Control)label_DongleVersion).Name = "label_DongleVersion";
		((Control)label_DongleVersion).Size = new Size(262, 20);
		((Control)label_DongleVersion).TabIndex = 13;
		((Control)label_DongleVersion).Text = "dongle固件版本：";
		label_DongleVersion.TextAlign = (ContentAlignment)64;
		((Control)label_DriveVersion).AutoSize = true;
		((Control)label_DriveVersion).BackColor = Color.Transparent;
		((Control)label_DriveVersion).ForeColor = Color.White;
		((Control)label_DriveVersion).Location = new Point(27, 20);
		((Control)label_DriveVersion).Name = "label_DriveVersion";
		((Control)label_DriveVersion).Size = new Size(113, 20);
		((Control)label_DriveVersion).TabIndex = 11;
		((Control)label_DriveVersion).Text = "驱动版本：v1.00";
		((Control)linkLabel_Web).AutoSize = true;
		((Control)linkLabel_Web).BackColor = Color.Transparent;
		((Control)linkLabel_Web).Font = new Font("微软雅黑", 10.5f, (FontStyle)2, (GraphicsUnit)3, (byte)134);
		((Control)linkLabel_Web).ForeColor = Color.White;
		linkLabel_Web.LinkBehavior = (LinkBehavior)3;
		linkLabel_Web.LinkColor = Color.White;
		((Control)linkLabel_Web).Location = new Point(385, 235);
		((Control)linkLabel_Web).Name = "linkLabel_Web";
		((Control)linkLabel_Web).Size = new Size(141, 20);
		((Control)linkLabel_Web).TabIndex = 16;
		linkLabel_Web.TabStop = true;
		((Control)linkLabel_Web).Text = "www.compx.com.cn";
		linkLabel_Web.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel_Web_LinkClicked);
		((Control)label_DongleVersionValue).AutoSize = true;
		((Control)label_DongleVersionValue).BackColor = Color.Transparent;
		((Control)label_DongleVersionValue).ForeColor = Color.White;
		((Control)label_DongleVersionValue).Location = new Point(272, 102);
		((Control)label_DongleVersionValue).Name = "label_DongleVersionValue";
		((Control)label_DongleVersionValue).Size = new Size(35, 20);
		((Control)label_DongleVersionValue).TabIndex = 18;
		((Control)label_DongleVersionValue).Text = "v1.0";
		((Control)label_MouseVersionValue).AutoSize = true;
		((Control)label_MouseVersionValue).BackColor = Color.Transparent;
		((Control)label_MouseVersionValue).ForeColor = Color.White;
		((Control)label_MouseVersionValue).Location = new Point(272, 156);
		((Control)label_MouseVersionValue).Name = "label_MouseVersionValue";
		((Control)label_MouseVersionValue).Size = new Size(35, 20);
		((Control)label_MouseVersionValue).TabIndex = 19;
		((Control)label_MouseVersionValue).Text = "v1.0";
		((Control)label_DriveVersionValue).AutoSize = true;
		((Control)label_DriveVersionValue).BackColor = Color.Transparent;
		((Control)label_DriveVersionValue).Location = new Point(159, 20);
		((Control)label_DriveVersionValue).Name = "label_DriveVersionValue";
		((Control)label_DriveVersionValue).Size = new Size(50, 20);
		((Control)label_DriveVersionValue).TabIndex = 27;
		((Control)label_DriveVersionValue).Text = "label1";
		((Control)label_UpdateTips).BackColor = Color.Transparent;
		((Control)label_UpdateTips).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)label_UpdateTips).Location = new Point(118, 195);
		((Control)label_UpdateTips).Name = "label_UpdateTips";
		((Control)label_UpdateTips).Size = new Size(346, 40);
		((Control)label_UpdateTips).TabIndex = 31;
		((Control)label_UpdateTips).Text = "让鼠标进入配对状态，并靠近接收器";
		((Control)label_UpdateTips).Visible = false;
		((Control)pictureBox2).BackColor = Color.Transparent;
		((Control)pictureBox2).BackgroundImage = (Image)(object)Resources.提示符号;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox2).Location = new Point(98, 199);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(14, 14);
		pictureBox2.SizeMode = (PictureBoxSizeMode)3;
		pictureBox2.TabIndex = 30;
		pictureBox2.TabStop = false;
		((Control)pictureBox2).Visible = false;
		customStateButton_MouseUpdate.ButtonState = ButtonStateEnum.Normal;
		customStateButton_MouseUpdate.DoingText = "正在检查";
		customStateButton_MouseUpdate.FailImage = (Image)(object)Resources.检测更新;
		((Control)customStateButton_MouseUpdate).Location = new Point(322, 152);
		((Control)customStateButton_MouseUpdate).Name = "customStateButton_MouseUpdate";
		customStateButton_MouseUpdate.NormalImage = (Image)(object)Resources.检测更新;
		customStateButton_MouseUpdate.NormalText = "检查更新";
		customStateButton_MouseUpdate.Percent = 0;
		((Control)customStateButton_MouseUpdate).Size = new Size(142, 26);
		customStateButton_MouseUpdate.SuccessImage = (Image)(object)Resources.检测更新;
		((Control)customStateButton_MouseUpdate).TabIndex = 29;
		((Control)customStateButton_MouseUpdate).Text = "检查更新";
		customStateButton_MouseUpdate.UpdatedImage = (Image)(object)Resources.正在更新;
		((Control)customStateButton_MouseUpdate).Click += customStateButton_MouseUpdate_Click;
		customStateButton_DongleUpdate.ButtonState = ButtonStateEnum.Normal;
		customStateButton_DongleUpdate.DoingText = "正在检查";
		customStateButton_DongleUpdate.FailImage = (Image)(object)Resources.检测更新;
		((Control)customStateButton_DongleUpdate).Location = new Point(322, 100);
		((Control)customStateButton_DongleUpdate).Name = "customStateButton_DongleUpdate";
		customStateButton_DongleUpdate.NormalImage = (Image)(object)Resources.检测更新;
		customStateButton_DongleUpdate.NormalText = "检查更新";
		customStateButton_DongleUpdate.Percent = 0;
		((Control)customStateButton_DongleUpdate).Size = new Size(142, 26);
		customStateButton_DongleUpdate.SuccessImage = (Image)(object)Resources.检测更新;
		((Control)customStateButton_DongleUpdate).TabIndex = 28;
		((Control)customStateButton_DongleUpdate).Text = "检查更新";
		customStateButton_DongleUpdate.UpdatedImage = (Image)(object)Resources.正在更新;
		((Control)customStateButton_DongleUpdate).Click += customStateButton_DongleUpdate_Click;
		((Control)customButton_Close).Location = new Point(523, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 26;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)customButton_Pairing).ForeColor = Color.White;
		((Control)customButton_Pairing).Location = new Point(31, 262);
		((Control)customButton_Pairing).Margin = new Padding(3, 4, 3, 4);
		customButton_Pairing.MouseDownImage = (Image)(object)Resources.配对工具;
		customButton_Pairing.MouseEnterImage = (Image)(object)Resources.配对工具;
		((Control)customButton_Pairing).Name = "customButton_Pairing";
		customButton_Pairing.NormalImage = (Image)(object)Resources.配对工具;
		((Control)customButton_Pairing).Size = new Size(112, 26);
		((Control)customButton_Pairing).TabIndex = 21;
		((Control)customButton_Pairing).Text = "配对工具";
		((Control)customButton_Pairing).Click += customButton_Pairing_Click;
		customComboBox1.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox1.ArrowImageNoraml = (Image)componentResourceManager.GetObject("customComboBox1.ArrowImageNoraml");
		((Control)customComboBox1).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox1).Font = new Font("微软雅黑", 9f);
		((Control)customComboBox1).Location = new Point(372, 14);
		((Control)customComboBox1).Name = "customComboBox1";
		customComboBox1.SelectIndex = -1;
		customComboBox1.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox1).Size = new Size(116, 22);
		((Control)customComboBox1).TabIndex = 17;
		customComboBox1.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox1.OnSelectedIndexChanged += customComboBox1_OnSelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.设置界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(549, 302);
		((Control)this).Controls.Add((Control)(object)label_UpdateTips);
		((Control)this).Controls.Add((Control)(object)pictureBox2);
		((Control)this).Controls.Add((Control)(object)customStateButton_MouseUpdate);
		((Control)this).Controls.Add((Control)(object)customStateButton_DongleUpdate);
		((Control)this).Controls.Add((Control)(object)label_DriveVersionValue);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).Controls.Add((Control)(object)customButton_Pairing);
		((Control)this).Controls.Add((Control)(object)label_MouseVersionValue);
		((Control)this).Controls.Add((Control)(object)label_DongleVersionValue);
		((Control)this).Controls.Add((Control)(object)customComboBox1);
		((Control)this).Controls.Add((Control)(object)linkLabel_Web);
		((Control)this).Controls.Add((Control)(object)label_Web);
		((Control)this).Controls.Add((Control)(object)label_MouseVersion);
		((Control)this).Controls.Add((Control)(object)label_DongleVersion);
		((Control)this).Controls.Add((Control)(object)label_DriveVersion);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormSetting";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormSetting";
		((Control)this).KeyDown += new KeyEventHandler(FormSetting_KeyDown);
		((ISupportInitialize)pictureBox2).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
