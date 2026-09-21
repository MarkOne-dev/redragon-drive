using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

[Category("自定义")]
[DefaultEvent("CustomScroll")]
public class CustomScrollBar : Control
{
	public delegate void CustomScrollEventHandler(object sender, CustomEventArgs e);

	private Color _BarColor;

	private Color _SliderColor;

	private bool _IsRound;

	private Orientation _Orientation;

	private int _BarSize;

	private int _BarRadius;

	private int _Interval;

	private float _PageSize;

	private float _DocSize;

	private float _DocPosition;

	private float _ScrollInterval;

	private float fPageDocRatio;

	private float fShowScrollRatio;

	private float fAbove;

	private float fCapWidth;

	private float fCapHalfWidth;

	private MouseStatus mouseStatus;

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
	[Description("控件方向")]
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
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			if (_Orientation != value)
			{
				_Orientation = value;
				if ((int)_Orientation == 0)
				{
					((Control)this).Size = new Size(((Control)this).Size.Height, ((Control)this).Size.Width);
				}
				else
				{
					((Control)this).Size = new Size(((Control)this).Size.Width, ((Control)this).Size.Height);
				}
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("背景条的长度")]
	public int BarSize
	{
		get
		{
			return _BarSize;
		}
		set
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (_BarSize != value)
			{
				_BarSize = value;
				if (_BarSize < 1)
				{
					_BarSize = 1;
				}
				if ((int)_Orientation == 0)
				{
					((Control)this).Size = new Size(((Control)this).Width, _BarSize);
				}
				else
				{
					((Control)this).Size = new Size(_BarSize, ((Control)this).Height);
				}
			}
		}
	}

	[Category("自定义")]
	[Description("背景条的弧线半径")]
	public int BarRadius
	{
		get
		{
			return _BarRadius;
		}
		set
		{
			_BarRadius = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块与背景条间隙的间隔")]
	public int Interval
	{
		get
		{
			return _Interval;
		}
		set
		{
			_Interval = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块位置")]
	[Browsable(false)]
	public float SliderPosition { get; set; }

	[Category("自定义")]
	[Description("显示长度/每页长度")]
	public float PageSize
	{
		get
		{
			return _PageSize;
		}
		set
		{
			_PageSize = value;
			CustomResize();
			ChangeSliderLocation();
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("文档长度")]
	public float DocSize
	{
		get
		{
			return _DocSize;
		}
		set
		{
			_DocSize = value;
			CustomResize();
			ChangeSliderLocation();
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("滑块可移动距离")]
	[Browsable(false)]
	public float SliderMoveRange { get; private set; }

	[Category("自定义")]
	[Description("滑块长度")]
	[Browsable(false)]
	public float SliderLength { get; private set; }

	[Category("自定义")]
	[Description("文档位置")]
	[Browsable(false)]
	public float DocPosition
	{
		get
		{
			return _DocPosition;
		}
		set
		{
			_DocPosition = value;
			ShowPositionToPosition();
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("鼠标滚轮一格的步进")]
	public float ScrollInterval
	{
		get
		{
			return _ScrollInterval;
		}
		set
		{
			_ScrollInterval = value;
			((Control)this).Invalidate();
		}
	}

	[Category("DemoUI")]
	[Description("滑块最小长度\r\n默认：20")]
	public float SliderMiniSize { get; set; }

	public event CustomScrollEventHandler CustomScroll;

	public CustomScrollBar()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_BarColor = Color.Gray;
		_SliderColor = Color.White;
		_IsRound = true;
		_Orientation = (Orientation)1;
		_BarSize = 10;
		_BarRadius = 16;
		_Interval = 2;
		_PageSize = 1f;
		_DocSize = 10f;
		_ScrollInterval = 10f;
		SliderMiniSize = 20f;
		mouseStatus = MouseStatus.Leave;
		((Control)this)._002Ector();
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Invalid comparison between Unknown and I4
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SetBoundsCore(x, y, ((int)_Orientation == 1) ? _BarSize : width, ((int)_Orientation == 0) ? _BarSize : height, specified);
	}

	private void CustomResize()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		fPageDocRatio = _PageSize / _DocSize;
		fCapWidth = (_IsRound ? _BarSize : 0);
		fCapHalfWidth = fCapWidth / 2f;
		if ((int)_Orientation == 1)
		{
			SliderLength = fPageDocRatio * ((float)((Control)this).Height - fCapWidth);
			if (SliderLength < SliderMiniSize)
			{
				SliderLength = SliderMiniSize;
			}
			SliderMoveRange = (float)((Control)this).Height - fCapWidth - SliderLength;
		}
		else
		{
			SliderLength = fPageDocRatio * ((float)((Control)this).Width - fCapWidth);
			if (SliderLength < SliderMiniSize)
			{
				SliderLength = SliderMiniSize;
			}
			SliderMoveRange = (float)((Control)this).Width - fCapWidth - SliderLength;
		}
		fShowScrollRatio = ((SliderMoveRange == 0f) ? 0f : ((_DocSize - _PageSize) / SliderMoveRange));
	}

	private void ShowPositionToPosition()
	{
		CustomResize();
		SliderPosition = ((fShowScrollRatio == 0f) ? 0f : (_DocPosition / fShowScrollRatio));
	}

	private void ChangeSliderLocation()
	{
		float num = (float)((Control)this).Height - fCapWidth;
		float num2 = SliderPosition + SliderLength;
		if (num2 > num && num > 0f)
		{
			SliderPosition -= num2 - num;
			_DocPosition = SliderPosition * fShowScrollRatio;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnPaint(e);
		CustomResize();
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Pen val = new Pen(_BarColor, fCapWidth);
		Pen val2 = new Pen(_SliderColor, fCapWidth - (float)(_Interval * 2));
		if (_IsRound)
		{
			val.StartCap = (LineCap)2;
			val.EndCap = (LineCap)2;
			val2.StartCap = (LineCap)2;
			val2.EndCap = (LineCap)2;
		}
		if ((int)_Orientation == 0)
		{
			e.Graphics.DrawLine(val, fCapHalfWidth, (float)(((Control)this).Height / 2), (float)((Control)this).Width - fCapHalfWidth, (float)(((Control)this).Height / 2));
			e.Graphics.DrawLine(val2, SliderPosition + fCapHalfWidth, (float)(((Control)this).Height / 2), SliderPosition + SliderLength - fCapHalfWidth, (float)(((Control)this).Height / 2));
		}
		else
		{
			e.Graphics.DrawLine(val, (float)(((Control)this).Width / 2), fCapHalfWidth, (float)(((Control)this).Width / 2), (float)((Control)this).Height - fCapHalfWidth);
			e.Graphics.DrawLine(val2, (float)(((Control)this).Width / 2), SliderPosition + fCapHalfWidth, (float)(((Control)this).Width / 2), SliderPosition + SliderLength + fCapHalfWidth);
		}
	}

	private int CheckMousePositionInSlider(Point point)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		if (_DocSize > _PageSize)
		{
			float sliderPosition = SliderPosition;
			float num = SliderPosition + SliderLength;
			float num2;
			float num3;
			if ((int)_Orientation == 1)
			{
				num2 = point.Y;
				num3 = ((Control)this).Height;
			}
			else
			{
				num2 = point.X;
				num3 = ((Control)this).Width;
			}
			if (num2 > 0f && num2 < sliderPosition)
			{
				return 0;
			}
			if (num2 > sliderPosition && num2 < num)
			{
				return 1;
			}
			if (num2 > num && num2 < num3)
			{
				return 2;
			}
			return -1;
		}
		return -1;
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		((Control)this).OnMouseDown(e);
		((Control)this).Select();
		switch (CheckMousePositionInSlider(e.Location))
		{
		case 0:
			_DocPosition -= _PageSize;
			if (_DocPosition < 0f)
			{
				_DocPosition = 0f;
			}
			ShowPositionToPosition();
			((Control)this).Invalidate();
			CustomScroll?.Invoke(this, new CustomEventArgs(_DocPosition));
			break;
		case 1:
			mouseStatus = MouseStatus.Down;
			if ((int)_Orientation == 1)
			{
				fAbove = (float)e.Location.Y - SliderPosition;
			}
			else
			{
				fAbove = (float)e.Location.X - SliderPosition;
			}
			break;
		case 2:
			_DocPosition += _PageSize;
			ShowPositionToPosition();
			if (SliderPosition > SliderMoveRange)
			{
				SliderPosition = SliderMoveRange;
				_DocPosition = SliderPosition * fShowScrollRatio;
			}
			((Control)this).Invalidate();
			CustomScroll?.Invoke(this, new CustomEventArgs(_DocPosition));
			break;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		((Control)this).OnMouseMove(e);
		if (mouseStatus == MouseStatus.Down)
		{
			if ((int)_Orientation == 1)
			{
				SliderPosition = (float)e.Location.Y - fAbove;
			}
			else
			{
				SliderPosition = (float)e.Location.X - fAbove;
			}
			if (SliderPosition < 0f)
			{
				SliderPosition = 0f;
			}
			if (SliderPosition > SliderMoveRange)
			{
				SliderPosition = SliderMoveRange;
			}
			_DocPosition = SliderPosition * fShowScrollRatio;
			((Control)this).Invalidate();
			CustomScroll?.Invoke(this, new CustomEventArgs(_DocPosition));
		}
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		mouseStatus = MouseStatus.Up;
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((Control)this).OnMouseWheel(e);
		if (!(_DocSize > _PageSize))
		{
			return;
		}
		bool num = e.Delta <= 0;
		_ = Convert.ToSingle(Math.Abs(e.Delta)) / 120f;
		if (num)
		{
			_DocPosition += _ScrollInterval;
			ShowPositionToPosition();
			if (SliderPosition > SliderMoveRange)
			{
				SliderPosition = SliderMoveRange;
				_DocPosition = SliderPosition * fShowScrollRatio;
			}
			((Control)this).Invalidate();
			CustomScroll?.Invoke(this, new CustomEventArgs(_DocPosition));
		}
		else
		{
			_DocPosition -= _ScrollInterval;
			if (_DocPosition < 0f)
			{
				_DocPosition = 0f;
			}
			ShowPositionToPosition();
			((Control)this).Invalidate();
			CustomScroll?.Invoke(this, new CustomEventArgs(_DocPosition));
		}
	}
}
