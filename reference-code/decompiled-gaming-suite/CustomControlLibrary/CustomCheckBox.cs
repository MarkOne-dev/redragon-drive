using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomCheckBox : Control
{
	public delegate void CheckChangeEventHandler(object sender, CustomEventArgs e);

	private bool _Checked;

	private Image _CheckImage;

	private Image _UncheckImage;

	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[Category("自定义")]
	[Description("按键状态：\r\n默认是：没按下")]
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
	[Description("选择时的图片")]
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
	[Description("未选择时的图片")]
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

	public event CheckChangeEventHandler CheckChange;

	public CustomCheckBox()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = (_Checked ? _CheckImage : UncheckImage);
		if (val == null)
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val2, e.ClipRectangle);
			((Brush)val2).Dispose();
		}
		else
		{
			PointF location = new PointF(0f, (((Control)this).Size.Height - val.Size.Height) / 2);
			new RectangleF(location, val.Size);
			e.Graphics.DrawImage(val, 0, 0, ((Control)this).Width, ((Control)this).Height);
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		((Control)this).OnMouseClick(e);
		_Checked = !_Checked;
		((Control)this).Invalidate();
		CheckChange?.Invoke(this, new CustomEventArgs(_Checked));
	}
}
