using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomConnectedRadioButton : RadioButton
{
	private Image _NormalImage;

	private Image _ConnectedImage;

	private string _TextString;

	[Category("自定义")]
	[Description("没连接时的背景图片")]
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
	[Description("没连接时的背景图片")]
	public Image ConnectedImage
	{
		get
		{
			return _ConnectedImage;
		}
		set
		{
			_ConnectedImage = value;
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

	public CustomConnectedRadioButton()
	{
		((Control)this).SetStyle((ControlStyles)206866, true);
		((Control)this).UpdateStyles();
		((RadioButton)this).Appearance = (Appearance)1;
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((Control)this).AutoSize = false;
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		int val = ((_ConnectedImage != null) ? _ConnectedImage.Width : 0);
		int val2 = ((_ConnectedImage != null) ? _ConnectedImage.Height : 0);
		int val3 = ((_NormalImage != null) ? _NormalImage.Width : 0);
		int val4 = ((_NormalImage != null) ? _NormalImage.Height : 0);
		int num = Math.Max(val, val3);
		num = ((num == 0) ? width : num);
		int num2 = Math.Max(val2, val4);
		num2 = ((num2 == 0) ? height : num2);
		((Control)this).SetBoundsCore(x, y, num, num2, specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		((ButtonBase)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = (((RadioButton)this).Checked ? _ConnectedImage : _NormalImage);
		if (val == null)
		{
			SolidBrush val2 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val2, e.ClipRectangle);
			((Brush)val2).Dispose();
		}
		else
		{
			Rectangle rectangle = new Rectangle(0, 0, val.Size.Width, val.Size.Height);
			e.Graphics.DrawImageUnscaledAndClipped(val, rectangle);
		}
		SizeF size = ((Control)this).CreateGraphics().MeasureString(_TextString, ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location = new PointF((((Control)this).Size.Width - num) / 2, (((Control)this).Size.Height - num2) / 2);
		RectangleF rectangleF = new RectangleF(location, size);
		e.Graphics.DrawString(_TextString, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF);
	}
}
