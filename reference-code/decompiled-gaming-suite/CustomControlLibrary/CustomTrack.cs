using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomTrack : Control
{
	public delegate void PointChangedEventHandler(object sender, CustomEventArgs e);

	private Color _BarColor = Color.FromArgb(97, 97, 97);

	private Color _SliderColor = Color.FromArgb(255, 106, 0);

	private int _SliderLength = 150;

	private int _SliderPosition;

	private int _BarSize = 2;

	private bool _AutoSize;

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

	[Category("自定义")]
	[Description("滑块长度")]
	public int SliderLength
	{
		get
		{
			return _SliderLength;
		}
		set
		{
			_SliderLength = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块起始地址")]
	public int SliderPosition
	{
		get
		{
			return _SliderPosition;
		}
		set
		{
			_SliderPosition = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑条高度")]
	public int BarSize
	{
		get
		{
			return _BarSize;
		}
		set
		{
			int num = ((Control)this).Size.Height - _BarSize;
			_ = ((Control)this).Size.Width;
			_ = _BarSize;
			_BarSize = value;
			if (_BarSize < 1)
			{
				_BarSize = 1;
			}
			num += _BarSize;
			((Control)this).Size = new Size(((Control)this).Width, num);
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
			if (_AutoSize != value)
			{
				_AutoSize = value;
				((Control)this).SetStyle((ControlStyles)64, _AutoSize);
				((Control)this).SetStyle((ControlStyles)32, false);
				((Control)this).OnAutoSizeChanged(EventArgs.Empty);
			}
		}
	}

	public event PointChangedEventHandler PointChanged;

	public CustomTrack()
	{
		((Control)this).SetStyle((ControlStyles)141330, true);
		((Control)this).CreateControl();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		_ = _BarSize;
		if (_AutoSize)
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

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Pen val = new Pen(_BarColor, (float)_BarSize);
		Pen val2 = new Pen(_SliderColor, (float)_BarSize);
		e.Graphics.DrawLine(val, 0, ((Control)this).Height / 2, ((Control)this).Width, ((Control)this).Height / 2);
		e.Graphics.DrawLine(val2, _SliderPosition, ((Control)this).Height / 2, _SliderPosition + _SliderLength, ((Control)this).Height / 2);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		mouseStatus = MouseStatus.Down;
		mousePoint = e.Location;
		((Control)this).Invalidate();
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (mouseStatus == MouseStatus.Down)
		{
			int num = Convert.ToInt32(e.Location.X) - Convert.ToInt32(mousePoint.X);
			_SliderPosition += num;
			_SliderPosition = ((_SliderPosition >= 0) ? _SliderPosition : 0);
			_SliderPosition = ((_SliderPosition > ((Control)this).Width - _SliderLength) ? (((Control)this).Width - _SliderLength) : _SliderPosition);
			PointChanged?.Invoke(this, new CustomEventArgs(_SliderPosition));
			mousePoint = e.Location;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		mouseStatus = MouseStatus.Up;
		mousePoint = e.Location;
		((Control)this).Invalidate();
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
