using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class _2In1 : UserControl
{
	public delegate void ValueChangeEventHandler(object sender, FlashDataMap e);

	private bool UpdateUIFlag;

	private int[] SensorUI;

	public FlashDataMap gFlashDataMap;

	private DriveConfig.DeviceParam gDeviceParam = new DriveConfig.DeviceParam();

	private IContainer components;

	private Panel panel_ReportRate;

	private CustomRadioButton customRadioButton_Report1000Hz;

	private CustomRadioButton customRadioButton_Report500Hz;

	private CustomRadioButton customRadioButton_Report250Hz;

	private CustomRadioButton customRadioButton_Report125Hz;

	private Label label_Report;

	private Label label_DPIBrightnessValue;

	public CustomTrackBar customTrackBar_DPIBrightness;

	private Label label_DPISpeedValue;

	public CustomTrackBar customTrackBar_DPISpeed;

	private Label label_DPIBrightness;

	private Label label_DPISpeed;

	private CustomComboBox customComboBox_DPIEffect;

	private Label label_DPIEffect;

	private PictureBox pictureBox1;

	private PictureBox pictureBox2;

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
			((Control)this).Invalidate();
		}
	}

	public event ValueChangeEventHandler ValueChange;

	public _2In1()
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
			customComboBox_DPIEffect.SelectIndex = 0;
		}
		((Control)label_DPISpeedValue).Text = FormMain.gFlashDataMap.dpiLed.breathSpeed.ToString();
		customTrackBar_DPISpeed.Value = FormMain.gFlashDataMap.dpiLed.breathSpeed;
		int value = ValueConvert.DPIBrightnessToIndex(FormMain.gFlashDataMap.dpiLed.brightness);
		((Control)label_DPIBrightnessValue).Text = value.ToString();
		customTrackBar_DPIBrightness.Value = value;
		UpdateUIFlag = false;
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
		UpdateReportRateUI(FormMain.gFlashDataMap.mouseConfig.reportRate);
		if (type > 1 && (FormMain.gFlashDataMap.mouseConfig.reportRate == 16 || FormMain.gFlashDataMap.mouseConfig.reportRate == 32))
		{
			((RadioButton)customRadioButton_Report1000Hz).Checked = true;
		}
		if (SensorUI[2] == 0)
		{
			((Control)label_DPIEffect).Visible = false;
			((Control)customComboBox_DPIEffect).Visible = false;
			((Control)label_DPISpeedValue).Visible = false;
			((Control)customTrackBar_DPISpeed).Visible = false;
			((Control)label_DPIBrightnessValue).Visible = false;
			((Control)customTrackBar_DPIBrightness).Visible = false;
			((Control)pictureBox2).Visible = false;
			((Control)label_DPISpeed).Visible = false;
			((Control)label_DPIBrightness).Visible = false;
			((Control)panel_ReportRate).Location = new Point(3, 80);
		}
		else
		{
			customComboBox_DPIEffect.SelectIndex = SensorUI[3];
			((Control)label_DPISpeedValue).Text = SensorUI[4].ToString();
			customTrackBar_DPISpeed.Value = SensorUI[4];
			int value = ValueConvert.DPIBrightnessToIndex(SensorUI[5]);
			((Control)label_DPIBrightnessValue).Text = value.ToString();
			customTrackBar_DPIBrightness.Value = value;
		}
		UpdateDPIEffect(FormMain.gFlashDataMap);
		UpdateUIFlag = false;
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
		UpdateReportRateUI((byte)SensorUI[1]);
		if (SensorUI[2] == 0)
		{
			((Control)label_DPIEffect).Visible = false;
			((Control)customComboBox_DPIEffect).Visible = false;
			((Control)label_DPISpeedValue).Visible = false;
			((Control)customTrackBar_DPISpeed).Visible = false;
			((Control)label_DPIBrightnessValue).Visible = false;
			((Control)customTrackBar_DPIBrightness).Visible = false;
			((Control)pictureBox2).Visible = false;
			((Control)label_DPISpeed).Visible = false;
			((Control)label_DPIBrightness).Visible = false;
			((Control)panel_ReportRate).Location = new Point(3, 80);
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
		UpdateUIFlag = false;
	}

	public void UpdateFlash(FlashDataMap flashDataMap)
	{
	}

	private void customRadioButton_ReportRateClick(object sender, EventArgs e)
	{
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		FormMain.gFlashDataMap.mouseConfig.reportRate = Convert.ToByte(((Control)customRadioButton).Tag);
		UpdateValue();
	}

	private void UpdateDPIEffectUI(bool[] bools)
	{
		string[] array = gDeviceParam.SensorUI.Split(new char[1] { ',' });
		SensorUI = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			SensorUI[i] = Convert.ToInt32(array[i]);
		}
		if (SensorUI[2] == 0)
		{
			customTrackBar_DPIBrightness.Enable = false;
			customTrackBar_DPISpeed.Enable = false;
			((Control)label_DPIBrightnessValue).Visible = false;
			((Control)label_DPISpeedValue).Visible = false;
			((Control)customComboBox_DPIEffect).Enabled = false;
			((Control)label_DPIEffect).ForeColor = Color.Black;
			((Control)label_DPISpeed).ForeColor = Color.Black;
			((Control)label_DPIBrightness).ForeColor = Color.Black;
		}
		else
		{
			customTrackBar_DPIBrightness.Enable = bools[0];
			customTrackBar_DPISpeed.Enable = bools[1];
			((Control)label_DPIBrightnessValue).Visible = bools[0];
			((Control)label_DPISpeedValue).Visible = bools[1];
			CustomTrackBar customTrackBar = customTrackBar_DPIBrightness;
			Color sliderColor = (customTrackBar_DPISpeed.SliderColor = FormMain.driveParam.SliderClr);
			customTrackBar.SliderColor = sliderColor;
		}
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
		UpdateValue();
	}

	private void customTrackBar_DPIBrightness_ValueChanged(object sender, CustomEventArgs e)
	{
		((Control)label_DPIBrightnessValue).Text = customTrackBar_DPIBrightness.Value.ToString();
		FormMain.gFlashDataMap.dpiLed.brightness = (byte)ValueConvert.DPIIndexToBrightness(customTrackBar_DPIBrightness.Value);
	}

	private void customTrackBar_DPIBrightness_SetValue(object sender, CustomEventArgs e)
	{
		UpdateValue();
	}

	private void UpdateValue()
	{
		if (!UpdateUIFlag)
		{
			ValueChange?.Invoke(this, FormMain.gFlashDataMap);
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Expected O, but got Unknown
		//IL_0f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f51: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		panel_ReportRate = new Panel();
		pictureBox1 = new PictureBox();
		customRadioButton_Report1000Hz = new CustomRadioButton();
		customRadioButton_Report500Hz = new CustomRadioButton();
		customRadioButton_Report250Hz = new CustomRadioButton();
		customRadioButton_Report125Hz = new CustomRadioButton();
		label_Report = new Label();
		label_DPIBrightnessValue = new Label();
		label_DPISpeedValue = new Label();
		label_DPIBrightness = new Label();
		label_DPISpeed = new Label();
		label_DPIEffect = new Label();
		customTrackBar_DPIBrightness = new CustomTrackBar();
		customTrackBar_DPISpeed = new CustomTrackBar();
		customComboBox_DPIEffect = new CustomComboBox();
		pictureBox2 = new PictureBox();
		((Control)panel_ReportRate).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)panel_ReportRate).Controls.Add((Control)(object)pictureBox1);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report1000Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report500Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report250Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)customRadioButton_Report125Hz);
		((Control)panel_ReportRate).Controls.Add((Control)(object)label_Report);
		((Control)panel_ReportRate).Location = new Point(3, 40);
		((Control)panel_ReportRate).Name = "panel_ReportRate";
		((Control)panel_ReportRate).Size = new Size(823, 94);
		((Control)panel_ReportRate).TabIndex = 91;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox1).Location = new Point(26, 16);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(33, 20);
		pictureBox1.TabIndex = 5;
		pictureBox1.TabStop = false;
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
		((Control)customRadioButton_Report1000Hz).Location = new Point(429, 45);
		customRadioButton_Report1000Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report1000Hz).Name = "customRadioButton_Report1000Hz";
		((Control)customRadioButton_Report1000Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report1000Hz).TabIndex = 4;
		((RadioButton)customRadioButton_Report1000Hz).TabStop = true;
		((Control)customRadioButton_Report1000Hz).Tag = "1";
		customRadioButton_Report1000Hz.TextString = "1000Hz";
		customRadioButton_Report1000Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report1000Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report1000Hz).Click += customRadioButton_ReportRateClick;
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
		((Control)customRadioButton_Report500Hz).Location = new Point(309, 45);
		customRadioButton_Report500Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report500Hz).Name = "customRadioButton_Report500Hz";
		((Control)customRadioButton_Report500Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report500Hz).TabIndex = 3;
		((RadioButton)customRadioButton_Report500Hz).TabStop = true;
		((Control)customRadioButton_Report500Hz).Tag = "2";
		customRadioButton_Report500Hz.TextString = "500Hz";
		customRadioButton_Report500Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report500Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report500Hz).Click += customRadioButton_ReportRateClick;
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
		((Control)customRadioButton_Report250Hz).Location = new Point(189, 45);
		customRadioButton_Report250Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report250Hz).Name = "customRadioButton_Report250Hz";
		((Control)customRadioButton_Report250Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report250Hz).TabIndex = 2;
		((RadioButton)customRadioButton_Report250Hz).TabStop = true;
		((Control)customRadioButton_Report250Hz).Tag = "4";
		customRadioButton_Report250Hz.TextString = "250Hz";
		customRadioButton_Report250Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report250Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report250Hz).Click += customRadioButton_ReportRateClick;
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
		((Control)customRadioButton_Report125Hz).Location = new Point(69, 45);
		customRadioButton_Report125Hz.MouseEnterImage = (Image)(object)Resources.回报率按键鼠标进入;
		((Control)customRadioButton_Report125Hz).Name = "customRadioButton_Report125Hz";
		((Control)customRadioButton_Report125Hz).Size = new Size(74, 26);
		((Control)customRadioButton_Report125Hz).TabIndex = 1;
		((RadioButton)customRadioButton_Report125Hz).TabStop = true;
		((Control)customRadioButton_Report125Hz).Tag = "8";
		customRadioButton_Report125Hz.TextString = "125Hz";
		customRadioButton_Report125Hz.UncheckImage = (Image)(object)Resources.回报率按键未选择;
		((ButtonBase)customRadioButton_Report125Hz).UseVisualStyleBackColor = true;
		((Control)customRadioButton_Report125Hz).Click += customRadioButton_ReportRateClick;
		((Control)label_Report).AutoSize = true;
		((Control)label_Report).ForeColor = Color.White;
		((Control)label_Report).Location = new Point(65, 16);
		((Control)label_Report).Name = "label_Report";
		((Control)label_Report).Size = new Size(64, 24);
		((Control)label_Report).TabIndex = 0;
		((Control)label_Report).Text = "回报率";
		((Control)label_DPIBrightnessValue).AutoSize = true;
		((Control)label_DPIBrightnessValue).ForeColor = Color.White;
		((Control)label_DPIBrightnessValue).Location = new Point(502, 233);
		((Control)label_DPIBrightnessValue).Name = "label_DPIBrightnessValue";
		((Control)label_DPIBrightnessValue).Size = new Size(21, 24);
		((Control)label_DPIBrightnessValue).TabIndex = 90;
		((Control)label_DPIBrightnessValue).Text = "7";
		((Control)label_DPISpeedValue).AutoSize = true;
		((Control)label_DPISpeedValue).ForeColor = Color.White;
		((Control)label_DPISpeedValue).Location = new Point(502, 198);
		((Control)label_DPISpeedValue).Name = "label_DPISpeedValue";
		((Control)label_DPISpeedValue).Size = new Size(21, 24);
		((Control)label_DPISpeedValue).TabIndex = 88;
		((Control)label_DPISpeedValue).Text = "3";
		((Control)label_DPIBrightness).AutoSize = true;
		((Control)label_DPIBrightness).ForeColor = Color.White;
		((Control)label_DPIBrightness).Location = new Point(247, 233);
		((Control)label_DPIBrightness).Name = "label_DPIBrightness";
		((Control)label_DPIBrightness).Size = new Size(46, 24);
		((Control)label_DPIBrightness).TabIndex = 86;
		((Control)label_DPIBrightness).Text = "亮度";
		((Control)label_DPISpeed).AutoSize = true;
		((Control)label_DPISpeed).ForeColor = Color.White;
		((Control)label_DPISpeed).Location = new Point(247, 198);
		((Control)label_DPISpeed).Name = "label_DPISpeed";
		((Control)label_DPISpeed).Size = new Size(46, 24);
		((Control)label_DPISpeed).TabIndex = 85;
		((Control)label_DPISpeed).Text = "速度";
		((Control)label_DPIEffect).AutoSize = true;
		((Control)label_DPIEffect).ForeColor = Color.White;
		((Control)label_DPIEffect).Location = new Point(68, 161);
		((Control)label_DPIEffect).Name = "label_DPIEffect";
		((Control)label_DPIEffect).Size = new Size(76, 24);
		((Control)label_DPIEffect).TabIndex = 83;
		((Control)label_DPIEffect).Text = "DPI灯效";
		customTrackBar_DPIBrightness.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_DPIBrightness.BarSize = 2;
		customTrackBar_DPIBrightness.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_DPIBrightness.Enable = true;
		customTrackBar_DPIBrightness.IsRound = true;
		((Control)customTrackBar_DPIBrightness).Location = new Point(332, 233);
		customTrackBar_DPIBrightness.Maximum = 10;
		customTrackBar_DPIBrightness.Minimum = 1;
		((Control)customTrackBar_DPIBrightness).Name = "customTrackBar_DPIBrightness";
		customTrackBar_DPIBrightness.Orientation = (Orientation)0;
		((Control)customTrackBar_DPIBrightness).Size = new Size(164, 23);
		customTrackBar_DPIBrightness.SizeSlidSize = new Size(6, 18);
		customTrackBar_DPIBrightness.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_DPIBrightness.Step = 1;
		((Control)customTrackBar_DPIBrightness).TabIndex = 89;
		customTrackBar_DPIBrightness.Value = 7;
		customTrackBar_DPIBrightness.ValueChanged += customTrackBar_DPIBrightness_ValueChanged;
		customTrackBar_DPIBrightness.SetValue += customTrackBar_DPIBrightness_SetValue;
		customTrackBar_DPISpeed.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar_DPISpeed.BarSize = 2;
		customTrackBar_DPISpeed.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar_DPISpeed.Enable = true;
		customTrackBar_DPISpeed.IsRound = true;
		((Control)customTrackBar_DPISpeed).Location = new Point(332, 198);
		customTrackBar_DPISpeed.Maximum = 5;
		customTrackBar_DPISpeed.Minimum = 1;
		((Control)customTrackBar_DPISpeed).Name = "customTrackBar_DPISpeed";
		customTrackBar_DPISpeed.Orientation = (Orientation)0;
		((Control)customTrackBar_DPISpeed).Size = new Size(164, 23);
		customTrackBar_DPISpeed.SizeSlidSize = new Size(6, 18);
		customTrackBar_DPISpeed.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar_DPISpeed.Step = 1;
		((Control)customTrackBar_DPISpeed).TabIndex = 87;
		customTrackBar_DPISpeed.Value = 3;
		customTrackBar_DPISpeed.ValueChanged += customTrackBar_DPISpeed_ValueChanged;
		customTrackBar_DPISpeed.SetValue += customTrackBar_DPISpeed_SetValue;
		customComboBox_DPIEffect.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_DPIEffect.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_DPIEffect).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_DPIEffect).Font = new Font("微软雅黑", 10.5f);
		((Control)customComboBox_DPIEffect).ForeColor = Color.White;
		customComboBox_DPIEffect.Item.AddRange(new object[3] { "关闭", "常亮", "呼吸" });
		((Control)customComboBox_DPIEffect).Location = new Point(74, 198);
		((Control)customComboBox_DPIEffect).Name = "customComboBox_DPIEffect";
		customComboBox_DPIEffect.SelectIndex = -1;
		customComboBox_DPIEffect.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_DPIEffect).Size = new Size(126, 26);
		((Control)customComboBox_DPIEffect).TabIndex = 84;
		customComboBox_DPIEffect.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_DPIEffect.OnSelectedIndexChanged += customComboBox_DPIEffect_OnSelectedIndexChanged;
		((Control)pictureBox2).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox2).Location = new Point(29, 161);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(33, 20);
		pictureBox2.TabIndex = 6;
		pictureBox2.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(10f, 23f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox2);
		((Control)this).Controls.Add((Control)(object)panel_ReportRate);
		((Control)this).Controls.Add((Control)(object)label_DPIBrightnessValue);
		((Control)this).Controls.Add((Control)(object)customTrackBar_DPIBrightness);
		((Control)this).Controls.Add((Control)(object)label_DPISpeedValue);
		((Control)this).Controls.Add((Control)(object)customTrackBar_DPISpeed);
		((Control)this).Controls.Add((Control)(object)label_DPIBrightness);
		((Control)this).Controls.Add((Control)(object)label_DPISpeed);
		((Control)this).Controls.Add((Control)(object)customComboBox_DPIEffect);
		((Control)this).Controls.Add((Control)(object)label_DPIEffect);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "_2In1";
		((Control)this).Size = new Size(866, 368);
		((Control)panel_ReportRate).ResumeLayout(false);
		((Control)panel_ReportRate).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
