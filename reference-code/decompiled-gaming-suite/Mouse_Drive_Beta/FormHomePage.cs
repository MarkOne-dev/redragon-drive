using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;
using WindControls;

namespace Mouse_Drive_Beta;

public class FormHomePage : Form
{
	private SkinForm skinForm = new SkinForm(movable: true);

	private FormMover formMover = new FormMover();

	private FormResize formResize = new FormResize();

	private List<Image> DeviceOnlineImage = new List<Image>();

	private List<Image> DeviceOfflineImage = new List<Image>();

	private Image UnknownDeviceImage;

	private DriveConfig.DriveParam driveParam = new DriveConfig.DriveParam();

	private LanguageFile languageFile = new LanguageFile();

	private List<CustomDeviceButton> CustomDeviceButtonList = new List<CustomDeviceButton>();

	private List<DeviceAllInfo> AllDeviceInfoList = new List<DeviceAllInfo>();

	public DeviceAllInfo SelectDevice;

	private Timer StartUSBProcessTimer;

	private Timer GetDeviceInfoTimer;

	public int SelectIndex = -1;

	private int CurrentIndex;

	public bool SelectChangeFlag;

	private List<Point> InitLocation = new List<Point>();

	private Timer HideTimer;

	private Timer HandleCreTimer;

	private FormMain formMain;

	public bool LoadFlag;

	private List<int> UnknownDeviceIndex = new List<int>();

	private SystemLog systemLog = new SystemLog("HomePage_log");

	private List<byte> updateFialInfo = new List<byte>();

	private bool formProgram;

	private bool toolTipFlag;

	private IContainer components;

	private CustomButton customButton_Mini;

	private CustomButton customButton_Close;

	private CustomTrack customTrack1;

	private CustomPanel customPanel_Device;

	private ContextMenuStrip contextMenuStrip_Pallet;

	private ToolStripMenuItem ToolStripMenuItem_OpenMenu;

	private ToolStripMenuItem ToolStripMenuItem_Exit;

	private NotifyIcon notifyIcon_Main;

	private Label label_ConnctDevice;

	private CustomComboBox customComboBox1;

	private CustomButton customButton_Previous;

	private CustomButton customButton_Next;

	private CustomButton customButton_Repair;

	private ToolTip toolTip;

	private Label label_EnterUse;

	public FormHomePage(DriveConfig.DriveParam config)
	{
		InitializeComponent();
		LoadFlag = false;
		SetStyles();
		formResize.Resize((Form)(object)this);
		formMover.AddForm((Form)(object)this);
		formMover.AddControl((Control)(object)customPanel_Device);
		LanguageFile.FormSetColor((Control)(object)this);
		driveParam = config;
		if (languageFile.GetLanguageFileCount() == 0)
		{
			((Form)this).Close();
		}
		else
		{
			languageFile.SetLanguage(languageFile.LanguageIndex, (Control)(object)this);
		}
		List<string> oSVersion = GetOSVersion();
		for (int i = 0; i < oSVersion.Count; i++)
		{
			systemLog.WriteLog(oSVersion[i]);
		}
		UIInit();
	}

	public FormHomePage(DriveConfig.DriveParam config, bool formPro)
	{
		InitializeComponent();
		LoadFlag = false;
		formProgram = formPro;
		SetStyles();
		formResize.Resize((Form)(object)this);
		formMover.AddForm((Form)(object)this);
		formMover.AddControl((Control)(object)customPanel_Device);
		LanguageFile.FormSetColor((Control)(object)this);
		driveParam = config;
		if (languageFile.GetLanguageFileCount() == 0)
		{
			((Form)this).Close();
		}
		else
		{
			languageFile.SetLanguage(languageFile.LanguageIndex, (Control)(object)this);
		}
		List<string> oSVersion = GetOSVersion();
		for (int i = 0; i < oSVersion.Count; i++)
		{
			systemLog.WriteLog(oSVersion[i]);
		}
		if (!Directory.Exists(Environment.CurrentDirectory + "\\bin"))
		{
			formProgram = false;
		}
		UIInit();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	public static List<string> GetOSVersion()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		List<string> list = new List<string>();
		try
		{
			ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ManagementObject val = (ManagementObject)enumerator.Current;
					list.Add(((ManagementBaseObject)val)["Name"].ToString().Trim() + "     " + (Environment.Is64BitOperatingSystem ? "64bit" : "32bit"));
					list.Add(((ManagementBaseObject)val)["SerialNumber"].ToString().Trim());
					list.Add(((ManagementBaseObject)val)["OSLanguage"].ToString().Trim());
					list.Add(((ManagementBaseObject)val)["Manufacturer"].ToString().Trim());
				}
			}
			finally
			{
				((IDisposable)enumerator)?.Dispose();
			}
		}
		catch
		{
			list.Add("null");
		}
		return list;
	}

	private void UIInit()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		ImageListInit();
		((Control)label_ConnctDevice).Visible = false;
		ConnectLabelInit(LanguageFile.Dialogs[59]);
		((Control)label_EnterUse).Visible = driveParam.ClickMouseEnterTips == 1;
		((Control)label_EnterUse).Text = LanguageFile.Dialogs[95];
		customTrack1.SliderColor = driveParam.SliderClr;
		((Control)customButton_Previous).Visible = false;
		((Control)customButton_Next).Visible = false;
		SetComboBox(languageFile.LanguageType);
		if (StartUSBProcessTimer == null)
		{
			StartUSBProcessTimer = new Timer();
			StartUSBProcessTimer.Interval = 300;
			StartUSBProcessTimer.Tick += StartUSBProcessTimer_Tick;
		}
		StartUSBProcessTimer.Start();
		if (GetDeviceInfoTimer == null)
		{
			GetDeviceInfoTimer = new Timer();
			GetDeviceInfoTimer.Interval = 1000;
			GetDeviceInfoTimer.Tick += GetDeviceInfoTimer_Tick;
		}
		if (formProgram && !RegeditManager.GetFirmwareUpdateTips())
		{
			if (HandleCreTimer == null)
			{
				HandleCreTimer = new Timer();
				HandleCreTimer.Interval = 200;
				HandleCreTimer.Tick += HandleCreTimer_Tick;
			}
			HandleCreTimer.Start();
		}
		((Control)customButton_Repair).Visible = RegeditManager.GetUpdateFailInfo(out updateFialInfo);
		if (((Control)customButton_Repair).Visible)
		{
			ConnectLabelInit(LanguageFile.Dialogs[60]);
		}
		LoadToolStripMenuItem();
	}

	private void HandleCreTimer_Tick(object sender, EventArgs e)
	{
		if (((Control)this).IsHandleCreated)
		{
			HandleCreTimer.Stop();
		}
	}

	private void GetDeviceInfoTimer_Tick(object sender, EventArgs e)
	{
		List<int> list = new List<int>();
		if (UnknownDeviceIndex.Count > 0)
		{
			for (int i = 0; i < UnknownDeviceIndex.Count; i++)
			{
				DeviceAllInfo deviceAllInfo = new DeviceAllInfo
				{
					deviceString = AllDeviceInfoList[UnknownDeviceIndex[i]].deviceString,
					VID = AllDeviceInfoList[UnknownDeviceIndex[i]].VID,
					PID = AllDeviceInfoList[UnknownDeviceIndex[i]].PID
				};
				HIDD_ATTRIBUTES hidd_attributes = default(HIDD_ATTRIBUTES);
				UsbFinder.GetUsbDeviceAttribute(deviceAllInfo.deviceString, out hidd_attributes);
				int versionNumber = hidd_attributes.VersionNumber;
				deviceAllInfo.DongleVersion = "v" + ValueConvert.IntToVersion(versionNumber);
				deviceAllInfo.online = false;
				deviceAllInfo.deviceIndex = driveParam.DeviceTotal;
				if (GetDeviceInfo(ref deviceAllInfo) != 0)
				{
					AllDeviceInfoList.RemoveAt(i);
					if (GetDeviceInfo(ref deviceAllInfo) != 3)
					{
						AllDeviceInfoList.Insert(i, deviceAllInfo);
					}
					list.Add(i);
				}
			}
		}
		if (list.Count > 0)
		{
			UpdateDeviceState();
			for (int num = list.Count - 1; num >= 0; num--)
			{
				UnknownDeviceIndex.RemoveAt(list[num]);
			}
		}
		if (UnknownDeviceIndex.Count == 0)
		{
			GetDeviceInfoTimer.Stop();
		}
	}

	private void ConnectLabelInit(string str)
	{
		((Control)label_ConnctDevice).Text = str;
	}

	private void StartUSBProcessTimer_Tick(object sender, EventArgs e)
	{
		LoadFlag = true;
		StartUSBProcessTimer.Stop();
		((Control)label_ConnctDevice).Visible = true;
		USBProcess_Init();
	}

	public void USBProcess_Init()
	{
		UsbFinder.StartUsbChanged(OnUsbChangedEvent, 600);
	}

	private void ImageListInit()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0106: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_0141: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Expected O, but got Unknown
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_021e: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		notifyIcon_Main.Text = driveParam.DriveName;
		notifyIcon_Main.Icon = new Icon(Application.StartupPath + "\\res\\logo.ico");
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\1HomePage\\background.png");
		for (int i = 0; i < driveParam.DeviceTotal; i++)
		{
			Bitmap item = new Bitmap(baseDirectory + "\\res\\1HomePage\\dev" + (i + 1) + "online.png");
			DeviceOnlineImage.Add((Image)(object)item);
			item = new Bitmap(baseDirectory + "\\res\\1HomePage\\dev" + (i + 1) + "offline.png");
			DeviceOfflineImage.Add((Image)(object)item);
		}
		UnknownDeviceImage = (Image)new Bitmap(baseDirectory + "\\res\\1HomePage\\unknown.png");
		CustomButton customButton = customButton_Previous;
		CustomButton customButton2 = customButton_Previous;
		CustomButton customButton3 = customButton_Previous;
		Bitmap val = new Bitmap(baseDirectory + "\\res\\1HomePage\\forward.png");
		Image val2 = (Image)val;
		customButton3.NormalImage = (Image)val;
		Image mouseDownImage = (customButton2.MouseEnterImage = val2);
		customButton.MouseDownImage = mouseDownImage;
		CustomButton customButton4 = customButton_Next;
		CustomButton customButton5 = customButton_Next;
		CustomButton customButton6 = customButton_Next;
		Bitmap val4 = new Bitmap(baseDirectory + "\\res\\1HomePage\\backward.png");
		val2 = (Image)val4;
		customButton6.NormalImage = (Image)val4;
		mouseDownImage = (customButton5.MouseEnterImage = val2);
		customButton4.MouseDownImage = mouseDownImage;
		customButton_Mini.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_nr.png");
		customButton_Mini.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_down.png");
		customButton_Mini.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_enter.png");
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_enter.png");
		CustomButton customButton7 = customButton_Repair;
		CustomButton customButton8 = customButton_Repair;
		CustomButton customButton9 = customButton_Repair;
		Bitmap val6 = new Bitmap(baseDirectory + "\\res\\1HomePage\\repair.png");
		val2 = (Image)val6;
		customButton9.NormalImage = (Image)val6;
		mouseDownImage = (customButton8.MouseDownImage = val2);
		customButton7.MouseEnterImage = mouseDownImage;
		CustomComboBox customComboBox = customComboBox1;
		Color unselectItemColor = (((Control)customComboBox1).BackColor = driveParam.HPCbbBgClr);
		customComboBox.UnselectItemColor = unselectItemColor;
		customComboBox1.SelectItemColor = driveParam.HPCbbItemClr;
		((Control)customComboBox1).ForeColor = driveParam.HPCbbForeClr;
		customComboBox1.ArrowImageNoraml = null;
		((Control)customTrack1).Visible = false;
	}

	private void LoadToolStripMenuItem()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		FieldInfo[] fields = ((object)this).GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++)
		{
			if (!(fields[i].FieldType.Name == "ToolStripMenuItem"))
			{
				continue;
			}
			ToolStripMenuItem val = (ToolStripMenuItem)fields[i].GetValue(this);
			for (int j = 0; j < languageFile.ToolStripMenuItemLabelStructs.Count; j++)
			{
				if (((ToolStripItem)val).Name == languageFile.ToolStripMenuItemLabelStructs[j].Name)
				{
					((ToolStripItem)val).Text = languageFile.ToolStripMenuItemLabelStructs[j].Text;
					break;
				}
			}
		}
	}

	private void AddDevice(int count)
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		int[] array = new int[count];
		InitLocation.Clear();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		bool flag = false;
		if (File.Exists(baseDirectory + "\\res\\1HomePage\\query.png") && driveParam.OfflineCantIn > 0)
		{
			flag = true;
		}
		for (int i = 0; i < count; i++)
		{
			CustomDeviceButton customDeviceButton = new CustomDeviceButton();
			((RadioButton)customDeviceButton).Appearance = (Appearance)1;
			((ButtonBase)customDeviceButton).FlatAppearance.BorderSize = 0;
			((ButtonBase)customDeviceButton).FlatAppearance.CheckedBackColor = Color.Transparent;
			((ButtonBase)customDeviceButton).FlatAppearance.MouseDownBackColor = Color.Transparent;
			((ButtonBase)customDeviceButton).FlatAppearance.MouseOverBackColor = Color.Transparent;
			((ButtonBase)customDeviceButton).FlatStyle = (FlatStyle)0;
			((Control)customDeviceButton).Name = "customDeviceButton" + i;
			customDeviceButton.OfflineImage = ((AllDeviceInfoList[i].deviceIndex == driveParam.DeviceTotal) ? UnknownDeviceImage : DeviceOfflineImage[AllDeviceInfoList[i].deviceIndex]);
			customDeviceButton.Online = false;
			customDeviceButton.OnlineImage = ((AllDeviceInfoList[i].deviceIndex == driveParam.DeviceTotal) ? UnknownDeviceImage : DeviceOnlineImage[AllDeviceInfoList[i].deviceIndex]);
			Bitmap wiredImage = new Bitmap(baseDirectory + "\\res\\1HomePage\\wired.png");
			customDeviceButton.WiredImage = (Image)(object)wiredImage;
			wiredImage = new Bitmap(baseDirectory + "\\res\\1HomePage\\wireless.png");
			customDeviceButton.WirelessImage = (Image)(object)wiredImage;
			if (flag)
			{
				wiredImage = new Bitmap(baseDirectory + "\\res\\1HomePage\\query.png");
				customDeviceButton.QuestionImage = (Image)(object)wiredImage;
			}
			if (AllDeviceInfoList[i].deviceInfo.DeviceType == 1)
			{
				customDeviceButton.ReportRateImage = (Image)new Bitmap(baseDirectory + "\\res\\1HomePage\\4KHz.png");
			}
			else if (AllDeviceInfoList[i].deviceInfo.DeviceType == 4)
			{
				customDeviceButton.ReportRateImage = (Image)new Bitmap(baseDirectory + "\\res\\1HomePage\\2KHz.png");
			}
			else
			{
				try
				{
					customDeviceButton.ReportRateImage = (Image)new Bitmap(baseDirectory + "\\res\\1HomePage\\1KHz.png");
				}
				catch
				{
					customDeviceButton.ReportRateImage = null;
				}
			}
			((Control)customDeviceButton).Size = new Size(customDeviceButton.OnlineImage.Width, customDeviceButton.OnlineImage.Height);
			((Control)customDeviceButton).TabIndex = 0;
			((RadioButton)customDeviceButton).TabStop = true;
			((Control)customDeviceButton).Text = "";
			((ButtonBase)customDeviceButton).UseVisualStyleBackColor = true;
			((Control)customDeviceButton).Tag = i;
			int num = 0;
			switch (count)
			{
			case 1:
				num = (((Control)customPanel_Device).Width - ((Control)customDeviceButton).Width) / 2;
				break;
			case 2:
				num = ((i != 0) ? (((Control)customPanel_Device).Width / 3 + ((Control)customDeviceButton).Width / 2 + 15 * FormResize.GetDpi() / 96) : (((Control)customPanel_Device).Width / 3 - ((Control)customDeviceButton).Width / 2));
				break;
			default:
			{
				if (i == 0)
				{
					num = 0;
					break;
				}
				for (int j = 0; j < i; j++)
				{
					num += array[j] + 15 * FormResize.GetDpi() / 96;
				}
				break;
			}
			}
			((Control)customDeviceButton).Location = new Point(num, (((Control)customPanel_Device).Height - ((Control)customDeviceButton).Size.Height) / 2);
			InitLocation.Add(new Point(num, 3));
			array[i] = ((Control)customDeviceButton).Width;
			((Control)customDeviceButton).Click += CustomDeviceButton_Click;
			((Control)customDeviceButton).MouseEnter += CustomDeviceButton_MouseEnter;
			((Control)customDeviceButton).MouseLeave += CustomDeviceButton_MouseEnter;
			if (flag)
			{
				customDeviceButton.QuestionEnter += Button_QuestionEnter;
				customDeviceButton.QuestionLevae += Button_QuestionLevae;
			}
			CustomDeviceButtonList.Add(customDeviceButton);
			((Control)customPanel_Device).Controls.Add((Control)(object)customDeviceButton);
		}
		if (count > 3)
		{
			int num2 = 0;
			for (int k = 3; k < count; k++)
			{
				num2 += array[count - 1];
			}
			customTrack1.SliderLength = ((Control)customTrack1).Width - num2 - 15 * FormResize.GetDpi() / 96;
			((Control)customButton_Previous).Visible = true;
			((Control)customButton_Next).Visible = true;
		}
		else
		{
			((Control)customTrack1).Visible = false;
			((Control)customButton_Previous).Visible = false;
			((Control)customButton_Next).Visible = false;
		}
	}

	private void Button_QuestionLevae(object sender)
	{
		if (toolTipFlag)
		{
			toolTipFlag = false;
			toolTip.Active = false;
		}
	}

	private void Button_QuestionEnter(object sender)
	{
		if (toolTipFlag)
		{
			return;
		}
		toolTipFlag = true;
		toolTip.Active = true;
		CustomDeviceButton customDeviceButton = (CustomDeviceButton)sender;
		if (AllDeviceInfoList.Count == 1)
		{
			toolTip.SetToolTip((Control)(object)customDeviceButton, LanguageFile.Dialogs[86] + "\r\n" + LanguageFile.Dialogs[88]);
		}
		else if (AllDeviceInfoList.Count == 2)
		{
			int num = 0;
			for (int i = 0; i < AllDeviceInfoList.Count; i++)
			{
				if (AllDeviceInfoList[i].online)
				{
					num++;
				}
			}
			switch (num)
			{
			case 0:
				toolTip.SetToolTip((Control)(object)customDeviceButton, LanguageFile.Dialogs[31]);
				break;
			case 1:
				if (AllDeviceInfoList[0].deviceInfo.MID == AllDeviceInfoList[1].deviceInfo.MID)
				{
					toolTip.SetToolTip((Control)(object)customDeviceButton, LanguageFile.Dialogs[86] + "\r\n" + LanguageFile.Dialogs[87]);
				}
				else
				{
					toolTip.SetToolTip((Control)(object)customDeviceButton, LanguageFile.Dialogs[31]);
				}
				break;
			}
		}
		else
		{
			toolTip.SetToolTip((Control)(object)customDeviceButton, LanguageFile.Dialogs[31]);
		}
	}

	private void CustomDeviceButton_MouseEnter(object sender, EventArgs e)
	{
		if (AllDeviceInfoList.Count == 0)
		{
			return;
		}
		int num = (int)((Control)(CustomDeviceButton)sender).Tag;
		if (num >= AllDeviceInfoList.Count)
		{
			return;
		}
		string deviceString = AllDeviceInfoList[num].deviceString;
		DeviceAllInfo item = AllDeviceInfoList[num];
		item.online = false;
		byte[] deviceOnLineWithAddress = UsbFinder.GetDeviceOnLineWithAddress(deviceString);
		item.online = deviceOnLineWithAddress[6] == 1;
		item.address = new List<byte>();
		for (int i = 7; i < 10; i++)
		{
			item.address.Add(deviceOnLineWithAddress[i]);
		}
		if (item.online)
		{
			int version = UsbFinder.GetVersion(deviceString);
			item.MouseVersion = "v" + ValueConvert.IntToVersion(version);
			if (version != 0)
			{
				RegeditManager.SetMouseVersion(item.deviceInfo.CID, item.deviceInfo.MID, item.MouseVersion);
			}
		}
		AllDeviceInfoList.RemoveAt(num);
		AllDeviceInfoList.Insert(num, item);
		CustomDeviceButtonList[num].Online = AllDeviceInfoList[num].online;
	}

	private void CustomDeviceButton_Click(object sender, EventArgs e)
	{
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = true;
		CustomDeviceButton customDeviceButton = (CustomDeviceButton)sender;
		SelectChangeFlag = true;
		SelectIndex = (int)((Control)customDeviceButton).Tag;
		SelectDevice = AllDeviceInfoList[(int)((Control)customDeviceButton).Tag];
		if (AllDeviceInfoList[SelectIndex].deviceIndex == driveParam.DeviceTotal)
		{
			int num = 0;
			for (int i = 0; i < AllDeviceInfoList.Count; i++)
			{
				if (AllDeviceInfoList[i].deviceIndex == driveParam.DeviceTotal && !AllDeviceInfoList[i].isUSB)
				{
					num++;
				}
			}
			if (num == 1)
			{
				((Form)new FormPair(AllDeviceInfoList[SelectIndex], driveParam.CID)).ShowDialog();
				OnUsbChangedEvent(isPlug: true);
			}
			return;
		}
		if (AllDeviceInfoList.Count == 2)
		{
			if (AllDeviceInfoList[0].deviceIndex == AllDeviceInfoList[1].deviceIndex)
			{
				byte[] deviceOnLineWithAddress = UsbFinder.GetDeviceOnLineWithAddress(AllDeviceInfoList[0].deviceString);
				byte[] deviceOnLineWithAddress2 = UsbFinder.GetDeviceOnLineWithAddress(AllDeviceInfoList[1].deviceString);
				if (deviceOnLineWithAddress[6] != deviceOnLineWithAddress2[1])
				{
					DeviceAllInfo item = AllDeviceInfoList[1 - SelectIndex];
					int version = UsbFinder.GetVersion(item.deviceString);
					item.MouseVersion = "v" + ValueConvert.IntToVersion(version);
					if (version != 0)
					{
						RegeditManager.SetMouseVersion(item.deviceInfo.CID, item.deviceInfo.MID, item.MouseVersion);
					}
					AllDeviceInfoList.RemoveAt(1 - SelectIndex);
					AllDeviceInfoList.Insert(1 - SelectIndex, item);
					if ((deviceOnLineWithAddress[6] == 0 && SelectIndex == 0) || (deviceOnLineWithAddress2[6] == 0 && SelectIndex == 1))
					{
						bool flag3 = DeviceUpdateFile.HasNewVersion(210, "v" + AllDeviceInfoList[1 - SelectIndex].MouseVersion, driveParam.DeviceParams[AllDeviceInfoList[1 - SelectIndex].deviceIndex].MM, AllDeviceInfoList[1 - SelectIndex].deviceInfo.CID, AllDeviceInfoList[1 - SelectIndex].deviceInfo.MID);
						string mcu = driveParam.DeviceParams[AllDeviceInfoList[SelectIndex].deviceIndex].DM;
						DeviceType deviceType = (DeviceType)AllDeviceInfoList[SelectIndex].deviceInfo.DeviceType;
						int num2 = 1;
						if (deviceType.ToString().Contains("Wireless"))
						{
							if (deviceType.ToString().Contains("2K"))
							{
								num2 = 2;
								mcu = driveParam.DeviceParams[AllDeviceInfoList[SelectIndex].deviceIndex].D2M;
							}
							if (deviceType.ToString().Contains("4K"))
							{
								num2 = 4;
								mcu = driveParam.DeviceParams[AllDeviceInfoList[SelectIndex].deviceIndex].D4M;
							}
						}
						bool flag4 = DeviceUpdateFile.HasNewVersion(211, "v" + AllDeviceInfoList[SelectIndex].DongleVersion, mcu, AllDeviceInfoList[SelectIndex].deviceInfo.CID, AllDeviceInfoList[SelectIndex].deviceInfo.MID);
						if (flag3 == flag4)
						{
							FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[30], DialogButtons.OKCanel);
							((Form)formDialog).ShowDialog();
							flag = formDialog.resault;
							if (!flag)
							{
								flag2 = false;
							}
						}
						else
						{
							flag2 = false;
							if (!flag3 & flag4)
							{
								FormDialog formDialog2 = new FormDialog(LanguageFile.Dialogs[92], DialogButtons.OKCanel);
								((Form)formDialog2).ShowDialog();
								if (formDialog2.resault)
								{
									byte[] buffers = new byte[1024];
									DeviceUpdateFile.HasDeviceUpdateFile(211, "v" + AllDeviceInfoList[SelectIndex].DongleVersion, mcu, AllDeviceInfoList[SelectIndex].deviceInfo.CID, AllDeviceInfoList[SelectIndex].deviceInfo.MID, out buffers);
									((Form)new FormUpdate(buffers, (byte)num2)).ShowDialog();
									flag = false;
								}
							}
							else if (!flag4 & flag3)
							{
								((Form)new FormDialog(LanguageFile.Dialogs[91], DialogButtons.OK)).ShowDialog();
							}
						}
					}
				}
			}
			if (flag)
			{
				FormPair formPair = new FormPair(AllDeviceInfoList[SelectIndex], driveParam.CID);
				((Form)formPair).ShowDialog();
				if (formPair != null)
				{
					ButtonStateEnum pairState = formPair.PairState;
					systemLog.WriteLog("Pair " + pairState);
				}
				systemLog.WriteLog("ExitPair");
				OnUsbChangedEvent(isPlug: true);
				flag2 = false;
			}
		}
		if (flag2)
		{
			HomePageExit();
			if (HideTimer == null)
			{
				HideTimer = new Timer();
				HideTimer.Interval = 100;
				HideTimer.Tick += HideTimer_Tick;
			}
			HideTimer.Start();
			if (formMain == null)
			{
				formMain = new FormMain(SelectDevice, driveParam);
				((Form)formMain).StartPosition = (FormStartPosition)0;
				((Form)formMain).Location = new Point(((Form)this).Location.X, ((Form)this).Location.Y);
				((Form)formMain).ShowDialog();
			}
		}
	}

	private void HomePageExit()
	{
		GetDeviceInfoTimer.Stop();
		notifyIcon_Main.Visible = false;
		notifyIcon_Main.Icon = null;
		UsbFinder.StopUsbChanged();
		UsbServer.Exit();
		systemLog.WriteLog("UsbServer Exit");
	}

	private void HideTimer_Tick(object sender, EventArgs e)
	{
		if (formMain != null && formMain.LoadFlag)
		{
			((Component)(object)notifyIcon_Main).Dispose();
			HideTimer.Stop();
			systemLog.WriteLog("HomePage Exit");
			((Control)this).Hide();
		}
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		try
		{
			HomePageExit();
			Environment.Exit(0);
			((Form)this).Close();
		}
		catch
		{
			systemLog.WriteLog("HomePage Exit");
			Environment.Exit(0);
			((Form)this).Close();
		}
	}

	private void customButton_Mini_Click(object sender, EventArgs e)
	{
		((Control)this).Hide();
		((Form)this).WindowState = (FormWindowState)1;
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

	private void ToolStripMenuItem_OpenMenu_Click(object sender, EventArgs e)
	{
		((Control)this).Show();
		((Form)this).WindowState = (FormWindowState)0;
		SwitchToThisWindow(((Control)this).Handle, fAltTab: true);
	}

	private void notifyIcon_Main_MouseClick(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			((Control)this).Show();
			((Form)this).WindowState = (FormWindowState)0;
			SwitchToThisWindow(((Control)this).Handle, fAltTab: true);
		}
	}

	private void OnUsbChangedEvent(bool isPlug)
	{
		int interfaceid = driveParam.Interfaceid;
		int deviceid = driveParam.Deviceid;
		GetDeviceInfoTimer.Stop();
		UnknownDeviceIndex.Clear();
		AllDeviceInfoList.Clear();
		List<string> list = new List<string>();
		for (int i = 0; i < driveParam.M_PID.Count; i++)
		{
			list.Add(driveParam.M_PID[i]);
		}
		for (int j = 0; j < driveParam.D_PID.Count; j++)
		{
			list.Add(driveParam.D_PID[j]);
		}
		GetDriveInfo(driveParam.VID, list, interfaceid, deviceid);
		UpdateDeviceState();
		if (UnknownDeviceIndex.Count > 0)
		{
			GetDeviceInfoTimer.Start();
		}
	}

	private bool GetDeviceAddress(int index)
	{
		bool result = false;
		int num = 0;
		string deviceString = AllDeviceInfoList[index].deviceString;
		DeviceAllInfo item = AllDeviceInfoList[index];
		item.online = false;
		byte[] deviceOnLineWithAddress = UsbFinder.GetDeviceOnLineWithAddress(deviceString);
		item.online = deviceOnLineWithAddress[6] == 1;
		item.address = new List<byte>();
		for (int i = 7; i < 10; i++)
		{
			item.address.Add(deviceOnLineWithAddress[i]);
			if (deviceOnLineWithAddress[i] == 0)
			{
				num++;
			}
		}
		if (num < 3)
		{
			result = true;
		}
		AllDeviceInfoList.RemoveAt(index);
		AllDeviceInfoList.Insert(index, item);
		return result;
	}

	private int GetDeviceInfo(ref DeviceAllInfo deviceAllInfo)
	{
		int num = 0;
		int i = 0;
		string deviceString = deviceAllInfo.deviceString;
		systemLog.WriteLog(deviceString);
		DeviceInfo deviceInfo = default(DeviceInfo);
		for (; i < 10; i++)
		{
			if (deviceInfo.CID != 0)
			{
				break;
			}
			if (deviceInfo.MID != 0)
			{
				break;
			}
			UsbFinder.GetDeviceInfo(deviceString, out deviceInfo);
		}
		if (i < 10)
		{
			systemLog.WriteLog("Cid:" + deviceInfo.CID + "Mid:" + deviceInfo.MID);
			DeviceType deviceType = (DeviceType)deviceInfo.DeviceType;
			if (deviceType.ToString().Contains("Wireless"))
			{
				deviceAllInfo.isUSB = false;
			}
			else
			{
				deviceAllInfo.isUSB = true;
			}
			deviceAllInfo.deviceInfo = deviceInfo;
			if (UsbFinder.GetDeviceOnLine(deviceAllInfo.deviceString))
			{
				deviceAllInfo.online = true;
				int version = UsbFinder.GetVersion(deviceString);
				deviceAllInfo.MouseVersion = "v" + ValueConvert.IntToVersion(version);
				if (version != 0)
				{
					RegeditManager.SetMouseVersion(deviceAllInfo.deviceInfo.CID, deviceAllInfo.deviceInfo.MID, deviceAllInfo.MouseVersion);
				}
			}
			for (int j = 0; j < driveParam.DeviceTotal; j++)
			{
				if (deviceInfo.CID == driveParam.CID && deviceInfo.MID == driveParam.DeviceParams[j].MID)
				{
					deviceAllInfo.deviceIndex = j;
					num = 1;
					break;
				}
			}
			if (num == 0)
			{
				if (deviceInfo.CID == driveParam.CID && num == 0)
				{
					deviceAllInfo.deviceIndex = driveParam.DeviceTotal;
					num = 2;
				}
				else
				{
					num = 3;
				}
			}
		}
		return num;
	}

	private void GetDriveInfo(List<string> VID, List<string> PID, int interfaceId, int deviceId)
	{
		for (int i = 0; i < VID.Count; i++)
		{
			string text = VID[i];
			for (int j = 0; j < PID.Count; j++)
			{
				systemLog.WriteLog("Scaning Vid：" + VID?.ToString() + "，Pid：" + PID[j] + "，interfaceId：" + interfaceId + "，deviceId：" + deviceId);
				if (!(PID[j] != "0000"))
				{
					continue;
				}
				string[] array = UsbFinder.FindHidDevicesByDeviceId(text, PID[j], interfaceId, deviceId);
				for (int k = 0; k < array.Length; k++)
				{
					DeviceAllInfo deviceAllInfo = new DeviceAllInfo
					{
						deviceString = array[k],
						VID = text,
						PID = PID[j]
					};
					HIDD_ATTRIBUTES hidd_attributes = default(HIDD_ATTRIBUTES);
					UsbFinder.GetUsbDeviceAttribute(array[k], out hidd_attributes);
					int versionNumber = hidd_attributes.VersionNumber;
					deviceAllInfo.DongleVersion = "v" + ValueConvert.IntToVersion(versionNumber);
					deviceAllInfo.online = false;
					deviceAllInfo.deviceIndex = driveParam.DeviceTotal;
					if (GetDeviceInfo(ref deviceAllInfo) == 0)
					{
						AllDeviceInfoList.Add(deviceAllInfo);
						UnknownDeviceIndex.Add(AllDeviceInfoList.Count - 1);
					}
					else if (GetDeviceInfo(ref deviceAllInfo) != 3)
					{
						AllDeviceInfoList.Add(deviceAllInfo);
					}
				}
			}
		}
	}

	private void UpdateDeviceState()
	{
		bool flag = false;
		List<List<int>> list = new List<List<int>>();
		int[] array = new int[driveParam.DeviceTotal + 1];
		int[] array2 = new int[driveParam.DeviceTotal + 1];
		int[] array3 = new int[driveParam.DeviceTotal + 1];
		int[] array4 = new int[driveParam.DeviceTotal + 1];
		for (int i = 0; i < AllDeviceInfoList.Count; i++)
		{
			if (AllDeviceInfoList[i].isUSB)
			{
				array[AllDeviceInfoList[i].deviceIndex]++;
			}
			else if (AllDeviceInfoList[i].deviceInfo.DeviceType == 0)
			{
				array2[AllDeviceInfoList[i].deviceIndex]++;
			}
			else if (AllDeviceInfoList[i].deviceInfo.DeviceType == 4)
			{
				array3[AllDeviceInfoList[i].deviceIndex]++;
			}
			else if (AllDeviceInfoList[i].deviceInfo.DeviceType == 1)
			{
				array4[AllDeviceInfoList[i].deviceIndex]++;
			}
			GetDeviceAddress(i);
		}
		for (int j = 0; j < driveParam.DeviceTotal; j++)
		{
			List<int> list2 = new List<int>();
			list2.Add(array[j]);
			list2.Add(array2[j]);
			list2.Add(array3[j]);
			list2.Add(array4[j]);
			list.Add(list2);
		}
		for (int k = 0; k < driveParam.DeviceTotal; k++)
		{
			for (int l = 0; l < list.Count; l++)
			{
				if (list[k][l] > 1)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			if (list[k][0] != 1)
			{
				continue;
			}
			List<int> list3 = new List<int>();
			for (int m = 0; m < AllDeviceInfoList.Count; m++)
			{
				if (AllDeviceInfoList[m].deviceIndex == k && !AllDeviceInfoList[m].isUSB)
				{
					list3.Add(m);
				}
			}
			for (int num = list3.Count - 1; num >= 0; num--)
			{
				AllDeviceInfoList.RemoveAt(list3[num]);
			}
		}
		((Control)customPanel_Device).Controls.Clear();
		if (flag)
		{
			ConnectLabelInit(LanguageFile.Dialogs[29]);
			((Control)customPanel_Device).Controls.Add((Control)(object)label_ConnctDevice);
			return;
		}
		if (AllDeviceInfoList.Count == 0)
		{
			if (((Control)customButton_Repair).Visible)
			{
				ConnectLabelInit(LanguageFile.Dialogs[60]);
			}
			else
			{
				ConnectLabelInit(LanguageFile.Dialogs[59]);
			}
			((Control)customPanel_Device).Controls.Add((Control)(object)label_ConnctDevice);
			return;
		}
		CustomDeviceButtonList.Clear();
		InitLocation.Clear();
		AddDevice(AllDeviceInfoList.Count);
		for (int n = 0; n < AllDeviceInfoList.Count; n++)
		{
			CustomDeviceButtonList[n].Online = AllDeviceInfoList[n].online;
			CustomDeviceButtonList[n].IsWired = AllDeviceInfoList[n].isUSB;
		}
	}

	private void SetComboBox(string[] language)
	{
		customComboBox1.Item.Clear();
		for (int i = 0; i < language.Length; i++)
		{
			customComboBox1.Item.Add((object)language[i]);
		}
		customComboBox1.SelectIndex = languageFile.LanguageIndex;
	}

	private void customComboBox1_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		RegeditManager.SaveLanguageIndex(customComboBox1.SelectIndex);
		languageFile.LanguageIndex = customComboBox1.SelectIndex;
		languageFile.SetLanguage(customComboBox1.SelectIndex, null);
		if (((Control)customButton_Repair).Visible)
		{
			ConnectLabelInit(LanguageFile.Dialogs[60]);
		}
		else
		{
			ConnectLabelInit(LanguageFile.Dialogs[59]);
		}
		LoadToolStripMenuItem();
		((Control)label_EnterUse).Text = LanguageFile.Dialogs[95];
	}

	private void customTrack1_PointChanged(object sender, CustomEventArgs e)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)customPanel_Device).Controls).Count; i++)
		{
			((Control)customPanel_Device).Controls[i].Location = new Point(InitLocation[i].X - customTrack1.SliderPosition, ((Control)customPanel_Device).Controls[i].Location.Y);
		}
	}

	private void customButton_Previous_Click(object sender, EventArgs e)
	{
		if (CurrentIndex > 0)
		{
			CurrentIndex--;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)customPanel_Device).Controls).Count; i++)
			{
				((Control)customPanel_Device).Controls[i].Location = new Point(InitLocation[i].X - InitLocation[CurrentIndex].X, ((Control)customPanel_Device).Controls[i].Location.Y);
			}
		}
	}

	private void customButton_Next_Click(object sender, EventArgs e)
	{
		if (CurrentIndex < ((ArrangedElementCollection)((Control)customPanel_Device).Controls).Count - 3)
		{
			CurrentIndex++;
			for (int i = 0; i < ((ArrangedElementCollection)((Control)customPanel_Device).Controls).Count; i++)
			{
				((Control)customPanel_Device).Controls[i].Location = new Point(InitLocation[i].X - InitLocation[CurrentIndex].X, ((Control)customPanel_Device).Controls[i].Location.Y);
			}
		}
	}

	private void FormHomePage_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.KeyCode == 13)
		{
			if (RegeditManager.GetUpdateFailInfo(out updateFialInfo))
			{
				customButton_Repair_Click(null, null);
			}
		}
		else
		{
			if ((int)e.KeyCode != 32)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			string mcu = driveParam.DeviceParams[AllDeviceInfoList[0].deviceIndex].DM;
			int num = 1;
			if (AllDeviceInfoList.Count == 1 && AllDeviceInfoList[0].PID != "0000" && !AllDeviceInfoList[0].isUSB)
			{
				flag = true;
				if (AllDeviceInfoList[0].deviceInfo.CID != 0 && AllDeviceInfoList[0].deviceIndex < driveParam.DeviceTotal)
				{
					string mouseVersion = RegeditManager.GetMouseVersion(AllDeviceInfoList[0].deviceInfo.CID, AllDeviceInfoList[0].deviceInfo.MID);
					bool flag3 = false;
					if (mouseVersion != null)
					{
						flag3 = DeviceUpdateFile.HasNewVersion(210, mouseVersion, driveParam.DeviceParams[AllDeviceInfoList[0].deviceIndex].MM, AllDeviceInfoList[0].deviceInfo.CID, AllDeviceInfoList[0].deviceInfo.MID);
						DeviceType deviceType = (DeviceType)AllDeviceInfoList[0].deviceInfo.DeviceType;
						if (deviceType.ToString().Contains("Wireless"))
						{
							if (deviceType.ToString().Contains("2K"))
							{
								num = 2;
								mcu = driveParam.DeviceParams[AllDeviceInfoList[0].deviceIndex].D2M;
							}
							if (deviceType.ToString().Contains("4K"))
							{
								num = 4;
								mcu = driveParam.DeviceParams[AllDeviceInfoList[0].deviceIndex].D4M;
							}
						}
					}
					bool flag4 = DeviceUpdateFile.HasNewVersion(211, "v" + AllDeviceInfoList[0].DongleVersion, mcu, AllDeviceInfoList[0].deviceInfo.CID, AllDeviceInfoList[0].deviceInfo.MID);
					if (flag3 != flag4)
					{
						if (!flag3 & flag4)
						{
							FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[92], DialogButtons.OKCanel);
							((Form)formDialog).ShowDialog();
							flag2 = formDialog.resault;
							flag = formDialog.resault;
						}
						else if (!flag4 & flag3)
						{
							((Form)new FormDialog(LanguageFile.Dialogs[91], DialogButtons.OK)).ShowDialog();
							flag = false;
						}
					}
				}
			}
			if (flag2)
			{
				byte[] buffers = new byte[1024];
				DeviceUpdateFile.HasDeviceUpdateFile(211, "v" + AllDeviceInfoList[0].DongleVersion, mcu, AllDeviceInfoList[0].deviceInfo.CID, AllDeviceInfoList[0].deviceInfo.MID, out buffers);
				((Form)new FormUpdate(buffers, (byte)num)).ShowDialog();
			}
			else if (flag)
			{
				systemLog.WriteLog("EnterPair");
				FormPair formPair = new FormPair(AllDeviceInfoList[0], driveParam.CID);
				((Form)formPair).ShowDialog();
				if (formPair != null)
				{
					ButtonStateEnum pairState = formPair.PairState;
					systemLog.WriteLog("Pair " + pairState);
				}
				systemLog.WriteLog("ExitPair");
				OnUsbChangedEvent(isPlug: true);
			}
		}
	}

	private void customButton_Repair_Click(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Form)new FormUpdate(driveParam)).ShowDialog();
		((Control)customButton_Repair).Visible = RegeditManager.GetUpdateFailInfo(out updateFialInfo);
		if (((Control)customButton_Repair).Visible)
		{
			ConnectLabelInit(LanguageFile.Dialogs[60]);
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Expected O, but got Unknown
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_0821: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b0: Expected O, but got Unknown
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormHomePage));
		contextMenuStrip_Pallet = new ContextMenuStrip(components);
		ToolStripMenuItem_OpenMenu = new ToolStripMenuItem();
		ToolStripMenuItem_Exit = new ToolStripMenuItem();
		notifyIcon_Main = new NotifyIcon(components);
		toolTip = new ToolTip(components);
		customButton_Repair = new CustomButton();
		customButton_Next = new CustomButton();
		customButton_Previous = new CustomButton();
		customComboBox1 = new CustomComboBox();
		customPanel_Device = new CustomPanel();
		label_ConnctDevice = new Label();
		customTrack1 = new CustomTrack();
		customButton_Mini = new CustomButton();
		customButton_Close = new CustomButton();
		label_EnterUse = new Label();
		((Control)contextMenuStrip_Pallet).SuspendLayout();
		((Control)customPanel_Device).SuspendLayout();
		((Control)this).SuspendLayout();
		((ToolStrip)contextMenuStrip_Pallet).ImageScalingSize = new Size(18, 18);
		((ToolStrip)contextMenuStrip_Pallet).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)ToolStripMenuItem_OpenMenu,
			(ToolStripItem)ToolStripMenuItem_Exit
		});
		((Control)contextMenuStrip_Pallet).Name = "contextMenuStrip_Pallet";
		((Control)contextMenuStrip_Pallet).Size = new Size(137, 48);
		((ToolStripItem)ToolStripMenuItem_OpenMenu).Name = "ToolStripMenuItem_OpenMenu";
		((ToolStripItem)ToolStripMenuItem_OpenMenu).Size = new Size(136, 22);
		((ToolStripItem)ToolStripMenuItem_OpenMenu).Text = "打开主界面";
		((ToolStripItem)ToolStripMenuItem_OpenMenu).Click += ToolStripMenuItem_OpenMenu_Click;
		((ToolStripItem)ToolStripMenuItem_Exit).Name = "ToolStripMenuItem_Exit";
		((ToolStripItem)ToolStripMenuItem_Exit).Size = new Size(136, 22);
		((ToolStripItem)ToolStripMenuItem_Exit).Text = "退出程序";
		((ToolStripItem)ToolStripMenuItem_Exit).Click += customButton_Close_Click;
		notifyIcon_Main.ContextMenuStrip = contextMenuStrip_Pallet;
		notifyIcon_Main.Icon = (Icon)componentResourceManager.GetObject("notifyIcon_Main.Icon");
		notifyIcon_Main.Text = "Mouse Deive Beta";
		notifyIcon_Main.Visible = true;
		notifyIcon_Main.MouseClick += new MouseEventHandler(notifyIcon_Main_MouseClick);
		toolTip.AutoPopDelay = 20000;
		toolTip.InitialDelay = 500;
		toolTip.IsBalloon = true;
		toolTip.ReshowDelay = 100;
		((Control)customButton_Repair).Location = new Point(783, 8);
		customButton_Repair.MouseDownImage = (Image)(object)Resources.设备修复;
		customButton_Repair.MouseEnterImage = (Image)(object)Resources.设备修复;
		((Control)customButton_Repair).Name = "customButton_Repair";
		customButton_Repair.NormalImage = (Image)(object)Resources.设备修复;
		((Control)customButton_Repair).Size = new Size(36, 33);
		((Control)customButton_Repair).TabIndex = 21;
		((Control)customButton_Repair).Click += customButton_Repair_Click;
		((Control)customButton_Next).Location = new Point(972, 380);
		customButton_Next.MouseDownImage = (Image)(object)Resources.下一页;
		customButton_Next.MouseEnterImage = (Image)(object)Resources.下一页;
		((Control)customButton_Next).Name = "customButton_Next";
		customButton_Next.NormalImage = (Image)(object)Resources.下一页;
		((Control)customButton_Next).Size = new Size(36, 36);
		((Control)customButton_Next).TabIndex = 20;
		((Control)customButton_Next).Click += customButton_Next_Click;
		((Control)customButton_Previous).Location = new Point(15, 380);
		customButton_Previous.MouseDownImage = (Image)(object)Resources.上一页;
		customButton_Previous.MouseEnterImage = (Image)(object)Resources.上一页;
		((Control)customButton_Previous).Name = "customButton_Previous";
		customButton_Previous.NormalImage = (Image)(object)Resources.上一页;
		((Control)customButton_Previous).Size = new Size(36, 36);
		((Control)customButton_Previous).TabIndex = 19;
		((Control)customButton_Previous).Click += customButton_Previous_Click;
		customComboBox1.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox1.ArrowImageNoraml = null;
		((Control)customComboBox1).BackColor = Color.FromArgb(35, 35, 35);
		((Control)customComboBox1).Font = new Font("微软雅黑", 10.18868f);
		((Control)customComboBox1).Location = new Point(825, 8);
		((Control)customComboBox1).Name = "customComboBox1";
		customComboBox1.SelectIndex = -1;
		customComboBox1.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox1).Size = new Size(110, 30);
		((Control)customComboBox1).TabIndex = 18;
		customComboBox1.UnselectItemColor = Color.FromArgb(35, 35, 35);
		customComboBox1.OnSelectedIndexChanged += customComboBox1_OnSelectedIndexChanged;
		((Control)customPanel_Device).BackColor = Color.Transparent;
		((Control)customPanel_Device).Controls.Add((Control)(object)label_ConnctDevice);
		((Control)customPanel_Device).Location = new Point(70, 194);
		((Control)customPanel_Device).Name = "customPanel_Device";
		((Control)customPanel_Device).Size = new Size(883, 402);
		((Control)customPanel_Device).TabIndex = 9;
		((Control)label_ConnctDevice).Font = new Font("微软雅黑", 24f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_ConnctDevice).Location = new Point(0, 186);
		((Control)label_ConnctDevice).Name = "label_ConnctDevice";
		((Control)label_ConnctDevice).Size = new Size(883, 82);
		((Control)label_ConnctDevice).TabIndex = 1;
		((Control)label_ConnctDevice).Text = "label1";
		label_ConnctDevice.TextAlign = (ContentAlignment)32;
		customTrack1.BarColor = Color.FromArgb(97, 97, 97);
		customTrack1.BarSize = 2;
		((Control)customTrack1).Location = new Point(88, 685);
		((Control)customTrack1).Name = "customTrack1";
		((Control)customTrack1).Size = new Size(883, 10);
		customTrack1.SliderColor = Color.FromArgb(255, 106, 0);
		customTrack1.SliderLength = 150;
		customTrack1.SliderPosition = 0;
		((Control)customTrack1).TabIndex = 7;
		((Control)customTrack1).Text = "customTrack1";
		customTrack1.PointChanged += customTrack1_PointChanged;
		((Control)customButton_Mini).Location = new Point(942, 12);
		customButton_Mini.MouseDownImage = (Image)(object)Resources.最小化按键按下;
		customButton_Mini.MouseEnterImage = (Image)(object)Resources.最小化按键鼠标进入;
		((Control)customButton_Mini).Name = "customButton_Mini";
		customButton_Mini.NormalImage = (Image)(object)Resources.最小化按键;
		((Control)customButton_Mini).Size = new Size(20, 20);
		((Control)customButton_Mini).TabIndex = 6;
		((Control)customButton_Mini).Click += customButton_Mini_Click;
		((Control)customButton_Close).Location = new Point(977, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.关闭按键;
		((Control)customButton_Close).Size = new Size(20, 20);
		((Control)customButton_Close).TabIndex = 5;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)label_EnterUse).BackColor = Color.Transparent;
		((Control)label_EnterUse).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_EnterUse).Location = new Point(66, 609);
		((Control)label_EnterUse).Name = "label_EnterUse";
		((Control)label_EnterUse).Size = new Size(887, 74);
		((Control)label_EnterUse).TabIndex = 22;
		((Control)label_EnterUse).Text = "点击图案进入开始使用";
		label_EnterUse.TextAlign = (ContentAlignment)32;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.主页背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(1020, 700);
		((Control)this).Controls.Add((Control)(object)label_EnterUse);
		((Control)this).Controls.Add((Control)(object)customButton_Repair);
		((Control)this).Controls.Add((Control)(object)customButton_Next);
		((Control)this).Controls.Add((Control)(object)customButton_Previous);
		((Control)this).Controls.Add((Control)(object)customComboBox1);
		((Control)this).Controls.Add((Control)(object)customPanel_Device);
		((Control)this).Controls.Add((Control)(object)customTrack1);
		((Control)this).Controls.Add((Control)(object)customButton_Mini);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormHomePage";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "LAMZU";
		((Control)this).KeyDown += new KeyEventHandler(FormHomePage_KeyDown);
		((Control)contextMenuStrip_Pallet).ResumeLayout(false);
		((Control)customPanel_Device).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
