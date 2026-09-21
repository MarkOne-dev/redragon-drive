using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.ComboControlLibrary;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;
using WindControls;

namespace Mouse_Drive_Beta;

public class FormMain : Form
{
	private SkinForm skinForm;

	private FormMover formMover;

	private FormResize formResize;

	public static SystemLog systemLog;

	public static LanguageFile languageFile = new LanguageFile();

	public static DriveConfig.DriveParam driveParam = new DriveConfig.DriveParam();

	private DriveConfig.DeviceParam deviceParam;

	private FormSetting formSetting;

	private FormDialog formDialogMaxMacroKey;

	private FormDialog formDialogMultiOnline;

	private FormDialog ConnectingDialog;

	private FormDialog ImportConfigDialog;

	private static FormDialog OfflineConfigDialog;

	private static ConnectState gConnectState = ConnectState.Disconnect;

	public static FlashDataMap gFlashDataMap = new FlashDataMap(0);

	private DeviceStatusChanged gDeviceStatusChanged;

	public static List<DeviceAllInfo> AllDeviceInfoList = new List<DeviceAllInfo>();

	public static DeviceAllInfo ConnectedDeviceInfo = default(DeviceAllInfo);

	public static DeviceAllInfo SelectedDeviceInfo = default(DeviceAllInfo);

	public static Timer DeviceStateTimer;

	private Timer ExitTimer;

	private Timer ImportConfigTimer;

	public static Timer IntervalTimer;

	private Timer UpdateUIErrTimer;

	private Timer UsbPlugTimer;

	private int ConnectTimeoutCount;

	public static int OnlineCheckCount = 0;

	private int ConfigPressTimeout;

	private bool ConfigPressFlag;

	private bool USBProcessInitFlag;

	private bool MultiOnlineFlag;

	private bool IsSetting;

	private bool deviceRestore;

	private bool batteryHandleInit;

	private string[] gComboBoxConfigItems;

	private ComboBoxStruct gComboBoxInsertEvent;

	private bool UpdateingConfigFlag;

	private bool UIInitFlag;

	public static bool ThisHandleCreate = false;

	private int ThisHandleCreateCnt;

	private Point XIn1Point;

	private int[] ConfigParam;

	private _2In1 _2In1_Main;

	public static bool SetLanguage = false;

	private bool NoChangeSaveVisiable;

	public bool LoadFlag;

	private bool dpiUpdateRes;

	private bool advSetting;

	private bool longRangeMode;

	private bool dongle4KLED;

	private byte[] dongleRGB;

	private bool CalPass;

	private int usbPlugCount;

	private bool usbPlugFlag;

	private bool windowsMini;

	private KeyboardHook keyboardHook;

	public static List<MacroKeyAndCycle> MacroKeyAndCycleList = new List<MacroKeyAndCycle>(0);

	private KeyboardCode keyboardCode;

	private MacroKeyDriver macroKeyDriver;

	private Stopwatch stopWatch;

	private FormHomePage formHomePage;

	public bool ToHomePage;

	private Image btnSaveNormal;

	private List<CustomButton> ButtonFunctions;

	private int panel_CycleProcess_Tag;

	private Timer HideTimer;

	private bool BackToHomePage;

	private bool UpdateLEDFlag;

	private bool BrightnessMouseDown;

	private bool SpeedMouseDown;

	private ImageList mediaKeyImageList;

	private int currentTimes;

	private IContainer components;

	private CustomTabControl customTabControl_Main;

	private TabPage tabPage_Button;

	private TabPage tabPage_Sensor;

	private TabPage tabPage_Macro;

	private TabPage tabPage_Light;

	private CustomTabSelector customTabSelector_Main;

	private CustomButton customButton_Close;

	private PictureBox pictureBox_Config;

	private Panel panel_Device;

	private CustomButton customButton_Mini;

	private CustomButton customButton_Setting;

	private CustomComboBox customComboBox_Config;

	private Label label_LightSpeed;

	private Label label_LightBrightness;

	private Label label_LightMode;

	private Label label_PowerSaveTime;

	private CustomCheckBox customCheckBox_MovingCloseLight;

	private Label label_MovingCloseLight;

	private CustomComboBox customComboBox_LEDMode;

	private Label label_Title;

	private CustomListView customListView_Macro;

	private Label label_KeyList;

	private Label label_MacroList;

	private ListView listView_Keys;

	private Label label_AutoDelay;

	private Label label_InsertEvent;

	private Label label_LightBrightnessValue;

	private CustomTrackBar customTrackBar_LightBrightness;

	private CustomTrackBar customTrackBar_LightSpeed;

	private Label label_LightSpeedValue;

	private CustomComboBox customComboBox_PowerSaveTime;

	private NotifyIcon notifyIcon_Main;

	private ContextMenuStrip contextMenuStrip_Pallet;

	private ToolStripMenuItem ToolStripMenuItem_OpenMenu;

	private ToolStripMenuItem ToolStripMenuItem_Exit;

	public CustomBattery customBattery1;

	private Label label_CycleTimes;

	private Label label_UntilKeyPressed;

	private Label label_UntilKeyReleased;

	private CustomRadioButton customRadioButton_UntilKeyReleased;

	private CustomRadioButton customRadioButton_CycleTimes;

	private CustomRadioButton customRadioButton_UntilKeyPressed;

	private CustomButton customButton_Export;

	private CustomButton customButton_Import;

	private CustomButton customButton_DeleteButton;

	private CustomButton customButton_ModifyButton;

	private CustomButton customButton_DeleteMacro;

	private CustomButton customButton_NewMacro;

	private CustomButton customButton_Save;

	private CustomRecordMacro customRecordMacro1;

	private CustomButton customButton_SystemMouse;

	private CustomComboBox customComboBox_InsertEvent;

	private CustomButton customButton2;

	private Panel panel_LEDColor;

	private AdjustControl adjustControl_B;

	private AdjustControl adjustControl_G;

	private AdjustControl adjustControl_R;

	private Label label_LEDB;

	private Label label_LEDG;

	private Label label_LEDR;

	private PictureBox pictureBox_Color14;

	private PictureBox pictureBox_Color7;

	private PictureBox pictureBox_Color13;

	private PictureBox pictureBox_Color12;

	private PictureBox pictureBox_Color11;

	private PictureBox pictureBox_Color6;

	private PictureBox pictureBox_Color5;

	private PictureBox pictureBox_Color4;

	private PictureBox pictureBox_Color10;

	private PictureBox pictureBox_Color9;

	private PictureBox pictureBox_Color8;

	private PictureBox pictureBox_Color3;

	private PictureBox pictureBox_Color2;

	private PictureBox pictureBox_Color1;

	private PictureBox pictureBox_LEDPreview;

	private PictureBox pictureBox_Palette;

	private Label label_LEDPreset;

	private Label label_LEDPreview;

	private Label label_LEDColor;

	private CustomKeyFunction customKeyFunction1;

	private Panel panel_CycleProcess;

	private TextBox textBox_CycleTimes;

	private TextBox textBox_AutoDelayTime;

	private ContextMenuStrip contextMenuStrip_Keys;

	private ToolStripMenuItem ToolStripMenuItem_DeleteKeys;

	private ContextMenuStrip contextMenuStrip_SelectMacro;

	private ToolStripMenuItem ToolStripMenuItem_ExportMacro;

	private ToolStripMenuItem ToolStripMenuItem_Rename;

	private ToolStripMenuItem ToolStripMenuItem_DeleteMacro;

	private ContextMenuStrip contextMenuStrip_UnselectMacro;

	private ToolStripMenuItem ToolStripMenuItem_NewMacro;

	private ToolStripMenuItem ToolStripMenuItem_ImportMacro;

	private CustomButton customButton6;

	private CustomButton customButton5;

	private CustomButton customButton4;

	private CustomButton customButton3;

	private CustomButton customButton1;

	private DPIControl dpiControl1;

	private CustomButton customButton16;

	private CustomButton customButton15;

	private CustomButton customButton14;

	private CustomButton customButton13;

	private CustomButton customButton12;

	private CustomButton customButton11;

	private CustomButton customButton10;

	private CustomButton customButton9;

	private CustomButton customButton8;

	private CustomButton customButton7;

	private Label label_DefaultDelay;

	private Panel panel_Delay;

	private CustomRadioButton customRadioButton_AutoDelay;

	private CustomRadioButton customRadioButton_DefaultDelay;

	private _3In1 _3In1_Main;

	public ToolStripMenuItem ToolStripMenuItem_Modify;

	private Label label_BatteryValue;

	private CustomButton customButton17;

	private TextBox textBox_fileName;

	private TextBox textBox_url;

	private CustomButton customButton_Reset;

	private CustomButton customButton_HomePage;

	private Label label_UntilThisKeyPressed;

	private CustomRadioButton customRadioButton_UntilThisKeyPressed;

	private PictureBox pictureBox1;

	private TabPage tabPage_Setting;

	private CustomSetting customSetting1;

	private ToolTip toolTip;

	public FormMain(DeviceAllInfo deviceAllInfo, DriveConfig.DriveParam config)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		skinForm = new SkinForm(movable: true);
		formMover = new FormMover();
		formResize = new FormResize();
		deviceParam = new DriveConfig.DeviceParam();
		UpdateingConfigFlag = true;
		ConfigParam = new int[2];
		_2In1_Main = new _2In1();
		NoChangeSaveVisiable = true;
		dpiUpdateRes = true;
		dongleRGB = new byte[10];
		keyboardHook = new KeyboardHook();
		keyboardCode = new KeyboardCode();
		macroKeyDriver = new MacroKeyDriver();
		stopWatch = new Stopwatch();
		ButtonFunctions = new List<CustomButton>();
		mediaKeyImageList = new ImageList();
		((Form)this)._002Ector();
		SelectedDeviceInfo = default(DeviceAllInfo);
		SelectedDeviceInfo = deviceAllInfo;
		FormInit(config);
		deviceParam = driveParam.DeviceParams[SelectedDeviceInfo.deviceIndex];
		UIInit();
		UIInit(SelectedDeviceInfo.deviceIndex);
	}

	public FormMain(DriveConfig.DriveParam config)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		skinForm = new SkinForm(movable: true);
		formMover = new FormMover();
		formResize = new FormResize();
		deviceParam = new DriveConfig.DeviceParam();
		UpdateingConfigFlag = true;
		ConfigParam = new int[2];
		_2In1_Main = new _2In1();
		NoChangeSaveVisiable = true;
		dpiUpdateRes = true;
		dongleRGB = new byte[10];
		keyboardHook = new KeyboardHook();
		keyboardCode = new KeyboardCode();
		macroKeyDriver = new MacroKeyDriver();
		stopWatch = new Stopwatch();
		ButtonFunctions = new List<CustomButton>();
		mediaKeyImageList = new ImageList();
		((Form)this)._002Ector();
		FormInit(config);
		SelectedDeviceInfo = default(DeviceAllInfo);
		SelectedDeviceInfo.deviceInfo.CID = (byte)driveParam.CID;
		SelectedDeviceInfo.deviceInfo.MID = (byte)driveParam.DeviceParams[0].MID;
		SelectedDeviceInfo.deviceIndex = 0;
		deviceParam = driveParam.DeviceParams[0];
		UIInit();
		UIInit(SelectedDeviceInfo.deviceIndex);
		((Control)customButton_HomePage).Visible = false;
	}

	private void FormInit(DriveConfig.DriveParam config)
	{
		InitializeComponent();
		ThisHandleCreate = false;
		SetStyles();
		formMover.AddForm((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		TabControl_Init();
		driveParam = config;
		systemLog = new SystemLog("Device" + (SelectedDeviceInfo.deviceIndex + 1) + "_log");
		if (languageFile.GetLanguageFileCount() == 0)
		{
			systemLog.WriteLog("no language file  detected,please check first!!");
			((Form)this).Close();
		}
		else
		{
			systemLog.WriteLog("find language file!!");
		}
		systemLog.WriteLog("GetDpi:" + FormResize.GetDpi());
		languageFile.SetLanguage(languageFile.LanguageIndex, (Control)(object)this);
		keyboardHook.KeyDown += KeyboardHook_KeyDown;
		keyboardHook.KeyUp += KeyboardHook_KeyUp;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void UIInit()
	{
		SetSaveButton(enable: false);
		XIn1Point = ((Control)_3In1_Main).Location;
		ImageListInit();
		ListViewKeysImageInit();
		GetComboBoxConfigItems();
		ReadLocalMacroInit();
		Panel_CycleProcess_Init();
		LoadToolStripMenuItem();
		TimerInit();
		gConnectState = ConnectState.Disconnect;
	}

	private void SetButton(Control father)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			if (val is CustomButton)
			{
				((Control)(CustomButton)(object)val).MouseEnter += Button_MouseEnter;
			}
			else if (val.HasChildren)
			{
				SetButton(val);
			}
		}
	}

	private void Button_MouseEnter(object sender, EventArgs e)
	{
		CustomButton customButton = (CustomButton)sender;
		if (((Control)customButton).Name == "customButton_HomePage")
		{
			toolTip.SetToolTip((Control)(object)customButton, LanguageFile.Dialogs[96]);
		}
		else if (Convert.ToInt32(((Control)this).CreateGraphics().MeasureString(((Control)customButton).Text, ((Control)customButton).Font).Width) >= ((Control)customButton).Width)
		{
			toolTip.SetToolTip((Control)(object)customButton, ((Control)customButton).Text);
		}
	}

	private void DPIInit()
	{
		dpiControl1.MaxGrade = deviceParam.DPIMaxGrade;
		dpiControl1.Step = deviceParam.SensorDPI.Step[0];
		dpiControl1.MinValue = deviceParam.SensorDPI.Min[0];
		dpiControl1.MaxValue = deviceParam.SensorDPI.Max[deviceParam.SensorDPI.Max.Count - 1];
		dpiControl1.ValueChange -= UpdateFlashDataMap_ValueChange;
		dpiControl1.ValueChange += UpdateFlashDataMap_ValueChange;
		dpiControl1.UpdateUIFlag = false;
	}

	private void ButtonFunctionInit(int value)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		ButtonFunctions.Clear();
		int num = value / 10;
		bool flag = value % 10 == 1;
		for (int i = 1; i < 17; i++)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)panel_Device).Controls)
			{
				Control val = item;
				if (!(val is CustomButton))
				{
					continue;
				}
				CustomButton customButton = (CustomButton)(object)val;
				customButton.NormalImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\2Button\\mouse_key_nr.png");
				customButton.MouseDownImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\2Button\\mouse_key_down.png");
				customButton.MouseEnterImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\2Button\\mouse_key_enter.png");
				if (((Control)customButton).Name == "customButton" + i)
				{
					((Control)customButton).Location = new Point(deviceParam.KeyParams[i - 1].point.X, deviceParam.KeyParams[i - 1].point.Y);
					((Control)customButton).Tag = i - 1;
					((Control)customButton).Click -= Button_Click;
					((Control)customButton).Click += Button_Click;
					if (i < num + 1 || ((i > 14) & flag))
					{
						((Control)customButton).Visible = true;
					}
					else
					{
						((Control)customButton).Visible = false;
					}
					((Control)customButton).ForeColor = driveParam.NumClr;
					ButtonFunctions.Add(customButton);
					break;
				}
			}
		}
		customKeyFunction1.ScrollSet = flag;
		customKeyFunction1.ButtonCount = num;
		customKeyFunction1.ValueChange -= FlashDataMap_ValueChange;
		customKeyFunction1.ValueChange += FlashDataMap_ValueChange;
		customKeyFunction1.UpdateUIFlag = false;
	}

	private void Button_Click(object sender, EventArgs e)
	{
		if (GetDeviceOnline())
		{
			int num = (int)((Control)(CustomButton)sender).Tag;
			switch (num)
			{
			case 3:
				num = 4;
				break;
			case 4:
				num = 3;
				break;
			}
			customKeyFunction1.SelectedIndex = num;
		}
	}

	private void Panel_CycleProcess_Init()
	{
		foreach (CustomRadioButton item in (ArrangedElementCollection)((Control)panel_CycleProcess).Controls)
		{
			((RadioButton)item).CheckedChanged += CustomRadioButton_CheckedChanged;
		}
	}

	private void ImageListInit()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Expected O, but got Unknown
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_0327: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_0362: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_039d: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_03e1: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Expected O, but got Unknown
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Expected O, but got Unknown
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Expected O, but got Unknown
		//IL_061f: Expected O, but got Unknown
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_066a: Expected O, but got Unknown
		//IL_0acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Expected O, but got Unknown
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Expected O, but got Unknown
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_078b: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_07b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c1: Expected O, but got Unknown
		//IL_07d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Expected O, but got Unknown
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0839: Expected O, but got Unknown
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c5: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092d: Expected O, but got Unknown
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Expected O, but got Unknown
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a05: Expected O, but got Unknown
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a81: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab7: Expected O, but got Unknown
		//IL_0965: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Expected O, but got Unknown
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a48: Expected O, but got Unknown
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Expected O, but got Unknown
		try
		{
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			notifyIcon_Main.Text = FormMain.driveParam.DriveName;
			notifyIcon_Main.Icon = new Icon(Application.StartupPath + "\\res\\logo.ico");
			((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\background.png");
			customButton_SystemMouse.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\mouse_nr.png");
			customButton_SystemMouse.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\mouse_down.png");
			customButton_SystemMouse.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\mouse_enter.png");
			customButton_HomePage.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\exit_nr.png");
			customButton_HomePage.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\exit_down.png");
			customButton_HomePage.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\exit_enter.png");
			customButton_Setting.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_nr.png");
			customButton_Setting.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_down.png");
			customButton_Setting.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_enter.png");
			customButton_Mini.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_nr.png");
			customButton_Mini.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_down.png");
			customButton_Mini.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\mini_enter.png");
			customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_nr.png");
			customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_down.png");
			customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\close_enter.png");
			customBattery1.EmptyBatteryImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\empty.png");
			customBattery1.FullBatteryImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\full.png");
			customBattery1.LowBatteryImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\low.png");
			customBattery1.ChargingImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\charing.png");
			customTabSelector_Main.CheckedImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\tab_select.png");
			customTabSelector_Main.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\tab_enter.png");
			customTabSelector_Main.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\tab_nr.png");
			((Control)pictureBox_Config).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\config.png");
			string text = "\\res\\2Button\\dev" + (SelectedDeviceInfo.deviceIndex + 1) + ".png";
			((Control)panel_Device).BackgroundImage = (Image)new Bitmap(baseDirectory + text);
			CustomButton customButton = customButton_Reset;
			CustomButton customButton2 = customButton_Import;
			CustomButton customButton3 = customButton_Export;
			Bitmap val = new Bitmap(baseDirectory + "\\res\\2Button\\restore_nr.png");
			Image val2 = (Image)val;
			customButton3.NormalImage = (Image)val;
			Image normalImage = (customButton2.NormalImage = val2);
			customButton.NormalImage = normalImage;
			CustomButton customButton4 = customButton_Reset;
			CustomButton customButton5 = customButton_Import;
			CustomButton customButton6 = customButton_Export;
			Bitmap val4 = new Bitmap(baseDirectory + "\\res\\2Button\\restore_enter.png");
			val2 = (Image)val4;
			customButton6.MouseEnterImage = (Image)val4;
			normalImage = (customButton5.MouseEnterImage = val2);
			customButton4.MouseEnterImage = normalImage;
			CustomButton customButton7 = customButton_Reset;
			CustomButton customButton8 = customButton_Import;
			CustomButton customButton9 = customButton_Export;
			Bitmap val6 = new Bitmap(baseDirectory + "\\res\\2Button\\restore_down.png");
			val2 = (Image)val6;
			customButton9.MouseDownImage = (Image)val6;
			normalImage = (customButton8.MouseDownImage = val2);
			customButton7.MouseDownImage = normalImage;
			CustomButton customButton10 = customButton_Reset;
			CustomButton customButton11 = customButton_Import;
			CustomButton customButton12 = customButton_Export;
			Font val8 = new Font("微软雅黑", LanguageFile.GetFontSize(languageFile.LanguageIndex));
			Font val9 = val8;
			((Control)customButton12).Font = val8;
			Font font = (((Control)customButton11).Font = val9);
			((Control)customButton10).Font = font;
			DriveConfig.DriveParam driveParam = DriveConfig.GetDriveParam();
			customKeyFunction1.ButtonForeColor = driveParam.NumClr;
			((Control)customKeyFunction1).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_func_bg.png");
			customKeyFunction1.MainKeyBackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_sub_func_bg.png");
			customKeyFunction1.SubKeyBackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_sub_func_bg.png");
			customKeyFunction1.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_list_nr.png");
			customKeyFunction1.SelectImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_list_down.png");
			customKeyFunction1.NumberButtonImage = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_index.png");
			customKeyFunction1.KeyNormalImag = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_func_nr.png");
			customKeyFunction1.KeyNormalImagArrow = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_func_nr(arr).png");
			customKeyFunction1.KeySelectImag = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_func_down.png");
			customKeyFunction1.KeySelectImagArrow = (Image)new Bitmap(baseDirectory + "\\res\\2Button\\key_func_down(arr).png");
			customKeyFunction1.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_select.png");
			customKeyFunction1.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_unselect.png");
			dpiControl1.CurrentDPINormalImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\current_DPI_nr.png");
			dpiControl1.CurrentDPIMouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\current_DPI_enter.png");
			dpiControl1.CurrentDPIMouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\current_DPI_enter.png");
			dpiControl1.DPICheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\btn_DPI_Select.png");
			dpiControl1.DPIUncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\btn_DPI_Unselect.png");
			dpiControl1.TitlePicture = (Image)new Bitmap(baseDirectory + "\\res\\7General\\title.png");
			AdjustControl adjustControl = adjustControl_R;
			AdjustControl adjustControl2 = adjustControl_G;
			AdjustControl adjustControl3 = adjustControl_B;
			DPIControl dPIControl = dpiControl1;
			Bitmap val11 = new Bitmap(baseDirectory + "\\res\\7General\\sub.png");
			Image val12 = (Image)val11;
			dPIControl.SubImage = (Image)val11;
			val2 = (adjustControl3.SubImage = val12);
			normalImage = (adjustControl2.SubImage = val2);
			adjustControl.SubImage = normalImage;
			AdjustControl adjustControl4 = adjustControl_R;
			AdjustControl adjustControl5 = adjustControl_G;
			AdjustControl adjustControl6 = adjustControl_B;
			DPIControl dPIControl2 = dpiControl1;
			Bitmap val15 = new Bitmap(baseDirectory + "\\res\\7General\\add.png");
			val12 = (Image)val15;
			dPIControl2.AddImage = (Image)val15;
			val2 = (adjustControl6.AddImage = val12);
			normalImage = (adjustControl5.AddImage = val2);
			adjustControl4.AddImage = normalImage;
			if (deviceParam.XIn1 <= 2)
			{
				_2In1_Main.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\ReportRate_Select.png");
				_2In1_Main.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\ReportRate_Unselect.png");
			}
			else
			{
				_3In1_Main.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\ReportRate_Select.png");
				_3In1_Main.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\ReportRate_Unselect.png");
				_3In1_Main.FullCheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_select.png");
				_3In1_Main.FullUncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_unselect.png");
				_3In1_Main.SensorCheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\on.png");
				_3In1_Main.SensorUncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\off.png");
			}
			customRecordMacro1.StartImage_MouseEnter = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\start_record_enter.png");
			customRecordMacro1.StartImage_Normal = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\start_record_nr.png");
			customRecordMacro1.StopImage_MouseEnter = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\stop_record_enter.png");
			customRecordMacro1.StopImage_Normal = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\stop_record_nr.png");
			foreach (Control item in (ArrangedElementCollection)((Control)panel_Delay).Controls)
			{
				Control val18 = item;
				if (val18 is CustomRadioButton)
				{
					CustomRadioButton obj = (CustomRadioButton)(object)val18;
					obj.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_select.png");
					obj.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_unselect.png");
				}
			}
			foreach (Control item2 in (ArrangedElementCollection)((Control)panel_CycleProcess).Controls)
			{
				Control val19 = item2;
				if (val19 is CustomRadioButton)
				{
					CustomRadioButton obj2 = (CustomRadioButton)(object)val19;
					obj2.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_select.png");
					obj2.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_unselect.png");
				}
			}
			((Control)pictureBox_Palette).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\5Light\\color_bar.png");
			for (int i = 1; i < 15; i++)
			{
				foreach (Control item3 in (ArrangedElementCollection)((Control)panel_LEDColor).Controls)
				{
					Control val20 = item3;
					if (val20 is PictureBox)
					{
						PictureBox val21 = (PictureBox)val20;
						if (((Control)val21).Name == "pictureBox_Color" + i)
						{
							((Control)val21).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\5Light\\color" + i + ".png");
							break;
						}
					}
				}
			}
			foreach (Control item4 in (ArrangedElementCollection)((Control)tabPage_Macro).Controls)
			{
				Control val22 = item4;
				if (val22 is CustomButton)
				{
					CustomButton customButton13 = (CustomButton)(object)val22;
					customButton13.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\new_macro_nr.png");
					customButton13.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\new_macro_down.png");
					if (((Control)customButton13).Name == "customButton_Save")
					{
						customButton13.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\new_macro_down.png");
					}
					else
					{
						customButton13.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\new_macro_nr.png");
					}
				}
			}
			btnSaveNormal = (Image)new Bitmap(baseDirectory + "\\res\\4Macro\\new_macro_nr.png");
			customCheckBox_MovingCloseLight.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\on.png");
			customCheckBox_MovingCloseLight.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\off.png");
		}
		catch
		{
			((Form)new FormDialog(LanguageFile.Dialogs[1], DialogButtons.OK)).ShowDialog();
		}
	}

	private void XIn1Init()
	{
		if (deviceParam.XIn1 >= 3)
		{
			if (((Control)tabPage_Sensor).Controls.Contains((Control)(object)_2In1_Main))
			{
				((Control)tabPage_Sensor).Controls.Remove((Control)(object)_2In1_Main);
			}
			if (!((Control)tabPage_Sensor).Controls.Contains((Control)(object)_3In1_Main))
			{
				((Control)_3In1_Main).Location = XIn1Point;
				((Control)tabPage_Sensor).Controls.Add((Control)(object)_3In1_Main);
			}
		}
		else
		{
			if (((Control)tabPage_Sensor).Controls.Contains((Control)(object)_3In1_Main))
			{
				((Control)tabPage_Sensor).Controls.Remove((Control)(object)_3In1_Main);
			}
			if (!((Control)tabPage_Sensor).Controls.Contains((Control)(object)_2In1_Main))
			{
				((Control)_2In1_Main).Location = XIn1Point;
				((Control)tabPage_Sensor).Controls.Add((Control)(object)_2In1_Main);
			}
		}
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

	private void ReadLocalMacroInit()
	{
		if (MacroFile.ReadLocalMacroKey(ref MacroKeyAndCycleList))
		{
			languageFile.KeyFunctionSubStructs[6].Clear();
			for (int i = 0; i < MacroKeyAndCycleList.Count; i++)
			{
				((ListView)customListView_Macro).Items.Add(MacroKeyAndCycleList[i].name);
				KeyFunctionStruct item = new KeyFunctionStruct
				{
					name = MacroKeyAndCycleList[i].name
				};
				languageFile.KeyFunctionSubStructs[6].Add(item);
			}
		}
	}

	private void RemoveMoveXY()
	{
		List<int> list = new List<int>();
		for (int num = MacroKeyAndCycleList.Count - 1; num > 0; num--)
		{
			for (int i = 0; i < MacroKeyAndCycleList[num].macroKey.contextCount; i++)
			{
				if (MacroKeyAndCycleList[num].macroKey.context[i].type == 5)
				{
					list.Add(num);
					break;
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			MacroKeyAndCycleList.RemoveAt(list[j]);
		}
		((ListView)customListView_Macro).Items.Clear();
		languageFile.KeyFunctionSubStructs[6].Clear();
		for (int k = 0; k < MacroKeyAndCycleList.Count; k++)
		{
			((ListView)customListView_Macro).Items.Add(MacroKeyAndCycleList[k].name);
			KeyFunctionStruct item = new KeyFunctionStruct
			{
				name = MacroKeyAndCycleList[k].name
			};
			languageFile.KeyFunctionSubStructs[6].Add(item);
		}
	}

	private void TabControl_Init()
	{
		((Control)customTabControl_Main).Location = new Point(((Control)customTabControl_Main).Left, ((Control)customTabControl_Main).Location.Y - ((TabControl)customTabControl_Main).ItemSize.Height);
		((TabControl)customTabControl_Main).ItemSize = new Size(((TabControl)customTabControl_Main).ItemSize.Width, 1);
		customTabControl_Main.TabPageUnselectFontColor = Color.Transparent;
		customTabControl_Main.TabPageSelectFontColor = Color.Transparent;
	}

	public void USBProcess_Init()
	{
		DeviceStateTimerInit(1300);
		UsbFinder.StartUsbChanged(OnUsbChangedEvent, 600);
	}

	private void OnUsbChangedEvent(bool isPlug)
	{
		int interfaceid = driveParam.Interfaceid;
		int deviceid = driveParam.Deviceid;
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
		if (AllDeviceInfoList.Count == 0)
		{
			if (SelectedDeviceInfo.deviceString == null)
			{
				return;
			}
			if (windowsMini)
			{
				if (!usbPlugFlag)
				{
					usbPlugFlag = true;
					usbPlugCount = 0;
					UsbPlugTimer.Enabled = true;
				}
				return;
			}
			usbPlugFlag = false;
			UsbPlugTimer.Enabled = false;
			if (!customSetting1.updating)
			{
				gConnectState = ConnectState.Disconnect;
				ConnectedDeviceInfo = default(DeviceAllInfo);
				((Control)customBattery1).Visible = false;
				((Control)label_BatteryValue).Visible = false;
			}
			if (formSetting != null)
			{
				if (!formSetting.isUpdating)
				{
					customButton_HomePage_Click(null, null);
				}
			}
			else
			{
				customButton_HomePage_Click(null, null);
			}
			return;
		}
		int num = 0;
		for (int k = 0; k < AllDeviceInfoList.Count; k++)
		{
			if (ConnectedDeviceInfo.deviceString != null)
			{
				if (AllDeviceInfoList[k].deviceIndex == SelectedDeviceInfo.deviceIndex && AllDeviceInfoList[k].isUSB && AllDeviceInfoList[k].online && !ConnectedDeviceInfo.isUSB && AllDeviceInfoList[k].address[0] == ConnectedDeviceInfo.address[0] && AllDeviceInfoList[k].address[1] == ConnectedDeviceInfo.address[1] && AllDeviceInfoList[k].address[2] == ConnectedDeviceInfo.address[2] && !windowsMini)
				{
					DeviceConnect(AllDeviceInfoList[k]);
				}
			}
			else if (!SelectedDeviceInfo.isUSB && SelectedDeviceInfo.online)
			{
				if (AllDeviceInfoList[k].deviceIndex == SelectedDeviceInfo.deviceIndex && AllDeviceInfoList[k].isUSB && AllDeviceInfoList[k].online && AllDeviceInfoList[k].address[0] == SelectedDeviceInfo.address[0] && AllDeviceInfoList[k].address[1] == SelectedDeviceInfo.address[1] && AllDeviceInfoList[k].address[2] == SelectedDeviceInfo.address[2])
				{
					DeviceConnect(AllDeviceInfoList[k]);
				}
			}
			else if (ConnectedDeviceInfo.deviceString == null && AllDeviceInfoList[k].deviceString == SelectedDeviceInfo.deviceString && AllDeviceInfoList[k].online)
			{
				DeviceConnect(AllDeviceInfoList[k]);
			}
			if (AllDeviceInfoList[k].deviceString != ConnectedDeviceInfo.deviceString && ConnectedDeviceInfo.deviceString != null)
			{
				num++;
			}
		}
		if (SelectedDeviceInfo.isUSB)
		{
			if (AllDeviceInfoList.Count == 2)
			{
				for (int l = 0; l < AllDeviceInfoList.Count; l++)
				{
					if (AllDeviceInfoList[l].address != null && !AllDeviceInfoList[l].isUSB)
					{
						customSetting1.DongleType = AllDeviceInfoList[l].deviceInfo.DeviceType;
						customSetting1.DongleVersion = AllDeviceInfoList[l].DongleVersion;
						break;
					}
				}
			}
			else
			{
				customSetting1.DongleVersion = "";
				customSetting1.RestDongleButton();
			}
		}
		if (num == AllDeviceInfoList.Count && SelectedDeviceInfo.deviceString != null)
		{
			if (windowsMini)
			{
				if (!usbPlugFlag)
				{
					usbPlugFlag = true;
					usbPlugCount = 0;
					UsbPlugTimer.Enabled = true;
				}
				return;
			}
			usbPlugFlag = false;
			UsbPlugTimer.Enabled = false;
			if (formSetting != null)
			{
				if (!formSetting.isUpdating)
				{
					customButton_HomePage_Click(null, null);
				}
			}
			else
			{
				customButton_HomePage_Click(null, null);
			}
		}
		else
		{
			usbPlugFlag = false;
			UsbPlugTimer.Enabled = false;
			if (usbPlugCount > 10)
			{
				customButton_HomePage_Click(null, null);
			}
			else if (ConnectedDeviceInfo.deviceString != null && windowsMini)
			{
				UsbServer.Start(ConnectedDeviceInfo.deviceString, ConnectedDeviceInfo.deviceString, onUsbDataReceived);
			}
			systemLog.WriteLog("Usb Plug Count:" + usbPlugCount);
		}
	}

	private void USBReadAllFlashData()
	{
		UsbServer.ReadAllFlashData();
		systemLog.WriteLog("USBReadAllFlashData");
	}

	private void onUsbDataReceived(UsbCommand command)
	{
		UsbCommandID id = (UsbCommandID)command.id;
		int num = command.address;
		if (id == UsbCommandID.WriteFlashData && num > 128)
		{
			num -= num % 128;
		}
		string str = id.ToString() + " (0x" + num.ToString("X2") + ") Len(" + ((command.receivedData == null) ? "0" : command.receivedData.Length.ToString()) + "):";
		systemLog.WriteLog(str);
		if (command.receivedData != null)
		{
			str = "(0x" + num.ToString("X2") + "):";
			int num2 = 0;
			int num3 = 0;
			if (command.receivedData.Length >= 16)
			{
				for (int i = 0; i < command.receivedData.Length; i++)
				{
					str = str + command.receivedData[i].ToString("X2") + " ";
					num2++;
					if (num2 == 16)
					{
						systemLog.WriteLog(str);
						num += num2;
						num3 += 16;
						num2 = 0;
						str = "(0x" + num.ToString("X2") + "):";
					}
				}
			}
			if (command.receivedData.Length % 16 != 0)
			{
				str = "(0x" + num.ToString("X2") + "):";
				for (int j = 0; j < command.receivedData.Length % 16; j++)
				{
					str = str + command.receivedData[num3 + j].ToString("X2") + " ";
				}
				systemLog.WriteLog(str);
			}
		}
		switch ((UsbCommandID)command.id)
		{
		case UsbCommandID.EncryptionData:
			DataParser.GetDeviceInfo(command.receivedData);
			break;
		case UsbCommandID.DeviceOnLine:
			DataParser.isDeviceOnLine(command.receivedData);
			break;
		case UsbCommandID.BatteryLevel:
		{
			if (windowsMini)
			{
				break;
			}
			BatteryStatus batteryStatus = DataParser.GetDeviceBatteryStatus(command.receivedData);
			if (driveParam.ShowBatteryValue)
			{
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					customBattery1.BatteryCharging = batteryStatus.isCharging > 0;
					customBattery1.BatteryLevel = batteryStatus.level;
					((Control)label_BatteryValue).Text = batteryStatus.level + "%";
					((Control)label_BatteryValue).Location = new Point(((Control)customBattery1).Left - ((Control)label_BatteryValue).Width, ((Control)label_BatteryValue).Location.Y);
					if (!batteryHandleInit)
					{
						batteryHandleInit = true;
						((Control)customBattery1).Visible = true;
						((Control)label_BatteryValue).Visible = true;
					}
				});
				break;
			}
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				customBattery1.BatteryCharging = batteryStatus.isCharging > 0;
				customBattery1.BatteryLevel = batteryStatus.level;
				if (!batteryHandleInit)
				{
					batteryHandleInit = true;
					((Control)customBattery1).Visible = true;
				}
			});
			break;
		}
		case UsbCommandID.ClearSetting:
			if (!ConnectedDeviceInfo.isUSB && deviceParam.Advanced != null && deviceParam.Advanced[0] >= 0)
			{
				UsbServer.SetLongRangeMode(deviceParam.Advanced[0] > 0);
			}
			if (!ConnectedDeviceInfo.isUSB && ConnectedDeviceInfo.deviceInfo.DeviceType == 1 && deviceParam.Advanced != null && deviceParam.Advanced[1] >= 0)
			{
				DongleRGB dongleRGB = new DongleRGB
				{
					mode = (byte)deviceParam.Advanced[1]
				};
				UsbServer.Set4KDongleRGB(ref dongleRGB);
			}
			USBReadAllFlashData();
			deviceRestore = true;
			break;
		case UsbCommandID.StatusChanged:
			gDeviceStatusChanged = DataParser.GetDeviceStatusChanged(command.receivedData);
			USBDeviceStatusChange();
			break;
		case UsbCommandID.GetCurrentConfig:
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				UpdateingConfigFlag = true;
				if (command.receivedData == null)
				{
					if (ConfigParam[0] != 1)
					{
						SetComboBoxConfigItems(1);
						ConfigParam[0] = 1;
						customComboBox_Config.SelectIndex = 0;
						ConfigParam[1] = customComboBox_Config.SelectIndex;
						int index = 0;
						for (int k = 0; k < LanguageFile.ComboBoxStructs.Count; k++)
						{
							if (LanguageFile.ComboBoxStructs[k].Name == ((Control)customComboBox_InsertEvent).Name)
							{
								index = k;
								break;
							}
						}
						customComboBox_InsertEvent.Item.Clear();
						gComboBoxInsertEvent.Items = new List<string>();
						gComboBoxInsertEvent.Items.Clear();
						gComboBoxInsertEvent.Values = new List<int>();
						gComboBoxInsertEvent.Values.Clear();
						for (int l = 0; l < LanguageFile.ComboBoxStructs[index].Values.Count; l++)
						{
							if ((LanguageFile.ComboBoxStructs[index].Values[l] & 0xF0000) != 327680)
							{
								customComboBox_InsertEvent.Item.Add((object)LanguageFile.ComboBoxStructs[index].Items[l]);
								gComboBoxInsertEvent.Items.Add(LanguageFile.ComboBoxStructs[index].Items[l]);
								gComboBoxInsertEvent.Values.Add(LanguageFile.ComboBoxStructs[index].Values[l]);
							}
						}
						((Control)customComboBox_Config).Enabled = false;
					}
				}
				else
				{
					if (ConfigParam[0] != 4)
					{
						SetComboBoxConfigItems(4);
						ConfigParam[0] = 4;
						customComboBox_Config.SelectIndex = command.receivedData[0];
						ConfigParam[1] = customComboBox_Config.SelectIndex;
						int index2 = 0;
						for (int m = 0; m < LanguageFile.ComboBoxStructs.Count; m++)
						{
							if (LanguageFile.ComboBoxStructs[m].Name == ((Control)customComboBox_InsertEvent).Name)
							{
								index2 = m;
								break;
							}
						}
						customComboBox_InsertEvent.Item.Clear();
						gComboBoxInsertEvent.Items = new List<string>();
						gComboBoxInsertEvent.Items.Clear();
						gComboBoxInsertEvent.Values = new List<int>();
						gComboBoxInsertEvent.Values.Clear();
						for (int n = 0; n < LanguageFile.ComboBoxStructs[index2].Values.Count; n++)
						{
							customComboBox_InsertEvent.Item.Add((object)LanguageFile.ComboBoxStructs[index2].Items[n]);
							gComboBoxInsertEvent.Items.Add(LanguageFile.ComboBoxStructs[index2].Items[n]);
							gComboBoxInsertEvent.Values.Add(LanguageFile.ComboBoxStructs[index2].Values[n]);
						}
						((Control)customComboBox_Config).Enabled = true;
					}
					else if (command.receivedData[0] != ConfigParam[1])
					{
						customComboBox_Config.SelectIndex = command.receivedData[0];
						ConfigParam[1] = customComboBox_Config.SelectIndex;
					}
					if (deviceParam.UnchangeProfile == 1)
					{
						((Control)customComboBox_Config).Enabled = false;
					}
					else
					{
						((Control)customComboBox_Config).Enabled = true;
					}
				}
				UpdateingConfigFlag = false;
			});
			break;
		case UsbCommandID.ReadVersionID:
		{
			int version = DataParser.GetDeviceVersion(command.receivedData);
			ConnectedDeviceInfo.MouseVersion = "v" + ValueConvert.IntToVersion(version);
			if (version != 0)
			{
				RegeditManager.SetMouseVersion(ConnectedDeviceInfo.deviceInfo.CID, ConnectedDeviceInfo.deviceInfo.MID, ConnectedDeviceInfo.MouseVersion);
			}
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				customSetting1.MouseVersion = "v" + ValueConvert.IntToVersion(version);
			});
			break;
		}
		case UsbCommandID.ReadFlashData:
			if (command.address == 0 && command.receivedData.Length == 6912)
			{
				DataParser.ProtocolParser(command.receivedData, ref gFlashDataMap);
				ConfigFile.SaveDeviceConfigFile("Device_CID" + ConnectedDeviceInfo.deviceInfo.CID.ToString().ToUpper() + "_MID" + ConnectedDeviceInfo.deviceInfo.MID.ToString().ToUpper() + ".bin", gFlashDataMap);
				UpdateFlashData();
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					UpdateUI();
				});
				UsbServer.ReadDPILed();
				UsbServer.ReadBatteryLevel();
				UsbServer.ReadVersion();
				UsbServer.ReadConfig();
				if (!ConnectedDeviceInfo.isUSB && deviceParam.Advanced != null && deviceParam.Advanced[0] >= 0)
				{
					UsbServer.GetLongRangeMode();
				}
				if (!ConnectedDeviceInfo.isUSB && ConnectedDeviceInfo.deviceInfo.DeviceType == 1 && deviceParam.Advanced != null && deviceParam.Advanced[1] >= 0)
				{
					UsbServer.Get4KDongleRGBValue();
				}
				if (deviceRestore)
				{
					deviceRestore = false;
					for (int num4 = 0; num4 < 16; num4++)
					{
						if (deviceParam.KeyParams[num4].type != 5)
						{
							gFlashDataMap.shortCutKey[num4] = new ShortCutKey(0);
						}
						gFlashDataMap.macroKey[num4] = new MacroKey(0);
					}
					DataParser.Update(gFlashDataMap);
				}
				if (ConnectedDeviceInfo.isUSB)
				{
					((Control)this).Invoke((Delegate)(EventHandler)delegate
					{
						customSetting1.RemoteAdvanced();
					});
				}
			}
			else if (command.address == 0 && command.receivedData.Length == 1)
			{
				if (ConnectedDeviceInfo.deviceInfo.DeviceType == 2 && command.receivedData[0] >= 16)
				{
					break;
				}
				gFlashDataMap.mouseConfig.reportRate = command.receivedData[0];
				UpdateDllFlashMap();
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					if (deviceParam.XIn1 >= 3)
					{
						_3In1_Main.UpdateReportRateUI(command.receivedData[0]);
					}
					else if (deviceParam.XIn1 <= 2)
					{
						_2In1_Main.UpdateReportRateUI(command.receivedData[0]);
					}
				});
			}
			else if (command.address == 4 && command.receivedData.Length == 1)
			{
				gFlashDataMap.mouseConfig.currentDPI = command.receivedData[0];
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					dpiControl1.UpdateCurrentDPI(gFlashDataMap.mouseConfig.currentDPI);
				});
			}
			else if (command.address == 76 && command.receivedData.Length == 8)
			{
				DataParser.BufferToDPILed(command.receivedData, ref gFlashDataMap.dpiLed);
				UpdateDllFlashMap();
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					if (deviceParam.XIn1 >= 3)
					{
						_3In1_Main.UpdateDPIEffect(gFlashDataMap);
					}
					else if (deviceParam.XIn1 <= 2)
					{
						_2In1_Main.UpdateDPIEffect(gFlashDataMap);
					}
				});
			}
			else if (command.address == 160 && command.receivedData.Length == 9)
			{
				DataParser.BufferToLedBar(command.receivedData, ref gFlashDataMap.ledBar);
				UpdateDllFlashMap();
				((Control)this).Invoke((Delegate)(EventHandler)delegate
				{
					UpdateLightUI();
				});
			}
			break;
		case UsbCommandID.Get4KDongleRGBValue:
			if (command.receivedData == null)
			{
				break;
			}
			dongle4KLED = true;
			Array.Copy(command.receivedData, this.dongleRGB, command.receivedData.Length);
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				if (customSetting1 != null)
				{
					customSetting1.AddDongleRGB();
					customSetting1.SetDongleRGB(this.dongleRGB);
				}
			});
			break;
		case UsbCommandID.GetLongRangeMode:
			if (command.receivedData != null)
			{
				if (command.receivedData.Length == 10)
				{
					((Control)this).Invoke((Delegate)(EventHandler)delegate
					{
						if (customSetting1 != null)
						{
							customSetting1.AddLongDistance();
							customSetting1.LongDistance = command.receivedData[0] > 0;
						}
					});
				}
			}
			else
			{
				advSetting = false;
			}
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				if (customSetting1 != null && !ConnectedDeviceInfo.isUSB)
				{
					customSetting1.DongleType = ConnectedDeviceInfo.deviceInfo.DeviceType;
					customSetting1.DongleVersion = ConnectedDeviceInfo.DongleVersion;
					customSetting1.MouseVersion = ConnectedDeviceInfo.MouseVersion;
				}
			});
			break;
		}
	}

	private void USBDeviceStatusChange()
	{
		if (gDeviceStatusChanged.isDPIChanged > 0)
		{
			UsbServer.ReadCurrentDPI();
		}
		if (gDeviceStatusChanged.isReportRateChanged > 0)
		{
			UsbServer.ReadReportRate();
		}
		if (gDeviceStatusChanged.isBatteryLevelChanged > 0)
		{
			UsbServer.ReadBatteryLevel();
		}
		if (gDeviceStatusChanged.isLedBarChanged > 0)
		{
			UsbServer.ReadLedBar();
		}
		if (gDeviceStatusChanged.isDPILedChanged > 0)
		{
			UsbServer.ReadDPILed();
		}
		_ = gDeviceStatusChanged.isLogoLedChanged;
		_ = 0;
		if (gDeviceStatusChanged.isConfigChanged <= 0)
		{
			return;
		}
		((Control)this).Invoke((Delegate)(EventHandler)delegate
		{
			ConfigPressFlag = true;
			ConfigPressTimeout = 0;
			if (gConnectState != ConnectState.Connecting)
			{
				ConnectTimeoutCount = 0;
				gConnectState = ConnectState.Connecting;
			}
			else
			{
				ConnectTimeoutCount = 0;
			}
		});
	}

	private bool GetDeviceOnline()
	{
		if (UIInitFlag)
		{
			return true;
		}
		return GetDeviceOnlineFlag();
	}

	public static bool GetDeviceOnlineFlag()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		if (!ThisHandleCreate)
		{
			return true;
		}
		if (SetLanguage)
		{
			return true;
		}
		if (AllDeviceInfoList.Count == 0)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[6])).ShowDialog();
		}
		else if (UsbFinder.GetDeviceOnLine(ConnectedDeviceInfo.deviceString))
		{
			result = true;
		}
		else
		{
			gConnectState = ConnectState.Disconnect;
			ConnectedDeviceInfo = default(DeviceAllInfo);
			DeviceStateTimerReset(1320);
			OfflineConfigDialog = new FormDialog(LanguageFile.Dialogs[31]);
			((Form)OfflineConfigDialog).ShowDialog();
		}
		return result;
	}

	private void UpdataUSBData()
	{
		if (!UIInitFlag)
		{
			DataParser.Update(gFlashDataMap);
			UpdateFlashData();
		}
	}

	public static void UpdateDllFlashMap()
	{
		DataParser.SetDllProtocolData(in gFlashDataMap);
	}

	private void UpdateFlashData()
	{
		customKeyFunction1.UpdateFlash(gFlashDataMap);
		dpiControl1.UpdateFlash(gFlashDataMap);
		if (deviceParam.XIn1 >= 3)
		{
			_3In1_Main.UpdateFlash(gFlashDataMap);
		}
		else if (deviceParam.XIn1 <= 2)
		{
			_2In1_Main.UpdateFlash(gFlashDataMap);
		}
	}

	private void UpdateFlashDataMap_ValueChange(object sender, FlashDataMap e)
	{
		gFlashDataMap = e;
		UpdataUSBData();
	}

	public void FlashDataMap_ValueChange(object sender, FlashDataMap e)
	{
		if (GetDeviceOnline())
		{
			gFlashDataMap = e;
			UpdataUSBData();
		}
	}

	private bool GetDeviceInfo(ref DeviceAllInfo deviceAllInfo)
	{
		bool result = false;
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
			for (int j = 0; j < driveParam.DeviceTotal; j++)
			{
				if (deviceInfo.CID == driveParam.CID && deviceInfo.MID == driveParam.DeviceParams[j].MID)
				{
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
					deviceAllInfo.deviceIndex = j;
					deviceAllInfo.address = new List<byte>();
					byte[] deviceOnLineWithAddress = UsbFinder.GetDeviceOnLineWithAddress(deviceString);
					deviceAllInfo.online = deviceOnLineWithAddress[6] == 1;
					deviceAllInfo.address = new List<byte>();
					for (int k = 7; k < 10; k++)
					{
						deviceAllInfo.address.Add(deviceOnLineWithAddress[k]);
					}
					if (SelectedDeviceInfo.deviceString == deviceAllInfo.deviceString)
					{
						SelectedDeviceInfo = deviceAllInfo;
					}
					result = true;
					break;
				}
				if (SelectedDeviceInfo.deviceString == null)
				{
					deviceAllInfo.deviceIndex = -1;
				}
			}
		}
		return result;
	}

	private void GetDriveInfo(List<string> VID, List<string> PID, int interfaceId, int deviceId)
	{
		for (int i = 0; i < VID.Count; i++)
		{
			string text = VID[i];
			for (int j = 0; j < PID.Count; j++)
			{
				systemLog.WriteLog("Scaning Vid：" + text + "，Pid：" + PID[j] + "，interfaceId：" + interfaceId + "，deviceId：" + deviceId);
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
					if (SelectedDeviceInfo.deviceString == null)
					{
						AllDeviceInfoList.Add(deviceAllInfo);
					}
					else if (GetDeviceInfo(ref deviceAllInfo) && deviceAllInfo.deviceIndex == SelectedDeviceInfo.deviceIndex)
					{
						AllDeviceInfoList.Add(deviceAllInfo);
					}
				}
			}
		}
	}

	private void DeviceConnect(DeviceAllInfo deviceAllInfo)
	{
		if (UsbFinder.GetDeviceOnLine(deviceAllInfo.deviceString))
		{
			gConnectState = ConnectState.Connecting;
			ConnectedDeviceInfo = deviceAllInfo;
			SelectedDeviceInfo = ConnectedDeviceInfo;
			string text = "";
			for (int i = 0; i < ConnectedDeviceInfo.address.Count; i++)
			{
				text += ConnectedDeviceInfo.address[i].ToString("x2");
			}
			systemLog.WriteLog("Adress:" + text);
			systemLog.WriteLog("DeviceConnect:" + ConnectedDeviceInfo.deviceString);
			batteryHandleInit = false;
			int[] array = new int[21];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = deviceParam.BatteryParam[j + 1];
			}
			UsbServer.SetBatteryOptimizeParam(deviceParam.BatteryParam[0], 10000, 300000);
			UsbServer.SetBatteryOptimizeSection(array);
			UsbServer.Start(ConnectedDeviceInfo.deviceString, ConnectedDeviceInfo.deviceString, onUsbDataReceived);
			UsbServer.SetPCDriverStatus(isActived: true);
			USBReadAllFlashData();
		}
	}

	private void TimerInit()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		ImportConfigTimer = new Timer();
		ImportConfigTimer.Enabled = false;
		ImportConfigTimer.Interval = 4000;
		ImportConfigTimer.Tick += ImportConfigTimer_Tick;
		IntervalTimer = new Timer();
		IntervalTimer.Enabled = true;
		IntervalTimer.Interval = 100;
		IntervalTimer.Tick += IntervalTimer_Tick;
		UpdateUIErrTimer = new Timer();
		UpdateUIErrTimer.Enabled = true;
		UpdateUIErrTimer.Interval = 200;
		UpdateUIErrTimer.Tick += UpdateUIErrTimer_Tick;
		UsbPlugTimer = new Timer();
		UsbPlugTimer.Enabled = false;
		UsbPlugTimer.Interval = 200;
		UsbPlugTimer.Tick += UsbPlugTimer_Tick;
	}

	private void UsbPlugTimer_Tick(object sender, EventArgs e)
	{
		usbPlugCount++;
		systemLog.WriteLog("usbPlugCount:" + usbPlugCount);
		if (usbPlugCount > 10)
		{
			usbPlugFlag = false;
			UsbPlugTimer.Enabled = false;
			customButton_HomePage_Click(null, null);
		}
	}

	private void UpdateUIErrTimer_Tick(object sender, EventArgs e)
	{
		if (gConnectState == ConnectState.Connected && !dpiUpdateRes)
		{
			UpdateUIErrTimer.Enabled = false;
			UpdataUSBData();
		}
	}

	private void IntervalTimer_Tick(object sender, EventArgs e)
	{
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		if (gConnectState == ConnectState.Connecting)
		{
			if (OfflineConfigDialog != null && !((Control)OfflineConfigDialog).IsDisposed)
			{
				((Form)OfflineConfigDialog).Close();
				((Component)(object)OfflineConfigDialog).Dispose();
			}
			ConnectTimeoutCount++;
			if (ConnectTimeoutCount >= 80)
			{
				gConnectState = ConnectState.TimeOut;
			}
			else if (ConnectingDialog != null)
			{
				if (((Control)ConnectingDialog).IsDisposed && !windowsMini)
				{
					ConnectingDialog = new FormDialog(LanguageFile.Dialogs[4], DialogButtons.NO);
					((Form)ConnectingDialog).StartPosition = (FormStartPosition)0;
					((Form)ConnectingDialog).Location = new Point(((Control)this).Left + (((Control)this).Width - ((Control)ConnectingDialog).Width) / 2, ((Control)this).Top + (((Control)this).Height - ((Control)ConnectingDialog).Height) / 2);
					((Form)ConnectingDialog).ShowDialog();
				}
			}
			else if (!windowsMini)
			{
				ConnectingDialog = new FormDialog(LanguageFile.Dialogs[4], DialogButtons.NO);
				((Form)ConnectingDialog).StartPosition = (FormStartPosition)0;
				((Form)ConnectingDialog).Location = new Point(((Control)this).Left + (((Control)this).Width - ((Control)ConnectingDialog).Width) / 2, ((Control)this).Top + (((Control)this).Height - ((Control)ConnectingDialog).Height) / 2);
				((Form)ConnectingDialog).ShowDialog();
			}
		}
		else
		{
			ConnectTimeoutCount = 0;
			if (ConnectingDialog != null)
			{
				if (!((Control)ConnectingDialog).IsDisposed)
				{
					((Form)ConnectingDialog).Close();
					((Component)(object)ConnectingDialog).Dispose();
				}
				if (formSetting != null)
				{
					formSetting.SetMouseVersion(ConnectedDeviceInfo.MouseVersion);
				}
			}
			if (gConnectState == ConnectState.TimeOut)
			{
				gConnectState = ConnectState.Disconnect;
				IntervalTimer.Enabled = false;
				FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[5], DialogButtons.OK);
				((Form)formDialog).StartPosition = (FormStartPosition)0;
				((Form)formDialog).Location = new Point(((Control)this).Left + (((Control)this).Width - ((Control)ConnectingDialog).Width) / 2, ((Control)this).Top + (((Control)this).Height - ((Control)ConnectingDialog).Height) / 2);
				((Form)formDialog).ShowDialog();
				if (SelectedDeviceInfo.deviceString != null)
				{
					customButton_HomePage_Click(null, null);
				}
				else
				{
					IntervalTimer.Enabled = true;
					ConnectedDeviceInfo = default(DeviceAllInfo);
				}
			}
		}
		if (ConfigPressFlag)
		{
			ConfigPressTimeout++;
			if (ConfigPressTimeout > 10)
			{
				ConfigPressTimeout = 0;
				ConfigPressFlag = false;
				USBReadAllFlashData();
			}
		}
		else
		{
			ConfigPressTimeout = 0;
		}
		if (!((Control)this).IsHandleCreated)
		{
			return;
		}
		OnlineCheckCount++;
		if (USBProcessInitFlag && OnlineCheckCount > 18)
		{
			OnlineCheckCount = 0;
			if (ConnectedDeviceInfo.deviceString == null && SelectedDeviceInfo.deviceString != null && UsbFinder.GetDeviceOnLine(SelectedDeviceInfo.deviceString) && !customSetting1.updating)
			{
				SelectedDeviceInfo.online = true;
				DeviceConnect(SelectedDeviceInfo);
			}
		}
		if (!USBProcessInitFlag)
		{
			USBProcessInitFlag = true;
			if (SelectedDeviceInfo.online)
			{
				DeviceConnect(SelectedDeviceInfo);
			}
			USBProcess_Init();
		}
		if (ThisHandleCreateCnt < 10)
		{
			ThisHandleCreateCnt++;
		}
		if (ThisHandleCreateCnt >= 8 && !ThisHandleCreate)
		{
			ThisHandleCreate = true;
		}
		if (!LoadFlag)
		{
			LoadFlag = true;
		}
	}

	private void TimerUninit()
	{
		DeviceStateTimer.Enabled = false;
		IntervalTimer.Enabled = false;
	}

	private void DeviceStateTimerInit(int interval)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		DeviceStateTimer = new Timer();
		DeviceStateTimer.Enabled = true;
		DeviceStateTimer.Interval = interval;
		if (SelectedDeviceInfo.deviceString == null)
		{
			DeviceStateTimer.Tick += DeviceStateTimer_Tick;
		}
	}

	public static void DeviceStateTimerReset(int interval)
	{
		DeviceStateTimer.Enabled = false;
		DeviceStateTimer.Enabled = true;
		DeviceStateTimer.Interval = interval;
	}

	private void DeviceStateTimer_Tick(object sender, EventArgs e)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		List<DeviceAllInfo> list = new List<DeviceAllInfo>();
		list.Clear();
		if (MultiOnlineFlag)
		{
			if (!UsbFinder.GetDeviceOnLine(ConnectedDeviceInfo.deviceString))
			{
				MultiOnlineFlag = false;
			}
			else
			{
				IntervalTimer.Enabled = false;
				if (formDialogMultiOnline == null)
				{
					formDialogMultiOnline = new FormDialog(LanguageFile.Dialogs[29]);
					((Form)formDialogMultiOnline).ShowDialog();
					System_Exit(null, null);
				}
			}
		}
		for (int i = 0; i < AllDeviceInfoList.Count; i++)
		{
			bool flag = false;
			string deviceString = AllDeviceInfoList[i].deviceString;
			DeviceAllInfo deviceAllInfo = AllDeviceInfoList[i];
			deviceAllInfo.online = false;
			if (deviceAllInfo.deviceString != ConnectedDeviceInfo.deviceString && UsbFinder.GetDeviceOnLine(deviceString))
			{
				GetDeviceInfo(ref deviceAllInfo);
				if (deviceAllInfo.deviceIndex == SelectedDeviceInfo.deviceIndex)
				{
					if (ConnectedDeviceInfo.deviceString == null)
					{
						DeviceConnect(deviceAllInfo);
					}
					else if (UsbFinder.GetDeviceOnLine(ConnectedDeviceInfo.deviceString))
					{
						MultiOnlineFlag = true;
					}
					else
					{
						DeviceConnect(deviceAllInfo);
					}
				}
				else
				{
					flag = true;
				}
			}
			if (!flag)
			{
				list.Add(deviceAllInfo);
			}
		}
		AllDeviceInfoList.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			AllDeviceInfoList.Add(list[j]);
		}
	}

	private void UIInit(int index)
	{
		UIInitFlag = true;
		string fileName = "Device_CID" + driveParam.CID.ToString().ToUpper() + "_MID" + deviceParam.MID.ToString().ToUpper() + ".bin";
		if (!deviceParam.DisplayLight)
		{
			((Control)customTabControl_Main).Controls.RemoveAt(3);
		}
		customTabSelector_Main.TabControl = (TabControl)(object)customTabControl_Main;
		((Control)label_Title).Text = deviceParam.DeviceName;
		ButtonFunctionInit(deviceParam.KeyNumber);
		DPIInit();
		XIn1Init();
		byte[] data = new byte[10];
		if (ConfigFile.ReadDeviceConfigFile(fileName, ref data))
		{
			gFlashDataMap = ConfigFile.ByteToFlashDataMap(data);
			customKeyFunction1.UpdateUI(gFlashDataMap, languageFile, deviceParam.SensorDPI);
			customKeyFunction1.ButtonForeColor = driveParam.NumClr;
			UpdateXIn1UI();
			UpdateDPIUI();
			UpdateLightUI();
		}
		else
		{
			customKeyFunction1.UpdateUI(deviceParam, languageFile);
			dpiControl1.UpdateUI(deviceParam);
			if (deviceParam.XIn1 == 3)
			{
				_3In1_Main.UpdateUI(deviceParam);
				_3In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
				_3In1_Main.ValueChange -= FlashDataMap_ValueChange;
				_3In1_Main.ValueChange += FlashDataMap_ValueChange;
			}
			else
			{
				_2In1_Main.UpdateUI(deviceParam);
				_2In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
				_2In1_Main.ValueChange -= FlashDataMap_ValueChange;
				_2In1_Main.ValueChange += FlashDataMap_ValueChange;
			}
			UpdateLightUI(deviceParam);
		}
		customSetting1.LanguageChange += LanguageChange;
		customSetting1.DeviceUpdateFail += CustomSetting1_DeviceUpdateFail;
		customSetting1.UpdateUI(languageFile, deviceParam);
		if (!SelectedDeviceInfo.isUSB)
		{
			customSetting1.DongleType = SelectedDeviceInfo.deviceInfo.DeviceType;
			customSetting1.DongleVersion = SelectedDeviceInfo.DongleVersion;
		}
		SetButton((Control)(object)this);
		formResize.Resize((Form)(object)this);
		UIInitFlag = false;
	}

	private void CustomSetting1_DeviceUpdateFail(object sender)
	{
		customButton_HomePage_Click(null, null);
	}

	private void UpdateUI()
	{
		customKeyFunction1.UpdateUI(gFlashDataMap, languageFile, deviceParam.SensorDPI);
		UpdateXIn1UI();
		UpdateDPIUI();
		UpdateLightUI();
		gConnectState = ConnectState.Connected;
	}

	private void UpdateXIn1UI()
	{
		systemLog.WriteLog("UpdateXIn1UI:" + deviceParam.XIn1 + " DeviceType:" + SelectedDeviceInfo.deviceInfo.DeviceType);
		if (deviceParam.XIn1 >= 3)
		{
			_3In1_Main.UpdateUI(deviceParam, gFlashDataMap, SelectedDeviceInfo.deviceInfo.DeviceType);
			_3In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
			_3In1_Main.ValueChange -= FlashDataMap_ValueChange;
			_3In1_Main.ValueChange += FlashDataMap_ValueChange;
		}
		else
		{
			_2In1_Main.UpdateUI(deviceParam, gFlashDataMap, SelectedDeviceInfo.deviceInfo.DeviceType);
			_2In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
			_2In1_Main.ValueChange -= FlashDataMap_ValueChange;
			_2In1_Main.ValueChange += FlashDataMap_ValueChange;
		}
	}

	private void UpdateDPIUI()
	{
		dpiUpdateRes = dpiControl1.UpdateUI(gFlashDataMap, deviceParam.SensorDPI);
	}

	private void UpdateLightModeUI(bool[] bools)
	{
		customTrackBar_LightBrightness.Enable = bools[0];
		((Control)label_LightBrightnessValue).Visible = bools[0];
		customTrackBar_LightSpeed.Enable = bools[1];
		((Control)label_LightSpeedValue).Visible = bools[1];
		((Control)panel_LEDColor).Visible = bools[2];
	}

	private void UpdateLightUI()
	{
		UpdateLEDFlag = true;
		((Control)pictureBox_LEDPreview).BackColor = Color.FromArgb(gFlashDataMap.ledBar.color[0], gFlashDataMap.ledBar.color[1], gFlashDataMap.ledBar.color[2]);
		Color backColor = ((Control)pictureBox_LEDPreview).BackColor;
		adjustControl_R.Value = backColor.R;
		adjustControl_G.Value = backColor.G;
		adjustControl_B.Value = backColor.B;
		customTrackBar_LightBrightness.Value = gFlashDataMap.ledBar.brightness;
		int num = customTrackBar_LightBrightness.Value + 1;
		((Control)label_LightBrightnessValue).Text = num.ToString();
		customTrackBar_LightSpeed.Value = gFlashDataMap.ledBar.speed;
		num = customTrackBar_LightSpeed.Value + 1;
		((Control)label_LightSpeedValue).Text = num.ToString();
		if (gFlashDataMap.ledBar.enable > 0)
		{
			customComboBox_LEDMode.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_LEDMode, gFlashDataMap.ledBar.mode);
		}
		else
		{
			customComboBox_LEDMode.SelectIndex = 0;
		}
		UpdateLightModeUI(ValueConvert.SetLEDMode(customComboBox_LEDMode.SelectIndex));
		customCheckBox_MovingCloseLight.Checked = gFlashDataMap.mouseConfig.moveOffLedEnable > 0;
		customComboBox_PowerSaveTime.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_PowerSaveTime, gFlashDataMap.mouseConfig.allLedOffTime);
		UpdateLEDFlag = false;
	}

	private void UpdateLightUI(DriveConfig.DeviceParam deviceParam)
	{
		UpdateLEDFlag = true;
		string[] array = deviceParam.LightUI.Split(new char[1] { ',' });
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = Convert.ToInt32(array[i]);
		}
		((Control)pictureBox_LEDPreview).BackColor = Color.FromArgb(array2[5], array2[6], array2[7]);
		Color backColor = ((Control)pictureBox_LEDPreview).BackColor;
		adjustControl_R.Value = backColor.R;
		adjustControl_G.Value = backColor.G;
		adjustControl_B.Value = backColor.B;
		customTrackBar_LightBrightness.Value = array2[1];
		int num = customTrackBar_LightBrightness.Value + 1;
		((Control)label_LightBrightnessValue).Text = num.ToString();
		customTrackBar_LightSpeed.Value = array2[2];
		num = customTrackBar_LightSpeed.Value + 1;
		((Control)label_LightSpeedValue).Text = num.ToString();
		customComboBox_LEDMode.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_LEDMode, array2[0]);
		UpdateLightModeUI(ValueConvert.SetLEDMode(customComboBox_LEDMode.SelectIndex));
		customCheckBox_MovingCloseLight.Checked = array2[3] > 0;
		customComboBox_PowerSaveTime.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_PowerSaveTime, array2[4]);
		UpdateLEDFlag = false;
	}

	private string[] GetConnectionVersion()
	{
		string[] array = new string[2];
		if (ConnectedDeviceInfo.online)
		{
			if (ConnectedDeviceInfo.isUSB)
			{
				array[0] = null;
				if (AllDeviceInfoList.Count == 2)
				{
					for (int i = 0; i < AllDeviceInfoList.Count; i++)
					{
						if (AllDeviceInfoList[i].address != null && !AllDeviceInfoList[i].isUSB)
						{
							array[0] = AllDeviceInfoList[i].DongleVersion;
							break;
						}
					}
				}
				array[1] = ConnectedDeviceInfo.MouseVersion;
			}
			else
			{
				array[0] = ConnectedDeviceInfo.DongleVersion;
				array[1] = ConnectedDeviceInfo.MouseVersion;
			}
		}
		else
		{
			array[0] = SelectedDeviceInfo.DongleVersion;
			array[1] = null;
		}
		return array;
	}

	private void customButton_Setting_Click(object sender, EventArgs e)
	{
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		if (customRecordMacro1.Checked)
		{
			return;
		}
		MethodBase method = new StackTrace().GetFrame(0).GetMethod();
		string[] connectionVersion = GetConnectionVersion();
		IsSetting = true;
		bool update = SelectedDeviceInfo.deviceString != null;
		int reportrate = 1;
		if (SelectedDeviceInfo.isUSB)
		{
			if (AllDeviceInfoList.Count == 2)
			{
				for (int i = 0; i < AllDeviceInfoList.Count; i++)
				{
					if (AllDeviceInfoList[i].address != null)
					{
						if (!AllDeviceInfoList[i].isUSB && AllDeviceInfoList[i].deviceInfo.DeviceType == 1)
						{
							reportrate = 4;
							break;
						}
						if (!AllDeviceInfoList[i].isUSB && AllDeviceInfoList[i].deviceInfo.DeviceType == 4)
						{
							reportrate = 2;
							break;
						}
						if (!AllDeviceInfoList[i].isUSB && AllDeviceInfoList[i].deviceInfo.DeviceType == 0)
						{
							reportrate = 1;
							break;
						}
					}
				}
			}
		}
		else if (SelectedDeviceInfo.deviceInfo.DeviceType == 1)
		{
			reportrate = 4;
		}
		else if (SelectedDeviceInfo.deviceInfo.DeviceType == 4)
		{
			reportrate = 2;
		}
		else if (SelectedDeviceInfo.deviceInfo.DeviceType == 0)
		{
			reportrate = 1;
		}
		systemLog.WriteLog(method.Name + " enter");
		formSetting = new FormSetting(languageFile, connectionVersion, languageFile.LanguageIndex, languageFile.FromSettingString, driveParam.CID, deviceParam, update, reportrate);
		formSetting.LanguageChange -= LanguageChange;
		formSetting.LanguageChange += LanguageChange;
		((Form)formSetting).StartPosition = (FormStartPosition)0;
		((Form)formSetting).Location = new Point(((Control)this).Left + (((Control)this).Width - ((Control)formSetting).Width) / 2, ((Control)this).Top + (((Control)this).Height - ((Control)formSetting).Height) / 2);
		((Form)formSetting).ShowDialog();
		IsSetting = false;
		systemLog.WriteLog(method.Name + " exit");
	}

	private void LanguageChange(object sneder, CustomEventArgs args)
	{
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_022c: Expected O, but got Unknown
		int num = (int)args.Value;
		if (num == languageFile.LanguageIndex)
		{
			return;
		}
		languageFile.LanguageIndex = num;
		SetLanguage = true;
		languageFile.SetLanguage(num, (Control)(object)this);
		if (ConnectedDeviceInfo.online)
		{
			customKeyFunction1.UpdateUI(gFlashDataMap, languageFile, deviceParam.SensorDPI);
		}
		else
		{
			customKeyFunction1.UpdateUI(deviceParam, languageFile);
		}
		if (deviceParam.XIn1 >= 3)
		{
			bool support = false;
			if (ConnectedDeviceInfo.online && (ConnectedDeviceInfo.isUSB || gFlashDataMap.mouseConfig.reportRate == 16 || gFlashDataMap.mouseConfig.reportRate == 32))
			{
				support = true;
			}
			_3In1_Main.UpdateModeSelect(support);
			if (CalPass)
			{
				_3In1_Main.AddLODItems(LanguageFile.Dialogs[66]);
			}
		}
		languageFile.GetHz(num);
		if (deviceParam.XIn1 >= 3)
		{
			_3In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
		}
		else
		{
			_2In1_Main.SetReportRateHz(languageFile.GetHz(languageFile.LanguageIndex));
		}
		languageFile.KeyFunctionSubStructs[6].Clear();
		for (int i = 0; i < MacroKeyAndCycleList.Count; i++)
		{
			KeyFunctionStruct item = new KeyFunctionStruct
			{
				name = MacroKeyAndCycleList[i].name
			};
			languageFile.KeyFunctionSubStructs[6].Add(item);
		}
		if (ConnectedDeviceInfo.online)
		{
			customKeyFunction1.UpdateKeyDebounce(languageFile, gFlashDataMap.mouseConfig.keyDebounceTime);
		}
		else
		{
			customKeyFunction1.UpdateKeyDebounce(languageFile, deviceParam.KeyDebounceTime);
		}
		CustomButton customButton = customButton_Reset;
		CustomButton customButton2 = customButton_Import;
		CustomButton customButton3 = customButton_Export;
		Font val = new Font("微软雅黑", LanguageFile.GetFontSize(languageFile.LanguageIndex));
		Font val2 = val;
		((Control)customButton3).Font = val;
		Font font = (((Control)customButton2).Font = val2);
		((Control)customButton).Font = font;
		LoadToolStripMenuItem();
		IsSetting = true;
		IsSetting = false;
		SetLanguage = false;
	}

	private void customButton_HomePage_Click(object sender, EventArgs e)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (customSetting1.updating)
		{
			return;
		}
		try
		{
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				if (formSetting != null && !((Control)formSetting).IsDisposed)
				{
					((Form)formSetting).Close();
					((Component)(object)formSetting).Dispose();
				}
				if (ConnectingDialog != null && !((Control)ConnectingDialog).IsDisposed)
				{
					((Form)ConnectingDialog).Close();
					((Component)(object)ConnectingDialog).Dispose();
				}
				if (OfflineConfigDialog != null && !((Control)OfflineConfigDialog).IsDisposed)
				{
					((Form)OfflineConfigDialog).Close();
					((Component)(object)OfflineConfigDialog).Dispose();
				}
			});
		}
		catch (Exception ex)
		{
			systemLog.WriteLog(ex.Message);
		}
		ToHomePage = true;
		TimerUninit();
		System_Exit(null, null);
		if (formHomePage == null)
		{
			systemLog.WriteLog("Start Back HomePage");
			if (HideTimer == null)
			{
				HideTimer = new Timer();
				HideTimer.Interval = 100;
				HideTimer.Tick += HideTimer_Tick;
			}
			HideTimer.Start();
		}
	}

	private void customButton_Mini_Click(object sender, EventArgs e)
	{
		((Control)this).Hide();
		windowsMini = true;
		UsbServer.SetPCDriverStatus(isActived: false);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

	private void notifyIcon_Main_MouseClick(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			ToolStripMenuItem_OpenMenu_Click(null, null);
		}
	}

	private void ToolStripMenuItem_OpenMenu_Click(object sender, EventArgs e)
	{
		((Control)this).Show();
		windowsMini = false;
		((Form)this).WindowState = (FormWindowState)0;
		SwitchToThisWindow(((Control)this).Handle, fAltTab: true);
		UsbServer.SetPCDriverStatus(isActived: true);
		for (int i = 0; i < AllDeviceInfoList.Count; i++)
		{
			if (ConnectedDeviceInfo.deviceString != null && AllDeviceInfoList[i].deviceIndex == SelectedDeviceInfo.deviceIndex && AllDeviceInfoList[i].isUSB && AllDeviceInfoList[i].online && !ConnectedDeviceInfo.isUSB && AllDeviceInfoList[i].address[0] == ConnectedDeviceInfo.address[0] && AllDeviceInfoList[i].address[1] == ConnectedDeviceInfo.address[1] && AllDeviceInfoList[i].address[2] == ConnectedDeviceInfo.address[2])
			{
				DeviceConnect(AllDeviceInfoList[i]);
			}
		}
	}

	private void System_Exit(object sender, EventArgs e)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		UsbFinder.StopUsbChanged();
		customSetting1.ExitPair();
		if (ConnectedDeviceInfo.online)
		{
			if (gConnectState == ConnectState.Connected)
			{
				ConfigFile.SaveDeviceConfigFile("Device_CID" + ConnectedDeviceInfo.deviceInfo.CID.ToString().ToUpper() + "_MID" + ConnectedDeviceInfo.deviceInfo.MID.ToString().ToUpper() + ".bin", gFlashDataMap);
			}
			UsbServer.SetPCDriverStatus(isActived: false);
		}
		systemLog.WriteLog("Main Exit");
		notifyIcon_Main.Visible = false;
		notifyIcon_Main.Icon = null;
		MacroFile.SaveLocalMacroKey(MacroKeyAndCycleList);
		if (ExitTimer == null)
		{
			ExitTimer = new Timer();
			ExitTimer.Tick += Delay100ms_Exit;
			ExitTimer.Interval = 100;
		}
		ExitTimer.Start();
	}

	private void Delay100ms_Exit(object sender, EventArgs e)
	{
		ExitTimer.Stop();
		UsbServer.Exit();
		systemLog.WriteLog("UsbServer Exit");
		((Component)(object)notifyIcon_Main).Dispose();
		ConnectedDeviceInfo = default(DeviceAllInfo);
		if (!ToHomePage)
		{
			Environment.Exit(0);
			((Form)this).Close();
		}
	}

	private void HideTimer_Tick(object sender, EventArgs e)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (!BackToHomePage)
		{
			BackToHomePage = true;
			formHomePage = new FormHomePage(driveParam);
			((Form)formHomePage).StartPosition = (FormStartPosition)0;
			((Form)formHomePage).Location = new Point(((Form)this).Location.X, ((Form)this).Location.Y);
			if (windowsMini)
			{
				((Control)formHomePage).Hide();
			}
			else
			{
				((Form)formHomePage).ShowDialog();
			}
		}
		else if (formHomePage != null && formHomePage.LoadFlag)
		{
			systemLog.WriteLog("Back To HomePage");
			HideTimer.Stop();
			((Control)this).Hide();
		}
	}

	private void SetAdvSetting(bool longRange)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		customButton_SystemMouse.NormalImage = (Image)new Bitmap(baseDirectory + (longRange ? "\\res\\2Button\\adv_ld.png" : "\\res\\2Button\\adv_nr.png"));
		customButton_SystemMouse.MouseDownImage = (Image)new Bitmap(baseDirectory + (longRange ? "\\res\\2Button\\adv_ld.png" : "\\res\\2Button\\adv_nr.png"));
		customButton_SystemMouse.MouseEnterImage = (Image)new Bitmap(baseDirectory + (longRange ? "\\res\\2Button\\adv_ld.png" : "\\res\\2Button\\adv_nr.png"));
	}

	private void customButton_SystemMouse_Click(object sender, EventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (!advSetting)
		{
			Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL main.cpl @0");
			return;
		}
		FormAdvancedSetting formAdvancedSetting = new FormAdvancedSetting(ConnectedDeviceInfo.isUSB, deviceParam.Sensor, longRangeMode, CalPass, dongle4KLED, dongleRGB);
		formAdvancedSetting.AdvancedChange += FormAdvancedSetting_AdvancedChange;
		((Form)formAdvancedSetting).ShowDialog();
		if ((formAdvancedSetting.longRangeMode == longRangeMode && formAdvancedSetting.CalPass == CalPass) || !GetDeviceOnline())
		{
			return;
		}
		if (formAdvancedSetting.longRangeMode != longRangeMode)
		{
			longRangeMode = formAdvancedSetting.longRangeMode;
			UsbServer.SetLongRangeMode(longRangeMode);
		}
		if (formAdvancedSetting.CalPass != CalPass)
		{
			CalPass = formAdvancedSetting.CalPass;
			if (((Control)tabPage_Sensor).Controls.Contains((Control)(object)_3In1_Main))
			{
				_3In1_Main.AddLODItems(LanguageFile.Dialogs[66]);
			}
		}
		SetAdvSetting(longRangeMode || CalPass);
	}

	private void FormAdvancedSetting_AdvancedChange(object sender, int index, int value)
	{
		if (GetDeviceOnline() && index == 1)
		{
			DongleRGB dongleRGB = new DongleRGB
			{
				mode = (byte)value
			};
			this.dongleRGB[0] = dongleRGB.mode;
			UsbServer.Set4KDongleRGB(ref dongleRGB);
		}
	}

	private void UpdateLEDPreview(PictureBox pictureBox)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Point point = ((Control)pictureBox).PointToClient(Control.MousePosition);
		Color pixel = new Bitmap(((Control)pictureBox).BackgroundImage).GetPixel(point.X * 96 / FormResize.GetDpi(), point.Y * 96 / FormResize.GetDpi());
		UpdateLEDFlag = true;
		((Control)pictureBox_LEDPreview).BackColor = pixel;
		adjustControl_R.Value = pixel.R;
		adjustControl_G.Value = pixel.G;
		adjustControl_B.Value = pixel.B;
		gFlashDataMap.ledBar.color[0] = pixel.R;
		gFlashDataMap.ledBar.color[1] = pixel.G;
		gFlashDataMap.ledBar.color[2] = pixel.B;
		UpdataUSBData();
		UpdateLEDFlag = false;
	}

	private void Light_Color_Change(object sender, EventArgs e)
	{
		if (GetDeviceOnline())
		{
			PictureBox pictureBox = (PictureBox)((sender is PictureBox) ? sender : null);
			UpdateLEDPreview(pictureBox);
		}
	}

	private void pictureBox_LEDPreview_Click(object sender, EventArgs e)
	{
	}

	private void pictureBox_LEDPreview_BackColorChanged(object sender, EventArgs e)
	{
		Color backColor = ((Control)pictureBox_LEDPreview).BackColor;
		adjustControl_R.Value = backColor.R;
		adjustControl_G.Value = backColor.G;
		adjustControl_B.Value = backColor.B;
	}

	private void ColorText_ValueChange(object sender, CustomEventArgs e)
	{
		if (!UpdateLEDFlag && GetDeviceOnline() && !UIInitFlag)
		{
			Color backColor = Color.FromArgb(adjustControl_R.Value, adjustControl_G.Value, adjustControl_B.Value);
			((Control)pictureBox_LEDPreview).BackColor = backColor;
			gFlashDataMap.ledBar.color[0] = backColor.R;
			gFlashDataMap.ledBar.color[1] = backColor.G;
			gFlashDataMap.ledBar.color[2] = backColor.B;
			UpdataUSBData();
		}
	}

	private void customTrackBar_LightBrightness_ValueChanged(object sender, CustomEventArgs e)
	{
		if (BrightnessMouseDown)
		{
			int num = customTrackBar_LightBrightness.Value + 1;
			((Control)label_LightBrightnessValue).Text = num.ToString();
			gFlashDataMap.ledBar.brightness = (byte)customTrackBar_LightBrightness.Value;
		}
		else if (!UpdateLEDFlag)
		{
			if (GetDeviceOnline())
			{
				BrightnessMouseDown = true;
				int num2 = customTrackBar_LightBrightness.Value + 1;
				((Control)label_LightBrightnessValue).Text = num2.ToString();
				gFlashDataMap.ledBar.brightness = (byte)customTrackBar_LightBrightness.Value;
			}
			else
			{
				BrightnessMouseDown = false;
			}
		}
	}

	private void customTrackBar_LightBrightness_SetValue(object sender, CustomEventArgs e)
	{
		if (BrightnessMouseDown)
		{
			BrightnessMouseDown = false;
			UpdataUSBData();
		}
	}

	private void customTrackBar_LightSpeed_ValueChanged(object sender, CustomEventArgs e)
	{
		if (SpeedMouseDown)
		{
			int num = customTrackBar_LightSpeed.Value + 1;
			((Control)label_LightSpeedValue).Text = num.ToString();
			gFlashDataMap.ledBar.speed = (byte)customTrackBar_LightSpeed.Value;
		}
		else if (!UpdateLEDFlag)
		{
			if (GetDeviceOnline())
			{
				SpeedMouseDown = true;
				int num2 = customTrackBar_LightSpeed.Value + 1;
				((Control)label_LightSpeedValue).Text = num2.ToString();
				gFlashDataMap.ledBar.speed = (byte)customTrackBar_LightSpeed.Value;
			}
			else
			{
				SpeedMouseDown = false;
			}
		}
	}

	private void customTrackBar_LightSpeed_SetValue(object sender, CustomEventArgs e)
	{
		if (SpeedMouseDown)
		{
			SpeedMouseDown = false;
			UpdataUSBData();
		}
	}

	private void customCheckBox_MovingCloseLight_CheckChange(object sender, CustomEventArgs e)
	{
		if (!UpdateLEDFlag && GetDeviceOnline())
		{
			gFlashDataMap.mouseConfig.moveOffLedEnable = (byte)(customCheckBox_MovingCloseLight.Checked ? 1u : 0u);
			UpdataUSBData();
		}
	}

	private void customComboBox_LEDMode_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (!UpdateLEDFlag && GetDeviceOnline())
		{
			int num = ValueConvert.ComboxIndexToValue(customComboBox_LEDMode);
			if (num > 0)
			{
				gFlashDataMap.ledBar.enable = 1;
				gFlashDataMap.ledBar.mode = (byte)customComboBox_LEDMode.SelectIndex;
			}
			else
			{
				gFlashDataMap.ledBar.enable = 0;
				customComboBox_LEDMode.SelectIndex = 0;
			}
			UpdateLightModeUI(ValueConvert.SetLEDMode(num));
			UpdataUSBData();
		}
	}

	private void customComboBox_PowerSaveTime_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (!UpdateLEDFlag && GetDeviceOnline())
		{
			gFlashDataMap.mouseConfig.allLedOffTime = (byte)ValueConvert.ComboxIndexToValue(customComboBox_PowerSaveTime);
			UpdataUSBData();
		}
	}

	private void customButton_Reset_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (GetDeviceOnline())
		{
			FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[24], DialogButtons.OKCanel);
			((Form)formDialog).ShowDialog();
			if (GetDeviceOnline() && formDialog.resault)
			{
				UsbServer.SetClearSetting();
				gConnectState = ConnectState.Connecting;
			}
		}
	}

	private void customButton_Import_Click(object sender, EventArgs e)
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		if (!GetDeviceOnline())
		{
			return;
		}
		byte[] data = new byte[10];
		int num = 0;
		string sensor = deviceParam.Sensor;
		num = ConfigFile.ImportConfigFile("Compx Inc", sensor, ref data);
		ImportConfigTimer.Enabled = true;
		FlashDataMap flashDataMap = ConfigFile.ByteToFlashDataMap(data);
		if (flashDataMap.mouseConfig.maxDPI > (byte)deviceParam.DPIMaxGrade)
		{
			num = 1;
		}
		byte b = 1;
		bool flag = false;
		DeviceType deviceType = (DeviceType)ConnectedDeviceInfo.deviceInfo.DeviceType;
		if (deviceType.ToString().Contains("1K"))
		{
			b = 1;
		}
		if (deviceType.ToString().Contains("2K"))
		{
			b <<= 4;
		}
		if (deviceType.ToString().Contains("4K"))
		{
			b <<= 5;
		}
		if (deviceType.ToString().Contains("8K"))
		{
			b <<= 6;
		}
		if (flashDataMap.mouseConfig.reportRate >= 16 && b < flashDataMap.mouseConfig.reportRate)
		{
			flag = true;
		}
		switch (num)
		{
		case 0:
		{
			for (int i = 0; i < 16; i++)
			{
				gFlashDataMap.shortCutKey[i] = new ShortCutKey(0);
				gFlashDataMap.macroKey[i] = new MacroKey(0);
			}
			DataParser.Update(gFlashDataMap);
			gFlashDataMap = ConfigFile.ByteToFlashDataMap(data);
			if (flag)
			{
				gFlashDataMap.mouseConfig.reportRate = b;
			}
			UpdataUSBData();
			UIInitFlag = true;
			((Control)label_Title).Text = deviceParam.DeviceName;
			ButtonFunctionInit(deviceParam.KeyNumber);
			DPIInit();
			XIn1Init();
			customKeyFunction1.UpdateUI(gFlashDataMap, languageFile, deviceParam.SensorDPI);
			UpdateXIn1UI();
			UpdateDPIUI();
			UpdateLightUI();
			UIInitFlag = false;
			ImportConfigDialog = new FormDialog(LanguageFile.Dialogs[25], DialogButtons.NO);
			((Form)ImportConfigDialog).ShowDialog();
			break;
		}
		case 1:
			((Form)new FormDialog(LanguageFile.Dialogs[26], DialogButtons.OK)).ShowDialog();
			break;
		case 2:
			((Form)new FormDialog(LanguageFile.Dialogs[27], DialogButtons.OK)).ShowDialog();
			break;
		}
	}

	private void ImportConfigTimer_Tick(object sender, EventArgs e)
	{
		ImportConfigTimer.Enabled = false;
		if (!((Control)ImportConfigDialog).IsDisposed)
		{
			((Form)ImportConfigDialog).Close();
			((Component)(object)ImportConfigDialog).Dispose();
		}
	}

	private void customButton_Export_Click(object sender, EventArgs e)
	{
		string sensor = deviceParam.Sensor;
		ConfigFile.ExportConfigFile("Compx Inc", sensor, gFlashDataMap);
	}

	private void SetComboBoxConfigItems(int count)
	{
		customComboBox_Config.Item.Clear();
		for (int i = 0; i < count; i++)
		{
			customComboBox_Config.Item.Add((object)gComboBoxConfigItems[i]);
		}
	}

	private void GetComboBoxConfigItems()
	{
		gComboBoxConfigItems = new string[customComboBox_Config.Item.Count];
		for (int i = 0; i < customComboBox_Config.Item.Count; i++)
		{
			gComboBoxConfigItems[i] = customComboBox_Config.Item[i].ToString();
		}
	}

	private void customComboBox_Config_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (GetDeviceOnline() && !UpdateingConfigFlag && !SetLanguage && customComboBox_Config.Item.Count != 1 && customComboBox_Config.SelectIndex != ConfigParam[1])
		{
			UsbServer.SetCurrentConfig(ValueConvert.ComboxIndexToValue(customComboBox_Config));
		}
	}

	private void ShowMaxMacroKey()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		formDialogMaxMacroKey = new FormDialog(LanguageFile.Dialogs[15]);
		((Form)formDialogMaxMacroKey).ShowDialog();
		((Form)formDialogMaxMacroKey).Close();
	}

	private uint StopWatchElapsedMilliseconds(uint ElapsedMilliseconds)
	{
		uint num = 65535u;
		return (ElapsedMilliseconds > num) ? num : ElapsedMilliseconds;
	}

	private void KeyboardHook_KeyDown(object sender, KeyEventArgs e, GlobalHook.KeyboardHookStruct hookStruct)
	{
		if (!customRecordMacro1.Checked)
		{
			return;
		}
		if (keyboardCode.vkCode != hookStruct.vkCode || keyboardCode.scanCode != hookStruct.scanCode || keyboardCode.flags != hookStruct.flags)
		{
			keyboardCode = KeyboardCodes.FindKeyboardCode(hookStruct);
			keyboardCode.flags = hookStruct.flags;
			if (((Control)textBox_AutoDelayTime).Text == "")
			{
				((Control)textBox_AutoDelayTime).Text = 10.ToString();
			}
			uint num = Convert.ToUInt32(((Control)textBox_AutoDelayTime).Text);
			num = ((num < 10) ? 10u : num);
			uint delay = (((RadioButton)customRadioButton_AutoDelay).Checked ? StopWatchElapsedMilliseconds((uint)stopWatch.ElapsedMilliseconds) : num);
			if (macroKeyDriver.macroKey.contextCount >= 70)
			{
				if (formDialogMaxMacroKey == null)
				{
					ShowMaxMacroKey();
				}
				else if (((Control)formDialogMaxMacroKey).IsDisposed)
				{
					ShowMaxMacroKey();
				}
			}
			else if (keyboardCode.hidCodeType != HID_CODE_TYPE.Media)
			{
				macroKeyDriver.AddMacroKey(KEY_STATE.KeyDown, keyboardCode.hidCodeType, keyboardCode.hidCode, delay);
				LoadMacroKeyContext(macroKeyDriver.macroKey);
			}
			if (((RadioButton)customRadioButton_AutoDelay).Checked)
			{
				stopWatch.Restart();
			}
		}
		e.Handled = false;
	}

	private void KeyboardHook_KeyUp(object sender, KeyEventArgs e, GlobalHook.KeyboardHookStruct hookStruct)
	{
		if (!customRecordMacro1.Checked)
		{
			return;
		}
		if (keyboardCode.vkCode != hookStruct.vkCode || keyboardCode.scanCode != hookStruct.scanCode || keyboardCode.flags != hookStruct.flags)
		{
			keyboardCode = KeyboardCodes.FindKeyboardCode(hookStruct);
			keyboardCode.flags = hookStruct.flags;
			if (((Control)textBox_AutoDelayTime).Text == "")
			{
				((Control)textBox_AutoDelayTime).Text = 10.ToString();
			}
			uint num = Convert.ToUInt32(((Control)textBox_AutoDelayTime).Text);
			num = ((num < 10) ? 10u : num);
			uint delay = (((RadioButton)customRadioButton_AutoDelay).Checked ? StopWatchElapsedMilliseconds((uint)stopWatch.ElapsedMilliseconds) : num);
			if (macroKeyDriver.macroKey.contextCount >= 70)
			{
				if (formDialogMaxMacroKey == null)
				{
					ShowMaxMacroKey();
				}
				else if (((Control)formDialogMaxMacroKey).IsDisposed)
				{
					ShowMaxMacroKey();
				}
			}
			else if (keyboardCode.hidCodeType != HID_CODE_TYPE.Media)
			{
				macroKeyDriver.AddMacroKey(KEY_STATE.KeyUp, keyboardCode.hidCodeType, keyboardCode.hidCode, delay);
				LoadMacroKeyContext(macroKeyDriver.macroKey);
			}
			if (((RadioButton)customRadioButton_AutoDelay).Checked)
			{
				stopWatch.Restart();
			}
		}
		e.Handled = false;
	}

	private void CustomRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		if (((RadioButton)customRadioButton).Checked && !NoChangeSaveVisiable)
		{
			int num = Convert.ToInt32(((Control)customRadioButton).Tag);
			if (num != panel_CycleProcess_Tag && !customRecordMacro1.Checked)
			{
				panel_CycleProcess_Tag = num;
				SetSaveButton(enable: true);
			}
		}
	}

	private void LoadMacroKeyContext(MacroKeyAndCycle macroKeyAndCycle)
	{
		int num = 0;
		listView_Keys.BeginUpdate();
		listView_Keys.Items.Clear();
		for (int i = 0; i < macroKeyAndCycle.macroKey.contextCount; i++)
		{
			bool flag = true;
			int hidKeyCode = macroKeyAndCycle.macroKey.context[i].value[0] | (macroKeyAndCycle.macroKey.context[i].value[1] << 8);
			int imageIndex = 0;
			if (macroKeyAndCycle.macroKey.context[i].keyState == 1)
			{
				imageIndex = 1;
			}
			if (macroKeyAndCycle.macroKey.context[i].type == 1 || macroKeyAndCycle.macroKey.context[i].type == 0)
			{
				KeyboardCode keyboardCode = KeyboardCodes.FindKeyboardCode(hidKeyCode, macroKeyAndCycle.macroKey.context[i].type);
				listView_Keys.Items.Add(keyboardCode.keyChar);
			}
			else if (macroKeyAndCycle.macroKey.context[i].type == 4)
			{
				int num2 = 4 << 16;
				num2 |= macroKeyAndCycle.macroKey.context[i].value[0] << 8;
				num2 |= macroKeyAndCycle.macroKey.context[i].value[1];
				listView_Keys.Items.Add(customComboBox_InsertEvent.Item[ValueConvert.ValueToComboxIndex(customComboBox_InsertEvent, num2)].ToString());
			}
			else if (macroKeyAndCycle.macroKey.context[i].type == 5)
			{
				if (macroKeyAndCycle.macroKey.context[i].value[0] == 0 && macroKeyAndCycle.macroKey.context[i].value[1] == 0)
				{
					flag = false;
				}
				else
				{
					string text = "";
					imageIndex = 3;
					byte b = macroKeyAndCycle.macroKey.context[i].value[0];
					if (b != 0)
					{
						if (b > 127)
						{
							b = (byte)(-b);
							text = LanguageFile.Dialogs[33] + b + " ";
						}
						else
						{
							text = LanguageFile.Dialogs[34] + b + " ";
						}
					}
					b = macroKeyAndCycle.macroKey.context[i].value[1];
					if (b != 0)
					{
						if (b > 127)
						{
							b = (byte)(-b);
							text = text + LanguageFile.Dialogs[35] + b;
						}
						else
						{
							text = text + LanguageFile.Dialogs[36] + b;
						}
					}
					listView_Keys.Items.Add(text);
				}
			}
			if (flag)
			{
				listView_Keys.Items[num++].ImageIndex = imageIndex;
			}
			if (macroKeyAndCycle.macroKey.context[i].delay != 0)
			{
				listView_Keys.Items.Add(macroKeyAndCycle.macroKey.context[i].delay + " ms");
				listView_Keys.Items[num++].ImageIndex = 2;
			}
		}
		listView_Keys.EndUpdate();
		if (listView_Keys.Items.Count > 0)
		{
			listView_Keys.Items[listView_Keys.Items.Count - 1].EnsureVisible();
		}
		NoChangeSaveVisiable = true;
		if (((ListView)customListView_Macro).SelectedItems.Count > 0)
		{
			_ = ((ListView)customListView_Macro).SelectedItems[0].Index;
			if (macroKeyAndCycle.CycleTimes == 0)
			{
				((Control)textBox_CycleTimes).Text = "1";
				((RadioButton)customRadioButton_CycleTimes).Checked = true;
			}
			else if (macroKeyAndCycle.CycleTimes < 251)
			{
				((RadioButton)customRadioButton_CycleTimes).Checked = true;
				((Control)textBox_CycleTimes).Text = macroKeyAndCycle.CycleTimes.ToString();
			}
			else
			{
				((Control)textBox_CycleTimes).Text = "1";
				if (macroKeyAndCycle.CycleTimes == 253)
				{
					((RadioButton)customRadioButton_UntilThisKeyPressed).Checked = true;
				}
				else if (macroKeyAndCycle.CycleTimes == 254)
				{
					((RadioButton)customRadioButton_UntilKeyReleased).Checked = true;
				}
				else
				{
					((RadioButton)customRadioButton_UntilKeyPressed).Checked = true;
				}
			}
		}
		NoChangeSaveVisiable = false;
	}

	private void LoadMacroKeyContext(MacroKey macroKey)
	{
		int num = 0;
		listView_Keys.BeginUpdate();
		listView_Keys.Items.Clear();
		for (int i = 0; i < macroKey.contextCount; i++)
		{
			bool flag = true;
			int hidKeyCode = macroKey.context[i].value[0] | (macroKey.context[i].value[1] << 8);
			int imageIndex = 0;
			if (macroKey.context[i].keyState == 1)
			{
				imageIndex = 1;
			}
			if (macroKey.context[i].type == 1 || macroKey.context[i].type == 0)
			{
				KeyboardCode keyboardCode = KeyboardCodes.FindKeyboardCode(hidKeyCode, macroKey.context[i].type);
				listView_Keys.Items.Add(keyboardCode.keyChar);
			}
			else if (macroKey.context[i].type == 4)
			{
				int num2 = 4 << 16;
				num2 |= macroKey.context[i].value[0] << 8;
				num2 |= macroKey.context[i].value[1];
				listView_Keys.Items.Add(customComboBox_InsertEvent.Item[ValueConvert.ValueToComboxIndex(customComboBox_InsertEvent, num2)].ToString());
			}
			else if (macroKey.context[i].type == 5)
			{
				if (macroKey.context[i].value[0] == 0 && macroKey.context[i].value[1] == 0)
				{
					flag = false;
				}
				else
				{
					string text = "";
					imageIndex = 3;
					byte b = macroKey.context[i].value[0];
					if (macroKey.context[i].value[0] != 0)
					{
						if (macroKey.context[i].value[0] > 127)
						{
							b = (byte)(-b);
							text = LanguageFile.Dialogs[33] + b + " ";
						}
						else
						{
							text = LanguageFile.Dialogs[34] + b + " ";
						}
					}
					b = macroKey.context[i].value[1];
					if (macroKey.context[i].value[1] != 0)
					{
						if (macroKey.context[i].value[1] > 127)
						{
							b = (byte)(-b);
							text = text + LanguageFile.Dialogs[35] + b;
						}
						else
						{
							text = text + LanguageFile.Dialogs[36] + b;
						}
					}
					listView_Keys.Items.Add(text);
				}
			}
			if (flag)
			{
				listView_Keys.Items[num++].ImageIndex = imageIndex;
			}
			if (macroKey.context[i].delay != 0)
			{
				listView_Keys.Items.Add(macroKey.context[i].delay + " ms");
				listView_Keys.Items[num++].ImageIndex = 2;
			}
		}
		listView_Keys.EndUpdate();
		if (listView_Keys.Items.Count > 0)
		{
			listView_Keys.Items[listView_Keys.Items.Count - 1].EnsureVisible();
		}
	}

	private int GetMacroKeyContextIndex(int SelectIndex)
	{
		int num = 0;
		if (SelectIndex == 0)
		{
			return 0;
		}
		for (int i = 0; i <= SelectIndex; i++)
		{
			if (!listView_Keys.Items[i].Text.Contains("ms"))
			{
				num++;
			}
		}
		return num;
	}

	private void ListViewKeysImageInit()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		mediaKeyImageList.Images.Clear();
		mediaKeyImageList.ColorDepth = (ColorDepth)32;
		mediaKeyImageList.ImageSize = new Size(16, 16);
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		Bitmap val = new Bitmap(baseDirectory + "\\res\\4Macro\\key_down.png");
		mediaKeyImageList.Images.Add((Image)(object)val);
		val = new Bitmap(baseDirectory + "\\res\\4Macro\\key_up.png");
		mediaKeyImageList.Images.Add((Image)(object)val);
		val = new Bitmap(baseDirectory + "\\res\\4Macro\\key_time.png");
		mediaKeyImageList.Images.Add((Image)(object)val);
		val = new Bitmap(baseDirectory + "\\res\\4Macro\\key_press_gun.png");
		mediaKeyImageList.Images.Add((Image)(object)val);
		listView_Keys.Clear();
		listView_Keys.SmallImageList = mediaKeyImageList;
		((ListView)customListView_Macro).Clear();
		((ListView)customListView_Macro).SmallImageList = mediaKeyImageList;
	}

	private void customButton_NewMacro_Click(object sender, EventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (IsRecordingMacro())
		{
			return;
		}
		FormEditDialog formEditDialog = new FormEditDialog(keys: new string[2]
		{
			LanguageFile.Dialogs[37],
			LanguageFile.Dialogs[38]
		}, str: LanguageFile.Dialogs[9], type: FormEditDialogType.MacroName);
		((Form)formEditDialog).ShowDialog();
		if (formEditDialog.text != null && formEditDialog.NeedUpdate && !MacroSameNameDialog(formEditDialog.text))
		{
			macroKeyDriver = new MacroKeyDriver();
			((ListView)customListView_Macro).Items.Add(formEditDialog.text);
			((ListView)customListView_Macro).Items[((ListView)customListView_Macro).Items.Count - 1].Selected = true;
			MacroKeyAndCycle item = new MacroKeyAndCycle(0);
			item.name = formEditDialog.text;
			item.macroKey.name = Encoding.Default.GetBytes(formEditDialog.text);
			item.macroKey.nameLength = (byte)item.macroKey.name.Length;
			MacroKeyAndCycleList.Add(item);
			listView_Keys.Items.Clear();
			((Control)textBox_CycleTimes).Text = "1";
			((RadioButton)customRadioButton_CycleTimes).Checked = true;
			languageFile.KeyFunctionSubStructs[6].Clear();
			for (int i = 0; i < ((ListView)customListView_Macro).Items.Count; i++)
			{
				KeyFunctionStruct item2 = new KeyFunctionStruct
				{
					name = ((ListView)customListView_Macro).Items[i].Text
				};
				languageFile.KeyFunctionSubStructs[6].Add(item2);
			}
			customKeyFunction1.UpdateMacro(languageFile, ((ListView)customListView_Macro).SelectedItems[0].Index);
			MacroFile.SaveLocalMacroKey(MacroKeyAndCycleList);
		}
	}

	private void customButton_DeleteMacro_Click(object sender, EventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (!IsRecordingMacro() && ((ListView)customListView_Macro).SelectedItems != null && ((ListView)customListView_Macro).Items.Count > 0)
		{
			FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[17], DialogButtons.OK);
			((Form)formDialog).ShowDialog();
			if (formDialog.resault)
			{
				languageFile.KeyFunctionSubStructs[6].RemoveAt(((ListView)customListView_Macro).SelectedItems[0].Index);
				customKeyFunction1.DeleteMacro(((ListView)customListView_Macro).SelectedItems[0].Index, deviceParam.KeyParams);
				MacroKeyAndCycleList.RemoveAt(((ListView)customListView_Macro).SelectedItems[0].Index);
				listView_Keys.Items.Clear();
				((ListView)customListView_Macro).Items.RemoveAt(((ListView)customListView_Macro).SelectedItems[0].Index);
			}
		}
	}

	private void customButton_ModifyButton_Click(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		if (IsRecordingMacro())
		{
			return;
		}
		if (((ListView)customListView_Macro).SelectedItems.Count == 0)
		{
			customRecordMacro1.Checked = false;
			((Form)new FormDialog(LanguageFile.Dialogs[12])).ShowDialog();
		}
		else
		{
			if (listView_Keys.Items.Count <= 0 || listView_Keys.SelectedItems.Count <= 0)
			{
				return;
			}
			if (listView_Keys.SelectedItems[0].Text.Contains("ms"))
			{
				FormEditDialog formEditDialog = new FormEditDialog(keys: new string[2]
				{
					LanguageFile.Dialogs[37],
					LanguageFile.Dialogs[38]
				}, str: LanguageFile.Dialogs[16], type: FormEditDialogType.Delay);
				((Form)formEditDialog).ShowDialog();
				if (formEditDialog.NeedUpdate)
				{
					int delay = formEditDialog.delay;
					int num = GetMacroKeyContextIndex(listView_Keys.SelectedItems[0].Index) - 1;
					macroKeyDriver.macroKey.context[num].delay = ((delay > 65535) ? 65535u : ((uint)delay));
					LoadMacroKeyContext(macroKeyDriver.macroKey);
					SetSaveButton(enable: true);
				}
				return;
			}
			int num2 = GetMacroKeyContextIndex(listView_Keys.SelectedItems[0].Index) - 1;
			bool flag = false;
			byte[] array = new byte[2];
			if (macroKeyDriver.macroKey.context[num2].type == 1 || macroKeyDriver.macroKey.context[num2].type == 4)
			{
				FormEditDialog formEditDialog2 = new FormEditDialog(keys: new string[2]
				{
					LanguageFile.Dialogs[37],
					LanguageFile.Dialogs[38]
				}, str: LanguageFile.Dialogs[14], type: FormEditDialogType.KeyEvent);
				((Form)formEditDialog2).ShowDialog();
				if (formEditDialog2.NeedUpdate)
				{
					flag = true;
					int hidCode = formEditDialog2.keyboardCode.hidCode;
					array[0] = (byte)hidCode;
					array[1] = (byte)(hidCode >> 8);
					macroKeyDriver.macroKey.context[num2].type = 1;
				}
			}
			else if (macroKeyDriver.macroKey.context[num2].type == 5)
			{
				FormMoveXY formMoveXY = new FormMoveXY(new string[4]
				{
					LanguageFile.Dialogs[35],
					LanguageFile.Dialogs[36],
					LanguageFile.Dialogs[33],
					LanguageFile.Dialogs[34]
				}, new string[2]
				{
					LanguageFile.Dialogs[37],
					LanguageFile.Dialogs[38]
				});
				((Form)formMoveXY).ShowDialog();
				if (formMoveXY.NeedUpdate)
				{
					flag = formMoveXY.NeedUpdate;
					array[0] = (byte)formMoveXY.x;
					array[1] = (byte)formMoveXY.y;
				}
			}
			if (flag)
			{
				macroKeyDriver.macroKey.context[num2].value[0] = array[0];
				macroKeyDriver.macroKey.context[num2].value[1] = array[1];
				LoadMacroKeyContext(macroKeyDriver.macroKey);
				SetSaveButton(enable: true);
			}
		}
	}

	private void customButton_DeleteButton_Click(object sender, EventArgs e)
	{
		if (IsRecordingMacro() || listView_Keys.Items.Count <= 0 || listView_Keys.SelectedItems.Count <= 0 || listView_Keys.SelectedItems == null)
		{
			return;
		}
		int macroKeyContextIndex = GetMacroKeyContextIndex(listView_Keys.SelectedItems[0].Index);
		if (listView_Keys.Items[listView_Keys.SelectedItems[0].Index].Text.Contains("ms"))
		{
			if (macroKeyContextIndex > 0)
			{
				macroKeyDriver.DeletcDelay(macroKeyContextIndex - 1);
			}
		}
		else if (macroKeyDriver.macroKey.context[macroKeyContextIndex].type == 5)
		{
			macroKeyDriver.DeleteMacroKey(macroKeyContextIndex);
			macroKeyDriver.DeleteMacroKey(macroKeyContextIndex + 1);
		}
		else
		{
			macroKeyDriver.DeleteMacroKey(macroKeyContextIndex);
		}
		listView_Keys.Items.RemoveAt(listView_Keys.SelectedItems[0].Index);
		SetSaveButton(enable: true);
	}

	private void customComboBox_InsertEvent_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		if (SetLanguage)
		{
			return;
		}
		if (((ListView)customListView_Macro).SelectedItems.Count == 0)
		{
			customRecordMacro1.Checked = false;
			((Form)new FormDialog(LanguageFile.Dialogs[12])).ShowDialog();
			return;
		}
		if (macroKeyDriver.macroKey.contextCount >= 70)
		{
			ShowMaxMacroKey();
			return;
		}
		int num = 0;
		if (((RadioButton)customRadioButton_DefaultDelay).Checked)
		{
			num = int.Parse(((Control)textBox_AutoDelayTime).Text);
			if (num < 10)
			{
				((Form)new FormDialog(LanguageFile.Dialogs[89])).ShowDialog();
				return;
			}
		}
		uint delay = (((RadioButton)customRadioButton_AutoDelay).Checked ? 10u : Convert.ToUInt32(((Control)textBox_AutoDelayTime).Text));
		bool flag = true;
		KeyboardCode keyboardCode = new KeyboardCode();
		int num2 = ValueConvert.ComboxIndexToValue(customComboBox_InsertEvent);
		byte[] array = new byte[3]
		{
			(byte)(num2 >> 16),
			(byte)(num2 >> 8),
			(byte)num2
		};
		int num3 = array[0] & 0x3F;
		if (num3 == 0)
		{
			FormEditDialog formEditDialog = new FormEditDialog(keys: new string[2]
			{
				LanguageFile.Dialogs[37],
				LanguageFile.Dialogs[38]
			}, str: LanguageFile.Dialogs[14], type: FormEditDialogType.KeyEvent);
			((Form)formEditDialog).ShowDialog();
			flag = formEditDialog.NeedUpdate;
			keyboardCode.hidCode = formEditDialog.keyboardCode.hidCode;
			keyboardCode.hidCodeType = formEditDialog.keyboardCode.hidCodeType;
		}
		else if (array[0] == byte.MaxValue)
		{
			FormEditDialog obj = new FormEditDialog(keys: new string[2]
			{
				LanguageFile.Dialogs[37],
				LanguageFile.Dialogs[38]
			}, str: LanguageFile.Dialogs[16], type: FormEditDialogType.Delay);
			((Form)obj).ShowDialog();
			num = obj.delay;
			flag = obj.NeedUpdate;
		}
		else if (array[0] == 5)
		{
			FormMoveXY formMoveXY = new FormMoveXY(new string[4]
			{
				LanguageFile.Dialogs[35],
				LanguageFile.Dialogs[36],
				LanguageFile.Dialogs[33],
				LanguageFile.Dialogs[34]
			}, new string[2]
			{
				LanguageFile.Dialogs[37],
				LanguageFile.Dialogs[38]
			});
			((Form)formMoveXY).ShowDialog();
			if (formMoveXY.NeedUpdate)
			{
				array[1] = (byte)formMoveXY.x;
				array[2] = (byte)formMoveXY.y;
			}
			flag = formMoveXY.NeedUpdate;
		}
		if (!flag)
		{
			return;
		}
		int selectIndex = ((listView_Keys.SelectedItems.Count == 0) ? (listView_Keys.Items.Count - 1) : listView_Keys.SelectedItems[0].Index);
		int num4 = GetMacroKeyContextIndex(selectIndex);
		if (macroKeyDriver.macroKey.context[num4].type == 5)
		{
			num4++;
		}
		if (num3 == 0)
		{
			KEY_STATE keyState = ((array[0] != 128) ? KEY_STATE.KeyUp : KEY_STATE.KeyDown);
			macroKeyDriver.InsertMacroKey(keyState, keyboardCode.hidCodeType, keyboardCode.hidCode, delay, num4);
		}
		else if (array[0] == byte.MaxValue)
		{
			num4 = ((num4 != 0) ? (num4 - 1) : 0);
			macroKeyDriver.InsertDelay((uint)num, num4);
		}
		else
		{
			switch (num3)
			{
			case 4:
			{
				byte[] value2 = new byte[2]
				{
					array[1],
					array[2]
				};
				macroKeyDriver.InsertMouseKey(value2, num4, delay);
				break;
			}
			case 5:
			{
				byte[] value = new byte[2]
				{
					array[1],
					array[2]
				};
				macroKeyDriver.InsertMoveXY(value, num4);
				break;
			}
			}
		}
		LoadMacroKeyContext(macroKeyDriver.macroKey);
		SetSaveButton(enable: true);
	}

	private void customListView_Macro_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Invalid comparison between Unknown and I4
		if (((ListView)customListView_Macro).SelectedItems.Count > 0)
		{
			SetSaveButton(enable: false);
			int index = ((ListView)customListView_Macro).SelectedItems[0].Index;
			if (index < MacroKeyAndCycleList.Count)
			{
				macroKeyDriver = new MacroKeyDriver();
				currentTimes = MacroKeyAndCycleList[index].CycleTimes;
				macroKeyDriver.macroKey.name = MacroKeyAndCycleList[index].macroKey.name;
				macroKeyDriver.macroKey.nameLength = MacroKeyAndCycleList[index].macroKey.nameLength;
				macroKeyDriver.macroKey.contextCount = MacroKeyAndCycleList[index].macroKey.contextCount;
				for (int i = 0; i < macroKeyDriver.macroKey.contextCount; i++)
				{
					macroKeyDriver.macroKey.context[i].delay = MacroKeyAndCycleList[index].macroKey.context[i].delay;
					macroKeyDriver.macroKey.context[i].type = MacroKeyAndCycleList[index].macroKey.context[i].type;
					macroKeyDriver.macroKey.context[i].keyState = MacroKeyAndCycleList[index].macroKey.context[i].keyState;
					macroKeyDriver.macroKey.context[i].value[0] = MacroKeyAndCycleList[index].macroKey.context[i].value[0];
					macroKeyDriver.macroKey.context[i].value[1] = MacroKeyAndCycleList[index].macroKey.context[i].value[1];
				}
				LoadMacroKeyContext(MacroKeyAndCycleList[index]);
			}
			else
			{
				macroKeyDriver = new MacroKeyDriver();
			}
			if ((int)Control.MouseButtons == 2097152 && !customRecordMacro1.Checked)
			{
				((Control)customListView_Macro).ContextMenuStrip = contextMenuStrip_SelectMacro;
			}
			else
			{
				((Control)customListView_Macro).ContextMenuStrip = null;
			}
		}
		else
		{
			listView_Keys.Items.Clear();
			SetSaveButton(enable: false);
			((Control)customListView_Macro).ContextMenuStrip = contextMenuStrip_UnselectMacro;
		}
	}

	private void customListView_Macro_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (((ListView)customListView_Macro).SelectedItems.Count > 0 && (int)Control.MouseButtons == 2097152 && !customRecordMacro1.Checked)
		{
			((Control)customListView_Macro).ContextMenuStrip = contextMenuStrip_SelectMacro;
		}
		else
		{
			((Control)customListView_Macro).ContextMenuStrip = contextMenuStrip_UnselectMacro;
		}
	}

	private void listView_Keys_SelectedIndexChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (listView_Keys.SelectedItems.Count > 0 && (int)Control.MouseButtons == 2097152 && !customRecordMacro1.Checked)
		{
			((Control)listView_Keys).ContextMenuStrip = contextMenuStrip_Keys;
		}
		else
		{
			((Control)listView_Keys).ContextMenuStrip = null;
		}
	}

	private void listView_Keys_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (listView_Keys.SelectedItems.Count > 0 && (int)Control.MouseButtons == 2097152 && !customRecordMacro1.Checked)
		{
			((Control)listView_Keys).ContextMenuStrip = contextMenuStrip_Keys;
		}
		else
		{
			((Control)listView_Keys).ContextMenuStrip = null;
		}
	}

	private void customRecordMacro1_Click(object sender, EventArgs e)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (((ListView)customListView_Macro).SelectedItems.Count == 0)
		{
			customRecordMacro1.Checked = false;
			((Form)new FormDialog(LanguageFile.Dialogs[12])).ShowDialog();
			return;
		}
		if (((RadioButton)customRadioButton_DefaultDelay).Checked && int.Parse(((Control)textBox_AutoDelayTime).Text) < 10)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[89])).ShowDialog();
			return;
		}
		customRecordMacro1.Checked = !customRecordMacro1.Checked;
		((Control)textBox_CycleTimes).Visible = !customRecordMacro1.Checked;
		if (!customRecordMacro1.Checked)
		{
			((Control)customTabSelector_Main).Enabled = true;
			SetSaveButton(enable: true);
			((Control)textBox_AutoDelayTime).Visible = true;
			((Control)customListView_Macro).Enabled = true;
			keyboardHook.Stop();
		}
		else
		{
			listView_Keys.Items.Clear();
			((Control)customListView_Macro).Enabled = false;
			((Control)textBox_AutoDelayTime).Visible = false;
			((Control)customTabSelector_Main).Enabled = false;
			macroKeyDriver = new MacroKeyDriver();
			keyboardHook.Start(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]));
		}
	}

	private void customButton_Save_Click(object sender, EventArgs e)
	{
		if (IsRecordingMacro())
		{
			return;
		}
		MacroKeyAndCycle OutParam = new MacroKeyAndCycle(0);
		byte cycleTimes = 0;
		if (((RadioButton)customRadioButton_CycleTimes).Checked)
		{
			if (((Control)textBox_CycleTimes).Text == "" || ((Control)textBox_CycleTimes).Text == "0")
			{
				((Control)textBox_CycleTimes).Text = 1.ToString();
			}
			cycleTimes = Convert.ToByte(((Control)textBox_CycleTimes).Text);
		}
		else if (((RadioButton)customRadioButton_UntilKeyPressed).Checked)
		{
			cycleTimes = byte.MaxValue;
		}
		else if (((RadioButton)customRadioButton_UntilKeyReleased).Checked)
		{
			cycleTimes = 254;
		}
		else if (((RadioButton)customRadioButton_UntilThisKeyPressed).Checked)
		{
			cycleTimes = 253;
		}
		MacroKeyDriver.Copy(macroKeyDriver.macroKey, ref OutParam, ((ListView)customListView_Macro).SelectedItems[0].Text, cycleTimes);
		MacroKeyAndCycleList.RemoveAt(((ListView)customListView_Macro).SelectedItems[0].Index);
		MacroKeyAndCycleList.Insert(((ListView)customListView_Macro).SelectedItems[0].Index, OutParam);
		LoadMacroKeyContext(OutParam);
		SetSaveButton(enable: false);
		languageFile.KeyFunctionSubStructs[6].Clear();
		for (int i = 0; i < ((ListView)customListView_Macro).Items.Count; i++)
		{
			KeyFunctionStruct item = new KeyFunctionStruct
			{
				name = ((ListView)customListView_Macro).Items[i].Text
			};
			languageFile.KeyFunctionSubStructs[6].Add(item);
		}
		customKeyFunction1.UpdateMacro(languageFile, ((ListView)customListView_Macro).SelectedItems[0].Index);
		MacroFile.SaveLocalMacroKey(MacroKeyAndCycleList);
	}

	private void customKeyFunction1_UpdateMacroUI(object sender, MacroKeyAndCycle macroKeyAndCycle)
	{
		((ListView)customListView_Macro).Items.Clear();
		languageFile.KeyFunctionSubStructs[6].Clear();
		for (int i = 0; i < MacroKeyAndCycleList.Count; i++)
		{
			KeyFunctionStruct item = default(KeyFunctionStruct);
			((ListView)customListView_Macro).Items.Add(MacroKeyAndCycleList[i].name);
			item.name = ((ListView)customListView_Macro).Items[i].Text;
			languageFile.KeyFunctionSubStructs[6].Add(item);
		}
	}

	private void ToolStripMenuItem_ExportMacro_Click(object sender, EventArgs e)
	{
		string text = "Compx Inc ";
		bool flag = false;
		for (int i = 0; i < MacroKeyAndCycleList[((ListView)customListView_Macro).SelectedItems[0].Index].macroKey.contextCount; i++)
		{
			if (MacroKeyAndCycleList[((ListView)customListView_Macro).SelectedItems[0].Index].macroKey.context[i].type == 5)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			text += "Move-XY";
		}
		string text2 = ((ListView)customListView_Macro).Items[((ListView)customListView_Macro).SelectedItems[0].Index].Text;
		MacroFile.ExportMacroFile(MacroKeyAndCycleList[((ListView)customListView_Macro).SelectedItems[0].Index], text2, text);
	}

	private void ToolStripMenuItem_ImportMacro_Click(object sender, EventArgs e)
	{
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		string text = "Compx Inc ";
		int index = 0;
		for (int i = 0; i < LanguageFile.ComboBoxStructs.Count; i++)
		{
			if (((Control)customComboBox_InsertEvent).Name == LanguageFile.ComboBoxStructs[i].Name)
			{
				index = i;
				break;
			}
		}
		for (int j = 0; j < customComboBox_InsertEvent.Item.Count; j++)
		{
			if (LanguageFile.ComboBoxStructs[index].Values[j] == 327680)
			{
				text += "Move-XY";
				break;
			}
		}
		MacroKeyAndCycle OutMacroKeyAndCycle = new MacroKeyAndCycle(0);
		switch (MacroFile.ImportMacroFile(ref OutMacroKeyAndCycle, text))
		{
		case 0:
		{
			string name = OutMacroKeyAndCycle.name;
			if (!MacroSameNameDialog(name))
			{
				((ListView)customListView_Macro).Items.Add(name);
				MacroKeyAndCycleList.Add(OutMacroKeyAndCycle);
				KeyFunctionStruct item = new KeyFunctionStruct
				{
					name = name
				};
				languageFile.KeyFunctionSubStructs[6].Add(item);
				LoadMacroKeyContext(OutMacroKeyAndCycle);
			}
			break;
		}
		case 1:
			((Form)new FormDialog(LanguageFile.Dialogs[8], DialogButtons.OK)).ShowDialog();
			break;
		}
	}

	private void ToolStripMenuItem_Rename_Click(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		FormEditDialog formEditDialog = new FormEditDialog(keys: new string[2]
		{
			LanguageFile.Dialogs[37],
			LanguageFile.Dialogs[38]
		}, str: LanguageFile.Dialogs[9], type: FormEditDialogType.MacroName);
		((Form)formEditDialog).ShowDialog();
		if (formEditDialog.text != null && formEditDialog.NeedUpdate && !MacroSameNameDialog(formEditDialog.text))
		{
			((ListView)customListView_Macro).Items[((ListView)customListView_Macro).SelectedItems[0].Index].Text = formEditDialog.text;
			int index = ((ListView)customListView_Macro).SelectedItems[0].Index;
			MacroKeyAndCycle OutParam = new MacroKeyAndCycle(0);
			MacroKeyDriver.Copy(MacroKeyAndCycleList[index].macroKey, ref OutParam, formEditDialog.text, MacroKeyAndCycleList[index].CycleTimes);
			MacroKeyAndCycleList.RemoveAt(index);
			MacroKeyAndCycleList.Insert(index, OutParam);
			languageFile.KeyFunctionSubStructs[6].Clear();
			for (int i = 0; i < ((ListView)customListView_Macro).Items.Count; i++)
			{
				KeyFunctionStruct item = new KeyFunctionStruct
				{
					name = ((ListView)customListView_Macro).Items[i].Text
				};
				languageFile.KeyFunctionSubStructs[6].Add(item);
			}
			customKeyFunction1.UpdateMacro(languageFile, index);
		}
	}

	private bool IsRecordingMacro()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (customRecordMacro1.Checked)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[13])).ShowDialog();
		}
		return customRecordMacro1.Checked;
	}

	private bool MacroSameNameDialog(string name)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ((ListView)customListView_Macro).Items.Count; i++)
		{
			if (name == ((ListView)customListView_Macro).Items[i].Text)
			{
				((Form)new FormDialog(LanguageFile.Dialogs[10])).ShowDialog();
				return true;
			}
		}
		return false;
	}

	private void SetSaveButton(bool enable)
	{
		((Control)customButton_Save).Enabled = enable;
		((Control)customButton_Save).ForeColor = (enable ? Color.White : Color.FromArgb(157, 157, 157));
		customButton_Save.NormalImage = (enable ? customButton_Save.MouseDownImage : btnSaveNormal);
	}

	private void textBox_AutoDelayTime_TextChanged(object sender, EventArgs e)
	{
		if (!(((Control)textBox_AutoDelayTime).Text == "") && int.Parse(((Control)textBox_AutoDelayTime).Text) > 65535)
		{
			((Control)textBox_AutoDelayTime).Text = "65535";
		}
	}

	private void textBox_AutoDelayTime_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void textBox_CycleTimes_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void textBox_CycleTimes_TextChanged(object sender, EventArgs e)
	{
		if (((Control)textBox_CycleTimes).Text == "")
		{
			return;
		}
		int num = int.Parse(((Control)textBox_CycleTimes).Text);
		if (MacroKeyAndCycleList[((ListView)customListView_Macro).SelectedItems[0].Index].CycleTimes < 250 && num != MacroKeyAndCycleList[((ListView)customListView_Macro).SelectedItems[0].Index].CycleTimes)
		{
			if (num > 250)
			{
				((Control)textBox_CycleTimes).Text = "250";
			}
			SetSaveButton(enable: true);
		}
	}

	private void customButton17_Click(object sender, EventArgs e)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		WebClient webClient = new WebClient();
		try
		{
			webClient.Credentials = CredentialCache.DefaultCredentials;
			webClient.DownloadFile(((Control)textBox_url).Text, "F:\\test\\" + ((Control)textBox_fileName).Text);
		}
		catch
		{
		}
	}

	private void FormMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32 && !customRecordMacro1.Checked)
		{
			customSetting1.EnterPair();
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Expected O, but got Unknown
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Expected O, but got Unknown
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Expected O, but got Unknown
		//IL_1135: Unknown result type (might be due to invalid IL or missing references)
		//IL_113f: Expected O, but got Unknown
		//IL_13fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cd: Expected O, but got Unknown
		//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ee: Expected O, but got Unknown
		//IL_171b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1725: Expected O, but got Unknown
		//IL_1736: Unknown result type (might be due to invalid IL or missing references)
		//IL_1740: Expected O, but got Unknown
		//IL_1751: Unknown result type (might be due to invalid IL or missing references)
		//IL_175b: Expected O, but got Unknown
		//IL_1782: Unknown result type (might be due to invalid IL or missing references)
		//IL_178c: Expected O, but got Unknown
		//IL_1796: Unknown result type (might be due to invalid IL or missing references)
		//IL_1823: Unknown result type (might be due to invalid IL or missing references)
		//IL_182d: Expected O, but got Unknown
		//IL_184b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1855: Expected O, but got Unknown
		//IL_1a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1c: Expected O, but got Unknown
		//IL_1af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b01: Expected O, but got Unknown
		//IL_27a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2804: Unknown result type (might be due to invalid IL or missing references)
		//IL_280e: Expected O, but got Unknown
		//IL_2825: Unknown result type (might be due to invalid IL or missing references)
		//IL_282f: Expected O, but got Unknown
		//IL_2850: Unknown result type (might be due to invalid IL or missing references)
		//IL_285a: Expected O, but got Unknown
		//IL_286b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2875: Expected O, but got Unknown
		//IL_2895: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ca: Expected O, but got Unknown
		//IL_28db: Unknown result type (might be due to invalid IL or missing references)
		//IL_28e5: Expected O, but got Unknown
		//IL_291d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2927: Expected O, but got Unknown
		//IL_2938: Unknown result type (might be due to invalid IL or missing references)
		//IL_2942: Expected O, but got Unknown
		//IL_2963: Unknown result type (might be due to invalid IL or missing references)
		//IL_296d: Expected O, but got Unknown
		//IL_297e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2988: Expected O, but got Unknown
		//IL_2999: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a3: Expected O, but got Unknown
		//IL_29b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29be: Expected O, but got Unknown
		//IL_29cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d9: Expected O, but got Unknown
		//IL_29f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_29fa: Expected O, but got Unknown
		//IL_2a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aab: Expected O, but got Unknown
		//IL_2ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad3: Expected O, but got Unknown
		//IL_2d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f6: Expected O, but got Unknown
		//IL_37b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_37bd: Expected O, but got Unknown
		//IL_37ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3809: Expected O, but got Unknown
		//IL_4056: Unknown result type (might be due to invalid IL or missing references)
		//IL_4060: Expected O, but got Unknown
		//IL_4158: Unknown result type (might be due to invalid IL or missing references)
		//IL_4162: Expected O, but got Unknown
		//IL_4185: Unknown result type (might be due to invalid IL or missing references)
		//IL_418f: Expected O, but got Unknown
		//IL_4225: Unknown result type (might be due to invalid IL or missing references)
		//IL_422f: Expected O, but got Unknown
		//IL_43f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_46bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c6: Expected O, but got Unknown
		//IL_46ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_46f7: Expected O, but got Unknown
		//IL_4728: Unknown result type (might be due to invalid IL or missing references)
		//IL_478f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4799: Expected O, but got Unknown
		//IL_47de: Unknown result type (might be due to invalid IL or missing references)
		//IL_47e8: Expected O, but got Unknown
		//IL_480f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4819: Expected O, but got Unknown
		//IL_483a: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_48ab: Expected O, but got Unknown
		//IL_48ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_48f7: Expected O, but got Unknown
		//IL_491e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4928: Expected O, but got Unknown
		//IL_4949: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ba: Expected O, but got Unknown
		//IL_4a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a1b: Expected O, but got Unknown
		//IL_4aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ab0: Expected O, but got Unknown
		//IL_4b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b45: Expected O, but got Unknown
		//IL_544d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5457: Expected O, but got Unknown
		//IL_54e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f0: Expected O, but got Unknown
		//IL_557c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5586: Expected O, but got Unknown
		//IL_5638: Unknown result type (might be due to invalid IL or missing references)
		//IL_5642: Expected O, but got Unknown
		//IL_5af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5afe: Expected O, but got Unknown
		//IL_5c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c8b: Expected O, but got Unknown
		//IL_5d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d1c: Expected O, but got Unknown
		//IL_5daf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db9: Expected O, but got Unknown
		//IL_5e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e53: Expected O, but got Unknown
		//IL_5ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eed: Expected O, but got Unknown
		//IL_5fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_6031: Unknown result type (might be due to invalid IL or missing references)
		//IL_603b: Expected O, but got Unknown
		//IL_6073: Unknown result type (might be due to invalid IL or missing references)
		//IL_61da: Unknown result type (might be due to invalid IL or missing references)
		//IL_61e4: Expected O, but got Unknown
		//IL_61f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6201: Expected O, but got Unknown
		//IL_620d: Unknown result type (might be due to invalid IL or missing references)
		//IL_623c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6246: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMain));
		label_Title = new Label();
		notifyIcon_Main = new NotifyIcon(components);
		contextMenuStrip_Pallet = new ContextMenuStrip(components);
		ToolStripMenuItem_OpenMenu = new ToolStripMenuItem();
		ToolStripMenuItem_Exit = new ToolStripMenuItem();
		contextMenuStrip_Keys = new ContextMenuStrip(components);
		ToolStripMenuItem_Modify = new ToolStripMenuItem();
		ToolStripMenuItem_DeleteKeys = new ToolStripMenuItem();
		contextMenuStrip_SelectMacro = new ContextMenuStrip(components);
		ToolStripMenuItem_ExportMacro = new ToolStripMenuItem();
		ToolStripMenuItem_Rename = new ToolStripMenuItem();
		ToolStripMenuItem_DeleteMacro = new ToolStripMenuItem();
		contextMenuStrip_UnselectMacro = new ContextMenuStrip(components);
		ToolStripMenuItem_NewMacro = new ToolStripMenuItem();
		ToolStripMenuItem_ImportMacro = new ToolStripMenuItem();
		label_BatteryValue = new Label();
		pictureBox1 = new PictureBox();
		customButton_HomePage = new CustomButton();
		customButton_SystemMouse = new CustomButton();
		customBattery1 = new CustomBattery();
		customButton_Setting = new CustomButton();
		customButton_Mini = new CustomButton();
		customButton_Close = new CustomButton();
		customTabSelector_Main = new CustomTabSelector();
		customTabControl_Main = new CustomTabControl();
		tabPage_Button = new TabPage();
		customButton_Reset = new CustomButton();
		textBox_fileName = new TextBox();
		textBox_url = new TextBox();
		customButton17 = new CustomButton();
		customKeyFunction1 = new CustomKeyFunction();
		customButton_Export = new CustomButton();
		customButton_Import = new CustomButton();
		customComboBox_Config = new CustomComboBox();
		panel_Device = new Panel();
		customButton16 = new CustomButton();
		customButton15 = new CustomButton();
		customButton14 = new CustomButton();
		customButton13 = new CustomButton();
		customButton12 = new CustomButton();
		customButton11 = new CustomButton();
		customButton10 = new CustomButton();
		customButton9 = new CustomButton();
		customButton8 = new CustomButton();
		customButton7 = new CustomButton();
		customButton6 = new CustomButton();
		customButton5 = new CustomButton();
		customButton4 = new CustomButton();
		customButton3 = new CustomButton();
		customButton1 = new CustomButton();
		customButton2 = new CustomButton();
		pictureBox_Config = new PictureBox();
		tabPage_Sensor = new TabPage();
		_3In1_Main = new _3In1();
		dpiControl1 = new DPIControl();
		tabPage_Macro = new TabPage();
		label_UntilThisKeyPressed = new Label();
		panel_Delay = new Panel();
		customRadioButton_AutoDelay = new CustomRadioButton();
		customRadioButton_DefaultDelay = new CustomRadioButton();
		label_DefaultDelay = new Label();
		panel_CycleProcess = new Panel();
		customRadioButton_UntilThisKeyPressed = new CustomRadioButton();
		customRadioButton_UntilKeyReleased = new CustomRadioButton();
		customRadioButton_UntilKeyPressed = new CustomRadioButton();
		customRadioButton_CycleTimes = new CustomRadioButton();
		textBox_CycleTimes = new TextBox();
		textBox_AutoDelayTime = new TextBox();
		customComboBox_InsertEvent = new CustomComboBox();
		customRecordMacro1 = new CustomRecordMacro();
		customButton_Save = new CustomButton();
		customButton_DeleteButton = new CustomButton();
		customButton_ModifyButton = new CustomButton();
		customButton_DeleteMacro = new CustomButton();
		customButton_NewMacro = new CustomButton();
		label_CycleTimes = new Label();
		label_UntilKeyPressed = new Label();
		label_UntilKeyReleased = new Label();
		label_InsertEvent = new Label();
		label_AutoDelay = new Label();
		listView_Keys = new ListView();
		customListView_Macro = new CustomListView();
		label_KeyList = new Label();
		label_MacroList = new Label();
		tabPage_Light = new TabPage();
		panel_LEDColor = new Panel();
		adjustControl_B = new AdjustControl();
		adjustControl_G = new AdjustControl();
		adjustControl_R = new AdjustControl();
		label_LEDB = new Label();
		label_LEDG = new Label();
		label_LEDR = new Label();
		pictureBox_Color14 = new PictureBox();
		pictureBox_Color7 = new PictureBox();
		pictureBox_Color13 = new PictureBox();
		pictureBox_Color12 = new PictureBox();
		pictureBox_Color11 = new PictureBox();
		pictureBox_Color6 = new PictureBox();
		pictureBox_Color5 = new PictureBox();
		pictureBox_Color4 = new PictureBox();
		pictureBox_Color10 = new PictureBox();
		pictureBox_Color9 = new PictureBox();
		pictureBox_Color8 = new PictureBox();
		pictureBox_Color3 = new PictureBox();
		pictureBox_Color2 = new PictureBox();
		pictureBox_Color1 = new PictureBox();
		pictureBox_LEDPreview = new PictureBox();
		pictureBox_Palette = new PictureBox();
		label_LEDPreset = new Label();
		label_LEDPreview = new Label();
		label_LEDColor = new Label();
		customComboBox_PowerSaveTime = new CustomComboBox();
		customTrackBar_LightSpeed = new CustomTrackBar();
		label_LightSpeedValue = new Label();
		customTrackBar_LightBrightness = new CustomTrackBar();
		label_LightBrightnessValue = new Label();
		customComboBox_LEDMode = new CustomComboBox();
		customCheckBox_MovingCloseLight = new CustomCheckBox();
		label_MovingCloseLight = new Label();
		label_PowerSaveTime = new Label();
		label_LightSpeed = new Label();
		label_LightBrightness = new Label();
		label_LightMode = new Label();
		tabPage_Setting = new TabPage();
		customSetting1 = new CustomSetting();
		toolTip = new ToolTip(components);
		((Control)contextMenuStrip_Pallet).SuspendLayout();
		((Control)contextMenuStrip_Keys).SuspendLayout();
		((Control)contextMenuStrip_SelectMacro).SuspendLayout();
		((Control)contextMenuStrip_UnselectMacro).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)customTabControl_Main).SuspendLayout();
		((Control)tabPage_Button).SuspendLayout();
		((Control)panel_Device).SuspendLayout();
		((ISupportInitialize)pictureBox_Config).BeginInit();
		((Control)tabPage_Sensor).SuspendLayout();
		((Control)tabPage_Macro).SuspendLayout();
		((Control)panel_Delay).SuspendLayout();
		((Control)panel_CycleProcess).SuspendLayout();
		((Control)tabPage_Light).SuspendLayout();
		((Control)panel_LEDColor).SuspendLayout();
		((ISupportInitialize)pictureBox_Color14).BeginInit();
		((ISupportInitialize)pictureBox_Color7).BeginInit();
		((ISupportInitialize)pictureBox_Color13).BeginInit();
		((ISupportInitialize)pictureBox_Color12).BeginInit();
		((ISupportInitialize)pictureBox_Color11).BeginInit();
		((ISupportInitialize)pictureBox_Color6).BeginInit();
		((ISupportInitialize)pictureBox_Color5).BeginInit();
		((ISupportInitialize)pictureBox_Color4).BeginInit();
		((ISupportInitialize)pictureBox_Color10).BeginInit();
		((ISupportInitialize)pictureBox_Color9).BeginInit();
		((ISupportInitialize)pictureBox_Color8).BeginInit();
		((ISupportInitialize)pictureBox_Color3).BeginInit();
		((ISupportInitialize)pictureBox_Color2).BeginInit();
		((ISupportInitialize)pictureBox_Color1).BeginInit();
		((ISupportInitialize)pictureBox_LEDPreview).BeginInit();
		((ISupportInitialize)pictureBox_Palette).BeginInit();
		((Control)tabPage_Setting).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)label_Title).AutoSize = true;
		((Control)label_Title).BackColor = Color.Transparent;
		((Control)label_Title).Font = new Font("微软雅黑", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Title).ForeColor = Color.White;
		((Control)label_Title).Location = new Point(221, 21);
		((Control)label_Title).Name = "label_Title";
		((Control)label_Title).Size = new Size(225, 28);
		((Control)label_Title).TabIndex = 7;
		((Control)label_Title).Text = "G430 wireless mouse";
		notifyIcon_Main.ContextMenuStrip = contextMenuStrip_Pallet;
		notifyIcon_Main.Icon = (Icon)componentResourceManager.GetObject("notifyIcon_Main.Icon");
		notifyIcon_Main.Text = "Mouse Deive Beta";
		notifyIcon_Main.Visible = true;
		notifyIcon_Main.MouseClick += new MouseEventHandler(notifyIcon_Main_MouseClick);
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
		((ToolStripItem)ToolStripMenuItem_Exit).Click += System_Exit;
		((ToolStrip)contextMenuStrip_Keys).ImageScalingSize = new Size(18, 18);
		((ToolStrip)contextMenuStrip_Keys).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)ToolStripMenuItem_Modify,
			(ToolStripItem)ToolStripMenuItem_DeleteKeys
		});
		((Control)contextMenuStrip_Keys).Name = "contextMenuStrip_Keys";
		((Control)contextMenuStrip_Keys).Size = new Size(101, 48);
		((ToolStripItem)ToolStripMenuItem_Modify).Name = "ToolStripMenuItem_Modify";
		((ToolStripItem)ToolStripMenuItem_Modify).Size = new Size(100, 22);
		((ToolStripItem)ToolStripMenuItem_Modify).Text = "修改";
		((ToolStripItem)ToolStripMenuItem_Modify).Click += customButton_ModifyButton_Click;
		((ToolStripItem)ToolStripMenuItem_DeleteKeys).Name = "ToolStripMenuItem_DeleteKeys";
		((ToolStripItem)ToolStripMenuItem_DeleteKeys).Size = new Size(100, 22);
		((ToolStripItem)ToolStripMenuItem_DeleteKeys).Text = "删除";
		((ToolStripItem)ToolStripMenuItem_DeleteKeys).Click += customButton_DeleteButton_Click;
		((ToolStrip)contextMenuStrip_SelectMacro).ImageScalingSize = new Size(18, 18);
		((ToolStrip)contextMenuStrip_SelectMacro).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[3]
		{
			(ToolStripItem)ToolStripMenuItem_ExportMacro,
			(ToolStripItem)ToolStripMenuItem_Rename,
			(ToolStripItem)ToolStripMenuItem_DeleteMacro
		});
		((Control)contextMenuStrip_SelectMacro).Name = "contextMenuStrip_SelectMacro";
		((Control)contextMenuStrip_SelectMacro).Size = new Size(125, 70);
		((ToolStripItem)ToolStripMenuItem_ExportMacro).Name = "ToolStripMenuItem_ExportMacro";
		((ToolStripItem)ToolStripMenuItem_ExportMacro).Size = new Size(124, 22);
		((ToolStripItem)ToolStripMenuItem_ExportMacro).Text = "导出该宏";
		((ToolStripItem)ToolStripMenuItem_ExportMacro).Click += ToolStripMenuItem_ExportMacro_Click;
		((ToolStripItem)ToolStripMenuItem_Rename).Name = "ToolStripMenuItem_Rename";
		((ToolStripItem)ToolStripMenuItem_Rename).Size = new Size(124, 22);
		((ToolStripItem)ToolStripMenuItem_Rename).Text = "重命名";
		((ToolStripItem)ToolStripMenuItem_Rename).Click += ToolStripMenuItem_Rename_Click;
		((ToolStripItem)ToolStripMenuItem_DeleteMacro).Name = "ToolStripMenuItem_DeleteMacro";
		((ToolStripItem)ToolStripMenuItem_DeleteMacro).Size = new Size(124, 22);
		((ToolStripItem)ToolStripMenuItem_DeleteMacro).Text = "删除";
		((ToolStripItem)ToolStripMenuItem_DeleteMacro).Click += customButton_DeleteMacro_Click;
		((ToolStrip)contextMenuStrip_UnselectMacro).ImageScalingSize = new Size(18, 18);
		((ToolStrip)contextMenuStrip_UnselectMacro).Items.AddRange((ToolStripItem[])(object)new ToolStripItem[2]
		{
			(ToolStripItem)ToolStripMenuItem_NewMacro,
			(ToolStripItem)ToolStripMenuItem_ImportMacro
		});
		((Control)contextMenuStrip_UnselectMacro).Name = "contextMenuStrip_UnselectMacro";
		((Control)contextMenuStrip_UnselectMacro).Size = new Size(113, 48);
		((ToolStripItem)ToolStripMenuItem_NewMacro).Name = "ToolStripMenuItem_NewMacro";
		((ToolStripItem)ToolStripMenuItem_NewMacro).Size = new Size(112, 22);
		((ToolStripItem)ToolStripMenuItem_NewMacro).Text = "新建宏";
		((ToolStripItem)ToolStripMenuItem_NewMacro).Click += customButton_NewMacro_Click;
		((ToolStripItem)ToolStripMenuItem_ImportMacro).Name = "ToolStripMenuItem_ImportMacro";
		((ToolStripItem)ToolStripMenuItem_ImportMacro).Size = new Size(112, 22);
		((ToolStripItem)ToolStripMenuItem_ImportMacro).Text = "导入宏";
		((ToolStripItem)ToolStripMenuItem_ImportMacro).Click += ToolStripMenuItem_ImportMacro_Click;
		((Control)label_BatteryValue).AutoSize = true;
		((Control)label_BatteryValue).BackColor = Color.Transparent;
		((Control)label_BatteryValue).ForeColor = Color.White;
		((Control)label_BatteryValue).Location = new Point(904, 64);
		((Control)label_BatteryValue).Name = "label_BatteryValue";
		((Control)label_BatteryValue).Size = new Size(45, 20);
		((Control)label_BatteryValue).TabIndex = 12;
		((Control)label_BatteryValue).Text = "100%";
		((Control)label_BatteryValue).Visible = false;
		((Control)pictureBox1).BackColor = Color.Transparent;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.Demo;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox1).Location = new Point(466, 8);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(99, 50);
		pictureBox1.TabIndex = 14;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Visible = false;
		((Control)customButton_HomePage).Location = new Point(38, 130);
		customButton_HomePage.MouseDownImage = (Image)(object)Resources.退出按键按下;
		customButton_HomePage.MouseEnterImage = (Image)(object)Resources.退出按键鼠标进入;
		((Control)customButton_HomePage).Name = "customButton_HomePage";
		customButton_HomePage.NormalImage = (Image)(object)Resources.退出按键;
		((Control)customButton_HomePage).Size = new Size(34, 30);
		((Control)customButton_HomePage).TabIndex = 13;
		((Control)customButton_HomePage).Click += customButton_HomePage_Click;
		((Control)customButton_SystemMouse).Location = new Point(38, 610);
		customButton_SystemMouse.MouseDownImage = (Image)(object)Resources.系统鼠标按键按下;
		customButton_SystemMouse.MouseEnterImage = (Image)(object)Resources.系统鼠标按键鼠标进入;
		((Control)customButton_SystemMouse).Name = "customButton_SystemMouse";
		customButton_SystemMouse.NormalImage = (Image)(object)Resources.系统鼠标按键;
		((Control)customButton_SystemMouse).Size = new Size(46, 46);
		((Control)customButton_SystemMouse).TabIndex = 9;
		((Control)customButton_SystemMouse).Visible = false;
		((Control)customButton_SystemMouse).Click += customButton_SystemMouse_Click;
		customBattery1.BatteryCharging = false;
		customBattery1.BatteryLevel = 19;
		customBattery1.ChargingImage = (Image)(object)Resources.电池充电图标;
		customBattery1.EmptyBatteryImage = (Image)(object)Resources.空电量;
		customBattery1.FullBatteryImage = (Image)(object)Resources.满电量;
		((Control)customBattery1).Location = new Point(950, 64);
		customBattery1.LowBatteryImage = (Image)(object)Resources.低电量;
		customBattery1.LowBatteryLevel = 15;
		((Control)customBattery1).Name = "customBattery1";
		((Control)customBattery1).Size = new Size(42, 20);
		((Control)customBattery1).TabIndex = 6;
		((Control)customBattery1).Text = "customBattery1";
		((Control)customBattery1).Visible = false;
		((Control)customButton_Setting).Location = new Point(906, 12);
		customButton_Setting.MouseDownImage = (Image)(object)Resources.设置按键按下;
		customButton_Setting.MouseEnterImage = (Image)(object)Resources.设置按键鼠标进入;
		((Control)customButton_Setting).Name = "customButton_Setting";
		customButton_Setting.NormalImage = (Image)(object)Resources.设置按键;
		((Control)customButton_Setting).Size = new Size(22, 22);
		((Control)customButton_Setting).TabIndex = 5;
		((Control)customButton_Setting).Visible = false;
		((Control)customButton_Setting).Click += customButton_Setting_Click;
		((Control)customButton_Mini).Location = new Point(942, 12);
		customButton_Mini.MouseDownImage = (Image)(object)Resources.最小化按键按下;
		customButton_Mini.MouseEnterImage = (Image)(object)Resources.最小化按键鼠标进入;
		((Control)customButton_Mini).Name = "customButton_Mini";
		customButton_Mini.NormalImage = (Image)(object)Resources.最小化按键;
		((Control)customButton_Mini).Size = new Size(20, 20);
		((Control)customButton_Mini).TabIndex = 4;
		((Control)customButton_Mini).Click += customButton_Mini_Click;
		((Control)customButton_Close).Location = new Point(977, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.关闭按键;
		((Control)customButton_Close).Size = new Size(20, 20);
		((Control)customButton_Close).TabIndex = 3;
		((Control)customButton_Close).Click += System_Exit;
		customTabSelector_Main.CheckedImage = (Image)(object)Resources.导航栏选择;
		((Control)customTabSelector_Main).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customTabSelector_Main.ItemSize = new Size(77, 28);
		((Control)customTabSelector_Main).Location = new Point(218, 64);
		customTabSelector_Main.MouseEnterImage = (Image)(object)Resources.导航栏鼠标进入;
		((Control)customTabSelector_Main).Name = "customTabSelector_Main";
		customTabSelector_Main.NormalImage = (Image)(object)Resources.导航栏默认;
		((Control)customTabSelector_Main).Size = new Size(385, 28);
		customTabSelector_Main.TabControl = (TabControl)(object)customTabControl_Main;
		((Control)customTabSelector_Main).TabIndex = 2;
		((Control)customTabSelector_Main).Text = "customTabSelector1";
		((Control)customTabControl_Main).Controls.Add((Control)(object)tabPage_Button);
		((Control)customTabControl_Main).Controls.Add((Control)(object)tabPage_Sensor);
		((Control)customTabControl_Main).Controls.Add((Control)(object)tabPage_Macro);
		((Control)customTabControl_Main).Controls.Add((Control)(object)tabPage_Light);
		((Control)customTabControl_Main).Controls.Add((Control)(object)tabPage_Setting);
		((Control)customTabControl_Main).Location = new Point(136, 98);
		((Control)customTabControl_Main).Name = "customTabControl_Main";
		((TabControl)customTabControl_Main).SelectedIndex = 0;
		((Control)customTabControl_Main).Size = new Size(886, 618);
		((Control)customTabControl_Main).TabIndex = 1;
		customTabControl_Main.TabPageSelectBackColor = Color.Transparent;
		customTabControl_Main.TabPageSelectFontColor = Color.Orange;
		customTabControl_Main.TabPageUnselectBackColor = Color.Transparent;
		customTabControl_Main.TabPageUnselectFontColor = Color.White;
		((Control)tabPage_Button).BackColor = Color.Transparent;
		((Control)tabPage_Button).Controls.Add((Control)(object)customButton_Reset);
		((Control)tabPage_Button).Controls.Add((Control)(object)textBox_fileName);
		((Control)tabPage_Button).Controls.Add((Control)(object)textBox_url);
		((Control)tabPage_Button).Controls.Add((Control)(object)customButton17);
		((Control)tabPage_Button).Controls.Add((Control)(object)customKeyFunction1);
		((Control)tabPage_Button).Controls.Add((Control)(object)customButton_Export);
		((Control)tabPage_Button).Controls.Add((Control)(object)customButton_Import);
		((Control)tabPage_Button).Controls.Add((Control)(object)customComboBox_Config);
		((Control)tabPage_Button).Controls.Add((Control)(object)panel_Device);
		((Control)tabPage_Button).Controls.Add((Control)(object)pictureBox_Config);
		tabPage_Button.Location = new Point(4, 26);
		((Control)tabPage_Button).Name = "tabPage_Button";
		((Control)tabPage_Button).Padding = new Padding(3);
		((Control)tabPage_Button).Size = new Size(878, 588);
		tabPage_Button.TabIndex = 0;
		((Control)tabPage_Button).Text = "按键";
		((Control)customButton_Reset).ForeColor = Color.White;
		((Control)customButton_Reset).Location = new Point(574, 350);
		customButton_Reset.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Reset.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Reset).Name = "customButton_Reset";
		customButton_Reset.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Reset).Size = new Size(96, 26);
		((Control)customButton_Reset).TabIndex = 77;
		((Control)customButton_Reset).Text = "导入配置";
		((Control)customButton_Reset).Click += customButton_Reset_Click;
		((Control)textBox_fileName).Location = new Point(684, 113);
		((Control)textBox_fileName).Name = "textBox_fileName";
		((Control)textBox_fileName).Size = new Size(100, 26);
		((Control)textBox_fileName).TabIndex = 19;
		((Control)textBox_fileName).Visible = false;
		((Control)textBox_url).Location = new Point(684, 69);
		((Control)textBox_url).Name = "textBox_url";
		((Control)textBox_url).Size = new Size(100, 26);
		((Control)textBox_url).TabIndex = 18;
		((Control)textBox_url).Visible = false;
		((Control)customButton17).ForeColor = Color.White;
		((Control)customButton17).Location = new Point(690, 186);
		customButton17.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton17.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton17).Name = "customButton17";
		customButton17.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton17).Size = new Size(78, 26);
		((Control)customButton17).TabIndex = 17;
		((Control)customButton17).Text = "下载";
		((Control)customButton17).Visible = false;
		((Control)customButton17).Click += customButton17_Click;
		((Control)customKeyFunction1).BackColor = Color.Transparent;
		((Control)customKeyFunction1).BackgroundImage = (Image)(object)Resources.按键功能背景;
		((Control)customKeyFunction1).BackgroundImageLayout = (ImageLayout)3;
		customKeyFunction1.ButtonCount = 6;
		customKeyFunction1.ButtonForeColor = Color.FromArgb(255, 128, 0);
		customKeyFunction1.ButtonInterval = 8;
		customKeyFunction1.CheckImage = (Image)componentResourceManager.GetObject("customKeyFunction1.CheckImage");
		((Control)customKeyFunction1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customKeyFunction1).ForeColor = Color.White;
		customKeyFunction1.KeyNormalImag = null;
		customKeyFunction1.KeyNormalImagArrow = (Image)componentResourceManager.GetObject("customKeyFunction1.KeyNormalImagArrow");
		customKeyFunction1.KeySelectImag = (Image)componentResourceManager.GetObject("customKeyFunction1.KeySelectImag");
		customKeyFunction1.KeySelectImagArrow = (Image)componentResourceManager.GetObject("customKeyFunction1.KeySelectImagArrow");
		((Control)customKeyFunction1).Location = new Point(2, 381);
		customKeyFunction1.MainKeyBackgroundImage = (Image)componentResourceManager.GetObject("customKeyFunction1.MainKeyBackgroundImage");
		((Control)customKeyFunction1).Margin = new Padding(4, 5, 4, 5);
		((Control)customKeyFunction1).Name = "customKeyFunction1";
		customKeyFunction1.NormalImage = (Image)(object)Resources.按键列表;
		customKeyFunction1.NumberButtonImage = (Image)(object)Resources.按键功能;
		customKeyFunction1.ScrollSet = false;
		customKeyFunction1.SelectedIndex = -1;
		customKeyFunction1.SelectImage = (Image)(object)Resources.按键列表按下;
		((Control)customKeyFunction1).Size = new Size(869, 207);
		customKeyFunction1.SubKeyBackgroundImage = (Image)componentResourceManager.GetObject("customKeyFunction1.SubKeyBackgroundImage");
		((Control)customKeyFunction1).TabIndex = 13;
		customKeyFunction1.UncheckImage = (Image)componentResourceManager.GetObject("customKeyFunction1.UncheckImage");
		customKeyFunction1.UpdateMacroUI += customKeyFunction1_UpdateMacroUI;
		((Control)customButton_Export).ForeColor = Color.White;
		((Control)customButton_Export).Location = new Point(772, 350);
		customButton_Export.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Export.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Export).Name = "customButton_Export";
		customButton_Export.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Export).Size = new Size(96, 26);
		((Control)customButton_Export).TabIndex = 12;
		((Control)customButton_Export).Text = "导出配置";
		((Control)customButton_Export).Click += customButton_Export_Click;
		((Control)customButton_Import).ForeColor = Color.White;
		((Control)customButton_Import).Location = new Point(673, 350);
		customButton_Import.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Import.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Import).Name = "customButton_Import";
		customButton_Import.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Import).Size = new Size(96, 26);
		((Control)customButton_Import).TabIndex = 11;
		((Control)customButton_Import).Text = "导入配置";
		((Control)customButton_Import).Click += customButton_Import_Click;
		customComboBox_Config.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_Config.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_Config).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_Config).Font = new Font("微软雅黑", 10.5f);
		customComboBox_Config.Item.AddRange(new object[4] { "配置1", "配置2", "配置3", "配置4" });
		((Control)customComboBox_Config).Location = new Point(44, 24);
		((Control)customComboBox_Config).Name = "customComboBox_Config";
		customComboBox_Config.SelectIndex = -1;
		customComboBox_Config.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_Config).Size = new Size(124, 21);
		((Control)customComboBox_Config).TabIndex = 16;
		customComboBox_Config.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_Config.OnSelectedIndexChanged += customComboBox_Config_OnSelectedIndexChanged;
		((Control)panel_Device).BackgroundImage = (Image)componentResourceManager.GetObject("panel_Device.BackgroundImage");
		((Control)panel_Device).BackgroundImageLayout = (ImageLayout)3;
		((Control)panel_Device).Controls.Add((Control)(object)customButton16);
		((Control)panel_Device).Controls.Add((Control)(object)customButton15);
		((Control)panel_Device).Controls.Add((Control)(object)customButton14);
		((Control)panel_Device).Controls.Add((Control)(object)customButton13);
		((Control)panel_Device).Controls.Add((Control)(object)customButton12);
		((Control)panel_Device).Controls.Add((Control)(object)customButton11);
		((Control)panel_Device).Controls.Add((Control)(object)customButton10);
		((Control)panel_Device).Controls.Add((Control)(object)customButton9);
		((Control)panel_Device).Controls.Add((Control)(object)customButton8);
		((Control)panel_Device).Controls.Add((Control)(object)customButton7);
		((Control)panel_Device).Controls.Add((Control)(object)customButton6);
		((Control)panel_Device).Controls.Add((Control)(object)customButton5);
		((Control)panel_Device).Controls.Add((Control)(object)customButton4);
		((Control)panel_Device).Controls.Add((Control)(object)customButton3);
		((Control)panel_Device).Controls.Add((Control)(object)customButton1);
		((Control)panel_Device).Controls.Add((Control)(object)customButton2);
		((Control)panel_Device).Location = new Point(182, 6);
		((Control)panel_Device).Name = "panel_Device";
		((Control)panel_Device).Size = new Size(432, 356);
		((Control)panel_Device).TabIndex = 2;
		((Control)customButton16).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton16).Location = new Point(251, 271);
		customButton16.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton16.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton16).Name = "customButton16";
		customButton16.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton16).Size = new Size(38, 38);
		((Control)customButton16).TabIndex = 21;
		((Control)customButton16).Text = "16";
		((Control)customButton15).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton15).Location = new Point(295, 271);
		customButton15.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton15.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton15).Name = "customButton15";
		customButton15.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton15).Size = new Size(38, 38);
		((Control)customButton15).TabIndex = 20;
		((Control)customButton15).Text = "15";
		((Control)customButton14).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton14).Location = new Point(339, 271);
		customButton14.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton14.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton14).Name = "customButton14";
		customButton14.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton14).Size = new Size(38, 38);
		((Control)customButton14).TabIndex = 19;
		((Control)customButton14).Text = "14";
		((Control)customButton13).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton13).Location = new Point(383, 271);
		customButton13.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton13.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton13).Name = "customButton13";
		customButton13.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton13).Size = new Size(38, 38);
		((Control)customButton13).TabIndex = 18;
		((Control)customButton13).Text = "13";
		((Control)customButton12).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton12).Location = new Point(383, 227);
		customButton12.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton12.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton12).Name = "customButton12";
		customButton12.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton12).Size = new Size(38, 38);
		((Control)customButton12).TabIndex = 17;
		((Control)customButton12).Text = "12";
		((Control)customButton11).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton11).Location = new Point(384, 183);
		customButton11.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton11.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton11).Name = "customButton11";
		customButton11.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton11).Size = new Size(38, 38);
		((Control)customButton11).TabIndex = 16;
		((Control)customButton11).Text = "11";
		((Control)customButton10).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton10).Location = new Point(384, 139);
		customButton10.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton10.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton10).Name = "customButton10";
		customButton10.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton10).Size = new Size(38, 38);
		((Control)customButton10).TabIndex = 15;
		((Control)customButton10).Text = "10";
		((Control)customButton9).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton9).Location = new Point(384, 95);
		customButton9.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton9.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton9).Name = "customButton9";
		customButton9.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton9).Size = new Size(38, 38);
		((Control)customButton9).TabIndex = 14;
		((Control)customButton9).Text = "9";
		((Control)customButton8).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton8).Location = new Point(384, 51);
		customButton8.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton8.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton8).Name = "customButton8";
		customButton8.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton8).Size = new Size(38, 38);
		((Control)customButton8).TabIndex = 13;
		((Control)customButton8).Text = "8";
		((Control)customButton7).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton7).Location = new Point(384, 9);
		customButton7.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton7.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton7).Name = "customButton7";
		customButton7.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton7).Size = new Size(38, 38);
		((Control)customButton7).TabIndex = 12;
		((Control)customButton7).Text = "7";
		((Control)customButton6).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton6).Location = new Point(229, 86);
		customButton6.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton6.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton6).Name = "customButton6";
		customButton6.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton6).Size = new Size(38, 38);
		((Control)customButton6).TabIndex = 11;
		((Control)customButton6).Text = "6";
		((Control)customButton5).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton5).Location = new Point(263, 159);
		customButton5.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton5.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton5).Name = "customButton5";
		customButton5.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton5).Size = new Size(38, 38);
		((Control)customButton5).TabIndex = 10;
		((Control)customButton5).Text = "5";
		((Control)customButton4).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton4).Location = new Point(203, 193);
		customButton4.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton4.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton4).Name = "customButton4";
		customButton4.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton4).Size = new Size(38, 38);
		((Control)customButton4).TabIndex = 9;
		((Control)customButton4).Text = "4";
		((Control)customButton3).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton3).Location = new Point(144, 95);
		customButton3.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton3.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton3).Name = "customButton3";
		customButton3.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton3).Size = new Size(38, 38);
		((Control)customButton3).TabIndex = 8;
		((Control)customButton3).Text = "3";
		((Control)customButton1).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton1).Location = new Point(94, 212);
		customButton1.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton1.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton1).Name = "customButton1";
		customButton1.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton1).Size = new Size(38, 38);
		((Control)customButton1).TabIndex = 7;
		((Control)customButton1).Text = "1";
		((Control)customButton2).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)customButton2).Location = new Point(53, 149);
		customButton2.MouseDownImage = (Image)(object)Resources.鼠标主图按键按下;
		customButton2.MouseEnterImage = (Image)(object)Resources.鼠标主图按键鼠标进入;
		((Control)customButton2).Name = "customButton2";
		customButton2.NormalImage = (Image)(object)Resources.鼠标主图按键;
		((Control)customButton2).Size = new Size(38, 38);
		((Control)customButton2).TabIndex = 2;
		((Control)customButton2).Text = "2";
		((Control)pictureBox_Config).BackgroundImage = (Image)(object)Resources.配置;
		((Control)pictureBox_Config).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Config).Location = new Point(18, 27);
		((Control)pictureBox_Config).Name = "pictureBox_Config";
		((Control)pictureBox_Config).Size = new Size(20, 14);
		pictureBox_Config.TabIndex = 0;
		pictureBox_Config.TabStop = false;
		((Control)tabPage_Sensor).BackColor = Color.Transparent;
		((Control)tabPage_Sensor).Controls.Add((Control)(object)_3In1_Main);
		((Control)tabPage_Sensor).Controls.Add((Control)(object)dpiControl1);
		tabPage_Sensor.Location = new Point(4, 26);
		((Control)tabPage_Sensor).Name = "tabPage_Sensor";
		((Control)tabPage_Sensor).Padding = new Padding(3);
		((Control)tabPage_Sensor).Size = new Size(878, 588);
		tabPage_Sensor.TabIndex = 1;
		((Control)tabPage_Sensor).Text = "传感器";
		((Control)_3In1_Main).BackColor = Color.Transparent;
		_3In1_Main.CheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.CheckImage");
		((Control)_3In1_Main).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)_3In1_Main).ForeColor = Color.White;
		_3In1_Main.FullCheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.FullCheckImage");
		_3In1_Main.FullUncheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.FullUncheckImage");
		((Control)_3In1_Main).Location = new Point(0, 208);
		((Control)_3In1_Main).Margin = new Padding(4, 5, 4, 5);
		((Control)_3In1_Main).Name = "_3In1_Main";
		_3In1_Main.SensorCheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.SensorCheckImage");
		_3In1_Main.SensorUncheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.SensorUncheckImage");
		((Control)_3In1_Main).Size = new Size(866, 368);
		((Control)_3In1_Main).TabIndex = 93;
		_3In1_Main.UncheckImage = (Image)componentResourceManager.GetObject("_3In1_Main.UncheckImage");
		dpiControl1.AddImage = (Image)componentResourceManager.GetObject("dpiControl1.AddImage");
		((Control)dpiControl1).BackColor = Color.Transparent;
		dpiControl1.CurrentDPIMouseDownImage = (Image)componentResourceManager.GetObject("dpiControl1.CurrentDPIMouseDownImage");
		dpiControl1.CurrentDPIMouseEnterImage = (Image)componentResourceManager.GetObject("dpiControl1.CurrentDPIMouseEnterImage");
		dpiControl1.CurrentDPINormalImage = (Image)componentResourceManager.GetObject("dpiControl1.CurrentDPINormalImage");
		dpiControl1.DPICheckImage = (Image)componentResourceManager.GetObject("dpiControl1.DPICheckImage");
		dpiControl1.DPIUncheckImage = (Image)componentResourceManager.GetObject("dpiControl1.DPIUncheckImage");
		((Control)dpiControl1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)dpiControl1).ForeColor = Color.White;
		((Control)dpiControl1).Location = new Point(0, 0);
		((Control)dpiControl1).Margin = new Padding(4, 5, 4, 5);
		dpiControl1.MaxGrade = 8;
		dpiControl1.MaxValue = 30000;
		dpiControl1.MinValue = 50;
		((Control)dpiControl1).Name = "dpiControl1";
		((Control)dpiControl1).Size = new Size(861, 200);
		dpiControl1.Step = 50;
		dpiControl1.SubImage = (Image)componentResourceManager.GetObject("dpiControl1.SubImage");
		((Control)dpiControl1).TabIndex = 92;
		dpiControl1.TitlePicture = (Image)componentResourceManager.GetObject("dpiControl1.TitlePicture");
		dpiControl1.Value = 1500;
		((Control)tabPage_Macro).BackColor = Color.Transparent;
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_UntilThisKeyPressed);
		((Control)tabPage_Macro).Controls.Add((Control)(object)panel_Delay);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_DefaultDelay);
		((Control)tabPage_Macro).Controls.Add((Control)(object)panel_CycleProcess);
		((Control)tabPage_Macro).Controls.Add((Control)(object)textBox_CycleTimes);
		((Control)tabPage_Macro).Controls.Add((Control)(object)textBox_AutoDelayTime);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customComboBox_InsertEvent);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customRecordMacro1);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customButton_Save);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customButton_DeleteButton);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customButton_ModifyButton);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customButton_DeleteMacro);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customButton_NewMacro);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_CycleTimes);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_UntilKeyPressed);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_UntilKeyReleased);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_InsertEvent);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_AutoDelay);
		((Control)tabPage_Macro).Controls.Add((Control)(object)listView_Keys);
		((Control)tabPage_Macro).Controls.Add((Control)(object)customListView_Macro);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_KeyList);
		((Control)tabPage_Macro).Controls.Add((Control)(object)label_MacroList);
		tabPage_Macro.Location = new Point(4, 26);
		((Control)tabPage_Macro).Name = "tabPage_Macro";
		((Control)tabPage_Macro).Padding = new Padding(3);
		((Control)tabPage_Macro).Size = new Size(878, 588);
		tabPage_Macro.TabIndex = 2;
		((Control)tabPage_Macro).Text = "宏";
		((Control)label_UntilThisKeyPressed).ForeColor = Color.White;
		((Control)label_UntilThisKeyPressed).Location = new Point(659, 269);
		((Control)label_UntilThisKeyPressed).Name = "label_UntilThisKeyPressed";
		((Control)label_UntilThisKeyPressed).Size = new Size(210, 40);
		((Control)label_UntilThisKeyPressed).TabIndex = 87;
		((Control)label_UntilThisKeyPressed).Text = "循环至此按键按下";
		((Control)panel_Delay).Controls.Add((Control)(object)customRadioButton_AutoDelay);
		((Control)panel_Delay).Controls.Add((Control)(object)customRadioButton_DefaultDelay);
		((Control)panel_Delay).Location = new Point(638, 83);
		((Control)panel_Delay).Name = "panel_Delay";
		((Control)panel_Delay).Size = new Size(18, 64);
		((Control)panel_Delay).TabIndex = 86;
		((RadioButton)customRadioButton_AutoDelay).Appearance = (Appearance)1;
		((RadioButton)customRadioButton_AutoDelay).Checked = true;
		customRadioButton_AutoDelay.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_AutoDelay.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_AutoDelay.DisableImage = null;
		((ButtonBase)customRadioButton_AutoDelay).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_AutoDelay).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_AutoDelay).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_AutoDelay).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_AutoDelay).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_AutoDelay).Location = new Point(3, 6);
		customRadioButton_AutoDelay.MouseEnterImage = null;
		((Control)customRadioButton_AutoDelay).Name = "customRadioButton_AutoDelay";
		((Control)customRadioButton_AutoDelay).Size = new Size(12, 12);
		((Control)customRadioButton_AutoDelay).TabIndex = 72;
		((RadioButton)customRadioButton_AutoDelay).TabStop = true;
		customRadioButton_AutoDelay.TextString = null;
		customRadioButton_AutoDelay.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_AutoDelay).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_DefaultDelay).Appearance = (Appearance)1;
		customRadioButton_DefaultDelay.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_DefaultDelay.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_DefaultDelay.DisableImage = null;
		((ButtonBase)customRadioButton_DefaultDelay).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_DefaultDelay).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_DefaultDelay).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_DefaultDelay).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_DefaultDelay).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_DefaultDelay).Location = new Point(3, 46);
		customRadioButton_DefaultDelay.MouseEnterImage = null;
		((Control)customRadioButton_DefaultDelay).Name = "customRadioButton_DefaultDelay";
		((Control)customRadioButton_DefaultDelay).Size = new Size(12, 12);
		((Control)customRadioButton_DefaultDelay).TabIndex = 73;
		customRadioButton_DefaultDelay.TextString = null;
		customRadioButton_DefaultDelay.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_DefaultDelay).UseVisualStyleBackColor = true;
		((Control)label_DefaultDelay).AutoSize = true;
		((Control)label_DefaultDelay).ForeColor = Color.White;
		((Control)label_DefaultDelay).Location = new Point(662, 124);
		((Control)label_DefaultDelay).Name = "label_DefaultDelay";
		((Control)label_DefaultDelay).Size = new Size(65, 20);
		((Control)label_DefaultDelay).TabIndex = 86;
		((Control)label_DefaultDelay).Text = "默认延时";
		((Control)panel_CycleProcess).Controls.Add((Control)(object)customRadioButton_UntilThisKeyPressed);
		((Control)panel_CycleProcess).Controls.Add((Control)(object)customRadioButton_UntilKeyReleased);
		((Control)panel_CycleProcess).Controls.Add((Control)(object)customRadioButton_UntilKeyPressed);
		((Control)panel_CycleProcess).Controls.Add((Control)(object)customRadioButton_CycleTimes);
		((Control)panel_CycleProcess).Location = new Point(641, 188);
		((Control)panel_CycleProcess).Name = "panel_CycleProcess";
		((Control)panel_CycleProcess).Size = new Size(18, 141);
		((Control)panel_CycleProcess).TabIndex = 85;
		((RadioButton)customRadioButton_UntilThisKeyPressed).Appearance = (Appearance)1;
		customRadioButton_UntilThisKeyPressed.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_UntilThisKeyPressed.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_UntilThisKeyPressed.DisableImage = null;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_UntilThisKeyPressed).Location = new Point(3, 85);
		customRadioButton_UntilThisKeyPressed.MouseEnterImage = null;
		((Control)customRadioButton_UntilThisKeyPressed).Name = "customRadioButton_UntilThisKeyPressed";
		((Control)customRadioButton_UntilThisKeyPressed).Size = new Size(12, 12);
		((Control)customRadioButton_UntilThisKeyPressed).TabIndex = 75;
		((RadioButton)customRadioButton_UntilThisKeyPressed).TabStop = true;
		((Control)customRadioButton_UntilThisKeyPressed).Tag = "1";
		customRadioButton_UntilThisKeyPressed.TextString = null;
		customRadioButton_UntilThisKeyPressed.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_UntilThisKeyPressed).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_UntilKeyReleased).Appearance = (Appearance)1;
		customRadioButton_UntilKeyReleased.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_UntilKeyReleased.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_UntilKeyReleased.DisableImage = null;
		((ButtonBase)customRadioButton_UntilKeyReleased).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_UntilKeyReleased).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyReleased).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyReleased).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyReleased).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_UntilKeyReleased).Location = new Point(3, 6);
		customRadioButton_UntilKeyReleased.MouseEnterImage = null;
		((Control)customRadioButton_UntilKeyReleased).Name = "customRadioButton_UntilKeyReleased";
		((Control)customRadioButton_UntilKeyReleased).Size = new Size(12, 12);
		((Control)customRadioButton_UntilKeyReleased).TabIndex = 72;
		((RadioButton)customRadioButton_UntilKeyReleased).TabStop = true;
		((Control)customRadioButton_UntilKeyReleased).Tag = "254";
		customRadioButton_UntilKeyReleased.TextString = null;
		customRadioButton_UntilKeyReleased.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_UntilKeyReleased).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_UntilKeyPressed).Appearance = (Appearance)1;
		customRadioButton_UntilKeyPressed.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_UntilKeyPressed.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_UntilKeyPressed.DisableImage = null;
		((ButtonBase)customRadioButton_UntilKeyPressed).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_UntilKeyPressed).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyPressed).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyPressed).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_UntilKeyPressed).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_UntilKeyPressed).Location = new Point(3, 45);
		customRadioButton_UntilKeyPressed.MouseEnterImage = null;
		((Control)customRadioButton_UntilKeyPressed).Name = "customRadioButton_UntilKeyPressed";
		((Control)customRadioButton_UntilKeyPressed).Size = new Size(12, 12);
		((Control)customRadioButton_UntilKeyPressed).TabIndex = 73;
		((RadioButton)customRadioButton_UntilKeyPressed).TabStop = true;
		((Control)customRadioButton_UntilKeyPressed).Tag = "255";
		customRadioButton_UntilKeyPressed.TextString = null;
		customRadioButton_UntilKeyPressed.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_UntilKeyPressed).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_CycleTimes).Appearance = (Appearance)1;
		customRadioButton_CycleTimes.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_CycleTimes.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_CycleTimes.DisableImage = null;
		((ButtonBase)customRadioButton_CycleTimes).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_CycleTimes).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_CycleTimes).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_CycleTimes).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_CycleTimes).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_CycleTimes).Location = new Point(3, 125);
		customRadioButton_CycleTimes.MouseEnterImage = null;
		((Control)customRadioButton_CycleTimes).Name = "customRadioButton_CycleTimes";
		((Control)customRadioButton_CycleTimes).Size = new Size(12, 12);
		((Control)customRadioButton_CycleTimes).TabIndex = 74;
		((RadioButton)customRadioButton_CycleTimes).TabStop = true;
		((Control)customRadioButton_CycleTimes).Tag = "1";
		customRadioButton_CycleTimes.TextString = null;
		customRadioButton_CycleTimes.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_CycleTimes).UseVisualStyleBackColor = true;
		((Control)textBox_CycleTimes).BackColor = Color.FromArgb(57, 57, 57);
		((TextBoxBase)textBox_CycleTimes).BorderStyle = (BorderStyle)1;
		((Control)textBox_CycleTimes).ForeColor = Color.White;
		((Control)textBox_CycleTimes).Location = new Point(666, 342);
		((Control)textBox_CycleTimes).Name = "textBox_CycleTimes";
		((Control)textBox_CycleTimes).Size = new Size(79, 26);
		((Control)textBox_CycleTimes).TabIndex = 84;
		((Control)textBox_CycleTimes).Text = "1";
		textBox_CycleTimes.TextAlign = (HorizontalAlignment)2;
		((Control)textBox_CycleTimes).TextChanged += textBox_CycleTimes_TextChanged;
		((Control)textBox_CycleTimes).KeyPress += new KeyPressEventHandler(textBox_CycleTimes_KeyPress);
		((Control)textBox_AutoDelayTime).BackColor = Color.FromArgb(57, 57, 57);
		((TextBoxBase)textBox_AutoDelayTime).BorderStyle = (BorderStyle)1;
		((Control)textBox_AutoDelayTime).ForeColor = Color.White;
		((Control)textBox_AutoDelayTime).Location = new Point(666, 147);
		((Control)textBox_AutoDelayTime).Name = "textBox_AutoDelayTime";
		((Control)textBox_AutoDelayTime).Size = new Size(79, 26);
		((Control)textBox_AutoDelayTime).TabIndex = 83;
		((Control)textBox_AutoDelayTime).Text = "10";
		textBox_AutoDelayTime.TextAlign = (HorizontalAlignment)2;
		((Control)textBox_AutoDelayTime).TextChanged += textBox_AutoDelayTime_TextChanged;
		((Control)textBox_AutoDelayTime).KeyPress += new KeyPressEventHandler(textBox_AutoDelayTime_KeyPress);
		customComboBox_InsertEvent.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_InsertEvent.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_InsertEvent).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_InsertEvent).Font = new Font("微软雅黑", 10.5f);
		((Control)customComboBox_InsertEvent).ImeMode = (ImeMode)3;
		customComboBox_InsertEvent.Item.AddRange(new object[6] { "按键按下", "按键弹起", "延时", "左键", "右键", "中键" });
		((Control)customComboBox_InsertEvent).Location = new Point(641, 422);
		((Control)customComboBox_InsertEvent).Name = "customComboBox_InsertEvent";
		customComboBox_InsertEvent.SelectIndex = -1;
		customComboBox_InsertEvent.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_InsertEvent).Size = new Size(173, 29);
		((Control)customComboBox_InsertEvent).TabIndex = 82;
		customComboBox_InsertEvent.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_InsertEvent.OnSelectedIndexChanged += customComboBox_InsertEvent_OnSelectedIndexChanged;
		customRecordMacro1.Checked = false;
		((Control)customRecordMacro1).ForeColor = Color.White;
		((Control)customRecordMacro1).Location = new Point(632, 35);
		((Control)customRecordMacro1).Name = "customRecordMacro1";
		((Control)customRecordMacro1).Size = new Size(190, 32);
		customRecordMacro1.StartImage_MouseEnter = (Image)(object)Resources.开始录制状态;
		customRecordMacro1.StartImage_Normal = (Image)(object)Resources.开始录制默认状态;
		customRecordMacro1.StartText = "开始录制宏";
		customRecordMacro1.StopImage_MouseEnter = (Image)(object)Resources.停止录制状态;
		customRecordMacro1.StopImage_Normal = (Image)(object)Resources.停止录制默认状态;
		customRecordMacro1.StopText = "停止录制宏";
		((Control)customRecordMacro1).TabIndex = 81;
		((Control)customRecordMacro1).Text = "customRecordMacro1";
		((Control)customRecordMacro1).Click += customRecordMacro1_Click;
		((Control)customButton_Save).ForeColor = Color.White;
		((Control)customButton_Save).Location = new Point(638, 470);
		customButton_Save.MouseDownImage = (Image)(object)Resources.新建宏按键按下;
		customButton_Save.MouseEnterImage = (Image)(object)Resources.新建宏按键按下;
		((Control)customButton_Save).Name = "customButton_Save";
		customButton_Save.NormalImage = (Image)(object)Resources.新建宏按键;
		((Control)customButton_Save).Size = new Size(116, 30);
		((Control)customButton_Save).TabIndex = 80;
		((Control)customButton_Save).Text = "保存";
		((Control)customButton_Save).Click += customButton_Save_Click;
		((Control)customButton_DeleteButton).ForeColor = Color.White;
		((Control)customButton_DeleteButton).Location = new Point(476, 525);
		customButton_DeleteButton.MouseDownImage = (Image)(object)Resources.新建宏按键按下;
		customButton_DeleteButton.MouseEnterImage = (Image)(object)Resources.新建宏按键鼠标进入;
		((Control)customButton_DeleteButton).Name = "customButton_DeleteButton";
		customButton_DeleteButton.NormalImage = (Image)(object)Resources.新建宏按键;
		((Control)customButton_DeleteButton).Size = new Size(116, 30);
		((Control)customButton_DeleteButton).TabIndex = 78;
		((Control)customButton_DeleteButton).Text = "删除";
		((Control)customButton_DeleteButton).Click += customButton_DeleteButton_Click;
		((Control)customButton_ModifyButton).ForeColor = Color.White;
		((Control)customButton_ModifyButton).Location = new Point(342, 525);
		customButton_ModifyButton.MouseDownImage = (Image)(object)Resources.新建宏按键按下;
		customButton_ModifyButton.MouseEnterImage = (Image)(object)Resources.新建宏按键鼠标进入;
		((Control)customButton_ModifyButton).Name = "customButton_ModifyButton";
		customButton_ModifyButton.NormalImage = (Image)(object)Resources.新建宏按键;
		((Control)customButton_ModifyButton).Size = new Size(116, 30);
		((Control)customButton_ModifyButton).TabIndex = 77;
		((Control)customButton_ModifyButton).Text = "修改";
		((Control)customButton_ModifyButton).Click += customButton_ModifyButton_Click;
		((Control)customButton_DeleteMacro).ForeColor = Color.White;
		((Control)customButton_DeleteMacro).Location = new Point(204, 525);
		customButton_DeleteMacro.MouseDownImage = (Image)(object)Resources.新建宏按键按下;
		customButton_DeleteMacro.MouseEnterImage = (Image)(object)Resources.新建宏按键鼠标进入;
		((Control)customButton_DeleteMacro).Name = "customButton_DeleteMacro";
		customButton_DeleteMacro.NormalImage = (Image)(object)Resources.新建宏按键;
		((Control)customButton_DeleteMacro).Size = new Size(116, 30);
		((Control)customButton_DeleteMacro).TabIndex = 76;
		((Control)customButton_DeleteMacro).Text = "删除";
		((Control)customButton_DeleteMacro).Click += customButton_DeleteMacro_Click;
		((Control)customButton_NewMacro).ForeColor = Color.White;
		((Control)customButton_NewMacro).Location = new Point(73, 525);
		customButton_NewMacro.MouseDownImage = (Image)(object)Resources.新建宏按键按下;
		customButton_NewMacro.MouseEnterImage = (Image)(object)Resources.新建宏按键鼠标进入;
		((Control)customButton_NewMacro).Name = "customButton_NewMacro";
		customButton_NewMacro.NormalImage = (Image)(object)Resources.新建宏按键;
		((Control)customButton_NewMacro).Size = new Size(116, 30);
		((Control)customButton_NewMacro).TabIndex = 75;
		((Control)customButton_NewMacro).Text = "新建宏";
		((Control)customButton_NewMacro).Click += customButton_NewMacro_Click;
		((Control)label_CycleTimes).ForeColor = Color.White;
		((Control)label_CycleTimes).Location = new Point(659, 309);
		((Control)label_CycleTimes).Name = "label_CycleTimes";
		((Control)label_CycleTimes).Size = new Size(210, 20);
		((Control)label_CycleTimes).TabIndex = 71;
		((Control)label_CycleTimes).Text = "循环次数";
		((Control)label_UntilKeyPressed).ForeColor = Color.White;
		((Control)label_UntilKeyPressed).Location = new Point(659, 229);
		((Control)label_UntilKeyPressed).Name = "label_UntilKeyPressed";
		((Control)label_UntilKeyPressed).Size = new Size(216, 40);
		((Control)label_UntilKeyPressed).TabIndex = 70;
		((Control)label_UntilKeyPressed).Text = "循环直到任意按键按下";
		((Control)label_UntilKeyReleased).ForeColor = Color.White;
		((Control)label_UntilKeyReleased).Location = new Point(659, 190);
		((Control)label_UntilKeyReleased).Name = "label_UntilKeyReleased";
		((Control)label_UntilKeyReleased).Size = new Size(216, 40);
		((Control)label_UntilKeyReleased).TabIndex = 69;
		((Control)label_UntilKeyReleased).Text = "循环直到按键松开";
		((Control)label_InsertEvent).AutoSize = true;
		((Control)label_InsertEvent).ForeColor = Color.White;
		((Control)label_InsertEvent).Location = new Point(640, 390);
		((Control)label_InsertEvent).Name = "label_InsertEvent";
		((Control)label_InsertEvent).Size = new Size(65, 20);
		((Control)label_InsertEvent).TabIndex = 67;
		((Control)label_InsertEvent).Text = "插入事件";
		((Control)label_AutoDelay).ForeColor = Color.White;
		((Control)label_AutoDelay).Location = new Point(663, 83);
		((Control)label_AutoDelay).Name = "label_AutoDelay";
		((Control)label_AutoDelay).Size = new Size(212, 40);
		((Control)label_AutoDelay).TabIndex = 66;
		((Control)label_AutoDelay).Text = "自动插入延时";
		((Control)listView_Keys).BackColor = Color.Black;
		listView_Keys.BorderStyle = (BorderStyle)0;
		((Control)listView_Keys).ForeColor = Color.White;
		listView_Keys.HideSelection = false;
		((Control)listView_Keys).ImeMode = (ImeMode)3;
		((Control)listView_Keys).Location = new Point(342, 70);
		((Control)listView_Keys).Name = "listView_Keys";
		((Control)listView_Keys).Size = new Size(250, 430);
		((Control)listView_Keys).TabIndex = 6;
		listView_Keys.UseCompatibleStateImageBehavior = false;
		listView_Keys.View = (View)2;
		listView_Keys.SelectedIndexChanged += listView_Keys_SelectedIndexChanged;
		((Control)listView_Keys).MouseDown += new MouseEventHandler(listView_Keys_MouseDown);
		((Control)customListView_Macro).BackColor = Color.Black;
		((ListView)customListView_Macro).BorderStyle = (BorderStyle)0;
		((Control)customListView_Macro).ForeColor = Color.White;
		((ListView)customListView_Macro).HideSelection = false;
		((Control)customListView_Macro).ImeMode = (ImeMode)1;
		customListView_Macro.ItemSelectColor = Color.FromArgb(57, 57, 57);
		((Control)customListView_Macro).Location = new Point(70, 70);
		((ListView)customListView_Macro).MultiSelect = false;
		((Control)customListView_Macro).Name = "customListView_Macro";
		((ListView)customListView_Macro).OwnerDraw = true;
		((Control)customListView_Macro).Size = new Size(250, 430);
		((Control)customListView_Macro).TabIndex = 3;
		((ListView)customListView_Macro).UseCompatibleStateImageBehavior = false;
		((ListView)customListView_Macro).View = (View)2;
		((ListView)customListView_Macro).SelectedIndexChanged += customListView_Macro_SelectedIndexChanged;
		((Control)customListView_Macro).MouseDown += new MouseEventHandler(customListView_Macro_MouseDown);
		((Control)label_KeyList).AutoSize = true;
		((Control)label_KeyList).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_KeyList).ForeColor = Color.White;
		label_KeyList.ImeMode = (ImeMode)0;
		((Control)label_KeyList).Location = new Point(338, 46);
		((Control)label_KeyList).Name = "label_KeyList";
		((Control)label_KeyList).Size = new Size(74, 21);
		((Control)label_KeyList).TabIndex = 2;
		((Control)label_KeyList).Text = "按键列表";
		((Control)label_MacroList).AutoSize = true;
		((Control)label_MacroList).Font = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_MacroList).ForeColor = Color.White;
		label_MacroList.ImeMode = (ImeMode)0;
		((Control)label_MacroList).Location = new Point(66, 46);
		((Control)label_MacroList).Name = "label_MacroList";
		((Control)label_MacroList).Size = new Size(58, 21);
		((Control)label_MacroList).TabIndex = 1;
		((Control)label_MacroList).Text = "宏列表";
		((Control)tabPage_Light).BackColor = Color.Transparent;
		((Control)tabPage_Light).Controls.Add((Control)(object)panel_LEDColor);
		((Control)tabPage_Light).Controls.Add((Control)(object)customComboBox_PowerSaveTime);
		((Control)tabPage_Light).Controls.Add((Control)(object)customTrackBar_LightSpeed);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_LightSpeedValue);
		((Control)tabPage_Light).Controls.Add((Control)(object)customTrackBar_LightBrightness);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_LightBrightnessValue);
		((Control)tabPage_Light).Controls.Add((Control)(object)customComboBox_LEDMode);
		((Control)tabPage_Light).Controls.Add((Control)(object)customCheckBox_MovingCloseLight);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_MovingCloseLight);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_PowerSaveTime);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_LightSpeed);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_LightBrightness);
		((Control)tabPage_Light).Controls.Add((Control)(object)label_LightMode);
		tabPage_Light.Location = new Point(4, 26);
		((Control)tabPage_Light).Name = "tabPage_Light";
		((Control)tabPage_Light).Padding = new Padding(3);
		((Control)tabPage_Light).Size = new Size(878, 588);
		tabPage_Light.TabIndex = 3;
		((Control)tabPage_Light).Text = "灯光";
		((Control)panel_LEDColor).Controls.Add((Control)(object)adjustControl_B);
		((Control)panel_LEDColor).Controls.Add((Control)(object)adjustControl_G);
		((Control)panel_LEDColor).Controls.Add((Control)(object)adjustControl_R);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDB);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDG);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDR);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color14);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color7);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color13);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color12);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color11);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color6);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color5);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color4);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color10);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color9);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color8);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color3);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color2);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Color1);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_LEDPreview);
		((Control)panel_LEDColor).Controls.Add((Control)(object)pictureBox_Palette);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDPreset);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDPreview);
		((Control)panel_LEDColor).Controls.Add((Control)(object)label_LEDColor);
		((Control)panel_LEDColor).Location = new Point(411, 44);
		((Control)panel_LEDColor).Name = "panel_LEDColor";
		((Control)panel_LEDColor).Size = new Size(377, 413);
		((Control)panel_LEDColor).TabIndex = 72;
		adjustControl_B.AddImage = (Image)componentResourceManager.GetObject("adjustControl_B.AddImage");
		((Control)adjustControl_B).BackColor = Color.Transparent;
		((Control)adjustControl_B).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)adjustControl_B).ForeColor = Color.White;
		((Control)adjustControl_B).Location = new Point(127, 226);
		((Control)adjustControl_B).Margin = new Padding(4, 4, 4, 4);
		adjustControl_B.MaxValue = 255;
		adjustControl_B.MinValue = 0;
		((Control)adjustControl_B).Name = "adjustControl_B";
		((Control)adjustControl_B).Size = new Size(94, 31);
		adjustControl_B.Step = 1;
		adjustControl_B.SubImage = (Image)componentResourceManager.GetObject("adjustControl_B.SubImage");
		((Control)adjustControl_B).TabIndex = 86;
		adjustControl_B.Value = 255;
		adjustControl_B.ValueChange += ColorText_ValueChange;
		adjustControl_G.AddImage = (Image)componentResourceManager.GetObject("adjustControl_G.AddImage");
		((Control)adjustControl_G).BackColor = Color.Transparent;
		((Control)adjustControl_G).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)adjustControl_G).Location = new Point(127, 196);
		((Control)adjustControl_G).Margin = new Padding(4, 4, 4, 4);
		adjustControl_G.MaxValue = 255;
		adjustControl_G.MinValue = 0;
		((Control)adjustControl_G).Name = "adjustControl_G";
		((Control)adjustControl_G).Size = new Size(94, 31);
		adjustControl_G.Step = 1;
		adjustControl_G.SubImage = (Image)componentResourceManager.GetObject("adjustControl_G.SubImage");
		((Control)adjustControl_G).TabIndex = 85;
		adjustControl_G.Value = 85;
		adjustControl_G.ValueChange += ColorText_ValueChange;
		adjustControl_R.AddImage = (Image)componentResourceManager.GetObject("adjustControl_R.AddImage");
		((Control)adjustControl_R).BackColor = Color.Transparent;
		((Control)adjustControl_R).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)adjustControl_R).Location = new Point(127, 166);
		((Control)adjustControl_R).Margin = new Padding(4, 4, 4, 4);
		adjustControl_R.MaxValue = 255;
		adjustControl_R.MinValue = 0;
		((Control)adjustControl_R).Name = "adjustControl_R";
		((Control)adjustControl_R).Size = new Size(94, 31);
		adjustControl_R.Step = 1;
		adjustControl_R.SubImage = (Image)componentResourceManager.GetObject("adjustControl_R.SubImage");
		((Control)adjustControl_R).TabIndex = 84;
		adjustControl_R.Value = 251;
		adjustControl_R.ValueChange += ColorText_ValueChange;
		((Control)label_LEDB).AutoSize = true;
		((Control)label_LEDB).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_LEDB).ForeColor = Color.White;
		((Control)label_LEDB).Location = new Point(101, 228);
		((Control)label_LEDB).Name = "label_LEDB";
		((Control)label_LEDB).Size = new Size(21, 20);
		((Control)label_LEDB).TabIndex = 83;
		((Control)label_LEDB).Text = "B:";
		((Control)label_LEDG).AutoSize = true;
		((Control)label_LEDG).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_LEDG).ForeColor = Color.White;
		((Control)label_LEDG).Location = new Point(101, 198);
		((Control)label_LEDG).Name = "label_LEDG";
		((Control)label_LEDG).Size = new Size(22, 20);
		((Control)label_LEDG).TabIndex = 82;
		((Control)label_LEDG).Text = "G:";
		((Control)label_LEDR).AutoSize = true;
		((Control)label_LEDR).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_LEDR).ForeColor = Color.White;
		((Control)label_LEDR).Location = new Point(101, 168);
		((Control)label_LEDR).Name = "label_LEDR";
		((Control)label_LEDR).Size = new Size(21, 20);
		((Control)label_LEDR).TabIndex = 81;
		((Control)label_LEDR).Text = "R:";
		((Control)pictureBox_Color14).BackgroundImage = (Image)(object)Resources.颜色14;
		((Control)pictureBox_Color14).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color14).Location = new Point(292, 371);
		((Control)pictureBox_Color14).Name = "pictureBox_Color14";
		((Control)pictureBox_Color14).Size = new Size(28, 28);
		pictureBox_Color14.TabIndex = 80;
		pictureBox_Color14.TabStop = false;
		((Control)pictureBox_Color14).Click += Light_Color_Change;
		((Control)pictureBox_Color7).BackgroundImage = (Image)(object)Resources.颜色7;
		((Control)pictureBox_Color7).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color7).Location = new Point(292, 315);
		((Control)pictureBox_Color7).Name = "pictureBox_Color7";
		((Control)pictureBox_Color7).Size = new Size(28, 28);
		pictureBox_Color7.TabIndex = 79;
		pictureBox_Color7.TabStop = false;
		((Control)pictureBox_Color7).Click += Light_Color_Change;
		((Control)pictureBox_Color13).BackgroundImage = (Image)(object)Resources.颜色13;
		((Control)pictureBox_Color13).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color13).Location = new Point(242, 371);
		((Control)pictureBox_Color13).Name = "pictureBox_Color13";
		((Control)pictureBox_Color13).Size = new Size(28, 28);
		pictureBox_Color13.TabIndex = 78;
		pictureBox_Color13.TabStop = false;
		((Control)pictureBox_Color13).Click += Light_Color_Change;
		((Control)pictureBox_Color12).BackgroundImage = (Image)(object)Resources.颜色12;
		((Control)pictureBox_Color12).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color12).Location = new Point(195, 371);
		((Control)pictureBox_Color12).Name = "pictureBox_Color12";
		((Control)pictureBox_Color12).Size = new Size(28, 28);
		pictureBox_Color12.TabIndex = 77;
		pictureBox_Color12.TabStop = false;
		((Control)pictureBox_Color12).Click += Light_Color_Change;
		((Control)pictureBox_Color11).BackgroundImage = (Image)(object)Resources.颜色11;
		((Control)pictureBox_Color11).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color11).Location = new Point(149, 371);
		((Control)pictureBox_Color11).Name = "pictureBox_Color11";
		((Control)pictureBox_Color11).Size = new Size(28, 28);
		pictureBox_Color11.TabIndex = 76;
		pictureBox_Color11.TabStop = false;
		((Control)pictureBox_Color11).Click += Light_Color_Change;
		((Control)pictureBox_Color6).BackgroundImage = (Image)(object)Resources.颜色6;
		((Control)pictureBox_Color6).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color6).Location = new Point(242, 315);
		((Control)pictureBox_Color6).Name = "pictureBox_Color6";
		((Control)pictureBox_Color6).Size = new Size(28, 28);
		pictureBox_Color6.TabIndex = 75;
		pictureBox_Color6.TabStop = false;
		((Control)pictureBox_Color6).Click += Light_Color_Change;
		((Control)pictureBox_Color5).BackgroundImage = (Image)(object)Resources.颜色5;
		((Control)pictureBox_Color5).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color5).Location = new Point(195, 315);
		((Control)pictureBox_Color5).Name = "pictureBox_Color5";
		((Control)pictureBox_Color5).Size = new Size(28, 28);
		pictureBox_Color5.TabIndex = 74;
		pictureBox_Color5.TabStop = false;
		((Control)pictureBox_Color5).Click += Light_Color_Change;
		((Control)pictureBox_Color4).BackgroundImage = (Image)(object)Resources.颜色4;
		((Control)pictureBox_Color4).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color4).Location = new Point(149, 315);
		((Control)pictureBox_Color4).Name = "pictureBox_Color4";
		((Control)pictureBox_Color4).Size = new Size(28, 28);
		pictureBox_Color4.TabIndex = 73;
		pictureBox_Color4.TabStop = false;
		((Control)pictureBox_Color4).Click += Light_Color_Change;
		((Control)pictureBox_Color10).BackgroundImage = (Image)(object)Resources.颜色10;
		((Control)pictureBox_Color10).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color10).Location = new Point(101, 371);
		((Control)pictureBox_Color10).Name = "pictureBox_Color10";
		((Control)pictureBox_Color10).Size = new Size(28, 28);
		pictureBox_Color10.TabIndex = 72;
		pictureBox_Color10.TabStop = false;
		((Control)pictureBox_Color10).Click += Light_Color_Change;
		((Control)pictureBox_Color9).BackgroundImage = (Image)(object)Resources.颜色9;
		((Control)pictureBox_Color9).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color9).Location = new Point(54, 371);
		((Control)pictureBox_Color9).Name = "pictureBox_Color9";
		((Control)pictureBox_Color9).Size = new Size(28, 28);
		pictureBox_Color9.TabIndex = 71;
		pictureBox_Color9.TabStop = false;
		((Control)pictureBox_Color9).Click += Light_Color_Change;
		((Control)pictureBox_Color8).BackgroundImage = (Image)(object)Resources.颜色8;
		((Control)pictureBox_Color8).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color8).Location = new Point(8, 371);
		((Control)pictureBox_Color8).Name = "pictureBox_Color8";
		((Control)pictureBox_Color8).Size = new Size(28, 28);
		pictureBox_Color8.TabIndex = 70;
		pictureBox_Color8.TabStop = false;
		((Control)pictureBox_Color8).Click += Light_Color_Change;
		((Control)pictureBox_Color3).BackgroundImage = (Image)(object)Resources.颜色3;
		((Control)pictureBox_Color3).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color3).Location = new Point(101, 315);
		((Control)pictureBox_Color3).Name = "pictureBox_Color3";
		((Control)pictureBox_Color3).Size = new Size(28, 28);
		pictureBox_Color3.TabIndex = 69;
		pictureBox_Color3.TabStop = false;
		((Control)pictureBox_Color3).Click += Light_Color_Change;
		((Control)pictureBox_Color2).BackgroundImage = (Image)(object)Resources.颜色2;
		((Control)pictureBox_Color2).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color2).Location = new Point(54, 315);
		((Control)pictureBox_Color2).Name = "pictureBox_Color2";
		((Control)pictureBox_Color2).Size = new Size(28, 28);
		pictureBox_Color2.TabIndex = 68;
		pictureBox_Color2.TabStop = false;
		((Control)pictureBox_Color2).Click += Light_Color_Change;
		((Control)pictureBox_Color1).BackgroundImage = (Image)(object)Resources.颜色1;
		((Control)pictureBox_Color1).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Color1).Location = new Point(8, 315);
		((Control)pictureBox_Color1).Name = "pictureBox_Color1";
		((Control)pictureBox_Color1).Size = new Size(28, 28);
		pictureBox_Color1.TabIndex = 67;
		pictureBox_Color1.TabStop = false;
		((Control)pictureBox_Color1).Click += Light_Color_Change;
		((Control)pictureBox_LEDPreview).BackColor = Color.FromArgb(251, 85, 255);
		((Control)pictureBox_LEDPreview).Location = new Point(8, 179);
		((Control)pictureBox_LEDPreview).Name = "pictureBox_LEDPreview";
		((Control)pictureBox_LEDPreview).Size = new Size(60, 60);
		pictureBox_LEDPreview.TabIndex = 66;
		pictureBox_LEDPreview.TabStop = false;
		((Control)pictureBox_LEDPreview).Click += pictureBox_LEDPreview_Click;
		((Control)pictureBox_Palette).BackgroundImage = (Image)(object)Resources.颜色条;
		((Control)pictureBox_Palette).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_Palette).Location = new Point(8, 62);
		((Control)pictureBox_Palette).Name = "pictureBox_Palette";
		((Control)pictureBox_Palette).Size = new Size(308, 34);
		pictureBox_Palette.TabIndex = 65;
		pictureBox_Palette.TabStop = false;
		((Control)pictureBox_Palette).Click += Light_Color_Change;
		((Control)label_LEDPreset).AutoSize = true;
		((Control)label_LEDPreset).Font = new Font("微软雅黑", 12f);
		((Control)label_LEDPreset).ForeColor = Color.White;
		label_LEDPreset.ImeMode = (ImeMode)0;
		((Control)label_LEDPreset).Location = new Point(6, 278);
		((Control)label_LEDPreset).Name = "label_LEDPreset";
		((Control)label_LEDPreset).Size = new Size(74, 21);
		((Control)label_LEDPreset).TabIndex = 64;
		((Control)label_LEDPreset).Text = "预设颜色";
		((Control)label_LEDPreview).AutoSize = true;
		((Control)label_LEDPreview).Font = new Font("微软雅黑", 12f);
		((Control)label_LEDPreview).ForeColor = Color.White;
		label_LEDPreview.ImeMode = (ImeMode)0;
		((Control)label_LEDPreview).Location = new Point(8, 126);
		((Control)label_LEDPreview).Name = "label_LEDPreview";
		((Control)label_LEDPreview).Size = new Size(74, 21);
		((Control)label_LEDPreview).TabIndex = 63;
		((Control)label_LEDPreview).Text = "灯光预览";
		((Control)label_LEDColor).AutoSize = true;
		((Control)label_LEDColor).Font = new Font("微软雅黑", 12f);
		((Control)label_LEDColor).ForeColor = Color.White;
		label_LEDColor.ImeMode = (ImeMode)0;
		((Control)label_LEDColor).Location = new Point(3, 22);
		((Control)label_LEDColor).Name = "label_LEDColor";
		((Control)label_LEDColor).Size = new Size(74, 21);
		((Control)label_LEDColor).TabIndex = 62;
		((Control)label_LEDColor).Text = "灯光颜色";
		customComboBox_PowerSaveTime.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_PowerSaveTime.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_PowerSaveTime).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_PowerSaveTime).Font = new Font("微软雅黑", 10.5f);
		customComboBox_PowerSaveTime.Item.AddRange(new object[11]
		{
			"10秒", "20秒", "1分钟", "5分钟", "10分钟", "15分钟", "20分钟", "25分钟", "30分钟", "35分钟",
			"40分钟"
		});
		((Control)customComboBox_PowerSaveTime).Location = new Point(214, 412);
		((Control)customComboBox_PowerSaveTime).Name = "customComboBox_PowerSaveTime";
		customComboBox_PowerSaveTime.SelectIndex = -1;
		customComboBox_PowerSaveTime.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_PowerSaveTime).Size = new Size(133, 31);
		((Control)customComboBox_PowerSaveTime).TabIndex = 71;
		customComboBox_PowerSaveTime.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_PowerSaveTime.OnSelectedIndexChanged += customComboBox_PowerSaveTime_OnSelectedIndexChanged;
		customTrackBar_LightSpeed.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_LightSpeed.BarSize = 2;
		customTrackBar_LightSpeed.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_LightSpeed.Enable = true;
		customTrackBar_LightSpeed.IsRound = true;
		((Control)customTrackBar_LightSpeed).Location = new Point(43, 297);
		customTrackBar_LightSpeed.Maximum = 9;
		customTrackBar_LightSpeed.Minimum = 0;
		((Control)customTrackBar_LightSpeed).Name = "customTrackBar_LightSpeed";
		customTrackBar_LightSpeed.Orientation = (Orientation)0;
		((Control)customTrackBar_LightSpeed).Size = new Size(267, 23);
		customTrackBar_LightSpeed.SizeSlidSize = new Size(6, 18);
		customTrackBar_LightSpeed.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_LightSpeed.Step = 1;
		((Control)customTrackBar_LightSpeed).TabIndex = 70;
		customTrackBar_LightSpeed.Value = 7;
		customTrackBar_LightSpeed.ValueChanged += customTrackBar_LightSpeed_ValueChanged;
		customTrackBar_LightSpeed.SetValue += customTrackBar_LightSpeed_SetValue;
		((Control)label_LightSpeedValue).AutoSize = true;
		((Control)label_LightSpeedValue).ForeColor = Color.White;
		((Control)label_LightSpeedValue).Location = new Point(330, 297);
		((Control)label_LightSpeedValue).Name = "label_LightSpeedValue";
		((Control)label_LightSpeedValue).Size = new Size(17, 20);
		((Control)label_LightSpeedValue).TabIndex = 69;
		((Control)label_LightSpeedValue).Text = "7";
		customTrackBar_LightBrightness.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_LightBrightness.BarSize = 2;
		customTrackBar_LightBrightness.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_LightBrightness.Enable = true;
		customTrackBar_LightBrightness.IsRound = true;
		((Control)customTrackBar_LightBrightness).Location = new Point(43, 207);
		customTrackBar_LightBrightness.Maximum = 9;
		customTrackBar_LightBrightness.Minimum = 0;
		((Control)customTrackBar_LightBrightness).Name = "customTrackBar_LightBrightness";
		customTrackBar_LightBrightness.Orientation = (Orientation)0;
		((Control)customTrackBar_LightBrightness).Size = new Size(267, 23);
		customTrackBar_LightBrightness.SizeSlidSize = new Size(6, 18);
		customTrackBar_LightBrightness.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_LightBrightness.Step = 1;
		((Control)customTrackBar_LightBrightness).TabIndex = 68;
		customTrackBar_LightBrightness.Value = 7;
		customTrackBar_LightBrightness.ValueChanged += customTrackBar_LightBrightness_ValueChanged;
		customTrackBar_LightBrightness.SetValue += customTrackBar_LightBrightness_SetValue;
		((Control)label_LightBrightnessValue).AutoSize = true;
		((Control)label_LightBrightnessValue).ForeColor = Color.White;
		((Control)label_LightBrightnessValue).Location = new Point(330, 210);
		((Control)label_LightBrightnessValue).Name = "label_LightBrightnessValue";
		((Control)label_LightBrightnessValue).Size = new Size(17, 20);
		((Control)label_LightBrightnessValue).TabIndex = 67;
		((Control)label_LightBrightnessValue).Text = "7";
		customComboBox_LEDMode.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_LEDMode.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_LEDMode).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_LEDMode).Font = new Font("微软雅黑", 10.5f);
		customComboBox_LEDMode.Item.AddRange(new object[7] { "关闭", "彩色流动", "单色呼吸", "混色常亮", "霓虹", "混彩呼吸", "炫彩常亮" });
		((Control)customComboBox_LEDMode).Location = new Point(43, 105);
		((Control)customComboBox_LEDMode).Name = "customComboBox_LEDMode";
		customComboBox_LEDMode.SelectIndex = -1;
		customComboBox_LEDMode.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_LEDMode).Size = new Size(304, 35);
		((Control)customComboBox_LEDMode).TabIndex = 65;
		customComboBox_LEDMode.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_LEDMode.OnSelectedIndexChanged += customComboBox_LEDMode_OnSelectedIndexChanged;
		customCheckBox_MovingCloseLight.Checked = false;
		customCheckBox_MovingCloseLight.CheckImage = (Image)(object)Resources.开;
		((Control)customCheckBox_MovingCloseLight).Location = new Point(214, 359);
		((Control)customCheckBox_MovingCloseLight).Name = "customCheckBox_MovingCloseLight";
		((Control)customCheckBox_MovingCloseLight).Size = new Size(36, 18);
		((Control)customCheckBox_MovingCloseLight).TabIndex = 64;
		customCheckBox_MovingCloseLight.UncheckImage = (Image)(object)Resources.关;
		customCheckBox_MovingCloseLight.CheckChange += customCheckBox_MovingCloseLight_CheckChange;
		((Control)label_MovingCloseLight).Font = new Font("微软雅黑", 10.5f);
		((Control)label_MovingCloseLight).ForeColor = Color.White;
		label_MovingCloseLight.ImeMode = (ImeMode)0;
		((Control)label_MovingCloseLight).Location = new Point(39, 359);
		((Control)label_MovingCloseLight).Name = "label_MovingCloseLight";
		((Control)label_MovingCloseLight).Size = new Size(146, 40);
		((Control)label_MovingCloseLight).TabIndex = 63;
		((Control)label_MovingCloseLight).Text = "移动关灯光";
		((Control)label_PowerSaveTime).Font = new Font("微软雅黑", 10.5f);
		((Control)label_PowerSaveTime).ForeColor = Color.White;
		label_PowerSaveTime.ImeMode = (ImeMode)0;
		((Control)label_PowerSaveTime).Location = new Point(39, 412);
		((Control)label_PowerSaveTime).Name = "label_PowerSaveTime";
		((Control)label_PowerSaveTime).Size = new Size(146, 41);
		((Control)label_PowerSaveTime).TabIndex = 62;
		((Control)label_PowerSaveTime).Text = "静止灯光节能时间";
		((Control)label_LightSpeed).AutoSize = true;
		((Control)label_LightSpeed).Font = new Font("微软雅黑", 12f);
		((Control)label_LightSpeed).ForeColor = Color.White;
		label_LightSpeed.ImeMode = (ImeMode)0;
		((Control)label_LightSpeed).Location = new Point(39, 262);
		((Control)label_LightSpeed).Name = "label_LightSpeed";
		((Control)label_LightSpeed).Size = new Size(42, 21);
		((Control)label_LightSpeed).TabIndex = 36;
		((Control)label_LightSpeed).Text = "速度";
		((Control)label_LightBrightness).AutoSize = true;
		((Control)label_LightBrightness).Font = new Font("微软雅黑", 12f);
		((Control)label_LightBrightness).ForeColor = Color.White;
		label_LightBrightness.ImeMode = (ImeMode)0;
		((Control)label_LightBrightness).Location = new Point(39, 170);
		((Control)label_LightBrightness).Name = "label_LightBrightness";
		((Control)label_LightBrightness).Size = new Size(42, 21);
		((Control)label_LightBrightness).TabIndex = 35;
		((Control)label_LightBrightness).Text = "亮度";
		((Control)label_LightMode).AutoSize = true;
		((Control)label_LightMode).Font = new Font("微软雅黑", 12f);
		((Control)label_LightMode).ForeColor = Color.White;
		label_LightMode.ImeMode = (ImeMode)0;
		((Control)label_LightMode).Location = new Point(39, 66);
		((Control)label_LightMode).Name = "label_LightMode";
		((Control)label_LightMode).Size = new Size(74, 21);
		((Control)label_LightMode).TabIndex = 32;
		((Control)label_LightMode).Text = "灯光模式";
		((Control)tabPage_Setting).BackColor = Color.Transparent;
		((Control)tabPage_Setting).Controls.Add((Control)(object)customSetting1);
		tabPage_Setting.Location = new Point(4, 26);
		((Control)tabPage_Setting).Name = "tabPage_Setting";
		((Control)tabPage_Setting).Padding = new Padding(3);
		((Control)tabPage_Setting).Size = new Size(878, 588);
		tabPage_Setting.TabIndex = 4;
		((Control)tabPage_Setting).Text = "设置";
		((Control)customSetting1).BackColor = Color.Transparent;
		customSetting1.DongleType = 0;
		customSetting1.DongleVersion = "";
		((Control)customSetting1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customSetting1).ForeColor = Color.White;
		((Control)customSetting1).Location = new Point(0, 0);
		customSetting1.LongDistance = true;
		((Control)customSetting1).Margin = new Padding(4, 5, 4, 5);
		customSetting1.MouseVersion = "";
		((Control)customSetting1).Name = "customSetting1";
		((Control)customSetting1).Size = new Size(877, 790);
		((Control)customSetting1).TabIndex = 0;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.主界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(1020, 700);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)customButton_HomePage);
		((Control)this).Controls.Add((Control)(object)label_BatteryValue);
		((Control)this).Controls.Add((Control)(object)customButton_SystemMouse);
		((Control)this).Controls.Add((Control)(object)label_Title);
		((Control)this).Controls.Add((Control)(object)customBattery1);
		((Control)this).Controls.Add((Control)(object)customButton_Setting);
		((Control)this).Controls.Add((Control)(object)customButton_Mini);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).Controls.Add((Control)(object)customTabSelector_Main);
		((Control)this).Controls.Add((Control)(object)customTabControl_Main);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormMain";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FormMain";
		((Control)this).KeyDown += new KeyEventHandler(FormMain_KeyDown);
		((Control)contextMenuStrip_Pallet).ResumeLayout(false);
		((Control)contextMenuStrip_Keys).ResumeLayout(false);
		((Control)contextMenuStrip_SelectMacro).ResumeLayout(false);
		((Control)contextMenuStrip_UnselectMacro).ResumeLayout(false);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)customTabControl_Main).ResumeLayout(false);
		((Control)tabPage_Button).ResumeLayout(false);
		((Control)tabPage_Button).PerformLayout();
		((Control)panel_Device).ResumeLayout(false);
		((ISupportInitialize)pictureBox_Config).EndInit();
		((Control)tabPage_Sensor).ResumeLayout(false);
		((Control)tabPage_Macro).ResumeLayout(false);
		((Control)tabPage_Macro).PerformLayout();
		((Control)panel_Delay).ResumeLayout(false);
		((Control)panel_CycleProcess).ResumeLayout(false);
		((Control)tabPage_Light).ResumeLayout(false);
		((Control)tabPage_Light).PerformLayout();
		((Control)panel_LEDColor).ResumeLayout(false);
		((Control)panel_LEDColor).PerformLayout();
		((ISupportInitialize)pictureBox_Color14).EndInit();
		((ISupportInitialize)pictureBox_Color7).EndInit();
		((ISupportInitialize)pictureBox_Color13).EndInit();
		((ISupportInitialize)pictureBox_Color12).EndInit();
		((ISupportInitialize)pictureBox_Color11).EndInit();
		((ISupportInitialize)pictureBox_Color6).EndInit();
		((ISupportInitialize)pictureBox_Color5).EndInit();
		((ISupportInitialize)pictureBox_Color4).EndInit();
		((ISupportInitialize)pictureBox_Color10).EndInit();
		((ISupportInitialize)pictureBox_Color9).EndInit();
		((ISupportInitialize)pictureBox_Color8).EndInit();
		((ISupportInitialize)pictureBox_Color3).EndInit();
		((ISupportInitialize)pictureBox_Color2).EndInit();
		((ISupportInitialize)pictureBox_Color1).EndInit();
		((ISupportInitialize)pictureBox_LEDPreview).EndInit();
		((ISupportInitialize)pictureBox_Palette).EndInit();
		((Control)tabPage_Setting).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
