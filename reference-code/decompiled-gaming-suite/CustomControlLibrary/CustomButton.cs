using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomButton : Control
{
	private Image _MouseEnterImage;

	private Image _MouseDownImage;

	private Image _NormalImage;

	private MouseStatus mouseStatus = MouseStatus.Leave;

	public override Color BackColor { get; set; } = Color.Transparent;

	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("鼠标进入框时的图片")]
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
	[Description("鼠标按下时的图片")]
	public Image MouseDownImage
	{
		get
		{
			return _MouseDownImage;
		}
		set
		{
			_MouseDownImage = value;
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

	public CustomButton()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = null;
		val = ((mouseStatus == MouseStatus.Down) ? _MouseDownImage : ((mouseStatus != MouseStatus.Enter) ? _NormalImage : _MouseEnterImage));
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
		SizeF size = ((Control)this).CreateGraphics().MeasureString(((Control)this).Text, ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location2 = new PointF((((Control)this).Size.Width - num) / 2, (((Control)this).Size.Height - num2) / 2);
		RectangleF rectangleF = new RectangleF(location2, size);
		e.Graphics.DrawString(((Control)this).Text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		mouseStatus = MouseStatus.Down;
		((Control)this).Invalidate();
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
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
