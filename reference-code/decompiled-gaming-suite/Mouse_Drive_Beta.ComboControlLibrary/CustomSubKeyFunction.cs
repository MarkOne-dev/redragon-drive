using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class CustomSubKeyFunction : UserControl
{
	public delegate void ButtonSelectEventHandler(object sender, CustomEventArgs e);

	public delegate void DoubleClickEventHandler(object sender, EventArgs e);

	private Image _NormalImage;

	private Image _NormalImageArrow;

	private Image _SelectImage;

	private Image _SelectImageArrow;

	private int _SelectedIndex = -1;

	private int _ButtonCount = 6;

	private int _ButtonInterval = 3;

	private Image _CheckImage;

	private Image _UncheckImage;

	public List<CustomRadioButton> CustomRadioButtons = new List<CustomRadioButton>();

	private int PageButtonCount = 6;

	private int CurrentIndex;

	private IContainer components;

	private Panel panel_ButtonList;

	private CustomScrollBar customScrollBar1;

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
	[Description("功能按钮正常时图片")]
	public Image NormalImageArrow
	{
		get
		{
			return _NormalImageArrow;
		}
		set
		{
			_NormalImageArrow = value;
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
	[Description("功能按钮选中时图片(带箭头)")]
	public Image SelectImageArrow
	{
		get
		{
			return _SelectImageArrow;
		}
		set
		{
			_SelectImageArrow = value;
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
	[Description("选择框选中时图片")]
	public Image CheckImage
	{
		get
		{
			return _CheckImage;
		}
		set
		{
			_CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选择框选中时图片")]
	public Image UncheckImage
	{
		get
		{
			return _UncheckImage;
		}
		set
		{
			_UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public event ButtonSelectEventHandler ButtonSelect;

	public event DoubleClickEventHandler DoubleClick;

	public CustomSubKeyFunction()
	{
		InitializeComponent();
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
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
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, ((i - num) * (_SelectImage.Height + _ButtonInterval) + _ButtonInterval) * FormResize.GetDpi() / 96);
		}
		customScrollBar1.DocPosition = num * (_SelectImage.Height + _ButtonInterval);
		CurrentIndex = num;
	}

	private void customScrollBar1_CustomScroll(object sender, CustomEventArgs e)
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
		{
			((Control)panel_ButtonList).Controls[i].Location = new Point(0, (i * (_SelectImage.Height + _ButtonInterval) + _ButtonInterval - Convert.ToInt32(customScrollBar1.DocPosition)) * FormResize.GetDpi() / 96);
		}
	}

	public void UpdateUI(bool[] pull, string[] text, string name)
	{
		if (_ButtonCount <= 0 || (_NormalImage == null && _SelectImage == null))
		{
			return;
		}
		((Control)panel_ButtonList).Controls.Clear();
		CustomRadioButtons.Clear();
		Image val = ((_NormalImage != null) ? _NormalImage : _SelectImage);
		for (int i = 0; i < _ButtonCount; i++)
		{
			CustomRadioButton customRadioButton = new CustomRadioButton();
			customRadioButton.CheckImage = (pull[i] ? _SelectImageArrow : _SelectImage);
			((Control)customRadioButton).ForeColor = FormMain.driveParam.BtnForeClr;
			((Control)customRadioButton).Location = new Point(0, i * (_ButtonInterval + val.Height) * FormResize.GetDpi() / 96);
			((Control)customRadioButton).Name = "customRadioButton_SubKeyFunction_" + name + i;
			customRadioButton.TextString = text[i];
			customRadioButton.UncheckImage = (pull[i] ? _NormalImageArrow : _NormalImage);
			((Control)customRadioButton).Size = new Size(((Control)panel_ButtonList).Width, (((Control)panel_ButtonList).Height - _ButtonInterval * 5) / 6);
			((Control)customRadioButton).Tag = i;
			((Control)customRadioButton).Click -= Button_Click;
			((Control)customRadioButton).Click += Button_Click;
			if (!pull[i])
			{
				customRadioButton.DoubleClick -= Button_DoubleClick;
				customRadioButton.DoubleClick += Button_DoubleClick;
			}
			((Control)panel_ButtonList).Controls.Add((Control)(object)customRadioButton);
			CustomRadioButtons.Add(customRadioButton);
		}
		customScrollBar1.PageSize = ((Control)panel_ButtonList).Size.Height;
		customScrollBar1.DocSize = _ButtonCount * (_ButtonInterval + val.Height);
		customScrollBar1.DocPosition = 0f;
		PageButtonCount = (((Control)panel_ButtonList).Size.Height + _ButtonInterval) / (_ButtonInterval + val.Height);
	}

	public void SetFontSize(float fontSize)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		for (int i = 0; i < _ButtonCount; i++)
		{
			((Control)CustomRadioButtons[i]).Font = new Font("微软雅黑", fontSize);
		}
	}

	private void Button_DoubleClick(object sender, EventArgs e)
	{
		((Control)this).Hide();
		DoubleClick?.Invoke(this, e);
	}

	private void Button_Click(object sender, EventArgs e)
	{
		int num = 0;
		num = (int)((Control)(CustomRadioButton)sender).Tag;
		_SelectedIndex = num;
		ButtonSelect?.Invoke(this, new CustomEventArgs(_SelectedIndex));
	}

	private void ChangePanelLocation()
	{
		int num = 0;
		if (_ButtonCount != 0 && CustomRadioButtons.Count != 0)
		{
			num = ((_SelectedIndex >= PageButtonCount / 2 && _ButtonCount > PageButtonCount) ? ((_SelectedIndex < _ButtonCount - PageButtonCount / 2) ? (_SelectedIndex - PageButtonCount / 2) : (_ButtonCount - PageButtonCount)) : 0);
			for (int i = 0; i < ((ArrangedElementCollection)((Control)panel_ButtonList).Controls).Count; i++)
			{
				((Control)panel_ButtonList).Controls[i].Location = new Point(0, ((i - num) * (_SelectImage.Height + _ButtonInterval) + _ButtonInterval) * FormResize.GetDpi() / 96);
			}
			CurrentIndex = num;
			customScrollBar1.DocPosition = num * (_SelectImage.Height + _ButtonInterval);
			((RadioButton)CustomRadioButtons[_SelectedIndex]).Checked = true;
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
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		panel_ButtonList = new Panel();
		customScrollBar1 = new CustomScrollBar();
		((Control)this).SuspendLayout();
		((Control)panel_ButtonList).BackColor = Color.Transparent;
		((Control)panel_ButtonList).Location = new Point(10, 10);
		((Control)panel_ButtonList).Margin = new Padding(4, 5, 4, 5);
		((Control)panel_ButtonList).Name = "panel_ButtonList";
		((Control)panel_ButtonList).Size = new Size(155, 175);
		((Control)panel_ButtonList).TabIndex = 1;
		customScrollBar1.BarColor = Color.FromArgb(30, 30, 30);
		customScrollBar1.BarRadius = 16;
		customScrollBar1.BarSize = 10;
		customScrollBar1.DocPosition = 0f;
		customScrollBar1.DocSize = 10f;
		customScrollBar1.Interval = 2;
		customScrollBar1.IsRound = true;
		((Control)customScrollBar1).Location = new Point(178, 9);
		((Control)customScrollBar1).Margin = new Padding(4, 5, 4, 5);
		((Control)customScrollBar1).Name = "customScrollBar1";
		customScrollBar1.Orientation = (Orientation)1;
		customScrollBar1.PageSize = 1f;
		customScrollBar1.ScrollInterval = 10f;
		((Control)customScrollBar1).Size = new Size(10, 176);
		customScrollBar1.SliderColor = Color.FromArgb(70, 70, 70);
		customScrollBar1.SliderMiniSize = 20f;
		customScrollBar1.SliderPosition = 0f;
		((Control)customScrollBar1).TabIndex = 2;
		((Control)customScrollBar1).Text = "customScrollBar1";
		customScrollBar1.CustomScroll += customScrollBar1_CustomScroll;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.按键功能背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Control)this).Controls.Add((Control)(object)customScrollBar1);
		((Control)this).Controls.Add((Control)(object)panel_ButtonList);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "CustomSubKeyFunction";
		((Control)this).Size = new Size(200, 194);
		((Control)this).ResumeLayout(false);
	}
}
