using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class CustomKeyFunction : UserControl
{
	public delegate void ValueChangeEventHandler(object sender, FlashDataMap e);

	public delegate void UpdateMacroEventHandler(object sender, MacroKeyAndCycle macroKeyAndCycle);

	private List<CustomRadioButton> CustomRadioButtons = new List<CustomRadioButton>();

	private int PageButtonCount = 6;

	private int CurrentIndex;

	private FlashDataMap gFlashDataMap;

	private SensorDPI Sensor;

	private bool NeedUpdate;

	public bool UpdateUIFlag;

	public bool isDilog;

	private LanguageFile gLanguageFile;

	private int[] MacroIndex = new int[16];

	private byte[] FireKeyTimes = new byte[16];

	private byte[] FireKeyInterval = new byte[16];

	private bool KeyDebounceFlag;

	private string DPILockString = "";

	private string MacroString = "";

	private Image _NumberButtonImage;

	private Image _NormalImage;

	private Image _SelectImage;

	private int _SelectedIndex = -1;

	private int _ButtonCount = 6;

	private Color _ButtonForeColor = Color.FromArgb(255, 128, 0);

	private int _ButtonInterval = 8;

	private bool _ScrollSet;

	private IContainer components;

	private CustomScrollBar customScrollBar1;

	private Panel panel_ButtonList;

	private Panel panel_Number;

	private CustomSubKeyFunction customSubKeyFunction_Main;

	private CustomSubKeyFunction customSubKeyFunction_Sub;

	private KeyDebounce keyDebounce1;

	[Category("自定义")]
	[Description("数字按钮图片")]
	public Image NumberButtonImage
	{
		get
		{
			return _NumberButtonImage;
		}
		set
		{
			_NumberButtonImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("功能按钮正常时图片")]
	public Image NormalImage
	{
		get
		{
			return _NormalImage;
		}
		set
		{
			_NormalImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("功能按钮选中时图片")]
	public Image SelectImage
	{
		get
		{
			return _SelectImage;
		}
		set
		{
			_SelectImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("功能按钮选中的位置")]
	public int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			if (_SelectedIndex != value)
			{
				_SelectedIndex = value;
				ChangePanelLocation();
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("功能按钮的个数")]
	public int ButtonCount
	{
		get
		{
			return _ButtonCount;
		}
		set
		{
			_ButtonCount = value;
			((Control)this).Invalidate();
			UIInit();
		}
	}

	[Category("自定义")]
	[Description("数字按钮的数字颜色")]
	public Color ButtonForeColor
	{
		get
		{
			return _ButtonForeColor;
		}
		set
		{
			_ButtonForeColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("两个功能按钮的间隔")]
	public int ButtonInterval
	{
		get
		{
			return _ButtonInterval;
		}
		set
		{
			_ButtonInterval = value;
			((Control)this).Invalidate();
		}
	}

	public Image MainKeyBackgroundImage
	{
		get
		{
			return ((Control)customSubKeyFunction_Main).BackgroundImage;
		}
		set
		{
			((Control)customSubKeyFunction_Main).BackgroundImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image SubKeyBackgroundImage
	{
		get
		{
			return ((Control)customSubKeyFunction_Sub).BackgroundImage;
		}
		set
		{
			((Control)customSubKeyFunction_Sub).BackgroundImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image KeyNormalImag
	{
		get
		{
			return customSubKeyFunction_Main.NormalImage;
		}
		set
		{
			customSubKeyFunction_Main.NormalImage = value;
			customSubKeyFunction_Sub.NormalImage = value;
			keyDebounce1.NormalImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image KeyNormalImagArrow
	{
		get
		{
			return customSubKeyFunction_Main.NormalImageArrow;
		}
		set
		{
			customSubKeyFunction_Main.NormalImageArrow = value;
			customSubKeyFunction_Sub.NormalImageArrow = value;
			((Control)this).Invalidate();
		}
	}

	public Image KeySelectImag
	{
		get
		{
			return customSubKeyFunction_Main.SelectImage;
		}
		set
		{
			customSubKeyFunction_Main.SelectImage = value;
			customSubKeyFunction_Sub.SelectImage = value;
			keyDebounce1.SelectImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image KeySelectImagArrow
	{
		get
		{
			return customSubKeyFunction_Main.SelectImageArrow;
		}
		set
		{
			customSubKeyFunction_Main.SelectImageArrow = value;
			customSubKeyFunction_Sub.SelectImageArrow = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选择框选中时图片")]
	public Image CheckImage
	{
		get
		{
			return keyDebounce1.CheckImage;
		}
		set
		{
			keyDebounce1.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选择框选中时图片")]
	public Image UncheckImage
	{
		get
		{
			return keyDebounce1.UncheckImage;
		}
		set
		{
			keyDebounce1.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public bool ScrollSet
	{
		get
		{
			return _ScrollSet;
		}
		set
		{
			_ScrollSet = value;
			((Control)this).Invalidate();
		}
	}

	public event ValueChangeEventHandler ValueChange;

	public event UpdateMacroEventHandler UpdateMacroUI;

	public CustomKeyFunction()
	{
		InitializeComponent();
		((Control)customSubKeyFunction_Main).Location = new Point(((Control)panel_ButtonList).Right, ((Control)panel_ButtonList).Top);
		((Control)customSubKeyFunction_Main).Hide();
		((Control)customSubKeyFunction_Sub).Location = new Point(((Control)customSubKeyFunction_Main).Right, ((Control)panel_ButtonList).Top);
		((Control)customSubKeyFunction_Sub).Hide();
		Application.AddMessageFilter((IMessageFilter)(object)new DropDownBoxMessager(this));
	}

	private void UpdateValue()
	{
		ValueChange?.Invoke(this, gFlashDataMap);
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		int num = (_ScrollSet ? (_ButtonCount + 2) : _ButtonCount);
		int num2 = CurrentIndex;
		Image val = ((_NormalImage != null) ? _NormalImage : _SelectImage);
		if ((!((Control)panel_ButtonList).Bounds.Contains(e.Location) && !((Control)customScrollBar1).Bounds.Contains(e.Location)) || CurrentIndex > num - PageButtonCount)
		{
			return;
		}
		if (e.Delta <= 0)
		{
			if (num2 < num - PageButtonCount)
			{
				num2++;
			}
		}
		else if (num2 > 0)
		{
			num2--;
		}
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_Number).Controls).Count; i++)
		{
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i - num2) * (val.Height + _ButtonInterval) * FormResize.GetDpi() / 96);
			((Control)panel_Number).Controls[i].Location = new Point(0, (i - num2) * (val.Height + _ButtonInterval) * FormResize.GetDpi() / 96);
		}
		((Control)panel_ButtonList).Controls[3].Location = new Point(0, (4 - num2) * (val.Height + _ButtonInterval) * FormResize.GetDpi() / 96);
		((Control)panel_ButtonList).Controls[4].Location = new Point(0, (3 - num2) * (val.Height + _ButtonInterval) * FormResize.GetDpi() / 96);
		customScrollBar1.DocPosition = num2 * (val.Height + _ButtonInterval);
		CurrentIndex = num2;
	}

	private void customScrollBar1_CustomScroll(object sender, CustomEventArgs e)
	{
		Image val = ((_NormalImage != null) ? _NormalImage : _SelectImage);
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_Number).Controls).Count; i++)
		{
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i * (val.Height + _ButtonInterval) - Convert.ToInt32(customScrollBar1.DocPosition)) * FormResize.GetDpi() / 96);
			((Control)panel_Number).Controls[i].Location = new Point(0, (i * (val.Height + _ButtonInterval) - Convert.ToInt32(customScrollBar1.DocPosition)) * FormResize.GetDpi() / 96);
		}
		((Control)panel_ButtonList).Controls[3].Location = new Point(0, (4 * (val.Height + _ButtonInterval) - Convert.ToInt32(customScrollBar1.DocPosition)) * FormResize.GetDpi() / 96);
		((Control)panel_ButtonList).Controls[4].Location = new Point(0, (3 * (val.Height + _ButtonInterval) - Convert.ToInt32(customScrollBar1.DocPosition)) * FormResize.GetDpi() / 96);
	}

	private void ChangePanelLocation()
	{
		int num = 0;
		int num2 = (_ScrollSet ? (_ButtonCount + 2) : _ButtonCount);
		Image val = ((_NormalImage != null) ? _NormalImage : _SelectImage);
		if (num2 != 0 && CustomRadioButtons.Count != 0)
		{
			num = ((_SelectedIndex >= PageButtonCount / 2 && num2 > PageButtonCount) ? ((_SelectedIndex < num2 - PageButtonCount / 2) ? (_SelectedIndex - PageButtonCount / 2) : (num2 - PageButtonCount)) : 0);
			for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
			{
				((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i - num) * (val.Height + _ButtonInterval));
				((Control)panel_Number).Controls[i].Location = new Point(0, (i - num) * (val.Height + _ButtonInterval));
			}
			((Control)panel_ButtonList).Controls[3].Location = new Point(0, (4 - num) * (val.Height + _ButtonInterval));
			((Control)panel_ButtonList).Controls[4].Location = new Point(0, (3 - num) * (val.Height + _ButtonInterval));
			CurrentIndex = num;
			customScrollBar1.DocPosition = num * (val.Height + _ButtonInterval);
			((RadioButton)CustomRadioButtons[_SelectedIndex]).Checked = true;
		}
	}

	private void UIInit()
	{
		if (_ButtonCount <= 0 || _NormalImage == null || _SelectImage == null)
		{
			return;
		}
		int num = (_ScrollSet ? 16 : _ButtonCount);
		int num2 = 0;
		((Control)panel_ButtonList).Controls.Clear();
		((Control)panel_Number).Controls.Clear();
		CustomRadioButtons.Clear();
		Image val = ((_NormalImage != null) ? _NormalImage : _SelectImage);
		for (int i = 0; i < num; i++)
		{
			CustomRadioButton customRadioButton = new CustomRadioButton();
			customRadioButton.CheckImage = _SelectImage;
			((Control)customRadioButton).ForeColor = FormMain.driveParam.BtnForeClr;
			((Control)customRadioButton).Name = "customRadioButton_KeyFunction" + i;
			customRadioButton.TextString = "";
			customRadioButton.UncheckImage = _NormalImage;
			((Control)customRadioButton).Size = new Size(((Control)panel_ButtonList).Width, (((Control)panel_ButtonList).Height - _ButtonInterval * 5) / 6);
			switch (i)
			{
			case 3:
				((Control)customRadioButton).Tag = 4;
				break;
			case 4:
				((Control)customRadioButton).Tag = 3;
				break;
			default:
				((Control)customRadioButton).Tag = i;
				break;
			}
			((Control)customRadioButton).Click -= Button_Click;
			((Control)customRadioButton).Click += Button_Click;
			if (_ScrollSet)
			{
				if (i < _ButtonCount)
				{
					((Control)customRadioButton).Location = new Point(0, num2 * (_ButtonInterval + val.Height));
				}
				else if (i == 14 || i == 15)
				{
					((Control)customRadioButton).Location = new Point(0, num2 * (_ButtonInterval + val.Height));
				}
				else
				{
					((Control)customRadioButton).Location = new Point(0, -50);
				}
			}
			else
			{
				((Control)customRadioButton).Location = new Point(0, i * (_ButtonInterval + val.Height));
				((Control)panel_Number).Controls.Add((Control)(object)customRadioButton);
			}
			CustomRadioButtons.Add(customRadioButton);
			CustomButton customButton = new CustomButton();
			((Control)customButton).Font = ((Control)this).Font;
			((Control)customButton).ForeColor = _ButtonForeColor;
			customButton.MouseDownImage = _NumberButtonImage;
			customButton.MouseEnterImage = _NumberButtonImage;
			customButton.NormalImage = _NumberButtonImage;
			((Control)customButton).Size = new Size(_NumberButtonImage.Width, _NumberButtonImage.Height);
			((Control)customButton).Name = "customButton_KeyFunction" + i;
			((Control)customButton).Text = (i + 1).ToString();
			if (_ScrollSet)
			{
				if (i < _ButtonCount)
				{
					((Control)customButton).Location = new Point(0, num2 * (_ButtonInterval + _NormalImage.Height));
					num2++;
					((Control)panel_Number).Controls.Add((Control)(object)customButton);
				}
				else if (i == 14 || i == 15)
				{
					((Control)customButton).Location = new Point(0, num2 * (_ButtonInterval + _NormalImage.Height));
					num2++;
					((Control)panel_Number).Controls.Add((Control)(object)customButton);
				}
				else
				{
					((Control)customButton).Location = new Point(0, -50);
				}
			}
			else
			{
				((Control)customButton).Location = new Point(0, i * (_ButtonInterval + _NormalImage.Height));
				((Control)panel_Number).Controls.Add((Control)(object)customButton);
			}
		}
		CustomRadioButton item = CustomRadioButtons[3];
		CustomRadioButton item2 = CustomRadioButtons[4];
		CustomRadioButtons.RemoveAt(4);
		CustomRadioButtons.RemoveAt(3);
		CustomRadioButtons.Insert(3, item2);
		CustomRadioButtons.Insert(4, item);
		for (int j = 0; j < _ButtonCount; j++)
		{
			((Control)panel_ButtonList).Controls.Add((Control)(object)CustomRadioButtons[j]);
		}
		if (_ScrollSet)
		{
			((Control)panel_ButtonList).Controls.Add((Control)(object)CustomRadioButtons[14]);
			((Control)panel_ButtonList).Controls.Add((Control)(object)CustomRadioButtons[15]);
		}
		customScrollBar1.PageSize = ((Control)panel_ButtonList).Size.Height;
		customScrollBar1.DocSize = (_ButtonCount + (_ScrollSet ? 2 : 0)) * (_ButtonInterval + _NormalImage.Height);
		customScrollBar1.DocPosition = 0f;
		PageButtonCount = (((Control)panel_ButtonList).Size.Height + _ButtonInterval) / (_ButtonInterval + val.Height);
	}

	private void UpdateText(string[] text)
	{
		for (int i = 0; i < CustomRadioButtons.Count; i++)
		{
			CustomRadioButtons[i].TextString = text[i];
		}
	}

	public void UpdateUI(FlashDataMap flashDataMap, LanguageFile languageFile, SensorDPI sensor)
	{
		string[] array = new string[16];
		Sensor = sensor;
		NeedUpdate = false;
		gLanguageFile = languageFile;
		gFlashDataMap = flashDataMap;
		MacroString = LanguageFile.Dialogs[0];
		for (int i = 0; i < languageFile.KeyFunctionStructs.Count; i++)
		{
			if (languageFile.KeyFunctionStructs[i].type == 10)
			{
				DPILockString = languageFile.KeyFunctionStructs[i].name;
				break;
			}
		}
		KeyFunctionTypeEnum keyFunctionTypeEnum = KeyFunctionTypeEnum.DPILock;
		List<KeyFunctionStruct> list = new List<KeyFunctionStruct>();
		for (int j = 0; j < languageFile.KeyFunctionSubStructs[(int)keyFunctionTypeEnum].Count; j++)
		{
			if ((KeyFunctionTypeEnum)languageFile.KeyFunctionSubStructs[(int)keyFunctionTypeEnum][j].type != keyFunctionTypeEnum)
			{
				continue;
			}
			KeyFunctionStruct item = languageFile.KeyFunctionSubStructs[(int)keyFunctionTypeEnum][j];
			if (sensor.Discord)
			{
				for (int k = 0; k < sensor.DPIList.Count; k++)
				{
					if (sensor.DPIList[k] == Convert.ToInt32(item.name))
					{
						item.param1 = (byte)sensor.DPIValueList[k];
						break;
					}
				}
				list.Add(item);
			}
			else
			{
				list.Add(item);
			}
		}
		gLanguageFile.KeyFunctionSubStructs.RemoveAt((int)keyFunctionTypeEnum);
		gLanguageFile.KeyFunctionSubStructs.Insert((int)keyFunctionTypeEnum, list);
		for (int l = 0; l < 16; l++)
		{
			MacroIndex[l] = -1;
			FireKeyInterval[l] = 10;
			FireKeyTimes[l] = 3;
			for (int m = 0; m < languageFile.AllKeyFunctionStructs.Count; m++)
			{
				keyFunctionTypeEnum = (KeyFunctionTypeEnum)flashDataMap.keys[l].type;
				switch (keyFunctionTypeEnum)
				{
				case KeyFunctionTypeEnum.FireKey:
					if (flashDataMap.keys[l].type == languageFile.AllKeyFunctionStructs[m].type)
					{
						array[l] = languageFile.AllKeyFunctionStructs[m].name;
						FireKeyInterval[l] = flashDataMap.keys[l].param1;
						FireKeyTimes[l] = flashDataMap.keys[l].param2;
					}
					continue;
				case KeyFunctionTypeEnum.ShortcutKey:
					if (flashDataMap.shortCutKey[l].context[0].type == 2)
					{
						if (flashDataMap.shortCutKey[l].context[0].value[0] != languageFile.AllKeyFunctionStructs[m].param1 || flashDataMap.shortCutKey[l].context[0].value[1] != languageFile.AllKeyFunctionStructs[m].param2)
						{
							continue;
						}
						array[l] = languageFile.AllKeyFunctionStructs[m].name;
					}
					else
					{
						array[l] = GetComboKeyText(flashDataMap.shortCutKey[l]);
					}
					break;
				case KeyFunctionTypeEnum.MacroDefineKey:
				{
					bool flag = false;
					byte[] array2 = new byte[flashDataMap.macroKey[l].nameLength];
					Array.Copy(flashDataMap.macroKey[l].name, array2, array2.Length);
					string text = Encoding.Default.GetString(array2);
					array[l] = MacroString + "-" + text;
					for (int n = 0; n < FormMain.MacroKeyAndCycleList.Count; n++)
					{
						if (text == FormMain.MacroKeyAndCycleList[n].name)
						{
							MacroKeyAndCycle OutParam = new MacroKeyAndCycle(0);
							MacroKeyDriver.Copy(flashDataMap.macroKey[l], ref OutParam, text, flashDataMap.keys[l].param2);
							FormMain.MacroKeyAndCycleList.RemoveAt(n);
							FormMain.MacroKeyAndCycleList.Insert(n, OutParam);
							UpdateMacroUI?.Invoke(this, OutParam);
							flag = true;
							MacroIndex[l] = n;
							break;
						}
					}
					if (!flag)
					{
						MacroKeyAndCycle OutParam2 = new MacroKeyAndCycle(0);
						MacroKeyDriver.Copy(flashDataMap.macroKey[l], ref OutParam2, text, flashDataMap.keys[l].param2);
						FormMain.MacroKeyAndCycleList.Add(OutParam2);
						MacroIndex[l] = FormMain.MacroKeyAndCycleList.Count() - 1;
						UpdateMacroUI?.Invoke(this, OutParam2);
						gLanguageFile.KeyFunctionSubStructs[(int)keyFunctionTypeEnum].Clear();
						for (int num = 0; num < FormMain.MacroKeyAndCycleList.Count(); num++)
						{
							KeyFunctionStruct item2 = new KeyFunctionStruct
							{
								name = FormMain.MacroKeyAndCycleList[num].name
							};
							gLanguageFile.KeyFunctionSubStructs[6].Add(item2);
						}
					}
					break;
				}
				case KeyFunctionTypeEnum.DPILock:
					if (sensor.Discord)
					{
						for (int num2 = 0; num2 < sensor.DPIList.Count; num2++)
						{
							if (sensor.DPIValueList[num2] == flashDataMap.keys[l].param1)
							{
								array[l] = DPILockString + "-" + sensor.DPIList[num2];
								break;
							}
						}
					}
					else
					{
						array[l] = DPILockString + "-" + (flashDataMap.keys[l].param1 + 1) * 50;
					}
					continue;
				default:
					if (flashDataMap.keys[l].type != languageFile.AllKeyFunctionStructs[m].type || flashDataMap.keys[l].param1 != languageFile.AllKeyFunctionStructs[m].param1 || flashDataMap.keys[l].param2 != languageFile.AllKeyFunctionStructs[m].param2)
					{
						continue;
					}
					array[l] = languageFile.AllKeyFunctionStructs[m].name;
					break;
				}
				break;
			}
		}
		UpdateText(array);
		int count = languageFile.KeyFunctionStructs.Count;
		string[] array3 = new string[count];
		bool[] array4 = new bool[count];
		for (int num3 = 0; num3 < count; num3++)
		{
			array3[num3] = languageFile.KeyFunctionStructs[num3].name;
			array4[num3] = languageFile.KeyFunctionStructs[num3].RightPull;
		}
		customSubKeyFunction_Main.ButtonCount = count;
		customSubKeyFunction_Main.UpdateUI(array4, array3, "main");
		customSubKeyFunction_Main.SetFontSize(LanguageFile.GetFontSize(languageFile.LanguageIndex) - 1f);
		int selectedIndex = 0;
		bool flag2 = false;
		count = languageFile.DebounceTimeList.Count;
		array3 = new string[count];
		for (int num4 = 0; num4 < count; num4++)
		{
			array3[num4] = languageFile.DebounceTimeList[num4].name;
			int num5 = languageFile.DebounceTimeList[num4].param1 + (languageFile.DebounceTimeList[num4].param2 << 8);
			if (flashDataMap.mouseConfig.keyDebounceTime == num5)
			{
				flag2 = true;
				selectedIndex = num4;
			}
		}
		if (!KeyDebounceFlag)
		{
			KeyDebounceFlag = true;
			keyDebounce1.ButtonCount = count;
			keyDebounce1.UpdateUI(array3);
		}
		keyDebounce1.Checked = false;
		if (flag2)
		{
			keyDebounce1.SelectedIndex = selectedIndex;
		}
		else
		{
			keyDebounce1.SelectedIndex = 0;
		}
		keyDebounce1.Checked = false;
		NeedUpdate = true;
		UpdateUIFlag = true;
	}

	public void UpdateKeyDebounce(LanguageFile languageFile, int debounceTime)
	{
		int selectedIndex = 0;
		bool flag = false;
		int count = languageFile.DebounceTimeList.Count;
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = languageFile.DebounceTimeList[i].name;
			int num = languageFile.DebounceTimeList[i].param1 + (languageFile.DebounceTimeList[i].param2 << 8);
			if (debounceTime == num)
			{
				flag = true;
				selectedIndex = i;
			}
		}
		keyDebounce1.ButtonCount = count;
		keyDebounce1.UpdateText(array);
		if (flag)
		{
			keyDebounce1.SelectedIndex = selectedIndex;
		}
		else
		{
			keyDebounce1.SelectedIndex = 0;
		}
		keyDebounce1.Checked = false;
	}

	public void UpdateUI(DriveConfig.DeviceParam deviceParam, LanguageFile languageFile)
	{
		string[] array = new string[16];
		NeedUpdate = false;
		gFlashDataMap = new FlashDataMap(0);
		for (int i = 0; i < 16; i++)
		{
			MacroIndex[i] = -1;
			FireKeyInterval[i] = 10;
			FireKeyTimes[i] = 3;
			for (int j = 0; j < languageFile.AllKeyFunctionStructs.Count; j++)
			{
				if (deviceParam.KeyParams[i].type == languageFile.AllKeyFunctionStructs[j].type && deviceParam.KeyParams[i].param1 == languageFile.AllKeyFunctionStructs[j].param1 && deviceParam.KeyParams[i].param2 == languageFile.AllKeyFunctionStructs[j].param2)
				{
					string text = "";
					gFlashDataMap.keys[i].type = languageFile.AllKeyFunctionStructs[j].type;
					gFlashDataMap.keys[i].param1 = languageFile.AllKeyFunctionStructs[j].param1;
					gFlashDataMap.keys[i].param2 = languageFile.AllKeyFunctionStructs[j].param2;
					text = ((gFlashDataMap.keys[i].type != 5 || gFlashDataMap.keys[i].param1 != 0 || gFlashDataMap.keys[i].param2 != 0) ? languageFile.AllKeyFunctionStructs[j].name : deviceParam.KeyParams[i].name);
					array[i] = text;
					break;
				}
			}
		}
		UpdateText(array);
		int count = languageFile.KeyFunctionStructs.Count;
		string[] array2 = new string[count];
		bool[] array3 = new bool[count];
		for (int k = 0; k < count; k++)
		{
			array2[k] = languageFile.KeyFunctionStructs[k].name;
			array3[k] = languageFile.KeyFunctionStructs[k].RightPull;
		}
		customSubKeyFunction_Main.ButtonCount = count;
		customSubKeyFunction_Main.UpdateUI(array3, array2, "main");
		customSubKeyFunction_Main.SetFontSize(LanguageFile.GetFontSize(languageFile.LanguageIndex) - 1f);
		int selectedIndex = 0;
		bool flag = false;
		count = languageFile.DebounceTimeList.Count;
		array2 = new string[count];
		for (int l = 0; l < count; l++)
		{
			array2[l] = languageFile.DebounceTimeList[l].name;
			int num = languageFile.DebounceTimeList[l].param1 + (languageFile.DebounceTimeList[l].param2 << 8);
			if (deviceParam.KeyDebounceTime == num)
			{
				flag = true;
				selectedIndex = l;
			}
		}
		if (!KeyDebounceFlag)
		{
			KeyDebounceFlag = true;
			keyDebounce1.ButtonCount = count;
			keyDebounce1.UpdateUI(array2);
		}
		if (flag)
		{
			keyDebounce1.SelectedIndex = selectedIndex;
		}
		else
		{
			keyDebounce1.SelectedIndex = 0;
		}
		keyDebounce1.Checked = false;
		gLanguageFile = languageFile;
		NeedUpdate = true;
		UpdateUIFlag = true;
	}

	public void UpdateFlash(FlashDataMap flashDataMap)
	{
		gFlashDataMap = flashDataMap;
	}

	public void UpdateMacro(LanguageFile languageFile, int index)
	{
		gLanguageFile = languageFile;
		for (int i = 0; i < _ButtonCount; i++)
		{
			if (MacroIndex[i] == index)
			{
				MacroKeyAndCycle macroKeyAndCycle = FormMain.MacroKeyAndCycleList[index];
				gFlashDataMap.keys[i].type = 6;
				gFlashDataMap.keys[i].param1 = (byte)i;
				gFlashDataMap.keys[i].param2 = macroKeyAndCycle.CycleTimes;
				gFlashDataMap.macroKey[i].nameLength = macroKeyAndCycle.macroKey.nameLength;
				for (int j = 0; j < macroKeyAndCycle.macroKey.nameLength; j++)
				{
					gFlashDataMap.macroKey[i].name[j] = macroKeyAndCycle.macroKey.name[j];
				}
				gFlashDataMap.macroKey[i].contextCount = macroKeyAndCycle.macroKey.contextCount;
				for (int k = 0; k < macroKeyAndCycle.macroKey.contextCount; k++)
				{
					gFlashDataMap.macroKey[i].context[k] = macroKeyAndCycle.macroKey.context[k];
				}
				UpdateValue();
				string name = FormMain.MacroKeyAndCycleList[index].name;
				CustomRadioButtons[i].TextString = MacroString + "-" + name;
			}
		}
	}

	public void DeleteMacro(int index, DriveConfig.KeyParam[] keyParams)
	{
		for (int i = 0; i < _ButtonCount; i++)
		{
			if (MacroIndex[i] != index)
			{
				continue;
			}
			MacroIndex[i] = -1;
			int num = i;
			switch (i)
			{
			case 3:
				num = 4;
				break;
			case 4:
				num = 3;
				break;
			}
			byte type = keyParams[num].type;
			byte param = keyParams[num].param1;
			byte param2 = keyParams[num].param2;
			gFlashDataMap.keys[i].type = type;
			gFlashDataMap.keys[i].param1 = param;
			gFlashDataMap.keys[i].param2 = param2;
			string text = "";
			for (int j = 0; j < gLanguageFile.AllKeyFunctionStructs.Count; j++)
			{
				if (type == gLanguageFile.AllKeyFunctionStructs[j].type && param == gLanguageFile.AllKeyFunctionStructs[j].param1 && param2 == gLanguageFile.AllKeyFunctionStructs[j].param2)
				{
					text = gLanguageFile.AllKeyFunctionStructs[j].name;
				}
			}
			if (type == 5)
			{
				List<MacroContext> list = new List<MacroContext>();
				if (param == 0 && param2 == 0)
				{
					text = keyParams[i].name;
					list = ValueConvert.App_StringToShortcutKey(text).ToList();
				}
				else
				{
					list = ValueConvert.App_AddMultiMedia(new byte[2] { param, param2 });
				}
				gFlashDataMap.shortCutKey[num].contextCount = (byte)list.Count();
				for (int k = 0; k < gFlashDataMap.shortCutKey[num].contextCount; k++)
				{
					gFlashDataMap.shortCutKey[num].context[k] = list[k];
				}
			}
			UpdateValue();
			CustomRadioButtons[i].TextString = text;
		}
	}

	private string GetComboKeyText(ShortCutKey shortCutKey)
	{
		string text = "";
		for (int i = 0; i < shortCutKey.contextCount / 2; i++)
		{
			string text2 = "";
			if (shortCutKey.context[i].type == 0)
			{
				for (int j = 0; j < 4; j++)
				{
					int num = 1 << j;
					if (shortCutKey.context[i].value[0] == num)
					{
						text2 = Convert.ToString((ModifyEnum)num);
						break;
					}
				}
			}
			else if (shortCutKey.context[i].type == 1)
			{
				text2 = KeyboardCodes.FindKeyboardCode(shortCutKey.context[i].value[0] | (shortCutKey.context[i].value[1] << 8), shortCutKey.context[i].type).keyChar;
			}
			if (text != "")
			{
				text += "+";
			}
			text += text2;
		}
		return text;
	}

	private void Button_Click(object sender, EventArgs e)
	{
		int num = (int)((Control)(CustomRadioButton)sender).Tag;
		if (UpdateUIFlag)
		{
			_SelectedIndex = num;
			FindeKeyFunction(num);
		}
	}

	private void FindeKeyFunction(int index)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		if (!KeepOneLeftButton(index))
		{
			if (!FormMain.GetDeviceOnlineFlag())
			{
				NeedUpdate = true;
				return;
			}
			HideActiveWindows();
			((Form)new FormDialog(LanguageFile.Dialogs[22])).ShowDialog();
			return;
		}
		NeedUpdate = false;
		((Control)keyDebounce1).Hide();
		((Control)customScrollBar1).Hide();
		bool flag = false;
		ShortCutKey shortCutKey = gFlashDataMap.shortCutKey[_SelectedIndex];
		KeyFunMap keyFunMap = gFlashDataMap.keys[index];
		((Control)customSubKeyFunction_Sub).Hide();
		((Control)customSubKeyFunction_Main).Show();
		if (gFlashDataMap.shortCutKey[index].context == null && gFlashDataMap.shortCutKey[index].contextCount == 0 && keyFunMap.type == 5 && (keyFunMap.param1 != 0 || keyFunMap.param2 != 0))
		{
			for (int i = 0; i < gLanguageFile.KeyFunctionStructs.Count; i++)
			{
				if (gLanguageFile.KeyFunctionStructs[i].type != 5 || !gLanguageFile.KeyFunctionStructs[i].RightPull)
				{
					continue;
				}
				customSubKeyFunction_Main.SelectedIndex = i;
				for (int j = 0; j < gLanguageFile.KeyFunctionSubStructs[5].Count; j++)
				{
					if (keyFunMap.param1 == gLanguageFile.KeyFunctionSubStructs[5][j].param1 && keyFunMap.param2 == gLanguageFile.KeyFunctionSubStructs[5][j].param2)
					{
						CustomSubKeyFunctionSub_Show(keyFunMap.type);
						customSubKeyFunction_Sub.SelectedIndex = j;
					}
				}
				flag = true;
				break;
			}
		}
		for (int k = 0; k < gLanguageFile.KeyFunctionStructs.Count; k++)
		{
			if (gLanguageFile.KeyFunctionStructs[k].type != keyFunMap.type || gLanguageFile.KeyFunctionStructs[k].param1 != keyFunMap.param1 || gLanguageFile.KeyFunctionStructs[k].param2 != keyFunMap.param2)
			{
				continue;
			}
			if (keyFunMap.type != 5)
			{
				customSubKeyFunction_Main.SelectedIndex = k;
				flag = !gLanguageFile.KeyFunctionStructs[k].RightPull;
				break;
			}
			if (gFlashDataMap.shortCutKey[index].context != null)
			{
				if (gFlashDataMap.shortCutKey[index].context[0].type == 2 && gLanguageFile.KeyFunctionStructs[k].RightPull)
				{
					customSubKeyFunction_Main.SelectedIndex = k;
					for (int l = 0; l < gLanguageFile.KeyFunctionSubStructs[5].Count; l++)
					{
						if (shortCutKey.context[0].value[0] == gLanguageFile.KeyFunctionSubStructs[5][l].param1 && shortCutKey.context[0].value[1] == gLanguageFile.KeyFunctionSubStructs[5][l].param2)
						{
							CustomSubKeyFunctionSub_Show(keyFunMap.type);
							customSubKeyFunction_Sub.SelectedIndex = l;
						}
					}
					flag = true;
					break;
				}
				if (gFlashDataMap.shortCutKey[index].context[0].type != 2 && !gLanguageFile.KeyFunctionStructs[k].RightPull)
				{
					customSubKeyFunction_Main.SelectedIndex = k;
					CustomRadioButtons[_SelectedIndex].TextString = GetComboKeyText(shortCutKey);
					flag = true;
					break;
				}
				continue;
			}
			customSubKeyFunction_Main.SelectedIndex = k;
			flag = true;
			break;
		}
		if (flag)
		{
			return;
		}
		for (int m = 0; m < gLanguageFile.KeyFunctionStructs.Count; m++)
		{
			if (gLanguageFile.KeyFunctionStructs[m].type != gFlashDataMap.keys[_SelectedIndex].type)
			{
				continue;
			}
			customSubKeyFunction_Main.SelectedIndex = m;
			int type = gFlashDataMap.keys[_SelectedIndex].type;
			if (MacroIndex[_SelectedIndex] != -1)
			{
				CustomSubKeyFunctionSub_Show(type);
				customSubKeyFunction_Sub.SelectedIndex = MacroIndex[_SelectedIndex];
				break;
			}
			for (int n = 0; n < gLanguageFile.KeyFunctionSubStructs[type].Count; n++)
			{
				if (gLanguageFile.KeyFunctionSubStructs[type][n].param1 != gFlashDataMap.keys[_SelectedIndex].param1 || gLanguageFile.KeyFunctionSubStructs[type][n].param2 != gFlashDataMap.keys[_SelectedIndex].param2)
				{
					continue;
				}
				if (gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].RightPull)
				{
					type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
					int num = type;
					int count = gLanguageFile.KeyFunctionSubStructs[type].Count;
					if (num == 6 && count == 0)
					{
						((Form)new FormDialog(LanguageFile.Dialogs[9])).ShowDialog();
					}
					else
					{
						CustomSubKeyFunctionSub_Show(type);
					}
				}
				((Control)customSubKeyFunction_Sub).Show();
				customSubKeyFunction_Sub.SelectedIndex = n;
				return;
			}
		}
	}

	private void CustomSubKeyFunctionSub_Show(int type)
	{
		int count = gLanguageFile.KeyFunctionSubStructs[type].Count;
		string[] array = new string[count];
		bool[] array2 = new bool[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = gLanguageFile.KeyFunctionSubStructs[type][i].name;
			array2[i] = gLanguageFile.KeyFunctionSubStructs[type][i].RightPull;
		}
		customSubKeyFunction_Sub.ButtonCount = count;
		customSubKeyFunction_Sub.UpdateUI(array2, array, "Sub");
		customSubKeyFunction_Sub.SetFontSize(LanguageFile.GetFontSize(gLanguageFile.LanguageIndex));
		((Control)customSubKeyFunction_Sub).Show();
	}

	private void customSubKeyFunction_Main_ButtonSelect(object sender, CustomEventArgs e)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		((Control)customSubKeyFunction_Sub).Hide();
		int type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
		KeyFunctionTypeEnum keyFunctionTypeEnum = (KeyFunctionTypeEnum)type;
		if (gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].RightPull)
		{
			_ = ref gFlashDataMap.keys[_SelectedIndex];
			int count = gLanguageFile.KeyFunctionSubStructs[type].Count;
			if (keyFunctionTypeEnum == KeyFunctionTypeEnum.MacroDefineKey && count == 0)
			{
				((Form)new FormDialog(LanguageFile.Dialogs[9])).ShowDialog();
			}
			else
			{
				CustomSubKeyFunctionSub_Show(type);
			}
			return;
		}
		if (NeedUpdate)
		{
			if (!FormMain.GetDeviceOnlineFlag())
			{
				NeedUpdate = true;
				return;
			}
			bool flag = false;
			switch (keyFunctionTypeEnum)
			{
			case KeyFunctionTypeEnum.ShortcutKey:
			{
				isDilog = true;
				FormEditDialog formEditDialog = new FormEditDialog(keys: new string[2]
				{
					LanguageFile.Dialogs[37],
					LanguageFile.Dialogs[38]
				}, str: LanguageFile.Dialogs[20], type: FormEditDialogType.ComboKey);
				((Form)formEditDialog).ShowDialog();
				if (FormMain.GetDeviceOnlineFlag() && formEditDialog.NeedUpdate)
				{
					flag = true;
					FireKeyTimes[_SelectedIndex] = 3;
					FireKeyInterval[_SelectedIndex] = 10;
					ValueConvert.App_AddShortCutKey(ref gFlashDataMap, _SelectedIndex, formEditDialog.ComboData);
					CustomRadioButtons[_SelectedIndex].TextString = formEditDialog.ComboString;
					gFlashDataMap.keys[_SelectedIndex].type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
					gFlashDataMap.keys[_SelectedIndex].param1 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param1;
					gFlashDataMap.keys[_SelectedIndex].param2 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param2;
				}
				isDilog = false;
				break;
			}
			case KeyFunctionTypeEnum.FireKey:
			{
				string[] labels = new string[5]
				{
					LanguageFile.Dialogs[18],
					LanguageFile.Dialogs[19],
					LanguageFile.Dialogs[21],
					LanguageFile.Dialogs[37],
					LanguageFile.Dialogs[38]
				};
				isDilog = true;
				FormFireKey formFireKey = new FormFireKey(labels, FireKeyTimes[_SelectedIndex], FireKeyInterval[_SelectedIndex]);
				((Form)formFireKey).ShowDialog();
				if (FormMain.GetDeviceOnlineFlag() && formFireKey.NeedUpdate)
				{
					flag = true;
					FireKeyTimes[_SelectedIndex] = formFireKey.times;
					FireKeyInterval[_SelectedIndex] = formFireKey.interval;
					gFlashDataMap.keys[_SelectedIndex].type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
					gFlashDataMap.keys[_SelectedIndex].param1 = formFireKey.interval;
					gFlashDataMap.keys[_SelectedIndex].param2 = formFireKey.times;
					CustomRadioButtons[_SelectedIndex].TextString = customSubKeyFunction_Main.CustomRadioButtons[customSubKeyFunction_Main.SelectedIndex].TextString;
				}
				isDilog = false;
				break;
			}
			default:
				flag = true;
				FireKeyTimes[_SelectedIndex] = 3;
				FireKeyInterval[_SelectedIndex] = 10;
				CustomRadioButtons[_SelectedIndex].TextString = customSubKeyFunction_Main.CustomRadioButtons[customSubKeyFunction_Main.SelectedIndex].TextString;
				gFlashDataMap.keys[_SelectedIndex].type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
				gFlashDataMap.keys[_SelectedIndex].param1 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param1;
				gFlashDataMap.keys[_SelectedIndex].param2 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param2;
				break;
			}
			if (flag)
			{
				UpdateValue();
			}
		}
		NeedUpdate = true;
	}

	private void customSubKeyFunction_Main_DoubleClick(object sender, EventArgs e)
	{
		HideActiveWindows();
	}

	private void customSubKeyFunction_Sub_ButtonSelect(object sender, CustomEventArgs e)
	{
		int num = (int)e.Value;
		if (NeedUpdate)
		{
			if (!FormMain.GetDeviceOnlineFlag())
			{
				return;
			}
			CustomRadioButtons[_SelectedIndex].TextString = customSubKeyFunction_Sub.CustomRadioButtons[num].TextString;
			byte type = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].type;
			switch ((KeyFunctionTypeEnum)type)
			{
			case KeyFunctionTypeEnum.ShortcutKey:
			{
				MacroIndex[_SelectedIndex] = -1;
				gFlashDataMap.keys[_SelectedIndex].type = type;
				gFlashDataMap.keys[_SelectedIndex].param1 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param1;
				gFlashDataMap.keys[_SelectedIndex].param2 = gLanguageFile.KeyFunctionStructs[customSubKeyFunction_Main.SelectedIndex].param2;
				MacroContext[] array2 = ValueConvert.App_AddMultiMedia(new byte[2]
				{
					gLanguageFile.KeyFunctionSubStructs[type][num].param1,
					gLanguageFile.KeyFunctionSubStructs[type][num].param2
				}).ToArray();
				gFlashDataMap.shortCutKey[_SelectedIndex].contextCount = (byte)array2.Count();
				for (int l = 0; l < gFlashDataMap.shortCutKey[_SelectedIndex].contextCount; l++)
				{
					gFlashDataMap.shortCutKey[_SelectedIndex].context[l] = array2[l];
				}
				break;
			}
			case KeyFunctionTypeEnum.MacroDefineKey:
			{
				MacroIndex[_SelectedIndex] = num;
				MacroKeyAndCycle macroKeyAndCycle = FormMain.MacroKeyAndCycleList[num];
				gFlashDataMap.keys[_SelectedIndex].type = type;
				gFlashDataMap.keys[_SelectedIndex].param1 = (byte)_SelectedIndex;
				gFlashDataMap.keys[_SelectedIndex].param2 = macroKeyAndCycle.CycleTimes;
				gFlashDataMap.macroKey[_SelectedIndex].nameLength = macroKeyAndCycle.macroKey.nameLength;
				for (int j = 0; j < macroKeyAndCycle.macroKey.nameLength; j++)
				{
					gFlashDataMap.macroKey[_SelectedIndex].name[j] = macroKeyAndCycle.macroKey.name[j];
				}
				gFlashDataMap.macroKey[_SelectedIndex].contextCount = macroKeyAndCycle.macroKey.contextCount;
				for (int k = 0; k < macroKeyAndCycle.macroKey.contextCount; k++)
				{
					gFlashDataMap.macroKey[_SelectedIndex].context[k] = macroKeyAndCycle.macroKey.context[k];
				}
				byte[] array = new byte[gFlashDataMap.macroKey[_SelectedIndex].nameLength];
				Array.Copy(gFlashDataMap.macroKey[_SelectedIndex].name, array, array.Length);
				string text = Encoding.Default.GetString(array);
				CustomRadioButtons[_SelectedIndex].TextString = MacroString + "-" + text;
				break;
			}
			case KeyFunctionTypeEnum.DPILock:
			{
				MacroIndex[_SelectedIndex] = -1;
				gFlashDataMap.keys[_SelectedIndex].type = type;
				int num2 = Convert.ToInt32(customSubKeyFunction_Sub.CustomRadioButtons[num].TextString);
				if (Sensor.Discord)
				{
					for (int i = 0; i < Sensor.DPIList.Count; i++)
					{
						if (Sensor.DPIList[i] == num2)
						{
							gFlashDataMap.keys[_SelectedIndex].param1 = (byte)Sensor.DPIValueList[i];
							gFlashDataMap.keys[_SelectedIndex].param2 = 0;
							break;
						}
					}
				}
				else
				{
					gFlashDataMap.keys[_SelectedIndex].param1 = (byte)(num2 / 50 - 1);
					gFlashDataMap.keys[_SelectedIndex].param2 = 0;
				}
				CustomRadioButtons[_SelectedIndex].TextString = DPILockString + "-" + customSubKeyFunction_Sub.CustomRadioButtons[num].TextString;
				break;
			}
			default:
				MacroIndex[_SelectedIndex] = -1;
				gFlashDataMap.keys[_SelectedIndex].type = type;
				gFlashDataMap.keys[_SelectedIndex].param1 = gLanguageFile.KeyFunctionSubStructs[type][num].param1;
				gFlashDataMap.keys[_SelectedIndex].param2 = gLanguageFile.KeyFunctionSubStructs[type][num].param2;
				break;
			}
			UpdateValue();
		}
		NeedUpdate = true;
	}

	private void customSubKeyFunction_Sub_DoubleClick(object sender, EventArgs e)
	{
		HideActiveWindows();
	}

	private void keyDebounce1_ButtonSelect(object sender, CustomEventArgs e)
	{
		if (NeedUpdate)
		{
			int num = (int)e.Value;
			gFlashDataMap.mouseConfig.keyDebounceTime = (byte)num;
			UpdateValue();
		}
	}

	public void HideActiveWindows()
	{
		if (_SelectedIndex != -1)
		{
			((RadioButton)CustomRadioButtons[_SelectedIndex]).Checked = false;
			_SelectedIndex = -1;
		}
		((Control)customSubKeyFunction_Main).Hide();
		((Control)customSubKeyFunction_Sub).Hide();
		((Control)customScrollBar1).Show();
		((Control)keyDebounce1).Show();
	}

	private bool KeepOneLeftButton(int index)
	{
		List<int> list = new List<int>();
		bool result = false;
		list.Clear();
		for (int i = 0; i < _ButtonCount; i++)
		{
			if (gFlashDataMap.keys[i].type == 1 && gFlashDataMap.keys[i].param1 == 1 && gFlashDataMap.keys[i].param2 == 0)
			{
				list.Add(i);
			}
		}
		if (list.Count == 1)
		{
			result = ((index != list[0]) ? true : false);
		}
		else if (list.Count > 1)
		{
			result = true;
		}
		return result;
	}

	public bool isInClientRect()
	{
		Point pt = ((Control)this).PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
		return ((Control)this).ClientRectangle.Contains(pt);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Expected O, but got Unknown
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Expected O, but got Unknown
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CustomKeyFunction));
		panel_ButtonList = new Panel();
		panel_Number = new Panel();
		keyDebounce1 = new KeyDebounce();
		customSubKeyFunction_Sub = new CustomSubKeyFunction();
		customSubKeyFunction_Main = new CustomSubKeyFunction();
		customScrollBar1 = new CustomScrollBar();
		((Control)this).SuspendLayout();
		((Control)panel_ButtonList).Location = new Point(188, 10);
		((Control)panel_ButtonList).Margin = new Padding(4, 5, 4, 5);
		((Control)panel_ButtonList).Name = "panel_ButtonList";
		((Control)panel_ButtonList).Size = new Size(264, 196);
		((Control)panel_ButtonList).TabIndex = 17;
		((Control)panel_Number).Location = new Point(158, 10);
		((Control)panel_Number).Margin = new Padding(4, 5, 4, 5);
		((Control)panel_Number).Name = "panel_Number";
		((Control)panel_Number).Size = new Size(30, 196);
		((Control)panel_Number).TabIndex = 16;
		((Control)keyDebounce1).BackColor = Color.Transparent;
		keyDebounce1.ButtonCount = 30;
		keyDebounce1.ButtonInterval = 3;
		keyDebounce1.Checked = true;
		keyDebounce1.CheckImage = (Image)componentResourceManager.GetObject("keyDebounce1.CheckImage");
		((Control)keyDebounce1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)keyDebounce1).Location = new Point(636, 5);
		((Control)keyDebounce1).Margin = new Padding(4, 5, 4, 5);
		keyDebounce1.MouseEnterImage = (Image)(object)Resources.按钮功能分配_鼠标进入;
		((Control)keyDebounce1).Name = "keyDebounce1";
		keyDebounce1.NormalImage = null;
		keyDebounce1.SelectedIndex = -1;
		keyDebounce1.SelectImage = (Image)(object)Resources.按钮功能分配_鼠标按下;
		((Control)keyDebounce1).Size = new Size(224, 211);
		((Control)keyDebounce1).TabIndex = 21;
		keyDebounce1.UncheckImage = (Image)componentResourceManager.GetObject("keyDebounce1.UncheckImage");
		keyDebounce1.ButtonSelect += keyDebounce1_ButtonSelect;
		((Control)customSubKeyFunction_Sub).BackgroundImage = (Image)(object)Resources.按键活动页背景;
		((Control)customSubKeyFunction_Sub).BackgroundImageLayout = (ImageLayout)3;
		customSubKeyFunction_Sub.ButtonCount = 6;
		customSubKeyFunction_Sub.ButtonInterval = 3;
		customSubKeyFunction_Sub.CheckImage = null;
		((Control)customSubKeyFunction_Sub).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customSubKeyFunction_Sub).Location = new Point(280, 248);
		((Control)customSubKeyFunction_Sub).Margin = new Padding(4, 5, 4, 5);
		((Control)customSubKeyFunction_Sub).Name = "customSubKeyFunction_Sub";
		customSubKeyFunction_Sub.NormalImage = null;
		customSubKeyFunction_Sub.NormalImageArrow = null;
		customSubKeyFunction_Sub.SelectedIndex = -1;
		customSubKeyFunction_Sub.SelectImage = (Image)(object)Resources.按钮功能分配_鼠标按下;
		customSubKeyFunction_Sub.SelectImageArrow = (Image)(object)Resources.按钮功能分配_带箭头__鼠标按下;
		((Control)customSubKeyFunction_Sub).Size = new Size(200, 194);
		((Control)customSubKeyFunction_Sub).TabIndex = 20;
		customSubKeyFunction_Sub.UncheckImage = null;
		customSubKeyFunction_Sub.ButtonSelect += customSubKeyFunction_Sub_ButtonSelect;
		customSubKeyFunction_Sub.DoubleClick += customSubKeyFunction_Sub_DoubleClick;
		((Control)customSubKeyFunction_Main).BackgroundImage = (Image)(object)Resources.按键活动页背景;
		((Control)customSubKeyFunction_Main).BackgroundImageLayout = (ImageLayout)3;
		customSubKeyFunction_Main.ButtonCount = 6;
		customSubKeyFunction_Main.ButtonInterval = 3;
		customSubKeyFunction_Main.CheckImage = null;
		((Control)customSubKeyFunction_Main).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customSubKeyFunction_Main).Location = new Point(33, 248);
		((Control)customSubKeyFunction_Main).Margin = new Padding(4, 5, 4, 5);
		((Control)customSubKeyFunction_Main).Name = "customSubKeyFunction_Main";
		customSubKeyFunction_Main.NormalImage = null;
		customSubKeyFunction_Main.NormalImageArrow = (Image)(object)Resources.按钮功能分配_带箭头__默认;
		customSubKeyFunction_Main.SelectedIndex = -1;
		customSubKeyFunction_Main.SelectImage = (Image)(object)Resources.按钮功能分配_鼠标按下;
		customSubKeyFunction_Main.SelectImageArrow = (Image)(object)Resources.按钮功能分配_带箭头__鼠标按下;
		((Control)customSubKeyFunction_Main).Size = new Size(200, 194);
		((Control)customSubKeyFunction_Main).TabIndex = 19;
		customSubKeyFunction_Main.UncheckImage = null;
		customSubKeyFunction_Main.ButtonSelect += customSubKeyFunction_Main_ButtonSelect;
		customSubKeyFunction_Main.DoubleClick += customSubKeyFunction_Main_DoubleClick;
		customScrollBar1.BarColor = Color.FromArgb(30, 30, 30);
		customScrollBar1.BarRadius = 16;
		customScrollBar1.BarSize = 10;
		customScrollBar1.DocPosition = 0f;
		customScrollBar1.DocSize = 10f;
		customScrollBar1.Interval = 2;
		customScrollBar1.IsRound = true;
		((Control)customScrollBar1).Location = new Point(618, 8);
		((Control)customScrollBar1).Margin = new Padding(4, 5, 4, 5);
		((Control)customScrollBar1).Name = "customScrollBar1";
		customScrollBar1.Orientation = (Orientation)1;
		customScrollBar1.PageSize = 1f;
		customScrollBar1.ScrollInterval = 10f;
		((Control)customScrollBar1).Size = new Size(10, 187);
		customScrollBar1.SliderColor = Color.FromArgb(70, 70, 70);
		customScrollBar1.SliderMiniSize = 20f;
		customScrollBar1.SliderPosition = 0f;
		((Control)customScrollBar1).TabIndex = 18;
		((Control)customScrollBar1).Text = "customScrollBar1";
		customScrollBar1.CustomScroll += customScrollBar1_CustomScroll;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Control)this).Controls.Add((Control)(object)keyDebounce1);
		((Control)this).Controls.Add((Control)(object)customSubKeyFunction_Sub);
		((Control)this).Controls.Add((Control)(object)customSubKeyFunction_Main);
		((Control)this).Controls.Add((Control)(object)customScrollBar1);
		((Control)this).Controls.Add((Control)(object)panel_ButtonList);
		((Control)this).Controls.Add((Control)(object)panel_Number);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "CustomKeyFunction";
		((Control)this).Size = new Size(959, 496);
		((Control)this).ResumeLayout(false);
	}
}
