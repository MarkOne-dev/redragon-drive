using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class KeyDebounce : UserControl
{
	public delegate void ButtonSelectEventHandler(object sender, CustomEventArgs e);

	private Image _SelectImage;

	private Image _MouseEnterImage;

	private Image _NormalImage;

	private int _ButtonCount = 30;

	private int _ButtonInterval = 3;

	private int _SelectedIndex = -1;

	public List<CustomRadioButton> CustomRadioButtons = new List<CustomRadioButton>();

	private int PageButtonCount = 6;

	private int CurrentIndex;

	private List<string> TextString = new List<string>();

	private IContainer components;

	private Label label_KeyDebounce;

	private Panel panel_ButtonList;

	private CustomScrollBar customScrollBar1;

	private CustomCheckBox customCheckBox1;

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
	[Description("功能按钮鼠标进入时图片")]
	public Image MouseEnterImage
	{
		get
		{
			return _MouseEnterImage;
		}
		set
		{
			_MouseEnterImage = value;
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
			_SelectedIndex = value;
			ChangePanelLocation();
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("按键消抖是否勾选")]
	public bool Checked
	{
		get
		{
			return customCheckBox1.Checked;
		}
		set
		{
			if (_SelectedIndex != -1)
			{
				customCheckBox1.Checked = value;
				customCheckBox1_CheckChange(null, null);
			}
		}
	}

	[Category("自定义")]
	[Description("选择框选中时图片")]
	public Image CheckImage
	{
		get
		{
			return customCheckBox1.CheckImage;
		}
		set
		{
			customCheckBox1.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选择框选中时图片")]
	public Image UncheckImage
	{
		get
		{
			return customCheckBox1.UncheckImage;
		}
		set
		{
			customCheckBox1.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public event ButtonSelectEventHandler ButtonSelect;

	public KeyDebounce()
	{
		InitializeComponent();
	}

	public void UpdateUI(string[] text)
	{
		if (_ButtonCount > 0 && (_NormalImage != null || _SelectImage != null || _MouseEnterImage != null))
		{
			((Control)panel_ButtonList).Controls.Clear();
			CustomRadioButtons.Clear();
			TextString.Clear();
			Image val = ((_NormalImage != null) ? _NormalImage : ((SelectImage != null) ? _SelectImage : _MouseEnterImage));
			for (int i = 0; i < _ButtonCount; i++)
			{
				CustomRadioButton customRadioButton = new CustomRadioButton();
				customRadioButton.CheckImage = _SelectImage;
				((Control)customRadioButton).Location = new Point(0, i * (_ButtonInterval + val.Height));
				customRadioButton.MouseEnterImage = _MouseEnterImage;
				((Control)customRadioButton).Name = "customRadioButton_KeyDebounce" + i;
				customRadioButton.TextString = text[i];
				customRadioButton.UncheckImage = _NormalImage;
				((Control)customRadioButton).Size = val.Size;
				((Control)customRadioButton).Tag = i;
				((Control)customRadioButton).Click += Button_Click;
				((Control)panel_ButtonList).Controls.Add((Control)(object)customRadioButton);
				CustomRadioButtons.Add(customRadioButton);
				TextString.Add(text[i]);
			}
			customScrollBar1.PageSize = ((Control)panel_ButtonList).Size.Height;
			customScrollBar1.DocSize = _ButtonCount * (_ButtonInterval + val.Height);
			customScrollBar1.DocPosition = 0f;
			PageButtonCount = (((Control)panel_ButtonList).Size.Height + _ButtonInterval) / (_ButtonInterval + val.Height);
		}
	}

	public void UpdateText(string[] text)
	{
		TextString.Clear();
		for (int i = 0; i < _ButtonCount; i++)
		{
			CustomRadioButtons[i].TextString = text[i];
			TextString.Add(text[i]);
		}
	}

	private void Button_Click(object sender, EventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		_SelectedIndex = (int)((Control)customRadioButton).Tag;
		string value = Regex.Replace(customRadioButton.TextString, "[^0-9]+", "");
		if (Convert.ToInt32(value) < 8)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[23], DialogButtons.OK, 9f)).ShowDialog();
		}
		CustomEventArgs e2 = new CustomEventArgs(Convert.ToInt32(value));
		ButtonSelect?.Invoke(this, e2);
		if (customCheckBox1.Checked)
		{
			customCheckBox1.Checked = false;
			customCheckBox1_CheckChange(null, null);
		}
	}

	private void customScrollBar1_CustomScroll(object sender, CustomEventArgs e)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
		{
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, i * (_SelectImage.Height + _ButtonInterval) + _ButtonInterval - Convert.ToInt32(customScrollBar1.DocPosition));
		}
	}

	private void ChangePanelLocation()
	{
		int num = 0;
		if (_ButtonCount != 0 && CustomRadioButtons.Count != 0)
		{
			num = ((_SelectedIndex >= PageButtonCount / 2 && _ButtonCount > PageButtonCount) ? ((_SelectedIndex < _ButtonCount - PageButtonCount / 2) ? (_SelectedIndex - PageButtonCount / 2) : (_ButtonCount - PageButtonCount)) : 0);
			for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
			{
				((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i - num) * (_SelectImage.Height + _ButtonInterval));
			}
			CurrentIndex = num;
			customScrollBar1.DocPosition = num * (_SelectImage.Height + _ButtonInterval);
			((RadioButton)CustomRadioButtons[_SelectedIndex]).Checked = true;
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		if (!customCheckBox1.Checked)
		{
			return;
		}
		int num = CurrentIndex;
		if (CurrentIndex > _ButtonCount - PageButtonCount)
		{
			return;
		}
		if (e.Delta <= 0)
		{
			if (num < _ButtonCount - PageButtonCount)
			{
				num++;
			}
		}
		else if (num > 0)
		{
			num--;
		}
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
		{
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i - num) * (_SelectImage.Height + _ButtonInterval) + _ButtonInterval);
		}
		customScrollBar1.DocPosition = num * (_SelectImage.Height + _ButtonInterval);
		CurrentIndex = num;
	}

	private void customCheckBox1_CheckChange(object sender, CustomEventArgs e)
	{
		if (customCheckBox1.Checked)
		{
			UpdateUI(TextString.ToArray());
			((RadioButton)CustomRadioButtons[_SelectedIndex]).Checked = true;
			ChangePanelLocation();
		}
		else if (_ButtonCount > 0 && _SelectImage != null && _MouseEnterImage != null)
		{
			((Control)panel_ButtonList).Controls.Clear();
			((Control)CustomRadioButtons[_SelectedIndex]).Location = new Point(0, _ButtonInterval);
			((Control)panel_ButtonList).Controls.Add((Control)(object)CustomRadioButtons[_SelectedIndex]);
		}
		((Control)customScrollBar1).Visible = customCheckBox1.Checked;
		((Control)customScrollBar1).Enabled = customCheckBox1.Checked;
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
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		label_KeyDebounce = new Label();
		panel_ButtonList = new Panel();
		customScrollBar1 = new CustomScrollBar();
		customCheckBox1 = new CustomCheckBox();
		((Control)this).SuspendLayout();
		((Control)label_KeyDebounce).AutoSize = true;
		((Control)label_KeyDebounce).ForeColor = Color.White;
		((Control)label_KeyDebounce).Location = new Point(41, 21);
		((Control)label_KeyDebounce).Margin = new Padding(4, 0, 4, 0);
		((Control)label_KeyDebounce).Name = "label_KeyDebounce";
		((Control)label_KeyDebounce).Size = new Size(93, 20);
		((Control)label_KeyDebounce).TabIndex = 1;
		((Control)label_KeyDebounce).Text = "按钮防抖时间";
		((Control)panel_ButtonList).Location = new Point(21, 44);
		((Control)panel_ButtonList).Name = "panel_ButtonList";
		((Control)panel_ButtonList).Size = new Size(158, 155);
		((Control)panel_ButtonList).TabIndex = 2;
		customScrollBar1.BarColor = Color.FromArgb(30, 30, 30);
		customScrollBar1.BarRadius = 16;
		customScrollBar1.BarSize = 10;
		customScrollBar1.DocPosition = 0f;
		customScrollBar1.DocSize = 100f;
		customScrollBar1.Interval = 2;
		customScrollBar1.IsRound = true;
		((Control)customScrollBar1).Location = new Point(198, 44);
		((Control)customScrollBar1).Name = "customScrollBar1";
		customScrollBar1.Orientation = (Orientation)1;
		customScrollBar1.PageSize = 10f;
		customScrollBar1.ScrollInterval = 10f;
		((Control)customScrollBar1).Size = new Size(10, 155);
		customScrollBar1.SliderColor = Color.FromArgb(70, 70, 70);
		customScrollBar1.SliderMiniSize = 20f;
		customScrollBar1.SliderPosition = 0f;
		((Control)customScrollBar1).TabIndex = 0;
		((Control)customScrollBar1).Text = "customScrollBar1";
		customScrollBar1.CustomScroll += customScrollBar1_CustomScroll;
		customCheckBox1.Checked = true;
		customCheckBox1.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox1).Location = new Point(21, 23);
		((Control)customCheckBox1).Name = "customCheckBox1";
		((Control)customCheckBox1).Size = new Size(16, 16);
		((Control)customCheckBox1).TabIndex = 3;
		((Control)customCheckBox1).Text = "customCheckBox1";
		customCheckBox1.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox1.CheckChange += customCheckBox1_CheckChange;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)customCheckBox1);
		((Control)this).Controls.Add((Control)(object)customScrollBar1);
		((Control)this).Controls.Add((Control)(object)panel_ButtonList);
		((Control)this).Controls.Add((Control)(object)label_KeyDebounce);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "KeyDebounce";
		((Control)this).Size = new Size(224, 211);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
