using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomDPIButton : RadioButton
{
	public delegate void ColorChangeEventHandler(object sender, Color e);

	private ColorDialog colorDialog;

	private Image _UncheckImage;

	private Image _CheckImage;

	private string _TextString;

	private Font _CheckFont;

	private Color _DisableColor;

	private Color _SelectForeColor;

	private bool _CurrentDPI;

	private PictureBox _PictureBox;

	private Image image;

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
	[Description("选中之后的字体大小")]
	public Font CheckFont
	{
		get
		{
			return _CheckFont;
		}
		set
		{
			_CheckFont = value;
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

	[Category("自定义")]
	[Description("DPI的颜色")]
	public Color DPIColor
	{
		get
		{
			return ((Control)_PictureBox).BackColor;
		}
		set
		{
			((Control)_PictureBox).BackColor = value;
			((Control)this).Invalidate();
		}
	}

	public Color SelectForeColor
	{
		get
		{
			return _SelectForeColor;
		}
		set
		{
			_SelectForeColor = value;
			((Control)this).Invalidate();
		}
	}

	public bool CurrentDPI
	{
		get
		{
			return _CurrentDPI;
		}
		set
		{
			_CurrentDPI = value;
			((Control)this).Invalidate();
		}
	}

	public event ColorChangeEventHandler ColorChange;

	public CustomDPIButton()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		_DisableColor = Color.FromArgb(57, 57, 57);
		_SelectForeColor = Color.Red;
		((RadioButton)this)._002Ector();
		((Control)this).SetStyle((ControlStyles)206866, true);
		((Control)this).UpdateStyles();
		((RadioButton)this).Appearance = (Appearance)1;
		((ButtonBase)this).FlatStyle = (FlatStyle)0;
		((ButtonBase)this).FlatAppearance.BorderSize = 0;
		((ButtonBase)this).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)this).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((Control)this).AutoSize = false;
		_PictureBox = new PictureBox();
		_PictureBox.BorderStyle = (BorderStyle)1;
		((Control)_PictureBox).Size = new Size(((Control)this).Width, 11);
		((Control)_PictureBox).Name = "pictureBox1";
		((Control)_PictureBox).Location = new Point(0, 11);
		((Control)_PictureBox).BackColor = Color.Yellow;
		((Control)_PictureBox).Click += _PictureBox_Click;
		((Control)this).Controls.Add((Control)(object)_PictureBox);
		colorDialog = new ColorDialog();
	}

	private void _PictureBox_Click(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		((RadioButton)this).Checked = true;
		((Control)this).OnClick((EventArgs)null);
		colorDialog.FullOpen = true;
		if ((int)((CommonDialog)colorDialog).ShowDialog() == 1)
		{
			Color e2 = (DPIColor = colorDialog.Color);
			ColorChange?.Invoke(this, e2);
		}
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		int val = ((_CheckImage != null) ? _CheckImage.Width : 0);
		int val2 = ((_CheckImage != null) ? _CheckImage.Height : 0);
		int val3 = ((_UncheckImage != null) ? _UncheckImage.Width : 0);
		int val4 = ((_UncheckImage != null) ? _UncheckImage.Height : 0);
		int val5 = Math.Max(val, val3);
		val5 = Math.Max(val5, val3);
		val5 = ((val5 == 0) ? width : val5);
		int val6 = Math.Max(val2, val4);
		val6 = Math.Max(val6, val4);
		val6 = ((val6 == 0) ? height : val6);
		((Control)this).SetBoundsCore(x, y, val5, val6, specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		((ButtonBase)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		if (((RadioButton)this).Checked)
		{
			image = _CheckImage;
		}
		else
		{
			image = _UncheckImage;
		}
		if (image == null)
		{
			SolidBrush val = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val, e.ClipRectangle);
			((Brush)val).Dispose();
		}
		else
		{
			Rectangle rectangle = new Rectangle((((Control)this).Width - image.Size.Width) / 2, (((Control)this).Height - image.Size.Height) / 2, image.Size.Width, image.Size.Height);
			e.Graphics.DrawImageUnscaledAndClipped(image, rectangle);
			((Control)_PictureBox).Size = new Size(image.Size.Width, image.Size.Height / 4);
			((Control)_PictureBox).Location = new Point((((Control)this).Width - image.Size.Width) / 2, (((Control)this).Height - image.Size.Height) / 2 + image.Size.Height * 13 / 16);
		}
		SizeF size = ((Control)this).CreateGraphics().MeasureString(_TextString, ((RadioButton)this).Checked ? CheckFont : ((Control)this).Font);
		int num = Convert.ToInt32(size.Width);
		int num2 = Convert.ToInt32(size.Height);
		PointF location = new PointF((((Control)this).Size.Width - num) / 2, (((Control)this).Size.Height - num2 - ((Control)_PictureBox).Size.Height) / 2);
		RectangleF rectangleF = new RectangleF(location, size);
		e.Graphics.DrawString(_TextString, ((RadioButton)this).Checked ? CheckFont : ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), rectangleF);
	}

	protected override void OnClick(EventArgs e)
	{
		((RadioButton)this).OnClick(e);
	}
}
