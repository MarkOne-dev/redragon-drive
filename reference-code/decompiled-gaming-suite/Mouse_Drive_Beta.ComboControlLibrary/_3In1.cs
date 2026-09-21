using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class _3In1 : UserControl
{
	public delegate void ValueChangeEventHandler(object sender, FlashDataMap e);

	private bool UpdateUIFlag;

	private bool USBConnect;

	private int[] SensorUI;

	public FlashDataMap gFlashDataMap;

	private DriveConfig.DeviceParam gDeviceParam = new DriveConfig.DeviceParam();

	private IContainer components;

	private Panel panel_ReportRate;

	private CustomRadioButton customRadioButton_Report4000Hz;

	private CustomRadioButton customRadioButton_Report2000Hz;

	private CustomRadioButton customRadioButton_Report1000Hz;

	private CustomRadioButton customRadioButton_Report500Hz;

	private CustomRadioButton customRadioButton_Report250Hz;

	private CustomRadioButton customRadioButton_Report125Hz;

	private Label label_Report;

	private PictureBox pictureBox1;

	private CustomRadioButton customRadioButton_Report8000Hz;

	private Panel panel_Sensor;

	private PictureBox pictureBox2;

	private CustomCheckBox customCheckBox_FullPerformance;

	private CustomComboBox customComboBox_LOD;

	private CustomComboBox customComboBox_ModeSelect;

	private CustomComboBox customComboBox_FullPerformance;

	private CustomCheckBox customCheckBox_MotionSync;

	private CustomCheckBox customCheckBox_FixLine;

	private CustomCheckBox customCheckBox_Ripple;

	private Label label_MotionSync;

	private Label label_FixLine;

	private Label label_Ripple;

	private Label label_LOD;

	private Label label_ModeSelect;

	private Label label_FullPerformance;

	private Label label_SensorSetting;

	private Panel panel_DPIEffect;

	private PictureBox pictureBox3;

	private Label label_DPIBrightnessValue;

	public CustomTrackBar customTrackBar_DPIBrightness;

	private Label label_DPISpeedValue;

	public CustomTrackBar customTrackBar_DPISpeed;

	private Label label_DPIBrightness;

	private Label label_DPISpeed;

	private CustomComboBox customComboBox_DPIEffect;

	private Label label_DPIEffect;

	private ToolTip toolTip;

	public Image CheckImage
	{
		get
		{
			return customRadioButton_Report125Hz.CheckImage;
		}
		set
		{
			customRadioButton_Report125Hz.CheckImage = value;
			customRadioButton_Report250Hz.CheckImage = value;
			customRadioButton_Report500Hz.CheckImage = value;
			customRadioButton_Report1000Hz.CheckImage = value;
			customRadioButton_Report2000Hz.CheckImage = value;
			customRadioButton_Report4000Hz.CheckImage = value;
			customRadioButton_Report8000Hz.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image UncheckImage
	{
		get
		{
			return customRadioButton_Report125Hz.UncheckImage;
		}
		set
		{
			customRadioButton_Report125Hz.UncheckImage = value;
			customRadioButton_Report250Hz.UncheckImage = value;
			customRadioButton_Report500Hz.UncheckImage = value;
			customRadioButton_Report1000Hz.UncheckImage = value;
			customRadioButton_Report2000Hz.UncheckImage = value;
			customRadioButton_Report4000Hz.UncheckImage = value;
			customRadioButton_Report8000Hz.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image FullCheckImage
	{
		get
		{
			return customCheckBox_FullPerformance.CheckImage;
		}
		set
		{
			customCheckBox_FullPerformance.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image FullUncheckImage
	{
		get
		{
			return customCheckBox_FullPerformance.UncheckImage;
		}
		set
		{
			customCheckBox_FullPerformance.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image SensorCheckImage
	{
		get
		{
			return customCheckBox_FixLine.CheckImage;
		}
		set
		{
			customCheckBox_FixLine.CheckImage = value;
			customCheckBox_MotionSync.CheckImage = value;
			customCheckBox_Ripple.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image SensorUncheckImage
	{
		get
		{
			return customCheckBox_FixLine.UncheckImage;
		}
		set
		{
			customCheckBox_FixLine.UncheckImage = value;
			customCheckBox_MotionSync.UncheckImage = value;
			customCheckBox_Ripple.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public event ValueChangeEventHandler ValueChange;

	public _3In1()
	{
		InitializeComponent();
	}

	public void UpdateReportRateUI(byte value)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		int num = SensorUI[1];
		foreach (Control item in (ArrangedElementCollection)((Control)panel_ReportRate).Controls)
		{
			Control val = item;
			if (val is CustomRadioButton)
			{
				CustomRadioButton customRadioButton = (CustomRadioButton)(object)val;
				if (num < 0)
				{
					((Control)customRadioButton).Enabled = false;
				}
				if (value == Convert.ToByte(((Control)customRadioButton).Tag))
				{
					((RadioButton)customRadioButton).Checked = true;
				}
			}
		}
		UpdateModeSelect(value == 16 || value == 32 || USBConnect);
	}

	public void UpdateDPIEffect(FlashDataMap flashDataMap)
	{
		UpdateUIFlag = true;
		if (FormMain.gFlashDataMap.dpiLed.enable > 0)
		{
			customComboBox_DPIEffect.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_DPIEffect, FormMain.gFlashDataMap.dpiLed.mode);
		}
		else
		{
			customComboBox_DPIEffect.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_DPIEffect, 0);
		}
		((Control)label_DPISpeedValue).Text = FormMain.gFlashDataMap.dpiLed.breathSpeed.ToString();
		customTrackBar_DPISpeed.Value = FormMain.gFlashDataMap.dpiLed.breathSpeed;
		CustomTrackBar customTrackBar = customTrackBar_DPIBrightness;
		Color sliderColor = (customTrackBar_DPISpeed.SliderColor = FormMain.driveParam.SliderClr);
		customTrackBar.SliderColor = sliderColor;
		int value = ValueConvert.DPIBrightnessToIndex(FormMain.gFlashDataMap.dpiLed.brightness);
		((Control)label_DPIBrightnessValue).Text = value.ToString();
		customTrackBar_DPIBrightness.Value = value;
		UpdateUIFlag = false;
	}

	public void UpdateModeSelect(bool support)
	{
		if (support)
		{
			if (!customComboBox_ModeSelect.Item.Contains((object)"Corded"))
			{
				customComboBox_ModeSelect.Item.Add((object)"Corded");
			}
			((Control)customComboBox_ModeSelect).Enabled = false;
			customComboBox_ModeSelect.SelectIndex = customComboBox_ModeSelect.Item.Count - 1;
		}
		else
		{
			if (customComboBox_ModeSelect.Item.Contains((object)"Corded"))
			{
				customComboBox_ModeSelect.Item.Remove((object)"Corded");
			}
			((Control)customComboBox_ModeSelect).Enabled = true;
			customComboBox_ModeSelect.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_ModeSelect, FormMain.gFlashDataMap.mouseConfig.sensorPowerSavingModeEnable);
		}
	}

	public void SetReportRateHz(string Hz)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)((Control)panel_ReportRate).Controls)
		{
			Control val = item;
			if (val is CustomRadioButton)
			{
				CustomRadioButton obj = (CustomRadioButton)(object)val;
				obj.TextString = int.Parse(Regex.Replace(obj.TextString, "[^0-9]+", "")) + Hz;
			}
		}
	}

	public void UpdateUI(DriveConfig.DeviceParam deviceParam, FlashDataMap flashDataMap, byte type)
	{
		UpdateUIFlag = true;
		gDeviceParam = deviceParam;
		string[] array = deviceParam.SensorUI.Split(new char[1] { ',' });
		SensorUI = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SensorUI[i] = Convert.ToInt32(array[i]);
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		DeviceType deviceType = (DeviceType)type;
		flag4 = !deviceType.ToString().Contains("Wireless");
		if (deviceType.ToString().Contains("2K"))
		{
			flag = true;
		}
		if (deviceType.ToString().Contains("4K"))
		{
			flag2 = true;
		}
		if (deviceType.ToString().Contains("8K"))
		{
			flag3 = true;
		}
		USBConnect = flag4;
		((Control)customRadioButton_Report2000Hz).Visible = (flag | flag2 | flag3) && SensorUI[0] > 4;
		((Control)customRadioButton_Report4000Hz).Visible = (flag2 | flag3) && SensorUI[0] > 5;
		((Control)customRadioButton_Report8000Hz).Visible = flag3 && SensorUI[0] > 6;
		UpdateReportRateUI(FormMain.gFlashDataMap.mouseConfig.reportRate);
		if (flag4 && (FormMain.gFlashDataMap.mouseConfig.reportRate == 16 || FormMain.gFlashDataMap.mouseConfig.reportRate == 32))
		{
			((RadioButton)customRadioButton_Report1000Hz).Checked = true;
		}
		customCheckBox_FullPerformance.Checked = FormMain.gFlashDataMap.mouseConfig.sensorCustomSleepTimeEnable > 0;
		UpdateFullPerformance();
		customComboBox_FullPerformance.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_FullPerformance, FormMain.gFlashDataMap.mouseConfig.sensorSleepTime);
		byte b = (byte)ValueConvert.ComboxIndexToValue(customComboBox_FullPerformance);
		UpdateModeSelect(flag4 || FormMain.gFlashDataMap.mouseConfig.reportRate == 16 || FormMain.gFlashDataMap.mouseConfig.reportRate == 32);
		customComboBox_LOD.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_LOD, FormMain.gFlashDataMap.mouseConfig.silenceHeight);
		customCheckBox_Ripple.Checked = FormMain.gFlashDataMap.mouseConfig.rippleControlEnable > 0;
		customCheckBox_FixLine.Checked = FormMain.gFlashDataMap.mouseConfig.linearCorrectionEnable > 0;
		customCheckBox_MotionSync.Checked = FormMain.gFlashDataMap.mouseConfig.motionSyncEnable > 0;
		if (SensorUI[2] == 0)
		{
			((Control)panel_DPIEffect).Visible = false;
			((Control)panel_ReportRate).Location = new Point(((Control)panel_ReportRate).Left, 25);
			((Control)panel_Sensor).Location = new Point(((Control)panel_Sensor).Left, 160);
		}
		else
		{
			UpdateDPIEffect(FormMain.gFlashDataMap);
		}
		UpdateSensorUI(deviceParam.Sensor);
		UpdateUIFlag = false;
		if (b != FormMain.gFlashDataMap.mouseConfig.sensorSleepTime)
		{
			FormMain.gFlashDataMap.mouseConfig.sensorSleepTime = b;
			UpdateValue();
		}
	}

	public void UpdateSensorUI(string sensor)
	{
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		if (((Control)customComboBox_ModeSelect).Enabled)
		{
			((Control)customComboBox_ModeSelect).Enabled = SensorDPIFile.SupportSensorModeSel(sensor);
		}
		((Control)customComboBox_LOD).Enabled = SensorDPIFile.SupportLODCal(sensor);
		((Control)customCheckBox_Ripple).Visible = SensorDPIFile.SupportRipple(sensor);
		((Control)label_Ripple).Visible = SensorDPIFile.SupportRipple(sensor);
		((Control)customCheckBox_FixLine).Visible = SensorDPIFile.SupportFixline(sensor);
		((Control)label_FixLine).Visible = SensorDPIFile.SupportFixline(sensor);
		((Control)customCheckBox_MotionSync).Visible = SensorDPIFile.SupportMotionSync(sensor);
		((Control)label_MotionSync).Visible = SensorDPIFile.SupportMotionSync(sensor);
		if (!SensorDPIFile.SupportMotionSync(sensor))
		{
			((Control)label_Ripple).Location = new Point(((Control)label_Ripple).Left, 45);
			((Control)customCheckBox_Ripple).Location = new Point(((Control)customCheckBox_Ripple).Left, 45);
			((Control)label_FixLine).Location = new Point(((Control)label_FixLine).Left, 85);
			((Control)customCheckBox_FixLine).Location = new Point(((Control)customCheckBox_FixLine).Left, 85);
		}
		foreach (Control item in (ArrangedElementCollection)((Control)panel_Sensor).Controls)
		{
			Control val = item;
			if (val is Label)
			{
				Label val2 = (Label)val;
				if (((Control)val2).Name != "label_SensorSetting")
				{
					((Control)val2).MouseEnter += Label_MouseEnter;
				}
			}
		}
	}

	private void Label_MouseEnter(object sender, EventArgs e)
	{
		Label val = (Label)((sender is Label) ? sender : null);
		string text = ((Control)val).Name.Replace("label_", "");
		text += "Des";
		DialogEnum index = (DialogEnum)Enum.Parse(typeof(DialogEnum), text);
		toolTip.SetToolTip((Control)(object)val, LanguageFile.Dialogs[(int)index]);
	}

	public void UpdateUI(DriveConfig.DeviceParam deviceParam)
	{
		UpdateUIFlag = true;
		gDeviceParam = deviceParam;
		string[] array = deviceParam.SensorUI.Split(new char[1] { ',' });
		SensorUI = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SensorUI[i] = Convert.ToInt32(array[i]);
		}
		((Control)customRadioButton_Report2000Hz).Visible = SensorUI[0] > 4;
		((Control)customRadioButton_Report4000Hz).Visible = SensorUI[0] > 5;
		((Control)customRadioButton_Report8000Hz).Visible = SensorUI[0] > 6;
		UpdateReportRateUI((byte)SensorUI[1]);
		if (SensorUI[2] == 0)
		{
			((Control)panel_DPIEffect).Visible = false;
			((Control)panel_ReportRate).Location = new Point(((Control)panel_ReportRate).Left, 25);
			((Control)panel_Sensor).Location = new Point(((Control)panel_Sensor).Left, 160);
		}
		else
		{
			customComboBox_DPIEffect.SelectIndex = SensorUI[3];
			((Control)label_DPISpeedValue).Text = SensorUI[4].ToString();
			customTrackBar_DPISpeed.Value = SensorUI[4];
			int value = ValueConvert.DPIBrightnessToIndex(SensorUI[5]);
			((Control)label_DPIBrightnessValue).Text = value.ToString();
			customTrackBar_DPIBrightness.Value = value;
			CustomTrackBar customTrackBar = customTrackBar_DPIBrightness;
			Color sliderColor = (customTrackBar_DPISpeed.SliderColor = FormMain.driveParam.SliderClr);
			customTrackBar.SliderColor = sliderColor;
		}
		customCheckBox_FullPerformance.Checked = SensorUI[8] > 0;
		UpdateFullPerformance();
		customComboBox_FullPerformance.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_FullPerformance, SensorUI[9]);
		customComboBox_ModeSelect.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_ModeSelect, SensorUI[6]);
		customComboBox_LOD.SelectIndex = ValueConvert.ValueToComboxIndex(customComboBox_LOD, SensorUI[7]);
		customCheckBox_Ripple.Checked = SensorUI[10] > 0;
		customCheckBox_FixLine.Checked = SensorUI[11] > 0;
		customCheckBox_MotionSync.Checked = SensorUI[12] > 0;
		UpdateSensorUI(deviceParam.Sensor);
		UpdateUIFlag = false;
	}

	public void UpdateFlash(FlashDataMap flashDataMap)
	{
	}

	private void customRadioButton_ReportRate_Click(object sender, EventArgs e)
	{
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		byte b = (FormMain.gFlashDataMap.mouseConfig.reportRate = (byte)Convert.ToInt32(((Control)customRadioButton).Tag));
		UpdateModeSelect(b == 16 || b == 32 || USBConnect);
		UpdateValue();
	}

	private void UpdateDPIEffectUI(bool[] bools)
	{
		customTrackBar_DPIBrightness.Enable = bools[0];
		customTrackBar_DPISpeed.Enable = bools[1];
		((Control)label_DPIBrightnessValue).Visible = bools[0];
		((Control)label_DPISpeedValue).Visible = bools[1];
	}

	public void AddLODItems(string str)
	{
		customComboBox_LOD.Item.Add((object)str);
	}

	private void customComboBox_DPIEffect_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		int selectIndex = customComboBox_DPIEffect.SelectIndex;
		UpdateDPIEffectUI(ValueConvert.SetDPIEffect(selectIndex));
		if (!UpdateUIFlag)
		{
			if (selectIndex == 0)
			{
				FormMain.gFlashDataMap.dpiLed.enable = 0;
			}
			else
			{
				FormMain.gFlashDataMap.dpiLed.enable = 1;
				FormMain.gFlashDataMap.dpiLed.mode = (byte)selectIndex;
			}
			UpdateValue();
		}
	}

	private void customTrackBar_DPISpeed_ValueChanged(object sender, CustomEventArgs e)
	{
		FormMain.gFlashDataMap.dpiLed.breathSpeed = (byte)customTrackBar_DPISpeed.Value;
		((Control)label_DPISpeedValue).Text = FormMain.gFlashDataMap.dpiLed.breathSpeed.ToString();
	}

	private void customTrackBar_DPISpeed_SetValue(object sender, CustomEventArgs e)
	{
		if (FormMain.gFlashDataMap.dpiLed.enable > 0 && FormMain.gFlashDataMap.dpiLed.mode == 2)
		{
			UpdateValue();
		}
	}

	private void customTrackBar_DPIBrightness_ValueChanged(object sender, CustomEventArgs e)
	{
		((Control)label_DPIBrightnessValue).Text = customTrackBar_DPIBrightness.Value.ToString();
		FormMain.gFlashDataMap.dpiLed.brightness = (byte)ValueConvert.DPIIndexToBrightness(customTrackBar_DPIBrightness.Value);
	}

	private void customTrackBar_DPIBrightness_SetValue(object sender, CustomEventArgs e)
	{
		if (FormMain.gFlashDataMap.dpiLed.enable > 0 && FormMain.gFlashDataMap.dpiLed.mode == 1)
		{
			UpdateValue();
		}
	}

	private void customComboBox_FullPerformance_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		int num = ValueConvert.ComboxIndexToValue(customComboBox_FullPerformance);
		if (!UpdateUIFlag)
		{
			FormMain.gFlashDataMap.mouseConfig.sensorSleepTime = (byte)num;
			UpdateValue();
		}
	}

	private void customComboBox_ModeSelect_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		int num = ValueConvert.ComboxIndexToValue(customComboBox_ModeSelect);
		if (!UpdateUIFlag && num != 10)
		{
			FormMain.gFlashDataMap.mouseConfig.sensorPowerSavingModeEnable = (byte)num;
			UpdateValue();
		}
	}

	private void customComboBox_LOD_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		int num = ValueConvert.ComboxIndexToValue(customComboBox_LOD);
		if (!UpdateUIFlag)
		{
			FormMain.gFlashDataMap.mouseConfig.silenceHeight = (byte)num;
			UpdateValue();
		}
	}

	private void UpdateValue()
	{
		if (!UpdateUIFlag)
		{
			ValueChange?.Invoke(this, FormMain.gFlashDataMap);
		}
	}

	private void customCheckBox_Ripple_CheckChange(object sender, CustomEventArgs e)
	{
		FormMain.gFlashDataMap.mouseConfig.rippleControlEnable = (byte)(customCheckBox_Ripple.Checked ? 1u : 0u);
		UpdateValue();
	}

	private void customCheckBox_FixLine_CheckChange(object sender, CustomEventArgs e)
	{
		FormMain.gFlashDataMap.mouseConfig.linearCorrectionEnable = (byte)(customCheckBox_FixLine.Checked ? 1u : 0u);
		UpdateValue();
	}

	private void customCheckBox_MotionSync_CheckChange(object sender, CustomEventArgs e)
	{
		FormMain.gFlashDataMap.mouseConfig.motionSyncEnable = (byte)(customCheckBox_MotionSync.Checked ? 1u : 0u);
		UpdateValue();
	}

	private void customCheckBox_FullPerformance_CheckChange(object sender, CustomEventArgs e)
	{
		UpdateFullPerformance();
		FormMain.gFlashDataMap.mouseConfig.sensorCustomSleepTimeEnable = (byte)(customCheckBox_FullPerformance.Checked ? 1u : 0u);
		UpdateValue();
	}

	private void UpdateFullPerformance()
	{
		((Control)customComboBox_FullPerformance).Enabled = customCheckBox_FullPerformance.Checked;
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_1159: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Expected O, but got Unknown
		//IL_125f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1269: Expected O, but got Unknown
		//IL_1d7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d84: Expected O, but got Unknown
		//IL_1f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2c: Expected O, but got Unknown
		//IL_1f3c: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		panel_ReportRate = new Panel();
		customRadioButton_Report8000Hz = new CustomRadioButton();
		pictureBox1 = new PictureBox();
		customRadioButton_Report4000Hz = new CustomRadioButton();
		customRadioButton_Report2000Hz = new CustomRadioButton();
		customRadioButton_Report1000Hz = new CustomRadioButton();
		customRadioButton_Report500Hz = new CustomRadioButton();
		customRadioButton_Report250Hz = new CustomRadioButton();
		customRadioButton_Report125Hz = new CustomRadioButton();
		label_Report = new Label();
		panel_Sensor = new Panel();
		pictureBox2 = new PictureBox();
		customCheckBox_FullPerformance = new CustomCheckBox();
		customComboBox_LOD = new CustomComboBox();
		customComboBox_ModeSelect = new CustomComboBox();
		customComboBox_FullPerformance = new CustomComboBox();
		customCheckBox_MotionSync = new CustomCheckBox();
		customCheckBox_FixLine = new CustomCheckBox();
		customCheckBox_Ripple = new CustomCheckBox();
		label_MotionSync = new Label();
		label_FixLine = new Label();
		label_Ripple = new Label();
		label_LOD = new Label();
		label_ModeSelect = new Label();
		label_FullPerformance = new Label();
		label_SensorSetting = new Label();
		panel_DPIEffect = new Panel();
		pictureBox3 = new PictureBox();
		label_DPIBrightnessValue = new Label();
		customTrackBar_DPIBrightness = new CustomTrackBar();
		label_DPISpeedValue = new Label();
		customTrackBar_DPISpeed = new CustomTrackBar();
		label_DPIBrightness = new Label();
		label_DPISpeed = new Label();
		customComboBox_DPIEffect = new CustomComboBox();
		label_DPIEffect = new Label();
		toolTip = new ToolTip(components);
		((Control)panel_ReportRate).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)panel_Sensor).SuspendLayout();
		((ISupportInitialize)pictureBox2).BeginInit();
		((Control)panel_DPIEffect).SuspendLayout();
		((ISupportInitialize)pictureBox3).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report8000Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)pictureBox1);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report4000Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report2000Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report1000Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report500Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report250Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report125Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)label_Report);
		((Control)panel_ReportRate).Location = new Point(3, 3);
		((Control)panel_ReportRate).Name = "panel_ReportRate";
		((Control)panel_ReportRate).Size = new Size(863, 95);
		((Control)panel_ReportRate).TabIndex = 82;
		((RadioButton)customRadioButton_Report8000Hz).Appearance = (Appearance)1;
		customRadioButton_Report8000Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report8000Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report8000Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report8000Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report8000Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report8000Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report8000Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report8000Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report8000Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report8000Hz).Location = new Point(670, 54);
		customRadioButton_Report8000Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report8000Hz).Name = "customRadioButton_Report8000Hz";
		((Control)customRadioButton_Report8000Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report8000Hz).TabIndex = 8;
		((RadioButton)customRadioButton_Report8000Hz).TabStop = true;
		((Control)customRadioButton_Report8000Hz).Tag = "64";
		customRadioButton_Report8000Hz.TextString = "8000Hz";
		customRadioButton_Report8000Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report8000Hz).UseVisualStyleBackColor = true;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox1).Location = new Point(27, 17);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(33, 20);
		pictureBox1.TabIndex = 7;
		pictureBox1.TabStop = false;
		((RadioButton)customRadioButton_Report4000Hz).Appearance = (Appearance)1;
		customRadioButton_Report4000Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report4000Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report4000Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report4000Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report4000Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report4000Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report4000Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report4000Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report4000Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report4000Hz).Location = new Point(570, 54);
		customRadioButton_Report4000Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report4000Hz).Name = "customRadioButton_Report4000Hz";
		((Control)customRadioButton_Report4000Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report4000Hz).TabIndex = 6;
		((RadioButton)customRadioButton_Report4000Hz).TabStop = true;
		((Control)customRadioButton_Report4000Hz).Tag = "32";
		customRadioButton_Report4000Hz.TextString = "4000Hz";
		customRadioButton_Report4000Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report4000Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report4000Hz).Click += customRadioButton_ReportRate_Click;
		((RadioButton)customRadioButton_Report2000Hz).Appearance = (Appearance)1;
		customRadioButton_Report2000Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report2000Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report2000Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report2000Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report2000Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report2000Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report2000Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report2000Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report2000Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report2000Hz).Location = new Point(470, 54);
		customRadioButton_Report2000Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report2000Hz).Name = "customRadioButton_Report2000Hz";
		((Control)customRadioButton_Report2000Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report2000Hz).TabIndex = 5;
		((RadioButton)customRadioButton_Report2000Hz).TabStop = true;
		((Control)customRadioButton_Report2000Hz).Tag = "16";
		customRadioButton_Report2000Hz.TextString = "2000Hz";
		customRadioButton_Report2000Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report2000Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report2000Hz).Click += customRadioButton_ReportRate_Click;
		((RadioButton)customRadioButton_Report1000Hz).Appearance = (Appearance)1;
		customRadioButton_Report1000Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report1000Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report1000Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report1000Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report1000Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report1000Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report1000Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report1000Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report1000Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report1000Hz).Location = new Point(370, 54);
		customRadioButton_Report1000Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report1000Hz).Name = "customRadioButton_Report1000Hz";
		((Control)customRadioButton_Report1000Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report1000Hz).TabIndex = 4;
		((RadioButton)customRadioButton_Report1000Hz).TabStop = true;
		((Control)customRadioButton_Report1000Hz).Tag = "1";
		customRadioButton_Report1000Hz.TextString = "1000Hz";
		customRadioButton_Report1000Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report1000Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report1000Hz).Click += customRadioButton_ReportRate_Click;
		((RadioButton)customRadioButton_Report500Hz).Appearance = (Appearance)1;
		customRadioButton_Report500Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report500Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report500Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report500Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report500Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report500Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report500Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report500Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report500Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report500Hz).Location = new Point(270, 54);
		customRadioButton_Report500Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report500Hz).Name = "customRadioButton_Report500Hz";
		((Control)customRadioButton_Report500Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report500Hz).TabIndex = 3;
		((RadioButton)customRadioButton_Report500Hz).TabStop = true;
		((Control)customRadioButton_Report500Hz).Tag = "2";
		customRadioButton_Report500Hz.TextString = "500Hz";
		customRadioButton_Report500Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report500Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report500Hz).Click += customRadioButton_ReportRate_Click;
		((RadioButton)customRadioButton_Report250Hz).Appearance = (Appearance)1;
		customRadioButton_Report250Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report250Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report250Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report250Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report250Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report250Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report250Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report250Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report250Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report250Hz).Location = new Point(170, 54);
		customRadioButton_Report250Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report250Hz).Name = "customRadioButton_Report250Hz";
		((Control)customRadioButton_Report250Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report250Hz).TabIndex = 2;
		((RadioButton)customRadioButton_Report250Hz).TabStop = true;
		((Control)customRadioButton_Report250Hz).Tag = "4";
		customRadioButton_Report250Hz.TextString = "250Hz";
		customRadioButton_Report250Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report250Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report250Hz).Click += customRadioButton_ReportRate_Click;
		((RadioButton)customRadioButton_Report125Hz).Appearance = (Appearance)1;
		customRadioButton_Report125Hz.CheckImage = (Image)(object)Resources.回报率按键选择;
		customRadioButton_Report125Hz.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Report125Hz.DisableImage = (Image)(object)Resources.回报率按键不可选;
		((ButtonBase)customRadioButton_Report125Hz).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Report125Hz).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report125Hz).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report125Hz).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Report125Hz).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Report125Hz).ForeColor = Color.White;
		((Control)customRadioButton_Report125Hz).Location = new Point(70, 54);
		customRadioButton_Report125Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report125Hz).Name = "customRadioButton_Report125Hz";
		((Control)customRadioButton_Report125Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report125Hz).TabIndex = 1;
		((RadioButton)customRadioButton_Report125Hz).TabStop = true;
		((Control)customRadioButton_Report125Hz).Tag = "8";
		customRadioButton_Report125Hz.TextString = "125Hz";
		customRadioButton_Report125Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report125Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report125Hz).Click += customRadioButton_ReportRate_Click;
		((Control)label_Report).AutoSize = true;
		((Control)label_Report).ForeColor = Color.White;
		((Control)label_Report).Location = new Point(66, 17);
		((Control)label_Report).Name = "label_Report";
		((Control)label_Report).Size = new Size(51, 20);
		((Control)label_Report).TabIndex = 0;
		((Control)label_Report).Text = "回报率";
		((Control)panel_Sensor).Controls.Add((Control)(object)pictureBox2);
		((Control)panel_Sensor).Controls.Add((Control)(object)customCheckBox_FullPerformance);
		((Control)panel_Sensor).Controls.Add((Control)(object)customComboBox_LOD);
		((Control)panel_Sensor).Controls.Add((Control)(object)customComboBox_ModeSelect);
		((Control)panel_Sensor).Controls.Add((Control)(object)customComboBox_FullPerformance);
		((Control)panel_Sensor).Controls.Add((Control)(object)customCheckBox_MotionSync);
		((Control)panel_Sensor).Controls.Add((Control)(object)customCheckBox_FixLine);
		((Control)panel_Sensor).Controls.Add((Control)(object)customCheckBox_Ripple);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_MotionSync);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_FixLine);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_Ripple);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_LOD);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_ModeSelect);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_FullPerformance);
		((Control)panel_Sensor).Controls.Add((Control)(object)label_SensorSetting);
		((Control)panel_Sensor).Location = new Point(3, 97);
		((Control)panel_Sensor).Name = "panel_Sensor";
		((Control)panel_Sensor).Size = new Size(860, 133);
		((Control)panel_Sensor).TabIndex = 100;
		((Control)pictureBox2).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox2).Location = new Point(27, 13);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(33, 20);
		pictureBox2.TabIndex = 99;
		pictureBox2.TabStop = false;
		customCheckBox_FullPerformance.Checked = true;
		customCheckBox_FullPerformance.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_FullPerformance).Location = new Point(442, 47);
		((Control)customCheckBox_FullPerformance).Name = "customCheckBox_FullPerformance";
		((Control)customCheckBox_FullPerformance).Size = new Size(16, 16);
		((Control)customCheckBox_FullPerformance).TabIndex = 113;
		customCheckBox_FullPerformance.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_FullPerformance.CheckChange += customCheckBox_FullPerformance_CheckChange;
		customComboBox_LOD.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_LOD.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_LOD).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_LOD).Font = new Font("微软雅黑", 10.5f);
		customComboBox_LOD.Item.AddRange(new object[3] { "不检测静默高度", "1mm", "2mm" });
		((Control)customComboBox_LOD).Location = new Point(252, 87);
		((Control)customComboBox_LOD).Name = "customComboBox_LOD";
		customComboBox_LOD.SelectIndex = -1;
		customComboBox_LOD.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_LOD).Size = new Size(126, 26);
		((Control)customComboBox_LOD).TabIndex = 112;
		customComboBox_LOD.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_LOD.OnSelectedIndexChanged += customComboBox_LOD_OnSelectedIndexChanged;
		customComboBox_ModeSelect.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_ModeSelect.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_ModeSelect).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_ModeSelect).Font = new Font("微软雅黑", 10.5f);
		customComboBox_ModeSelect.Item.AddRange(new object[2] { "LP", "HP" });
		((Control)customComboBox_ModeSelect).Location = new Point(70, 87);
		((Control)customComboBox_ModeSelect).Name = "customComboBox_ModeSelect";
		customComboBox_ModeSelect.SelectIndex = -1;
		customComboBox_ModeSelect.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_ModeSelect).Size = new Size(126, 26);
		((Control)customComboBox_ModeSelect).TabIndex = 111;
		customComboBox_ModeSelect.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_ModeSelect.OnSelectedIndexChanged += customComboBox_ModeSelect_OnSelectedIndexChanged;
		customComboBox_FullPerformance.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_FullPerformance.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_FullPerformance).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_FullPerformance).Font = new Font("微软雅黑", 10.5f);
		customComboBox_FullPerformance.Item.AddRange(new object[3] { "关闭", "10s", "20s" });
		((Control)customComboBox_FullPerformance).Location = new Point(442, 87);
		((Control)customComboBox_FullPerformance).Name = "customComboBox_FullPerformance";
		customComboBox_FullPerformance.SelectIndex = -1;
		customComboBox_FullPerformance.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_FullPerformance).Size = new Size(126, 26);
		((Control)customComboBox_FullPerformance).TabIndex = 110;
		customComboBox_FullPerformance.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_FullPerformance.OnSelectedIndexChanged += customComboBox_FullPerformance_OnSelectedIndexChanged;
		customCheckBox_MotionSync.Checked = false;
		customCheckBox_MotionSync.CheckImage = (Image)(object)Resources.开;
		((Control)customCheckBox_MotionSync).Location = new Point(630, 95);
		((Control)customCheckBox_MotionSync).Name = "customCheckBox_MotionSync";
		((Control)customCheckBox_MotionSync).Size = new Size(36, 18);
		((Control)customCheckBox_MotionSync).TabIndex = 109;
		customCheckBox_MotionSync.UncheckImage = (Image)(object)Resources.关;
		customCheckBox_MotionSync.CheckChange += customCheckBox_MotionSync_CheckChange;
		customCheckBox_FixLine.Checked = false;
		customCheckBox_FixLine.CheckImage = (Image)(object)Resources.开;
		((Control)customCheckBox_FixLine).Location = new Point(630, 61);
		((Control)customCheckBox_FixLine).Name = "customCheckBox_FixLine";
		((Control)customCheckBox_FixLine).Size = new Size(36, 18);
		((Control)customCheckBox_FixLine).TabIndex = 108;
		customCheckBox_FixLine.UncheckImage = (Image)(object)Resources.关;
		customCheckBox_FixLine.CheckChange += customCheckBox_FixLine_CheckChange;
		customCheckBox_Ripple.Checked = false;
		customCheckBox_Ripple.CheckImage = (Image)(object)Resources.开;
		((Control)customCheckBox_Ripple).Location = new Point(630, 27);
		((Control)customCheckBox_Ripple).Name = "customCheckBox_Ripple";
		((Control)customCheckBox_Ripple).Size = new Size(36, 18);
		((Control)customCheckBox_Ripple).TabIndex = 107;
		customCheckBox_Ripple.UncheckImage = (Image)(object)Resources.关;
		customCheckBox_Ripple.CheckChange += customCheckBox_Ripple_CheckChange;
		((Control)label_MotionSync).AutoSize = true;
		((Control)label_MotionSync).Location = new Point(672, 93);
		((Control)label_MotionSync).Name = "label_MotionSync";
		((Control)label_MotionSync).Size = new Size(91, 20);
		((Control)label_MotionSync).TabIndex = 106;
		((Control)label_MotionSync).Text = "motion sync";
		((Control)label_FixLine).AutoSize = true;
		((Control)label_FixLine).Location = new Point(672, 59);
		((Control)label_FixLine).Name = "label_FixLine";
		((Control)label_FixLine).Size = new Size(65, 20);
		((Control)label_FixLine).TabIndex = 105;
		((Control)label_FixLine).Text = "直线修正";
		((Control)label_Ripple).AutoSize = true;
		((Control)label_Ripple).Location = new Point(672, 27);
		((Control)label_Ripple).Name = "label_Ripple";
		((Control)label_Ripple).Size = new Size(65, 20);
		((Control)label_Ripple).TabIndex = 104;
		((Control)label_Ripple).Text = "波纹控制";
		((Control)label_LOD).AutoSize = true;
		((Control)label_LOD).Location = new Point(248, 43);
		((Control)label_LOD).Name = "label_LOD";
		((Control)label_LOD).Size = new Size(38, 20);
		((Control)label_LOD).TabIndex = 103;
		((Control)label_LOD).Text = "LOD";
		((Control)label_ModeSelect).AutoSize = true;
		((Control)label_ModeSelect).Location = new Point(66, 43);
		((Control)label_ModeSelect).Name = "label_ModeSelect";
		((Control)label_ModeSelect).Size = new Size(65, 20);
		((Control)label_ModeSelect).TabIndex = 102;
		((Control)label_ModeSelect).Text = "模式选择";
		((Control)label_FullPerformance).Location = new Point(466, 43);
		((Control)label_FullPerformance).Name = "label_FullPerformance";
		((Control)label_FullPerformance).Size = new Size(145, 40);
		((Control)label_FullPerformance).TabIndex = 101;
		((Control)label_FullPerformance).Text = "性能全开";
		((Control)label_SensorSetting).AutoSize = true;
		((Control)label_SensorSetting).Location = new Point(66, 13);
		((Control)label_SensorSetting).Name = "label_SensorSetting";
		((Control)label_SensorSetting).Size = new Size(82, 20);
		((Control)label_SensorSetting).TabIndex = 100;
		((Control)label_SensorSetting).Text = "Sensor设置";
		((Control)panel_DPIEffect).Controls.Add((Control)(object)pictureBox3);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)label_DPIBrightnessValue);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)customTrackBar_DPIBrightness);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)label_DPISpeedValue);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)customTrackBar_DPISpeed);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)label_DPIBrightness);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)label_DPISpeed);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)customComboBox_DPIEffect);
		((Control)panel_DPIEffect).Controls.Add((Control)(object)label_DPIEffect);
		((Control)panel_DPIEffect).Location = new Point(3, 233);
		((Control)panel_DPIEffect).Name = "panel_DPIEffect";
		((Control)panel_DPIEffect).Size = new Size(860, 129);
		((Control)panel_DPIEffect).TabIndex = 101;
		((Control)pictureBox3).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox3).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox3).Location = new Point(27, 4);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(33, 20);
		pictureBox3.TabIndex = 108;
		pictureBox3.TabStop = false;
		((Control)label_DPIBrightnessValue).AutoSize = true;
		((Control)label_DPIBrightnessValue).ForeColor = Color.White;
		((Control)label_DPIBrightnessValue).Location = new Point(455, 82);
		((Control)label_DPIBrightnessValue).Name = "label_DPIBrightnessValue";
		((Control)label_DPIBrightnessValue).Size = new Size(17, 20);
		((Control)label_DPIBrightnessValue).TabIndex = 107;
		((Control)label_DPIBrightnessValue).Text = "7";
		customTrackBar_DPIBrightness.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_DPIBrightness.BarSize = 2;
		customTrackBar_DPIBrightness.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_DPIBrightness.Enable = true;
		customTrackBar_DPIBrightness.IsRound = true;
		((Control)customTrackBar_DPIBrightness).Location = new Point(327, 82);
		customTrackBar_DPIBrightness.Maximum = 10;
		customTrackBar_DPIBrightness.Minimum = 1;
		((Control)customTrackBar_DPIBrightness).Name = "customTrackBar_DPIBrightness";
		customTrackBar_DPIBrightness.Orientation = (Orientation)0;
		((Control)customTrackBar_DPIBrightness).Size = new Size(122, 23);
		customTrackBar_DPIBrightness.SizeSlidSize = new Size(6, 18);
		customTrackBar_DPIBrightness.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_DPIBrightness.Step = 1;
		((Control)customTrackBar_DPIBrightness).TabIndex = 106;
		customTrackBar_DPIBrightness.Value = 3;
		customTrackBar_DPIBrightness.ValueChanged += customTrackBar_DPIBrightness_ValueChanged;
		customTrackBar_DPIBrightness.SetValue += customTrackBar_DPIBrightness_SetValue;
		((Control)label_DPISpeedValue).AutoSize = true;
		((Control)label_DPISpeedValue).ForeColor = Color.White;
		((Control)label_DPISpeedValue).Location = new Point(455, 45);
		((Control)label_DPISpeedValue).Name = "label_DPISpeedValue";
		((Control)label_DPISpeedValue).Size = new Size(17, 20);
		((Control)label_DPISpeedValue).TabIndex = 105;
		((Control)label_DPISpeedValue).Text = "3";
		customTrackBar_DPISpeed.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_DPISpeed.BarSize = 2;
		customTrackBar_DPISpeed.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_DPISpeed.Enable = true;
		customTrackBar_DPISpeed.IsRound = true;
		((Control)customTrackBar_DPISpeed).Location = new Point(327, 45);
		customTrackBar_DPISpeed.Maximum = 5;
		customTrackBar_DPISpeed.Minimum = 1;
		((Control)customTrackBar_DPISpeed).Name = "customTrackBar_DPISpeed";
		customTrackBar_DPISpeed.Orientation = (Orientation)0;
		((Control)customTrackBar_DPISpeed).Size = new Size(122, 23);
		customTrackBar_DPISpeed.SizeSlidSize = new Size(6, 18);
		customTrackBar_DPISpeed.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_DPISpeed.Step = 1;
		((Control)customTrackBar_DPISpeed).TabIndex = 104;
		customTrackBar_DPISpeed.Value = 3;
		customTrackBar_DPISpeed.ValueChanged += customTrackBar_DPISpeed_ValueChanged;
		customTrackBar_DPISpeed.SetValue += customTrackBar_DPISpeed_SetValue;
		((Control)label_DPIBrightness).AutoSize = true;
		((Control)label_DPIBrightness).ForeColor = Color.White;
		((Control)label_DPIBrightness).Location = new Point(248, 82);
		((Control)label_DPIBrightness).Name = "label_DPIBrightness";
		((Control)label_DPIBrightness).Size = new Size(37, 20);
		((Control)label_DPIBrightness).TabIndex = 103;
		((Control)label_DPIBrightness).Text = "亮度";
		((Control)label_DPISpeed).AutoSize = true;
		((Control)label_DPISpeed).ForeColor = Color.White;
		((Control)label_DPISpeed).Location = new Point(248, 45);
		((Control)label_DPISpeed).Name = "label_DPISpeed";
		((Control)label_DPISpeed).Size = new Size(37, 20);
		((Control)label_DPISpeed).TabIndex = 102;
		((Control)label_DPISpeed).Text = "速度";
		customComboBox_DPIEffect.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_DPIEffect.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_DPIEffect).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_DPIEffect).Font = new Font("微软雅黑", 10.5f);
		customComboBox_DPIEffect.Item.AddRange(new object[3] { "关闭", "常亮", "呼吸" });
		((Control)customComboBox_DPIEffect).Location = new Point(70, 45);
		((Control)customComboBox_DPIEffect).Name = "customComboBox_DPIEffect";
		customComboBox_DPIEffect.SelectIndex = -1;
		customComboBox_DPIEffect.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_DPIEffect).Size = new Size(126, 26);
		((Control)customComboBox_DPIEffect).TabIndex = 101;
		customComboBox_DPIEffect.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_DPIEffect.OnSelectedIndexChanged += customComboBox_DPIEffect_OnSelectedIndexChanged;
		((Control)label_DPIEffect).AutoSize = true;
		((Control)label_DPIEffect).ForeColor = Color.White;
		((Control)label_DPIEffect).Location = new Point(66, 4);
		((Control)label_DPIEffect).Name = "label_DPIEffect";
		((Control)label_DPIEffect).Size = new Size(61, 20);
		((Control)label_DPIEffect).TabIndex = 100;
		((Control)label_DPIEffect).Text = "DPI灯效";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)panel_DPIEffect);
		((Control)this).Controls.Add((Control)(object)panel_Sensor);
		((Control)this).Controls.Add((Control)(object)panel_ReportRate);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "_3In1";
		((Control)this).Size = new Size(866, 368);
		((Control)panel_ReportRate).ResumeLayout(false);
		((Control)panel_ReportRate).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)panel_Sensor).ResumeLayout(false);
		((Control)panel_Sensor).PerformLayout();
		((ISupportInitialize)pictureBox2).EndInit();
		((Control)panel_DPIEffect).ResumeLayout(false);
		((Control)panel_DPIEffect).PerformLayout();
		((ISupportInitialize)pictureBox3).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
