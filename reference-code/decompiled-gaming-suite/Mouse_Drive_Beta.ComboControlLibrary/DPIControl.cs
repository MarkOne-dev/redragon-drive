using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class DPIControl : UserControl
{
	public delegate void ValueChangeEventHandler(object sender, FlashDataMap e);

	private bool IsUpdateUI;

	public bool UpdateUIFlag;

	public FlashDataMap gFlashDataMap;

	private List<CustomDPIButton> CustomDPIButtons;

	private Timer PressTimer;

	private int LastDPIGrade;

	private int SelectIndex;

	private SensorDPI Sensor;

	private int _MinValue;

	private int _MaxValue;

	private int _Step;

	private int _Value;

	private int _MaxGrade;

	private bool TrackBarMouseDown;

	private int pressCount;

	private MouseStatus SubMouseStatus;

	private MouseStatus AddMouseStatus;

	private MouseStatus mouseStatus;

	private Point buttomPoint;

	private Point mousePoint;

	private IContainer components;

	private Label label_DPIGrade;

	private CustomComboBox customComboBox_DPIGrade;

	private Panel panel1;

	private CustomTrackBar customTrackBar1;

	private CustomDPIButton customDPIButton1;

	private CustomDPIButton customDPIButton2;

	private CustomDPIButton customDPIButton3;

	private CustomDPIButton customDPIButton4;

	private CustomDPIButton customDPIButton5;

	private CustomDPIButton customDPIButton6;

	private CustomDPIButton customDPIButton7;

	private CustomDPIButton customDPIButton8;

	private PictureBox pictureBox1;

	private CustomButton customButton_Sub;

	private CustomButton customButton_Add;

	private CustomButton customButton_CurrentDPI;

	private TextBox textBox_DPIValue;

	[Category("自定义")]
	[Description("最小值")]
	public int MinValue
	{
		get
		{
			return _MinValue;
		}
		set
		{
			_MinValue = value;
			customTrackBar1.Minimum = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("最大值")]
	public int MaxValue
	{
		get
		{
			return _MaxValue;
		}
		set
		{
			_MaxValue = value;
			customTrackBar1.Maximum = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("一格的大小")]
	public int Step
	{
		get
		{
			return _Step;
		}
		set
		{
			_Step = value;
			customTrackBar1.Step = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("当前值")]
	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
			customTrackBar1.Value = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("支持最多几档DPI")]
	public int MaxGrade
	{
		get
		{
			return _MaxGrade;
		}
		set
		{
			_MaxGrade = value;
			customComboBox_DPIGrade.Item.Clear();
			for (int i = 1; i < _MaxGrade + 1; i++)
			{
				customComboBox_DPIGrade.Item.Add((object)i);
			}
			((Control)this).Invalidate();
		}
	}

	public Image CurrentDPINormalImage
	{
		get
		{
			return customButton_CurrentDPI.NormalImage;
		}
		set
		{
			customButton_CurrentDPI.NormalImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image CurrentDPIMouseDownImage
	{
		get
		{
			return customButton_CurrentDPI.MouseDownImage;
		}
		set
		{
			customButton_CurrentDPI.MouseDownImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image CurrentDPIMouseEnterImage
	{
		get
		{
			return customButton_CurrentDPI.MouseEnterImage;
		}
		set
		{
			customButton_CurrentDPI.MouseEnterImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image DPICheckImage
	{
		get
		{
			return customDPIButton1.CheckImage;
		}
		set
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is CustomDPIButton)
				{
					((CustomDPIButton)(object)val).CheckImage = value;
				}
			}
			((Control)this).Invalidate();
		}
	}

	public Image DPIUncheckImage
	{
		get
		{
			return customDPIButton1.UncheckImage;
		}
		set
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is CustomDPIButton)
				{
					((CustomDPIButton)(object)val).UncheckImage = value;
				}
			}
			((Control)this).Invalidate();
		}
	}

	public Image TitlePicture
	{
		get
		{
			return ((Control)pictureBox1).BackgroundImage;
		}
		set
		{
			((Control)pictureBox1).BackgroundImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image SubImage
	{
		get
		{
			return customButton_Sub.NormalImage;
		}
		set
		{
			customButton_Sub.NormalImage = value;
			customButton_Sub.MouseDownImage = value;
			customButton_Sub.MouseEnterImage = value;
		}
	}

	public Image AddImage
	{
		get
		{
			return customButton_Add.NormalImage;
		}
		set
		{
			customButton_Add.NormalImage = value;
			customButton_Add.MouseDownImage = value;
			customButton_Add.MouseEnterImage = value;
		}
	}

	public event ValueChangeEventHandler ValueChange;

	public DPIControl()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		CustomDPIButtons = new List<CustomDPIButton>();
		SelectIndex = -1;
		_MinValue = 50;
		_MaxValue = 30000;
		_Step = 50;
		_Value = 1500;
		_MaxGrade = 8;
		SubMouseStatus = MouseStatus.Up;
		AddMouseStatus = MouseStatus.Up;
		mouseStatus = MouseStatus.Up;
		((UserControl)this)._002Ector();
		InitializeComponent();
		SetStyles();
		PressTimer = new Timer();
		PressTimer.Enabled = false;
		PressTimer.Interval = 100;
		PressTimer.Tick += PressTimer_Tick;
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	private void UpdateDPIButton()
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		int num = Convert.ToInt32(customComboBox_DPIGrade.Item[customComboBox_DPIGrade.Item.Count - 1].ToString());
		CustomDPIButtons.Clear();
		for (int i = 1; i < 9; i++)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is CustomDPIButton)
				{
					CustomDPIButton customDPIButton = (CustomDPIButton)(object)val;
					if (val.Name == "customDPIButton" + i)
					{
						((Control)customDPIButton).Tag = i - 1;
						((Control)customDPIButton).Click -= CustomDPIButton_Click;
						customDPIButton.ColorChange -= CustomDPIButton_ColorChange;
						((Control)customDPIButton).Click += CustomDPIButton_Click;
						customDPIButton.ColorChange += CustomDPIButton_ColorChange;
						string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
						customDPIButton.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\btn_DPI_Select.png");
						customDPIButton.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\3Sensor\\btn_DPI_Unselect.png");
						CustomDPIButtons.Add(customDPIButton);
						((Control)CustomDPIButtons[i - 1]).Visible = i - 1 < num;
						break;
					}
				}
			}
		}
	}

	public bool UpdateUI(FlashDataMap flashDataMap, SensorDPI sensor)
	{
		bool result = true;
		int num = 0;
		IsUpdateUI = true;
		UpdateDPIButton();
		try
		{
			int num2 = (customComboBox_DPIGrade.SelectIndex = FormMain.gFlashDataMap.mouseConfig.maxDPI - 1);
			num = num2;
		}
		catch
		{
			result = false;
			int num2 = (customComboBox_DPIGrade.SelectIndex = customComboBox_DPIGrade.Item.Count - 1);
			num = num2;
			FormMain.gFlashDataMap.mouseConfig.maxDPI = (byte)customComboBox_DPIGrade.Item.Count;
			if (FormMain.gFlashDataMap.mouseConfig.currentDPI > num)
			{
				FormMain.gFlashDataMap.mouseConfig.currentDPI = (byte)num;
			}
		}
		num++;
		Sensor = sensor;
		for (int i = 0; i < 8; i++)
		{
			((Control)CustomDPIButtons[i]).Visible = i < num;
			int num5 = FormMain.gFlashDataMap.dpiConfig[i].xDPI;
			int dPIex = FormMain.gFlashDataMap.dpiConfig[i].DPIex;
			int num6 = 0;
			if (Sensor.Discord)
			{
				for (int j = 0; j < Sensor.DPIList.Count; j++)
				{
					if (Sensor.DPIValueList[j] == num5)
					{
						num6 = Sensor.DPIList[j];
						break;
					}
				}
				if ((dPIex & 0x22) > 0)
				{
					num6 *= 2;
				}
			}
			else
			{
				if (Sensor.Type == "3395")
				{
					int num7 = dPIex >> 6;
					num7 <<= 8;
					num5 |= num7;
				}
				num6 = (((dPIex & 0x22) <= 0) ? ((num5 + 1) * 50) : ((num5 + 1) * 100));
			}
			if ((dPIex & 0x11) > 0)
			{
				num6 *= 2;
			}
			CustomDPIButtons[i].TextString = num6.ToString();
			Color dPIColor = Color.FromArgb(FormMain.gFlashDataMap.dpiConfig[i].color[0], FormMain.gFlashDataMap.dpiConfig[i].color[1], FormMain.gFlashDataMap.dpiConfig[i].color[2]);
			CustomDPIButtons[i].DPIColor = dPIColor;
			if (i == FormMain.gFlashDataMap.mouseConfig.currentDPI)
			{
				((RadioButton)CustomDPIButtons[i]).Checked = true;
				CustomDPIButton_Click(CustomDPIButtons[i], null);
			}
		}
		LastDPIGrade = (SelectIndex = FormMain.gFlashDataMap.mouseConfig.currentDPI);
		customTrackBar1.Value = Convert.ToInt32(CustomDPIButtons[SelectIndex].TextString);
		UpdateCurrentDPI(FormMain.gFlashDataMap.mouseConfig.currentDPI);
		IsUpdateUI = false;
		UpdateUIFlag = true;
		return result;
	}

	public void UpdateUI(DriveConfig.DeviceParam deviceParam)
	{
		IsUpdateUI = true;
		UpdateDPIButton();
		for (int i = 0; i < deviceParam.DPIMaxGrade; i++)
		{
			((Control)CustomDPIButtons[i]).Visible = i < deviceParam.DPIMaxGrade;
			CustomDPIButtons[i].TextString = deviceParam.DPIGrade[i].ToString();
			CustomDPIButtons[i].DPIColor = deviceParam.DPIColor[i];
		}
		UpdateCurrentDPI(deviceParam.DefaultDPI - 1);
		customComboBox_DPIGrade.SelectIndex = deviceParam.DPIMaxGrade - 1;
		SelectIndex = deviceParam.DefaultDPI - 1;
		_Value = Convert.ToInt32(CustomDPIButtons[SelectIndex].TextString);
		customTrackBar1.Value = _Value;
		((Control)textBox_DPIValue).Text = _Value.ToString();
		IsUpdateUI = false;
		UpdateUIFlag = true;
	}

	public void UpdateFlash(FlashDataMap flashDataMap)
	{
	}

	public void UpdateCurrentDPI(int index)
	{
		FormMain.gFlashDataMap.mouseConfig.currentDPI = (byte)index;
		for (int i = 0; i < FormMain.gFlashDataMap.mouseConfig.maxDPI; i++)
		{
			CustomDPIButtons[i].CurrentDPI = false;
		}
		CustomDPIButtons[index].CurrentDPI = true;
		((Control)customButton_CurrentDPI).Location = new Point(((Control)CustomDPIButtons[index]).Left + (((Control)CustomDPIButtons[index]).Width - ((Control)customButton_CurrentDPI).Width) / 2, ((Control)CustomDPIButtons[index]).Bottom + 3);
		((RadioButton)CustomDPIButtons[index]).Checked = true;
		_Value = Convert.ToInt32(CustomDPIButtons[index].TextString);
		((Control)textBox_DPIValue).Text = _Value.ToString();
		customTrackBar1.Value = _Value;
		FormMain.UpdateDllFlashMap();
	}

	private void UpdateValue()
	{
		if (!IsUpdateUI)
		{
			ValueChange?.Invoke(this, FormMain.gFlashDataMap);
		}
	}

	private void CustomDPIButton_Click(object sender, EventArgs e)
	{
		if (IsUpdateUI)
		{
			return;
		}
		int num = int.Parse(((Control)(CustomDPIButton)sender).Tag.ToString());
		if (FormMain.GetDeviceOnlineFlag())
		{
			CustomDPIButton customDPIButton = (CustomDPIButton)sender;
			SelectIndex = (int)((Control)customDPIButton).Tag;
			_Value = Convert.ToInt32(CustomDPIButtons[SelectIndex].TextString);
			((Control)textBox_DPIValue).Text = _Value.ToString();
			customTrackBar1.Value = _Value;
			FormMain.gFlashDataMap.mouseConfig.currentDPI = (byte)num;
			UpdateValue();
			int x = ((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Left + (((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Width - ((Control)customButton_CurrentDPI).Width) / 2;
			int y = ((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Bottom + 3;
			if (FormMain.gFlashDataMap.mouseConfig.currentDPI != num)
			{
				SelectIndex = num;
				x = ((Control)CustomDPIButtons[num]).Left + (((Control)CustomDPIButtons[num]).Width - ((Control)customButton_CurrentDPI).Width) / 2;
				y = ((Control)CustomDPIButtons[num]).Bottom + 3;
			}
			((Control)customButton_CurrentDPI).Location = new Point(x, y);
		}
	}

	private void CustomDPIButton_ColorChange(object sender, Color e)
	{
		if (UpdateUIFlag && FormMain.GetDeviceOnlineFlag() && !IsUpdateUI && (FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[0] != CustomDPIButtons[SelectIndex].DPIColor.R || FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[1] != CustomDPIButtons[SelectIndex].DPIColor.G || FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[2] != CustomDPIButtons[SelectIndex].DPIColor.B))
		{
			FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[0] = CustomDPIButtons[SelectIndex].DPIColor.R;
			FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[1] = CustomDPIButtons[SelectIndex].DPIColor.G;
			FormMain.gFlashDataMap.dpiConfig[SelectIndex].color[2] = CustomDPIButtons[SelectIndex].DPIColor.B;
			UpdateValue();
		}
	}

	private void customComboBox_DPIGrade_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (!IsUpdateUI && UpdateUIFlag && FormMain.GetDeviceOnlineFlag() && !IsUpdateUI)
		{
			byte b = Convert.ToByte(customComboBox_DPIGrade.Item[customComboBox_DPIGrade.SelectIndex]);
			FormMain.gFlashDataMap.mouseConfig.maxDPI = b;
			b--;
			if (FormMain.gFlashDataMap.mouseConfig.currentDPI > b)
			{
				LastDPIGrade = FormMain.gFlashDataMap.mouseConfig.currentDPI;
				string textString = CustomDPIButtons[b].TextString;
				CustomDPIButtons[b].TextString = CustomDPIButtons[LastDPIGrade].TextString;
				CustomDPIButtons[LastDPIGrade].TextString = textString;
				Color dPIColor = CustomDPIButtons[b].DPIColor;
				CustomDPIButtons[b].DPIColor = CustomDPIButtons[LastDPIGrade].DPIColor;
				CustomDPIButtons[LastDPIGrade].DPIColor = dPIColor;
				byte xDPI = FormMain.gFlashDataMap.dpiConfig[b].xDPI;
				FormMain.gFlashDataMap.dpiConfig[b].xDPI = FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].xDPI;
				FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].xDPI = xDPI;
				xDPI = FormMain.gFlashDataMap.dpiConfig[b].yDPI;
				FormMain.gFlashDataMap.dpiConfig[b].yDPI = FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].yDPI;
				FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].yDPI = xDPI;
				xDPI = FormMain.gFlashDataMap.dpiConfig[b].color[0];
				FormMain.gFlashDataMap.dpiConfig[b].color[0] = FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[0];
				FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[0] = xDPI;
				xDPI = FormMain.gFlashDataMap.dpiConfig[b].color[1];
				FormMain.gFlashDataMap.dpiConfig[b].color[1] = FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[1];
				FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[1] = xDPI;
				xDPI = FormMain.gFlashDataMap.dpiConfig[b].color[2];
				FormMain.gFlashDataMap.dpiConfig[b].color[2] = FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[2];
				FormMain.gFlashDataMap.dpiConfig[LastDPIGrade].color[2] = xDPI;
				FormMain.gFlashDataMap.mouseConfig.currentDPI = b;
				SelectIndex = b;
				_Value = Convert.ToInt32(CustomDPIButtons[b].TextString);
				((Control)textBox_DPIValue).Text = _Value.ToString();
				customTrackBar1.Value = _Value;
				((RadioButton)CustomDPIButtons[SelectIndex]).Checked = true;
				((Control)customButton_CurrentDPI).Location = new Point(((Control)CustomDPIButtons[b]).Left + (((Control)CustomDPIButtons[b]).Width - ((Control)customButton_CurrentDPI).Width) / 2, ((Control)CustomDPIButtons[b]).Bottom + 3);
			}
			for (int i = 0; i < CustomDPIButtons.Count; i++)
			{
				((Control)CustomDPIButtons[i]).Visible = i < FormMain.gFlashDataMap.mouseConfig.maxDPI;
			}
			UpdateValue();
		}
	}

	private void customTrackBar1_ValueChanged(object sender, CustomEventArgs e)
	{
		if (!UpdateUIFlag)
		{
			return;
		}
		if (!TrackBarMouseDown)
		{
			TrackBarMouseDown = true;
			if (!FormMain.GetDeviceOnlineFlag())
			{
				return;
			}
		}
		if (IsUpdateUI)
		{
			return;
		}
		for (int num = Sensor.Min.Count - 1; num > 0; num--)
		{
			if (customTrackBar1.Value >= Sensor.Min[num] - Sensor.Step[num])
			{
				customTrackBar1.Value = customTrackBar1.Value / Sensor.Step[num] * Sensor.Step[num];
				break;
			}
		}
		_Value = customTrackBar1.Value;
		SetValue();
	}

	private void customTrackBar1_SetValue(object sender, CustomEventArgs e)
	{
		TrackBarMouseDown = false;
		ValueToDPI(_Value);
	}

	private void PressTimer_Tick(object sender, EventArgs e)
	{
		if (AddMouseStatus == MouseStatus.Down)
		{
			if (pressCount < 15)
			{
				pressCount++;
			}
			if (pressCount > 10)
			{
				for (int num = Sensor.Min.Count - 1; num >= 0; num--)
				{
					if (_Value >= Sensor.Min[num] - Sensor.Step[num])
					{
						if (_Value < _MaxValue)
						{
							_Value += Sensor.Step[num];
						}
						break;
					}
				}
			}
		}
		else if (SubMouseStatus == MouseStatus.Down)
		{
			if (pressCount < 15)
			{
				pressCount++;
			}
			if (pressCount > 10)
			{
				for (int i = 0; i < Sensor.Max.Count; i++)
				{
					if (_Value <= Sensor.Max[i])
					{
						if (_Value > _MinValue)
						{
							_Value -= Sensor.Step[i];
						}
						break;
					}
				}
			}
		}
		else
		{
			pressCount = 0;
		}
		SetValue();
	}

	private void customButton_Add_MouseDown(object sender, MouseEventArgs e)
	{
		if (AddMouseStatus == MouseStatus.Down || !FormMain.GetDeviceOnlineFlag())
		{
			return;
		}
		AddMouseStatus = MouseStatus.Down;
		for (int num = Sensor.Min.Count - 1; num >= 0; num--)
		{
			if (_Value >= Sensor.Min[num] - Sensor.Step[num])
			{
				if (_Value < _MaxValue)
				{
					_Value += Sensor.Step[num];
				}
				break;
			}
		}
		SetValue();
	}

	private void customButton_Add_MouseUp(object sender, MouseEventArgs e)
	{
		if (AddMouseStatus == MouseStatus.Down)
		{
			UpdateDPI();
		}
		AddMouseStatus = MouseStatus.Up;
	}

	private void customButton_Sub_MouseDown(object sender, MouseEventArgs e)
	{
		if (SubMouseStatus == MouseStatus.Down || !FormMain.GetDeviceOnlineFlag())
		{
			return;
		}
		SubMouseStatus = MouseStatus.Down;
		for (int i = 0; i < Sensor.Max.Count; i++)
		{
			if (_Value <= Sensor.Max[i])
			{
				if (_Value > _MinValue)
				{
					_Value -= Sensor.Step[i];
				}
				break;
			}
		}
		SetValue();
	}

	private void customButton_Sub_MouseUp(object sender, MouseEventArgs e)
	{
		if (SubMouseStatus == MouseStatus.Down)
		{
			UpdateDPI();
		}
		SubMouseStatus = MouseStatus.Up;
	}

	private void SetValue()
	{
		((Control)textBox_DPIValue).Text = _Value.ToString();
		CustomDPIButtons[SelectIndex].TextString = _Value.ToString();
		customTrackBar1.Value = _Value;
	}

	private void UpdateDPI()
	{
		ValueToDPI(_Value);
		SetValue();
		PressTimer.Enabled = false;
		pressCount = 0;
	}

	private void ValueToDPI(int InValue)
	{
		_ = new byte[2];
		if (FormMain.gFlashDataMap.dpiConfig == null)
		{
			UpdateValue();
			return;
		}
		int dPIex = FormMain.gFlashDataMap.dpiConfig[SelectIndex].DPIex;
		bool flag = true;
		if ((dPIex & 0x22) > 0)
		{
			flag = false;
		}
		int num = (flag ? 50 : 100);
		int num2 = InValue / num;
		dPIex = 0;
		int num3 = InValue;
		int num4 = 1;
		if (Sensor.DPIex == 51)
		{
			int num5 = 0;
			for (int num6 = Sensor.Step.Count - 1; num6 >= 0; num6--)
			{
				if (num3 >= Sensor.Min[num6])
				{
					num5 = num6;
					break;
				}
			}
			switch (num5)
			{
			case 3:
				dPIex = 51;
				num4 = 4;
				break;
			case 2:
				dPIex = 17;
				num4 = 2;
				break;
			case 1:
				dPIex = 34;
				num4 = 2;
				break;
			}
		}
		else if ((Sensor.DPIex == 34 || Sensor.DPIex == 17) && num3 > Sensor.Max[0])
		{
			dPIex = Sensor.DPIex;
			num4 = 2;
		}
		if (Sensor.Discord)
		{
			num3 /= num4;
			for (int i = 0; i < Sensor.DPIList.Count; i++)
			{
				if (Sensor.DPIList[i] == num3)
				{
					num2 = Sensor.DPIValueList[i];
					break;
				}
			}
		}
		else
		{
			num = 50 * num4;
			num2 = InValue / num - 1;
			if (Sensor.Type == "3395")
			{
				int num7 = num2 >> 8;
				dPIex |= (num7 << 2) | (num7 << 6);
			}
		}
		FormMain.gFlashDataMap.dpiConfig[SelectIndex].xDPI = (byte)num2;
		FormMain.gFlashDataMap.dpiConfig[SelectIndex].yDPI = (byte)num2;
		FormMain.gFlashDataMap.dpiConfig[SelectIndex].DPIex = (byte)dPIex;
		UpdateValue();
	}

	private void customButton_CurrentDPI_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Invalid comparison between Unknown and I4
		if (FormMain.GetDeviceOnlineFlag() && (int)e.Button == 1048576)
		{
			mouseStatus = MouseStatus.Down;
			mousePoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
			buttomPoint = new Point(((Control)customButton_CurrentDPI).Location.X, ((Control)customButton_CurrentDPI).Location.Y);
		}
	}

	private void customButton_CurrentDPI_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseStatus == MouseStatus.Down)
		{
			int num = Control.MousePosition.X - mousePoint.X + buttomPoint.X;
			int num2 = Control.MousePosition.Y - mousePoint.Y + buttomPoint.Y;
			if (num < 0)
			{
				num = 0;
			}
			if (num > ((Control)this).Width - ((Control)customButton_CurrentDPI).Width)
			{
				num = ((Control)this).Width - ((Control)customButton_CurrentDPI).Width;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > ((Control)this).Height - ((Control)customButton_CurrentDPI).Height)
			{
				num2 = ((Control)this).Height - ((Control)customButton_CurrentDPI).Height;
			}
			((Control)customButton_CurrentDPI).Location = new Point(num, num2);
		}
	}

	private void customButton_CurrentDPI_MouseUp(object sender, MouseEventArgs e)
	{
		if (mouseStatus == MouseStatus.Down)
		{
			bool flag = false;
			int num = -1;
			int x = ((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Left + (((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Width - ((Control)customButton_CurrentDPI).Width) / 2;
			int y = ((Control)CustomDPIButtons[FormMain.gFlashDataMap.mouseConfig.currentDPI]).Bottom + 3;
			for (int i = 0; i < FormMain.gFlashDataMap.mouseConfig.maxDPI; i++)
			{
				if (((Control)CustomDPIButtons[i]).Location.X < ((Control)customButton_CurrentDPI).Location.X && ((Control)customButton_CurrentDPI).Location.X < ((Control)CustomDPIButtons[i]).Location.X + ((Control)CustomDPIButtons[i]).Width)
				{
					num = i;
					break;
				}
			}
			if (num != -1 && FormMain.gFlashDataMap.mouseConfig.currentDPI != num)
			{
				SelectIndex = num;
				x = ((Control)CustomDPIButtons[num]).Left + (((Control)CustomDPIButtons[num]).Width - ((Control)customButton_CurrentDPI).Width) / 2;
				y = ((Control)CustomDPIButtons[num]).Bottom + 3;
				flag = true;
			}
			((Control)customButton_CurrentDPI).Location = new Point(x, y);
			if (flag)
			{
				((RadioButton)CustomDPIButtons[SelectIndex]).Checked = true;
				_Value = Convert.ToInt32(CustomDPIButtons[SelectIndex].TextString);
				((Control)textBox_DPIValue).Text = _Value.ToString();
				customTrackBar1.Value = _Value;
				FormMain.gFlashDataMap.mouseConfig.currentDPI = (byte)num;
				UpdateValue();
			}
		}
		mouseStatus = MouseStatus.Up;
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Expected O, but got Unknown
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Expected O, but got Unknown
		//IL_0a36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Expected O, but got Unknown
		//IL_0ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af0: Expected O, but got Unknown
		//IL_0ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0baf: Expected O, but got Unknown
		//IL_0c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Expected O, but got Unknown
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Expected O, but got Unknown
		//IL_0dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dce: Expected O, but got Unknown
		//IL_0e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Expected O, but got Unknown
		//IL_0f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3d: Expected O, but got Unknown
		//IL_0ff2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Expected O, but got Unknown
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ac: Expected O, but got Unknown
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_116b: Expected O, but got Unknown
		//IL_122d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1237: Expected O, but got Unknown
		//IL_143a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1444: Expected O, but got Unknown
		//IL_1454: Unknown result type (might be due to invalid IL or missing references)
		label_DPIGrade = new Label();
		panel1 = new Panel();
		customButton_Add = new CustomButton();
		textBox_DPIValue = new TextBox();
		customButton_Sub = new CustomButton();
		customTrackBar1 = new CustomTrackBar();
		pictureBox1 = new PictureBox();
		customButton_CurrentDPI = new CustomButton();
		customDPIButton8 = new CustomDPIButton();
		customDPIButton7 = new CustomDPIButton();
		customDPIButton6 = new CustomDPIButton();
		customDPIButton5 = new CustomDPIButton();
		customDPIButton4 = new CustomDPIButton();
		customDPIButton3 = new CustomDPIButton();
		customDPIButton2 = new CustomDPIButton();
		customDPIButton1 = new CustomDPIButton();
		customComboBox_DPIGrade = new CustomComboBox();
		((Control)panel1).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_DPIGrade).AutoSize = true;
		((Control)label_DPIGrade).Location = new Point(76, 20);
		((Control)label_DPIGrade).Name = "label_DPIGrade";
		((Control)label_DPIGrade).Size = new Size(61, 20);
		((Control)label_DPIGrade).TabIndex = 0;
		((Control)label_DPIGrade).Text = "DPI级数";
		((Control)panel1).Controls.Add((Control)(object)customButton_Add);
		((Control)panel1).Controls.Add((Control)(object)textBox_DPIValue);
		((Control)panel1).Controls.Add((Control)(object)customButton_Sub);
		((Control)panel1).Controls.Add((Control)(object)customTrackBar1);
		((Control)panel1).Location = new Point(81, 55);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(747, 36);
		((Control)panel1).TabIndex = 2;
		((Control)customButton_Add).Location = new Point(721, 11);
		customButton_Add.MouseDownImage = (Image)(object)Resources.加键使能;
		customButton_Add.MouseEnterImage = (Image)(object)Resources.加键使能;
		((Control)customButton_Add).Name = "customButton_Add";
		customButton_Add.NormalImage = (Image)(object)Resources.加键使能;
		((Control)customButton_Add).Size = new Size(14, 14);
		((Control)customButton_Add).TabIndex = 3;
		((Control)customButton_Add).MouseClick += new MouseEventHandler(customButton_Add_MouseDown);
		((Control)customButton_Add).MouseDown += new MouseEventHandler(customButton_Add_MouseDown);
		((Control)customButton_Add).MouseUp += new MouseEventHandler(customButton_Add_MouseUp);
		((Control)textBox_DPIValue).BackColor = Color.FromArgb(57, 57, 57);
		((TextBoxBase)textBox_DPIValue).BorderStyle = (BorderStyle)0;
		((Control)textBox_DPIValue).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textBox_DPIValue).ForeColor = Color.White;
		((Control)textBox_DPIValue).Location = new Point(649, 8);
		((Control)textBox_DPIValue).Name = "textBox_DPIValue";
		((TextBoxBase)textBox_DPIValue).ReadOnly = true;
		((Control)textBox_DPIValue).Size = new Size(66, 19);
		((Control)textBox_DPIValue).TabIndex = 16;
		((Control)textBox_DPIValue).Text = "50";
		textBox_DPIValue.TextAlign = (HorizontalAlignment)2;
		((Control)customButton_Sub).Location = new Point(629, 17);
		customButton_Sub.MouseDownImage = (Image)(object)Resources.减键使能;
		customButton_Sub.MouseEnterImage = (Image)(object)Resources.减键使能;
		((Control)customButton_Sub).Name = "customButton_Sub";
		customButton_Sub.NormalImage = (Image)(object)Resources.减键使能;
		((Control)customButton_Sub).Size = new Size(14, 4);
		((Control)customButton_Sub).TabIndex = 1;
		((Control)customButton_Sub).MouseClick += new MouseEventHandler(customButton_Sub_MouseDown);
		((Control)customButton_Sub).MouseDown += new MouseEventHandler(customButton_Sub_MouseDown);
		((Control)customButton_Sub).MouseUp += new MouseEventHandler(customButton_Sub_MouseUp);
		customTrackBar1.BarColor = Color.FromArgb(255, 255, 255);
		customTrackBar1.BarSize = 2;
		customTrackBar1.DisableColor = Color.FromArgb(57, 57, 57);
		customTrackBar1.Enable = true;
		customTrackBar1.IsRound = true;
		((Control)customTrackBar1).Location = new Point(3, 7);
		customTrackBar1.Maximum = 100;
		customTrackBar1.Minimum = 0;
		((Control)customTrackBar1).Name = "customTrackBar1";
		customTrackBar1.Orientation = (Orientation)0;
		((Control)customTrackBar1).Size = new Size(620, 23);
		customTrackBar1.SizeSlidSize = new Size(6, 18);
		customTrackBar1.SliderColor = Color.FromArgb(255, 106, 0);
		customTrackBar1.Step = 1;
		((Control)customTrackBar1).TabIndex = 0;
		((Control)customTrackBar1).Text = "customTrackBar1";
		customTrackBar1.ValueChanged += customTrackBar1_ValueChanged;
		customTrackBar1.SetValue += customTrackBar1_SetValue;
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox1).Location = new Point(37, 20);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(33, 20);
		pictureBox1.TabIndex = 11;
		pictureBox1.TabStop = false;
		((Control)customButton_CurrentDPI).Location = new Point(198, 184);
		customButton_CurrentDPI.MouseDownImage = (Image)(object)Resources.当前DPI档位鼠标进入;
		customButton_CurrentDPI.MouseEnterImage = (Image)(object)Resources.当前DPI档位鼠标进入;
		((Control)customButton_CurrentDPI).Name = "customButton_CurrentDPI";
		customButton_CurrentDPI.NormalImage = (Image)(object)Resources.当前DPI档位;
		((Control)customButton_CurrentDPI).Size = new Size(24, 20);
		((Control)customButton_CurrentDPI).TabIndex = 13;
		((Control)customButton_CurrentDPI).MouseDown += new MouseEventHandler(customButton_CurrentDPI_MouseDown);
		((Control)customButton_CurrentDPI).MouseMove += new MouseEventHandler(customButton_CurrentDPI_MouseMove);
		((Control)customButton_CurrentDPI).MouseUp += new MouseEventHandler(customButton_CurrentDPI_MouseUp);
		((RadioButton)customDPIButton8).Appearance = (Appearance)1;
		customDPIButton8.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton8.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton8.CurrentDPI = false;
		customDPIButton8.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton8.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton8).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton8).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton8).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton8).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton8).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton8).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton8).Location = new Point(774, 108);
		((Control)customDPIButton8).Name = "customDPIButton8";
		customDPIButton8.SelectForeColor = Color.Red;
		((Control)customDPIButton8).Size = new Size(68, 70);
		((Control)customDPIButton8).TabIndex = 10;
		customDPIButton8.TextString = "26000";
		customDPIButton8.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton8).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton7).Appearance = (Appearance)1;
		customDPIButton7.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton7.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton7.CurrentDPI = false;
		customDPIButton7.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton7.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton7).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton7).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton7).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton7).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton7).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton7).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton7).Location = new Point(674, 108);
		((Control)customDPIButton7).Name = "customDPIButton7";
		customDPIButton7.SelectForeColor = Color.Red;
		((Control)customDPIButton7).Size = new Size(68, 70);
		((Control)customDPIButton7).TabIndex = 9;
		customDPIButton7.TextString = "26000";
		customDPIButton7.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton7).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton6).Appearance = (Appearance)1;
		customDPIButton6.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton6.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton6.CurrentDPI = false;
		customDPIButton6.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton6.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton6).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton6).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton6).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton6).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton6).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton6).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton6).Location = new Point(574, 108);
		((Control)customDPIButton6).Name = "customDPIButton6";
		customDPIButton6.SelectForeColor = Color.Red;
		((Control)customDPIButton6).Size = new Size(68, 70);
		((Control)customDPIButton6).TabIndex = 8;
		customDPIButton6.TextString = "26000";
		customDPIButton6.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton6).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton5).Appearance = (Appearance)1;
		customDPIButton5.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton5.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton5.CurrentDPI = false;
		customDPIButton5.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton5.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton5).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton5).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton5).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton5).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton5).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton5).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton5).Location = new Point(474, 108);
		((Control)customDPIButton5).Name = "customDPIButton5";
		customDPIButton5.SelectForeColor = Color.Red;
		((Control)customDPIButton5).Size = new Size(68, 70);
		((Control)customDPIButton5).TabIndex = 7;
		customDPIButton5.TextString = "26000";
		customDPIButton5.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton5).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton4).Appearance = (Appearance)1;
		customDPIButton4.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton4.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton4.CurrentDPI = false;
		customDPIButton4.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton4.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton4).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton4).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton4).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton4).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton4).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton4).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton4).Location = new Point(374, 108);
		((Control)customDPIButton4).Name = "customDPIButton4";
		customDPIButton4.SelectForeColor = Color.Red;
		((Control)customDPIButton4).Size = new Size(68, 70);
		((Control)customDPIButton4).TabIndex = 6;
		customDPIButton4.TextString = "26000";
		customDPIButton4.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton4).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton3).Appearance = (Appearance)1;
		customDPIButton3.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton3.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton3.CurrentDPI = false;
		customDPIButton3.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton3.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton3).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton3).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton3).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton3).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton3).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton3).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton3).Location = new Point(274, 108);
		((Control)customDPIButton3).Name = "customDPIButton3";
		customDPIButton3.SelectForeColor = Color.Red;
		((Control)customDPIButton3).Size = new Size(68, 70);
		((Control)customDPIButton3).TabIndex = 5;
		customDPIButton3.TextString = "26000";
		customDPIButton3.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton3).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton2).Appearance = (Appearance)1;
		customDPIButton2.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton2.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton2.CurrentDPI = false;
		customDPIButton2.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton2.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton2).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton2).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton2).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton2).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton2).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton2).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton2).Location = new Point(174, 108);
		((Control)customDPIButton2).Name = "customDPIButton2";
		customDPIButton2.SelectForeColor = Color.Red;
		((Control)customDPIButton2).Size = new Size(68, 70);
		((Control)customDPIButton2).TabIndex = 4;
		customDPIButton2.TextString = "26000";
		customDPIButton2.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton2).UseVisualStyleBackColor = true;
		((RadioButton)customDPIButton1).Appearance = (Appearance)1;
		customDPIButton1.CheckFont = new Font("微软雅黑", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		customDPIButton1.CheckImage = (Image)(object)Resources.DPI按键选择;
		customDPIButton1.CurrentDPI = false;
		customDPIButton1.DisableColor = Color.FromArgb(57, 57, 57);
		customDPIButton1.DPIColor = Color.Yellow;
		((ButtonBase)customDPIButton1).FlatAppearance.BorderSize = 0;
		((ButtonBase)customDPIButton1).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customDPIButton1).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customDPIButton1).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customDPIButton1).FlatStyle = (FlatStyle)0;
		((Control)customDPIButton1).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customDPIButton1).Location = new Point(80, 108);
		((Control)customDPIButton1).Name = "customDPIButton1";
		customDPIButton1.SelectForeColor = Color.Red;
		((Control)customDPIButton1).Size = new Size(68, 70);
		((Control)customDPIButton1).TabIndex = 3;
		customDPIButton1.TextString = "26000";
		customDPIButton1.UncheckImage = (Image)(object)Resources.DPI按键未选择;
		((ButtonBase)customDPIButton1).UseVisualStyleBackColor = true;
		customComboBox_DPIGrade.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_DPIGrade.ArrowImageNoraml = (Image)(object)Resources.下拉框按键;
		((Control)customComboBox_DPIGrade).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_DPIGrade).Font = new Font("微软雅黑", 10.5f);
		customComboBox_DPIGrade.Item.AddRange(new object[8] { "1", "2", "3", "4", "5", "6", "7", "8" });
		((Control)customComboBox_DPIGrade).Location = new Point(172, 20);
		((Control)customComboBox_DPIGrade).Name = "customComboBox_DPIGrade";
		customComboBox_DPIGrade.SelectIndex = -1;
		customComboBox_DPIGrade.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_DPIGrade).Size = new Size(65, 20);
		((Control)customComboBox_DPIGrade).TabIndex = 15;
		customComboBox_DPIGrade.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_DPIGrade.OnSelectedIndexChanged += customComboBox_DPIGrade_OnSelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)customButton_CurrentDPI);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)customDPIButton8);
		((Control)this).Controls.Add((Control)(object)customDPIButton7);
		((Control)this).Controls.Add((Control)(object)customDPIButton6);
		((Control)this).Controls.Add((Control)(object)customDPIButton5);
		((Control)this).Controls.Add((Control)(object)customDPIButton4);
		((Control)this).Controls.Add((Control)(object)customDPIButton3);
		((Control)this).Controls.Add((Control)(object)customDPIButton2);
		((Control)this).Controls.Add((Control)(object)customDPIButton1);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Controls.Add((Control)(object)customComboBox_DPIGrade);
		((Control)this).Controls.Add((Control)(object)label_DPIGrade);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "DPIControl";
		((Control)this).Size = new Size(870, 220);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
