using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomRadioButton : RadioButton
{
	public delegate void DoubleClickEventHandler(object sender, EventArgs e);

	private Image _UncheckImage;

	private Image _MouseEnterImage;

	private Image _CheckImage;

	private Image _DisableImage;

	private string _TextString;

	private Color _DisableColor = Color.FromArgb(57, 57, 57);

	[Category("自定义")]
	[Description("鼠标离开的背景图片")]
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

	[Category("自定义")]
	[Description("鼠标进入的背景图片")]
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
	[Description("鼠标按下的背景图片")]
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
	[Description("不使能时的背景图片")]
	public Image DisableImage
	{
		get
		{
			return _DisableImage;
		}
		set
		{
			_DisableImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("显示文本")]
	public string TextString
	{
		get
		{
			return _TextString;
		}
		set
		{
			_TextString = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("不使能时字体颜色")]
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

	public event DoubleClickEventHandler DoubleClick;

	public CustomRadioButton()
	{
		((Control)this).SetStyle((ControlStyles)211218, true);
		((Control)this).UpdateStyles();
		((RadioButton)this).Appearance = (Appearance)1;
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((Control)this).AutoSize = false;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		((ButtonBase)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = null;
		val = (((Control)this).Enabled ? ((!((RadioButton)this).Checked) ? _UncheckImage : _CheckImage) : ((!((RadioButton)this).Checked) ? _UncheckImage : _CheckImage));
		if (val == null)
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val2, e.ClipRectangle);
			((Brush)val2).Dispose();
		}
		else
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Size.Width, ((Control)this).Size.Height);
			e.Graphics.DrawImageUnscaledAndClipped(val, rectangle);
		}
		SizeF size = ((Control)this).CreateGraphics().MeasureString(_TextString, ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location = new PointF((((Control)this).Size.Width - num) / 2, (((Control)this).Size.Height - num2) / 2);
		RectangleF rectangleF = new RectangleF(location, size);
		e.Graphics.DrawString(_TextString, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF);
	}

	protected override void OnDoubleClick(EventArgs e)
	{
		((Control)this).OnDoubleClick(e);
		DoubleClick?.Invoke(this, e);
	}
}
