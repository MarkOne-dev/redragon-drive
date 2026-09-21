using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomBattery : Control
{
	private Image _EmptyBatteryImage;

	private Image _FullBatteryImage;

	private Image _LowBatteryImage;

	private Image _ChargingImage;

	private int _BatteryLevel;

	private int _LowBatteryLevel = 20;

	private bool _BatteryCharging;

	public override Color BackColor { get; set; } = Color.Transparent;

	public Image EmptyBatteryImage
	{
		get
		{
			return _EmptyBatteryImage;
		}
		set
		{
			_EmptyBatteryImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image FullBatteryImage
	{
		get
		{
			return _FullBatteryImage;
		}
		set
		{
			_FullBatteryImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image LowBatteryImage
	{
		get
		{
			return _LowBatteryImage;
		}
		set
		{
			_LowBatteryImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image ChargingImage
	{
		get
		{
			return _ChargingImage;
		}
		set
		{
			_ChargingImage = value;
			((Control)this).Invalidate();
		}
	}

	public int BatteryLevel
	{
		get
		{
			return _BatteryLevel;
		}
		set
		{
			_BatteryLevel = value;
			((Control)this).Invalidate();
		}
	}

	public int LowBatteryLevel
	{
		get
		{
			return _LowBatteryLevel;
		}
		set
		{
			_LowBatteryLevel = value;
			((Control)this).Invalidate();
		}
	}

	public bool BatteryCharging
	{
		get
		{
			return _BatteryCharging;
		}
		set
		{
			_BatteryCharging = value;
			((Control)this).Invalidate();
		}
	}

	public CustomBattery()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f6: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (_EmptyBatteryImage != null && _FullBatteryImage != null && _LowBatteryImage != null)
		{
			Graphics graphics = e.Graphics;
			Image val = ((_BatteryLevel <= _LowBatteryLevel) ? _LowBatteryImage : _FullBatteryImage);
			int num = ((_BatteryLevel < 3) ? 3 : _BatteryLevel);
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			graphics.DrawImage(_EmptyBatteryImage, rectangle);
			Image val2 = null;
			Bitmap val3 = new Bitmap(num * ((Control)this).Width / 100, ((Control)this).Height, (PixelFormat)139273);
			Graphics obj = Graphics.FromImage((Image)val3);
			Rectangle rectangle2 = new Rectangle(0, 0, num * _FullBatteryImage.Width / 100, _FullBatteryImage.Height);
			Rectangle rectangle3 = new Rectangle(0, 0, num * ((Control)this).Width / 100, ((Control)this).Height);
			obj.DrawImage(val, rectangle3, rectangle2, (GraphicsUnit)2);
			PixelProcess(val3);
			val2 = (Image)val3;
			rectangle = new Rectangle(0, 0, num * ((Control)this).Width / 100, ((Control)this).Height);
			graphics.DrawImage(val2, rectangle);
			if (_BatteryCharging && _ChargingImage != null)
			{
				rectangle = new Rectangle((((Control)this).Width - _ChargingImage.Width) / 2, (((Control)this).Height - _ChargingImage.Height) / 2, _ChargingImage.Width, _ChargingImage.Height);
				graphics.DrawImage(_ChargingImage, rectangle);
			}
		}
	}

	public static void PixelProcess(Bitmap bmp)
	{
		Color pixel = bmp.GetPixel(0, 0);
		bmp.MakeTransparent(pixel);
	}
}
