using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using USBUpdateTool;

namespace Mouse_Drive_Beta.View;

public class UC_DeviceUpdate : UserControl
{
	public delegate void DeviceUpdatingEventHandler(object sender, bool updating);

	public delegate void DeviceUpdateFailEventHandler(object sender);

	private byte[] UpdateBuffer = new byte[1024];

	private LanguageFile languageFile;

	private string[] boot1KDongle;

	private string[] boot2KDongle;

	private string[] boot4KDongle;

	private List<string> Dongle;

	private DriveConfig.DeviceParam deviceParam = new DriveConfig.DeviceParam();

	private int reportRate = 1;

	private int CID;

	private bool dongleLastVersion;

	private bool mouseLastVersion;

	private bool dongleHaveNewVer;

	private bool mouseHaveNewVer;

	private bool needUpdateAnother;

	private DialogEnum dialog = DialogEnum.LatestVersion;

	public bool updating;

	private byte _DongleType;

	private IContainer components;

	private Label label_UpdateTips;

	private PictureBox pictureBox_Tips;

	private CustomButton customButton_MouseUpdate;

	private CustomButton customButton_DongleUpdate;

	private PictureBox pictureBox_Title;

	private Label label_MouseVersionValue;

	private Label label_DongleVersionValue;

	private Label label_MouseVersion;

	private Label label_DongleVersion;

	private Label label_DeviceInfo;

	private Label label_NewVerTips;

	public string DongleVersion
	{
		get
		{
			return ((Control)label_DongleVersionValue).Text;
		}
		set
		{
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Expected O, but got Unknown
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Expected O, but got Unknown
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Expected O, but got Unknown
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Expected O, but got Unknown
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			((Control)label_DongleVersionValue).Text = value;
			string mcu = deviceParam.DM;
			DeviceType dongleType = (DeviceType)_DongleType;
			if (dongleType.ToString().Contains("Wireless"))
			{
				if (dongleType.ToString().Contains("2K"))
				{
					mcu = deviceParam.D2M;
				}
				if (dongleType.ToString().Contains("4K"))
				{
					mcu = deviceParam.D4M;
				}
			}
			if (((Control)label_DongleVersionValue).Text != "")
			{
				dongleHaveNewVer = DeviceUpdateFile.HasNewVersion(211, ((Control)label_DongleVersionValue).Text, mcu, (byte)CID, (byte)deviceParam.MID);
				if (dongleHaveNewVer)
				{
					((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[52];
					customButton_DongleUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\update_nr.png");
					customButton_DongleUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\update_enter.png");
				}
				else
				{
					((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[54];
					customButton_DongleUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
					customButton_DongleUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
					customButton_DongleUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
				}
			}
			else
			{
				((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[52];
				customButton_DongleUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
				customButton_DongleUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
				customButton_DongleUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
			}
		}
	}

	public byte DongleType
	{
		get
		{
			return _DongleType;
		}
		set
		{
			_DongleType = value;
		}
	}

	public string MouseVersion
	{
		get
		{
			return ((Control)label_MouseVersionValue).Text;
		}
		set
		{
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Expected O, but got Unknown
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Expected O, but got Unknown
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			((Control)label_MouseVersionValue).Text = value;
			if (((Control)label_MouseVersionValue).Text != "")
			{
				mouseHaveNewVer = DeviceUpdateFile.HasNewVersion(210, ((Control)label_MouseVersionValue).Text, deviceParam.MM, (byte)CID, (byte)deviceParam.MID);
				if (mouseHaveNewVer)
				{
					((Control)customButton_MouseUpdate).Text = LanguageFile.Dialogs[52];
					customButton_MouseUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\update_nr.png");
					customButton_MouseUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\update_enter.png");
				}
				else
				{
					((Control)customButton_MouseUpdate).Text = LanguageFile.Dialogs[54];
					customButton_MouseUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
					customButton_MouseUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
					customButton_MouseUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
				}
			}
			else
			{
				((Control)customButton_MouseUpdate).Text = LanguageFile.Dialogs[52];
				customButton_MouseUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
				customButton_MouseUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
				customButton_MouseUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
			}
		}
	}

	public event DeviceUpdatingEventHandler DeviceUpdating;

	public event DeviceUpdateFailEventHandler DeviceUpdateFail;

	public UC_DeviceUpdate(LanguageFile language, DriveConfig.DeviceParam param)
	{
		InitializeComponent();
		((Control)customButton_DongleUpdate).ForeColor = FormMain.driveParam.BtnForeClr;
		((Control)customButton_MouseUpdate).ForeColor = FormMain.driveParam.BtnForeClr;
		languageFile = language;
		LanguageChange();
		((Control)label_DongleVersionValue).Text = "";
		((Control)label_MouseVersionValue).Text = "";
		((Control)label_UpdateTips).ForeColor = FormMain.driveParam.TipsClr;
		deviceParam = param;
		CID = FormMain.driveParam.CID;
		if (!Directory.Exists(Environment.CurrentDirectory + "\\bin"))
		{
			((Control)customButton_DongleUpdate).Visible = false;
			((Control)customButton_MouseUpdate).Visible = false;
			((Control)label_NewVerTips).Visible = false;
			((Control)this).Height = ((Control)label_NewVerTips).Top;
		}
		else
		{
			((Control)label_NewVerTips).Visible = true;
			((Control)label_NewVerTips).Text = LanguageFile.Dialogs[90];
		}
	}

	public void ResetDongleButton()
	{
		dongleLastVersion = false;
		((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[52];
	}

	public void LanguageChange()
	{
		int num = 0;
		((Control)label_DeviceInfo).Text = LanguageFile.Dialogs[82];
		((Control)label_DongleVersion).Text = languageFile.FromSettingString[1];
		((Control)label_MouseVersion).Text = languageFile.FromSettingString[2];
		num = ((((Control)label_DongleVersion).Right > ((Control)label_MouseVersion).Right) ? ((Control)label_DongleVersion).Right : ((Control)label_MouseVersion).Right);
		num += 5;
		((Control)label_DongleVersionValue).Location = new Point(num, ((Control)label_DongleVersionValue).Top);
		((Control)label_MouseVersionValue).Location = new Point(num, ((Control)label_MouseVersionValue).Top);
		num = ((((Control)label_DongleVersionValue).Right > ((Control)label_MouseVersionValue).Right) ? ((Control)label_DongleVersionValue).Right : ((Control)label_MouseVersionValue).Right);
		num += 10;
		((Control)customButton_DongleUpdate).Text = (dongleLastVersion ? LanguageFile.Dialogs[54] : LanguageFile.Dialogs[52]);
		((Control)customButton_MouseUpdate).Text = (mouseLastVersion ? LanguageFile.Dialogs[54] : LanguageFile.Dialogs[52]);
		((Control)customButton_DongleUpdate).Location = new Point(num, ((Control)customButton_DongleUpdate).Top);
		((Control)customButton_MouseUpdate).Location = new Point(num, ((Control)customButton_MouseUpdate).Top);
		num = ((Control)customButton_MouseUpdate).Right + 20;
		((Control)pictureBox_Tips).Location = new Point(num, ((Control)pictureBox_Tips).Top);
		((Control)label_UpdateTips).Location = new Point(((Control)pictureBox_Tips).Right + 5, ((Control)label_UpdateTips).Top);
		((Control)label_UpdateTips).Text = LanguageFile.Dialogs[(int)dialog];
		((Control)label_NewVerTips).Text = LanguageFile.Dialogs[90];
	}

	private bool CheckDongleUpdate()
	{
		//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
		CXFILE_TYPE cXFILE_TYPE = CXFILE_TYPE.Dongle;
		string text = "";
		bool flag = false;
		int num = 0;
		Dongle = new List<string>();
		byte[] buffers;
		if (FormMain.ConnectedDeviceInfo.isUSB)
		{
			if (deviceParam.D4M != null)
			{
				text = deviceParam.D4M;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot4KDongle = UsbFinder.FindBootDevices(buffers);
			}
			if (deviceParam.D2M != null)
			{
				text = deviceParam.D2M;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot2KDongle = UsbFinder.FindBootDevices(buffers);
			}
			if (deviceParam.DM != null)
			{
				text = deviceParam.DM;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot1KDongle = UsbFinder.FindBootDevices(buffers);
			}
			Dongle = new List<string>();
			Dongle.Clear();
			for (int i = 0; i < FormMain.AllDeviceInfoList.Count; i++)
			{
				if (!FormMain.AllDeviceInfoList[i].isUSB)
				{
					Dongle.Add(FormMain.AllDeviceInfoList[i].deviceString);
				}
			}
		}
		else
		{
			if (FormMain.SelectedDeviceInfo.deviceInfo.DeviceType == 1)
			{
				reportRate = 4;
			}
			else if (FormMain.SelectedDeviceInfo.deviceInfo.DeviceType == 4)
			{
				reportRate = 2;
			}
			else if (FormMain.SelectedDeviceInfo.deviceInfo.DeviceType == 0)
			{
				reportRate = 1;
			}
			if (reportRate == 4)
			{
				text = deviceParam.D4M;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot4KDongle = UsbFinder.FindBootDevices(buffers);
			}
			else if (reportRate == 2)
			{
				text = deviceParam.D2M;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot2KDongle = UsbFinder.FindBootDevices(buffers);
			}
			else
			{
				text = deviceParam.DM;
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", text, (byte)CID, (byte)deviceParam.MID, out buffers);
				boot1KDongle = UsbFinder.FindBootDevices(buffers);
			}
			for (int j = 0; j < FormMain.AllDeviceInfoList.Count; j++)
			{
				if (!FormMain.AllDeviceInfoList[j].isUSB && FormMain.AllDeviceInfoList[j].deviceString == FormMain.SelectedDeviceInfo.deviceString)
				{
					Dongle.Add(FormMain.SelectedDeviceInfo.deviceString);
				}
			}
		}
		num = Dongle.Count + ((boot1KDongle != null) ? boot1KDongle.Length : 0) + ((boot2KDongle != null) ? boot2KDongle.Length : 0) + ((boot4KDongle != null) ? boot4KDongle.Length : 0);
		if (num == 1)
		{
			flag = false;
			if (boot1KDongle != null && boot1KDongle.Length == 1)
			{
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", deviceParam.DM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				flag = true;
				reportRate = 1;
			}
			if (boot2KDongle != null && !flag && boot2KDongle.Length == 1)
			{
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", deviceParam.D2M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				flag = true;
				reportRate = 2;
			}
			if (boot4KDongle != null && !flag && boot4KDongle.Length == 1)
			{
				flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, "", deviceParam.D4M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				flag = true;
				reportRate = 4;
			}
			if (!flag)
			{
				if (FormMain.ConnectedDeviceInfo.isUSB)
				{
					for (int k = 0; k < FormMain.AllDeviceInfoList.Count; k++)
					{
						if (!FormMain.AllDeviceInfoList[k].isUSB)
						{
							if (FormMain.AllDeviceInfoList[k].deviceInfo.DeviceType == 1)
							{
								reportRate = 4;
							}
							else if (FormMain.AllDeviceInfoList[k].deviceInfo.DeviceType == 4)
							{
								reportRate = 2;
							}
							else if (FormMain.AllDeviceInfoList[k].deviceInfo.DeviceType == 0)
							{
								reportRate = 1;
							}
							break;
						}
					}
				}
				if (reportRate == 4)
				{
					int slaveVersion = 0;
					string currentVersion = "";
					if (UsbFinder.GetSlaveVersion(Dongle[0], out slaveVersion))
					{
						currentVersion = "v" + ValueConvert.IntToVersion(slaveVersion);
					}
					flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, currentVersion, deviceParam.D4M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				}
				else if (reportRate == 2)
				{
					int slaveVersion2 = 0;
					string currentVersion2 = "";
					if (UsbFinder.GetSlaveVersion(Dongle[0], out slaveVersion2))
					{
						currentVersion2 = "v" + ValueConvert.IntToVersion(slaveVersion2);
					}
					flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, currentVersion2, deviceParam.D2M, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				}
				else
				{
					flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, ((Control)label_DongleVersionValue).Text, deviceParam.DM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
				}
				if (!flag)
				{
					((Control)pictureBox_Tips).Visible = false;
					((Control)label_UpdateTips).Visible = false;
					dongleLastVersion = true;
					((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[54];
				}
			}
		}
		else
		{
			flag = false;
			if (num == 0)
			{
				dialog = DialogEnum.NoDevice;
			}
			else
			{
				dialog = DialogEnum.MultiOnline;
			}
			if (!flag)
			{
				((Control)pictureBox_Tips).Visible = true;
				((Control)label_UpdateTips).Visible = true;
				((Control)pictureBox_Tips).Location = new Point(((Control)pictureBox_Tips).Left, ((Control)customButton_DongleUpdate).Top + (((Control)customButton_DongleUpdate).Height - ((Control)pictureBox_Tips).Height) / 2);
				((Control)label_UpdateTips).Location = new Point(((Control)pictureBox_Tips).Right + 5, ((Control)pictureBox_Tips).Top);
				((Control)label_UpdateTips).Text = LanguageFile.Dialogs[(int)dialog];
			}
		}
		if (needUpdateAnother && !flag)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[(int)dialog])).ShowDialog();
		}
		needUpdateAnother = false;
		return flag;
	}

	private void customButton_DongleUpdate_Click(object sender, EventArgs e)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		FormMain.OnlineCheckCount = 0;
		if (!CheckDongleUpdate())
		{
			return;
		}
		updating = true;
		DeviceUpdating?.Invoke(this, updating);
		((Control)pictureBox_Tips).Visible = false;
		((Control)label_UpdateTips).Visible = false;
		FormUpdate formUpdate = new FormUpdate(UpdateBuffer, (byte)reportRate);
		((Form)formUpdate).ShowDialog();
		updating = false;
		DeviceUpdating?.Invoke(this, updating);
		if (formUpdate.updateSuccess)
		{
			UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(UpdateBuffer);
			((Control)label_DongleVersionValue).Text = "v" + ValueConvert.IntToVersion((int)upgradeFileHeader.version);
			dongleLastVersion = true;
			((Control)customButton_DongleUpdate).Text = LanguageFile.Dialogs[54];
			dongleHaveNewVer = false;
			customButton_DongleUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
			customButton_DongleUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
			customButton_DongleUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
			if (mouseHaveNewVer)
			{
				needUpdateAnother = true;
				customButton_MouseUpdate_Click(null, null);
			}
		}
		else
		{
			((Control)label_DongleVersionValue).Text = "";
			DeviceUpdateFail?.Invoke(this);
		}
	}

	private bool CheckMouseUpdate()
	{
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		CXFILE_TYPE cXFILE_TYPE = CXFILE_TYPE.Mouse;
		bool flag = false;
		flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, ((Control)label_MouseVersionValue).Text, deviceParam.MM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
		string[] array = UsbFinder.FindBootDevices(UpdateBuffer);
		List<string> list = new List<string>();
		for (int i = 0; i < FormMain.AllDeviceInfoList.Count; i++)
		{
			if (FormMain.AllDeviceInfoList[i].isUSB)
			{
				list.Add(FormMain.AllDeviceInfoList[i].deviceString);
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			list.Add(array[j]);
		}
		if (list.Count == 1 || !flag)
		{
			flag = DeviceUpdateFile.HasDeviceUpdateFile((byte)cXFILE_TYPE, ((Control)label_MouseVersionValue).Text, deviceParam.MM, (byte)CID, (byte)deviceParam.MID, out UpdateBuffer);
			if (!flag)
			{
				((Control)pictureBox_Tips).Visible = false;
				((Control)label_UpdateTips).Visible = false;
				mouseLastVersion = true;
				((Control)customButton_MouseUpdate).Text = LanguageFile.Dialogs[54];
			}
		}
		else
		{
			flag = false;
			if (list.Count == 0)
			{
				dialog = DialogEnum.OnlyWired;
			}
			else
			{
				dialog = DialogEnum.MultiOnline;
			}
			((Control)pictureBox_Tips).Visible = true;
			((Control)label_UpdateTips).Visible = true;
			((Control)pictureBox_Tips).Location = new Point(((Control)pictureBox_Tips).Left, ((Control)customButton_MouseUpdate).Top + (((Control)customButton_MouseUpdate).Height - ((Control)pictureBox_Tips).Height) / 2);
			((Control)label_UpdateTips).Location = new Point(((Control)pictureBox_Tips).Right + 5, ((Control)pictureBox_Tips).Top);
			((Control)label_UpdateTips).Text = LanguageFile.Dialogs[(int)dialog];
		}
		if (needUpdateAnother && !flag)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[(int)dialog])).ShowDialog();
		}
		needUpdateAnother = false;
		return flag;
	}

	private void customButton_MouseUpdate_Click(object sender, EventArgs e)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		FormMain.OnlineCheckCount = 0;
		if (!CheckMouseUpdate())
		{
			return;
		}
		updating = true;
		DeviceUpdating?.Invoke(this, updating);
		((Control)pictureBox_Tips).Visible = false;
		((Control)label_UpdateTips).Visible = false;
		FormUpdate formUpdate = new FormUpdate(UpdateBuffer, 0);
		((Form)formUpdate).ShowDialog();
		updating = false;
		DeviceUpdating?.Invoke(this, updating);
		if (formUpdate.updateSuccess)
		{
			UpgradeFileHeader upgradeFileHeader = UsbUpgradeFile.ByteToStruct(UpdateBuffer);
			((Control)label_MouseVersionValue).Text = "v" + ValueConvert.IntToVersion((int)upgradeFileHeader.version);
			RegeditManager.SetMouseVersion((byte)CID, (byte)deviceParam.MID, ((Control)label_MouseVersionValue).Text);
			mouseLastVersion = true;
			((Control)customButton_MouseUpdate).Text = LanguageFile.Dialogs[54];
			mouseHaveNewVer = false;
			customButton_MouseUpdate.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_nr.png");
			customButton_MouseUpdate.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_down.png");
			customButton_MouseUpdate.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\ok_enter.png");
			if (dongleHaveNewVer)
			{
				needUpdateAnother = true;
				customButton_DongleUpdate_Click(null, null);
			}
		}
		else
		{
			((Control)label_DongleVersionValue).Text = "";
			DeviceUpdateFail?.Invoke(this);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		label_UpdateTips = new Label();
		label_MouseVersionValue = new Label();
		label_DongleVersionValue = new Label();
		label_MouseVersion = new Label();
		label_DongleVersion = new Label();
		label_DeviceInfo = new Label();
		pictureBox_Tips = new PictureBox();
		pictureBox_Title = new PictureBox();
		customButton_MouseUpdate = new CustomButton();
		customButton_DongleUpdate = new CustomButton();
		label_NewVerTips = new Label();
		((ISupportInitialize)pictureBox_Tips).BeginInit();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_UpdateTips).BackColor = Color.Transparent;
		((Control)label_UpdateTips).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)label_UpdateTips).Location = new Point(387, 58);
		((Control)label_UpdateTips).Name = "label_UpdateTips";
		((Control)label_UpdateTips).Size = new Size(346, 60);
		((Control)label_UpdateTips).TabIndex = 140;
		((Control)label_UpdateTips).Text = "让鼠标进入配对状态，并靠近接收器";
		((Control)label_UpdateTips).Visible = false;
		((Control)label_MouseVersionValue).AutoSize = true;
		((Control)label_MouseVersionValue).BackColor = Color.Transparent;
		((Control)label_MouseVersionValue).ForeColor = Color.White;
		((Control)label_MouseVersionValue).Location = new Point(167, 89);
		((Control)label_MouseVersionValue).Name = "label_MouseVersionValue";
		((Control)label_MouseVersionValue).Size = new Size(35, 20);
		((Control)label_MouseVersionValue).TabIndex = 135;
		((Control)label_MouseVersionValue).Text = "v1.0";
		((Control)label_DongleVersionValue).AutoSize = true;
		((Control)label_DongleVersionValue).BackColor = Color.Transparent;
		((Control)label_DongleVersionValue).ForeColor = Color.White;
		((Control)label_DongleVersionValue).Location = new Point(167, 54);
		((Control)label_DongleVersionValue).Name = "label_DongleVersionValue";
		((Control)label_DongleVersionValue).Size = new Size(35, 20);
		((Control)label_DongleVersionValue).TabIndex = 134;
		((Control)label_DongleVersionValue).Text = "v1.0";
		((Control)label_MouseVersion).AutoSize = true;
		((Control)label_MouseVersion).BackColor = Color.Transparent;
		((Control)label_MouseVersion).ForeColor = Color.White;
		((Control)label_MouseVersion).Location = new Point(22, 89);
		((Control)label_MouseVersion).Name = "label_MouseVersion";
		((Control)label_MouseVersion).Size = new Size(107, 20);
		((Control)label_MouseVersion).TabIndex = 133;
		((Control)label_MouseVersion).Text = "鼠标固件版本：";
		((Control)label_DongleVersion).AutoSize = true;
		((Control)label_DongleVersion).BackColor = Color.Transparent;
		((Control)label_DongleVersion).ForeColor = Color.White;
		((Control)label_DongleVersion).Location = new Point(21, 54);
		((Control)label_DongleVersion).Name = "label_DongleVersion";
		((Control)label_DongleVersion).Size = new Size(127, 20);
		((Control)label_DongleVersion).TabIndex = 132;
		((Control)label_DongleVersion).Text = "dongle固件版本：";
		((Control)label_DeviceInfo).AutoSize = true;
		((Control)label_DeviceInfo).ForeColor = Color.White;
		((Control)label_DeviceInfo).Location = new Point(57, 10);
		((Control)label_DeviceInfo).Name = "label_DeviceInfo";
		((Control)label_DeviceInfo).Size = new Size(65, 20);
		((Control)label_DeviceInfo).TabIndex = 131;
		((Control)label_DeviceInfo).Text = "设备信息";
		((Control)pictureBox_Tips).BackColor = Color.Transparent;
		((Control)pictureBox_Tips).BackgroundImage = (Image)(object)Resources.提示符号;
		((Control)pictureBox_Tips).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Tips).Location = new Point(355, 58);
		((Control)pictureBox_Tips).Name = "pictureBox_Tips";
		((Control)pictureBox_Tips).Size = new Size(20, 20);
		pictureBox_Tips.SizeMode = (PictureBoxSizeMode)3;
		pictureBox_Tips.TabIndex = 139;
		pictureBox_Tips.TabStop = false;
		((Control)pictureBox_Tips).Visible = false;
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 16);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 136;
		pictureBox_Title.TabStop = false;
		((Control)customButton_MouseUpdate).ForeColor = Color.White;
		((Control)customButton_MouseUpdate).Location = new Point(221, 85);
		((Control)customButton_MouseUpdate).Margin = new Padding(3, 4, 3, 4);
		customButton_MouseUpdate.MouseDownImage = (Image)(object)Resources.配对工具;
		customButton_MouseUpdate.MouseEnterImage = (Image)(object)Resources.配对工具;
		((Control)customButton_MouseUpdate).Name = "customButton_MouseUpdate";
		customButton_MouseUpdate.NormalImage = (Image)(object)Resources.配对工具;
		((Control)customButton_MouseUpdate).Size = new Size(112, 26);
		((Control)customButton_MouseUpdate).TabIndex = 138;
		((Control)customButton_MouseUpdate).Text = "检查升级";
		((Control)customButton_MouseUpdate).Click += customButton_MouseUpdate_Click;
		((Control)customButton_DongleUpdate).ForeColor = Color.White;
		((Control)customButton_DongleUpdate).Location = new Point(221, 50);
		((Control)customButton_DongleUpdate).Margin = new Padding(3, 4, 3, 4);
		customButton_DongleUpdate.MouseDownImage = (Image)(object)Resources.配对工具;
		customButton_DongleUpdate.MouseEnterImage = (Image)(object)Resources.配对工具;
		((Control)customButton_DongleUpdate).Name = "customButton_DongleUpdate";
		customButton_DongleUpdate.NormalImage = (Image)(object)Resources.配对工具;
		((Control)customButton_DongleUpdate).Size = new Size(112, 26);
		((Control)customButton_DongleUpdate).TabIndex = 137;
		((Control)customButton_DongleUpdate).Text = "检查升级";
		((Control)customButton_DongleUpdate).Click += customButton_DongleUpdate_Click;
		((Control)label_NewVerTips).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)label_NewVerTips).Location = new Point(22, 123);
		((Control)label_NewVerTips).Name = "label_NewVerTips";
		((Control)label_NewVerTips).Size = new Size(835, 53);
		((Control)label_NewVerTips).TabIndex = 141;
		((Control)label_NewVerTips).Text = "label1";
		label_NewVerTips.TextAlign = (ContentAlignment)16;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)label_NewVerTips);
		((Control)this).Controls.Add((Control)(object)label_UpdateTips);
		((Control)this).Controls.Add((Control)(object)pictureBox_Tips);
		((Control)this).Controls.Add((Control)(object)customButton_MouseUpdate);
		((Control)this).Controls.Add((Control)(object)customButton_DongleUpdate);
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Controls.Add((Control)(object)label_MouseVersionValue);
		((Control)this).Controls.Add((Control)(object)label_DongleVersionValue);
		((Control)this).Controls.Add((Control)(object)label_MouseVersion);
		((Control)this).Controls.Add((Control)(object)label_DongleVersion);
		((Control)this).Controls.Add((Control)(object)label_DeviceInfo);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_DeviceUpdate";
		((Control)this).Size = new Size(857, 188);
		((ISupportInitialize)pictureBox_Tips).EndInit();
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
