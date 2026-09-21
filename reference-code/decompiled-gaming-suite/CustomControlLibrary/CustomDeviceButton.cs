using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomDeviceButton : RadioButton
{
	public delegate void QuestionEnterEventHandler(object sender);

	public delegate void QuestionLevaeEventHandler(object sender);

	private Image _OnlineImage;

	private Image _OfflineImage;

	private Image _WiredImage;

	private Image _WirelessImage;

	private Image _QuestionImage;

	private Image _ReportRateImage;

	private bool _Online;

	private bool _IsWired;

	private string _DeviceName;

	private Rectangle imageRect;

	public override Font Font { get; set; }

	[Category("自定义")]
	[Description("鼠标按下的背景图片")]
	public Image OnlineImage
	{
		get
		{
			return _OnlineImage;
		}
		set
		{
			_OnlineImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("鼠标离开的背景图片")]
	public Image OfflineImage
	{
		get
		{
			return _OfflineImage;
		}
		set
		{
			_OfflineImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("有线连接的图片")]
	public Image WiredImage
	{
		get
		{
			return _WiredImage;
		}
		set
		{
			_WiredImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("无线连接的图片")]
	public Image WirelessImage
	{
		get
		{
			return _WirelessImage;
		}
		set
		{
			_WirelessImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image QuestionImage
	{
		get
		{
			return _QuestionImage;
		}
		set
		{
			_QuestionImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("支持最大的报告率图片")]
	public Image ReportRateImage
	{
		get
		{
			return _ReportRateImage;
		}
		set
		{
			_ReportRateImage = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("鼠标是否在线")]
	public bool Online
	{
		get
		{
			return _Online;
		}
		set
		{
			_Online = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("是不是有线连接")]
	public bool IsWired
	{
		get
		{
			return _IsWired;
		}
		set
		{
			_IsWired = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("鼠标的名称")]
	public string DeviceName
	{
		get
		{
			return _DeviceName;
		}
		set
		{
			_DeviceName = value;
			((Control)this).Invalidate();
		}
	}

	public event QuestionEnterEventHandler QuestionEnter;

	public event QuestionLevaeEventHandler QuestionLevae;

	public CustomDeviceButton()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		Font = new Font("微软雅黑", 10.5f, (FontStyle)0);
		_DeviceName = "G430";
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
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		int val = ((_OnlineImage != null) ? _OnlineImage.Width : 0);
		int val2 = ((_OnlineImage != null) ? _OnlineImage.Height : 0);
		int val3 = ((_OfflineImage != null) ? _OfflineImage.Width : 0);
		int val4 = ((_OfflineImage != null) ? _OfflineImage.Height : 0);
		int num = Math.Max(val, val3);
		num = ((num == 0) ? width : num);
		int num2 = Math.Max(val2, val4);
		num2 = ((num2 == 0) ? height : num2);
		if (_DeviceName != "")
		{
			int num3 = Convert.ToInt32(((Control)this).CreateGraphics().MeasureString(_DeviceName, ((Control)this).Font).Height) + 4;
			num2 += num3;
		}
		((Control)this).SetBoundsCore(x, y, num, num2, specified);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		((ButtonBase)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Image val = null;
		Image val2 = null;
		if (_QuestionImage == null)
		{
			val = ((!_Online) ? _OfflineImage : _OnlineImage);
			val2 = ((!_IsWired) ? _WirelessImage : _WiredImage);
		}
		else if (_Online)
		{
			val = _OnlineImage;
			val2 = ((!_IsWired) ? _WirelessImage : _WiredImage);
		}
		else
		{
			val = _OfflineImage;
			val2 = _QuestionImage;
		}
		if (val == null)
		{
			SolidBrush val3 = new SolidBrush(((Control)this).BackColor);
			e.Graphics.FillRectangle((Brush)(object)val3, e.ClipRectangle);
			((Brush)val3).Dispose();
			return;
		}
		Rectangle rectangle = new Rectangle(0, 0, val.Size.Width, val.Size.Height);
		e.Graphics.DrawImageUnscaledAndClipped(val, rectangle);
		if (val2 != null && ((_Online && _QuestionImage == null) || _QuestionImage != null))
		{
			imageRect = new Rectangle(((Control)this).Width - val2.Width - 25, 20, val2.Width, val2.Height);
			e.Graphics.DrawImageUnscaledAndClipped(val2, imageRect);
		}
		if (_ReportRateImage != null && !_IsWired)
		{
			rectangle = new Rectangle(25, 20, _ReportRateImage.Width, _ReportRateImage.Height);
			e.Graphics.DrawImageUnscaledAndClipped(_ReportRateImage, rectangle);
		}
	}

	protected override void OnMouseMove(MouseEventArgs mevent)
	{
		((ButtonBase)this).OnMouseMove(mevent);
		if (_QuestionImage == null)
		{
			return;
		}
		if (!_Online)
		{
			if (imageRect.Contains(mevent.Location))
			{
				QuestionEnter?.Invoke(this);
			}
			else
			{
				QuestionLevae?.Invoke(this);
			}
		}
		else
		{
			QuestionLevae?.Invoke(this);
		}
	}
}
