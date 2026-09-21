using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomRecordMacro : Control
{
	private Image _StartImage_MouseEnter;

	private Image _StartImage_Normal;

	private Image _StopImage_MouseEnter;

	private Image _StopImage_Normal;

	private bool _Checked;

	private string _StopText = "";

	private string _StartText = "";

	private MouseStatus mouseStatus = MouseStatus.Leave;

	public override Color BackColor { get; set; } = Color.Transparent;

	[Category("自定义")]
	[Description("鼠标进入框时的图片")]
	public Image StartImage_MouseEnter
	{
		get
		{
			return _StartImage_MouseEnter;
		}
		set
		{
			_StartImage_MouseEnter = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("普通模式时的图片")]
	public Image StartImage_Normal
	{
		get
		{
			return _StartImage_Normal;
		}
		set
		{
			if (_StartImage_Normal != value)
			{
				_StartImage_Normal = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("鼠标按下时的图片")]
	public Image StopImage_MouseEnter
	{
		get
		{
			return _StopImage_MouseEnter;
		}
		set
		{
			_StopImage_MouseEnter = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("普通模式时的图片")]
	public Image StopImage_Normal
	{
		get
		{
			return _StopImage_Normal;
		}
		set
		{
			if (_StopImage_Normal != value)
			{
				_StopImage_Normal = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("是否选择")]
	public bool Checked
	{
		get
		{
			return _Checked;
		}
		set
		{
			_Checked = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选择时出现的文本")]
	public string StopText
	{
		get
		{
			return _StopText;
		}
		set
		{
			_StopText = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("未选择时出现的文本")]
	public string StartText
	{
		get
		{
			return _StartText;
		}
		set
		{
			_StartText = value;
			((Control)this).Invalidate();
		}
	}

	public CustomRecordMacro()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = null;
		val = (_Checked ? ((mouseStatus != MouseStatus.Enter) ? _StopImage_Normal : _StopImage_MouseEnter) : ((mouseStatus != MouseStatus.Enter) ? _StartImage_Normal : _StartImage_MouseEnter));
		if (val == null)
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val2, e.ClipRectangle);
			((Brush)val2).Dispose();
		}
		else
		{
			PointF location = new PointF((((Control)this).Size.Width - val.Size.Width) / 2, (((Control)this).Size.Height - val.Size.Height) / 2);
			new RectangleF(location, val.Size);
			e.Graphics.DrawImage(val, 0, 0, ((Control)this).Width, ((Control)this).Height);
		}
		Graphics obj = ((Control)this).CreateGraphics();
		string text = (_Checked ? _StopText : _StartText);
		SizeF size = obj.MeasureString(text, ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location2 = new PointF((((Control)this).Size.Width - num + 30) / 2, (((Control)this).Size.Height - num2) / 2);
		RectangleF rectangleF = new RectangleF(location2, size);
		e.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		mouseStatus = MouseStatus.Down;
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		mouseStatus = MouseStatus.Up;
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		mouseStatus = MouseStatus.Enter;
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		mouseStatus = MouseStatus.Leave;
		((Control)this).Invalidate();
	}
}
