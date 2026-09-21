using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Mouse_Drive_Beta;

namespace CustomControlLibrary;

public class CustomStateButton : Control
{
	private Timer timer;

	private int count;

	private Image _FailImage;

	private Image _SuccessImage;

	private Image _NormalImage;

	private Image _UpdatingImage;

	private ButtonStateEnum _buttonState;

	private string _DoingText;

	private string _NormalText;

	private int _Percent;

	public override Color BackColor { get; set; }

	[Category("自定义")]
	[Description("失败时的图片")]
	public Image FailImage
	{
		get
		{
			return _FailImage;
		}
		set
		{
			_FailImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("成功时的图片")]
	public Image SuccessImage
	{
		get
		{
			return _SuccessImage;
		}
		set
		{
			_SuccessImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("普通模式时的图片")]
	public Image NormalImage
	{
		get
		{
			return _NormalImage;
		}
		set
		{
			if (_NormalImage != value)
			{
				_NormalImage = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("正在更新时的图片")]
	public Image UpdatedImage
	{
		get
		{
			return _UpdatingImage;
		}
		set
		{
			_UpdatingImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("当前状态")]
	public ButtonStateEnum ButtonState
	{
		get
		{
			return _buttonState;
		}
		set
		{
			if (_buttonState != value)
			{
				_buttonState = value;
				count = 0;
				((Control)this).Text = _DoingText;
				if (_buttonState == ButtonStateEnum.Updating)
				{
					timer.Start();
					_Percent = 0;
				}
				if (_buttonState == ButtonStateEnum.Doing)
				{
					timer.Start();
				}
				if (_buttonState == ButtonStateEnum.Fail)
				{
					timer.Stop();
				}
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("状态过程文本")]
	public string DoingText
	{
		get
		{
			return _DoingText;
		}
		set
		{
			_DoingText = value;
			((Control)this).Text = _DoingText;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("开始的文本")]
	public string NormalText
	{
		get
		{
			return _NormalText;
		}
		set
		{
			_NormalText = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("升级百分比")]
	public int Percent
	{
		get
		{
			return _Percent;
		}
		set
		{
			_Percent = value;
			((Control)this).Invalidate();
		}
	}

	public CustomStateButton()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		BackColor = Color.Transparent;
		_DoingText = "";
		_NormalText = "";
		((Control)this)._002Ector();
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
		if (timer == null)
		{
			timer = new Timer();
			timer.Interval = 600;
			timer.Tick += Timer_Tick;
		}
		timer.Start();
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		if (_buttonState == ButtonStateEnum.Doing || _buttonState == ButtonStateEnum.Updating)
		{
			if (count < 3)
			{
				count++;
				((Control)this).Text = ((Control)this).Text + ".";
			}
			else
			{
				count = 0;
				((Control)this).Text = _DoingText;
			}
			((Control)this).Invalidate();
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		int val = ((_FailImage != null) ? _FailImage.Width : 0);
		int val2 = ((_FailImage != null) ? _FailImage.Height : 0);
		int val3 = ((_SuccessImage != null) ? _SuccessImage.Width : 0);
		int val4 = ((_SuccessImage != null) ? _SuccessImage.Height : 0);
		int val5 = ((_NormalImage != null) ? _NormalImage.Width : 0);
		int val6 = ((_NormalImage != null) ? _NormalImage.Height : 0);
		int val7 = Math.Max(val, val3);
		val7 = Math.Max(val7, val5);
		val7 = ((val7 == 0) ? width : val7);
		int val8 = Math.Max(val2, val4);
		val8 = Math.Max(val8, val6);
		val8 = ((val8 == 0) ? height : val8);
		((Control)this).SetBoundsCore(x, y, val7, val8, specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = null;
		if (_buttonState == ButtonStateEnum.Normal || _buttonState == ButtonStateEnum.Doing)
		{
			val = _NormalImage;
		}
		else if (_buttonState == ButtonStateEnum.Success || _buttonState == ButtonStateEnum.PairSuccess)
		{
			val = _SuccessImage;
		}
		else if (_buttonState == ButtonStateEnum.Fail)
		{
			val = _FailImage;
		}
		else if (_buttonState == ButtonStateEnum.Updating)
		{
			val = _NormalImage;
		}
		if (val == null)
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val2, e.ClipRectangle);
			((Brush)val2).Dispose();
		}
		else if (_buttonState == ButtonStateEnum.Updating)
		{
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = (SmoothingMode)2;
			val = _UpdatingImage;
			Rectangle rectangle = new Rectangle(0, 0, _NormalImage.Width, _NormalImage.Height);
			graphics.DrawImageUnscaledAndClipped(_NormalImage, rectangle);
			rectangle = new Rectangle(0, 0, _Percent * _UpdatingImage.Width / 100, _UpdatingImage.Height);
			graphics.DrawImageUnscaledAndClipped(val, rectangle);
		}
		else
		{
			PointF location = new PointF((((Control)this).Size.Width - val.Size.Width) / 2, (((Control)this).Size.Height - val.Size.Height) / 2);
			RectangleF rectangleF = new RectangleF(location, val.Size);
			e.Graphics.DrawImage(val, rectangleF);
		}
		SizeF size = ((Control)this).CreateGraphics().MeasureString(((Control)this).Text, ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location2 = new PointF((((Control)this).Size.Width - num) / 2, (((Control)this).Size.Height - num2) / 2);
		RectangleF rectangleF2 = new RectangleF(location2, size);
		e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF2);
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		if (_buttonState != ButtonStateEnum.Doing && _buttonState != ButtonStateEnum.Updating && _buttonState != ButtonStateEnum.PairSuccess)
		{
			_buttonState = ButtonStateEnum.Normal;
			((Control)this).Text = _NormalText;
			((Control)this).Invalidate();
		}
	}
}
