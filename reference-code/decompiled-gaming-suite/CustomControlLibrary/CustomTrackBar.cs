using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CustomControlLibrary;

[DefaultEvent("ValueChanged")]
public class CustomTrackBar : Control
{
	public delegate void ValueChangedEventHandler(object sender, CustomEventArgs e);

	public delegate void SetValueEventHandler(object sender, CustomEventArgs e);

	public enum TrackModeEnum
	{
		Draw,
		Image
	}

	private Color _BarColor = Color.FromArgb(255, 255, 255);

	private Color _SliderColor = Color.FromArgb(255, 106, 0);

	private bool _Enable = true;

	private Color _DisableColor = Color.FromArgb(57, 57, 57);

	private bool _IsRound = true;

	private int _BarSize = 2;

	private Size _SliderSize = new Size(6, 18);

	private int _SliderRadius = 2;

	private bool _AutoSize;

	private Orientation _Orientation;

	private int _Value = 20;

	private int _Minimum;

	private int _Maximum = 100;

	private int _Step = 1;

	private Image _TrackImage;

	private int originalLength = GetSystemMetrics(3) * 8 / 3;

	private MouseStatus mouseStatus = MouseStatus.Leave;

	private PointF mousePoint = Point.Empty;

	public override Color BackColor { get; set; } = Color.Transparent;

	[Category("自定义")]
	[Description("背景条颜色")]
	public Color BarColor
	{
		get
		{
			return _BarColor;
		}
		set
		{
			_BarColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块颜色")]
	public Color SliderColor
	{
		get
		{
			return _SliderColor;
		}
		set
		{
			_SliderColor = value;
			((Control)this).Invalidate();
		}
	}

	public bool Enable
	{
		get
		{
			return _Enable;
		}
		set
		{
			_Enable = value;
			((Control)this).Invalidate();
		}
	}

	public Color DisableColor
	{
		get
		{
			return _DisableColor;
		}
		set
		{
			_DisableColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("是否是圆角\r\n默认：是")]
	public bool IsRound
	{
		get
		{
			return _IsRound;
		}
		set
		{
			_IsRound = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description(" 滑条高度（水平）/宽度（垂直）")]
	public int BarSize
	{
		get
		{
			return _BarSize;
		}
		set
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			int num = ((Control)this).Size.Height - _BarSize;
			int num2 = ((Control)this).Size.Width - _BarSize;
			_BarSize = value;
			if (_BarSize < 1)
			{
				_BarSize = 1;
			}
			if ((int)_Orientation == 0)
			{
				num += _BarSize;
				((Control)this).Size = new Size(((Control)this).Width, num);
			}
			else
			{
				num2 += _BarSize;
				((Control)this).Size = new Size(_BarSize, ((Control)this).Height);
			}
		}
	}

	[Category("自定义")]
	[Description("滑块大小，当TrackStyle为Image时,可配合使用")]
	[Browsable(true)]
	[DefaultValue(typeof(Size), "8,10")]
	public Size SizeSlidSize
	{
		get
		{
			return _SliderSize;
		}
		set
		{
			_SliderSize = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块四个角的弧度")]
	[Browsable(true)]
	[DefaultValue(typeof(int), "2")]
	public int SliderRadius
	{
		get
		{
			return _SliderRadius;
		}
		set
		{
			_SliderRadius = value;
			((Control)this).Invalidate();
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override bool AutoSize
	{
		get
		{
			return _AutoSize;
		}
		set
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (_AutoSize == value)
			{
				return;
			}
			_AutoSize = value;
			if ((int)_Orientation == 0)
			{
				((Control)this).SetStyle((ControlStyles)64, _AutoSize);
				((Control)this).SetStyle((ControlStyles)32, false);
			}
			else
			{
				((Control)this).SetStyle((ControlStyles)32, _AutoSize);
				((Control)this).SetStyle((ControlStyles)64, false);
			}
			int num = originalLength;
			try
			{
				if ((int)_Orientation == 0)
				{
					((Control)this).Height = (_AutoSize ? ((Control)this).DefaultSize.Height : num);
				}
				else
				{
					((Control)this).Width = (_AutoSize ? ((Control)this).DefaultSize.Height : num);
				}
			}
			finally
			{
				originalLength = num;
			}
			((Control)this).OnAutoSizeChanged(EventArgs.Empty);
		}
	}

	[Category("自定义")]
	[Description("控件方向垂直（Vertical）或者是水平（Horizontal）")]
	public Orientation Orientation
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _Orientation;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (_Orientation != value)
			{
				_Orientation = value;
				((Control)this).Size = new Size(((Control)this).Width, ((Control)this).Height);
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[DefaultValue(20)]
	[Browsable(true)]
	[Description("当前滑块的值")]
	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			if (_Value != value)
			{
				_Value = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("最大值")]
	public int Minimum
	{
		get
		{
			return _Minimum;
		}
		set
		{
			_Minimum = value;
			if (_Minimum >= _Maximum)
			{
				_Minimum = _Maximum - 1;
			}
			if (_Minimum < 0)
			{
				_Minimum = 0;
			}
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("最大值")]
	public int Maximum
	{
		get
		{
			return _Maximum;
		}
		set
		{
			_Maximum = value;
			if (_Maximum <= _Minimum)
			{
				_Maximum = _Minimum + 1;
			}
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("最大值")]
	public int Step
	{
		get
		{
			return _Step;
		}
		set
		{
			_Step = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[DefaultValue(TrackModeEnum.Draw)]
	[Browsable(true)]
	[Description("滑块的模式：\r\nDraw：滑块;Image：图片")]
	public TrackModeEnum TrackMode { get; set; }

	[Category("自定义")]
	[DefaultValue(null)]
	[Browsable(true)]
	[Description("滑块的图片")]
	public Image TrackImage
	{
		get
		{
			return _TrackImage;
		}
		set
		{
			_TrackImage = value;
		}
	}

	public event ValueChangedEventHandler ValueChanged;

	public event SetValueEventHandler SetValue;

	public CustomTrackBar()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	private static extern int GetSystemMetrics(int nIndex);

	private void ValueToPoint()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		if (_Enable)
		{
			if (_IsRound)
			{
				num = (float)_BarSize / 2f;
				num2 = _BarSize;
			}
			float num3 = Convert.ToSingle(_Value / _Step * _Step - _Minimum) / (float)(_Maximum - _Minimum);
			if ((int)_Orientation == 0)
			{
				float x = num3 * ((float)((Control)this).Width - num2) + num;
				mousePoint = new PointF(x, num);
			}
			else
			{
				float y = (float)((Control)this).Height - num - num3 * ((float)((Control)this).Height - num2);
				mousePoint = new PointF(num, y);
			}
		}
	}

	private void PointToValue()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = 0f;
		if (_Enable)
		{
			if (_IsRound)
			{
				num2 = _BarSize;
				num = (float)_BarSize / 2f;
			}
			int num3 = (((int)_Orientation != 0) ? Convert.ToInt32(Convert.ToSingle(mousePoint.Y - num) / ((float)((Control)this).Height - num2) * (float)(_Maximum - _Minimum) + (float)_Minimum) : Convert.ToInt32(Convert.ToSingle(mousePoint.X - num) / ((float)((Control)this).Width - num2) * (float)(_Maximum - _Minimum) + (float)_Minimum));
			num3 /= _Step;
			_Value = num3 * _Step;
			if (_Value < _Minimum)
			{
				_Value = _Minimum;
			}
			if (_Value > _Maximum)
			{
				_Value = _Maximum;
			}
			ValueChanged?.Invoke(this, new CustomEventArgs(_Value));
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		_ = _BarSize;
		originalLength = (((int)_Orientation == 0) ? height : width);
		if (_AutoSize && (int)_Orientation == 0)
		{
			if ((specified & 8) != 0)
			{
				height = ((Control)this).DefaultSize.Height;
			}
			else if ((specified & 4) != 0)
			{
				width = ((Control)this).DefaultSize.Width;
			}
		}
		((Control)this).SetBoundsCore(x, y, width, height, specified);
	}

	public static GraphicsPath CreateRoundPath(RectangleF rect, int radius)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		int num = 1;
		val.AddArc(rect.X, rect.Y, (float)radius, (float)radius, 180f, 90f);
		val.AddArc(rect.Right - (float)radius - (float)num, rect.Y, (float)radius, (float)radius, 270f, 90f);
		val.AddArc(rect.Right - (float)radius - (float)num, rect.Bottom - (float)radius - (float)num, (float)radius, (float)radius, 0f, 90f);
		val.AddArc(rect.X, rect.Bottom - (float)radius - (float)num, (float)radius, (float)radius, 90f, 90f);
		val.CloseFigure();
		return val;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		ValueToPoint();
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Pen val = new Pen(_Enable ? _BarColor : _DisableColor, (float)_BarSize);
		Pen val2 = new Pen(_Enable ? _SliderColor : _DisableColor, (float)_BarSize);
		float num = 0f;
		float num2 = 0f;
		if (_IsRound)
		{
			num2 = _BarSize;
			num = (float)_BarSize / 2f;
			val.StartCap = (LineCap)2;
			val.EndCap = (LineCap)2;
			val2.StartCap = (LineCap)2;
			val2.EndCap = (LineCap)2;
		}
		float num3 = 0f;
		if ((int)_Orientation == 0)
		{
			e.Graphics.DrawLine(val, num, (float)((Control)this).Height / 2f, (float)((Control)this).Width - num, (float)((Control)this).Height / 2f);
			num3 = mousePoint.X;
			if (num3 < num2)
			{
				num3 = num2;
			}
			if (num3 > (float)((Control)this).Width - num * 4f)
			{
				num3 = (float)((Control)this).Width - num * 4f;
			}
		}
		else
		{
			e.Graphics.DrawLine(val, (float)((Control)this).Width / 2f, num, (float)((Control)this).Width / 2f, (float)((Control)this).Height - num);
			num3 = mousePoint.Y;
			if (num3 < num2)
			{
				num3 = num2;
			}
			if (num3 > (float)((Control)this).Height - num)
			{
				num3 = (float)((Control)this).Height - num;
			}
		}
		if (_Enable)
		{
			if ((int)_Orientation == 0)
			{
				e.Graphics.DrawLine(val2, num, (float)((Control)this).Height / 2f, num3, (float)((Control)this).Height / 2f);
			}
			else
			{
				e.Graphics.DrawLine(val2, (float)((Control)this).Width / 2f, num3, (float)((Control)this).Width / 2f, (float)((Control)this).Height - num);
			}
		}
		if (!_Enable)
		{
			return;
		}
		if (TrackMode == TrackModeEnum.Draw)
		{
			PointF location = new PointF(num3 - num * 2f, (float)(((Control)this).Height - _SliderSize.Height) / 2f);
			RectangleF rect = new RectangleF(location, _SliderSize);
			SolidBrush val3 = new SolidBrush(_Enable ? _SliderColor : _DisableColor);
			try
			{
				GraphicsPath val4 = CreateRoundPath(rect, _SliderRadius);
				try
				{
					e.Graphics.FillPath((Brush)(object)val3, val4);
					Pen val5 = new Pen(_Enable ? _SliderColor : _DisableColor, 1f);
					try
					{
						e.Graphics.DrawPath(val5, val4);
						return;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		if (((Control)this).Enabled && TrackImage != null)
		{
			PointF location2 = new PointF(num3 - num * 2f, (float)(((Control)this).Height - _TrackImage.Height) / 2f);
			RectangleF rectangleF = new RectangleF(location2, _TrackImage.Size);
			e.Graphics.DrawImage(TrackImage, rectangleF);
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		mouseStatus = MouseStatus.Down;
		mousePoint = e.Location;
		PointToValue();
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (mouseStatus == MouseStatus.Down)
		{
			mousePoint = e.Location;
			PointToValue();
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		mouseStatus = MouseStatus.Up;
		SetValue?.Invoke(this, new CustomEventArgs(_Value));
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		mouseStatus = MouseStatus.Enter;
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		mouseStatus = MouseStatus.Leave;
	}
}
