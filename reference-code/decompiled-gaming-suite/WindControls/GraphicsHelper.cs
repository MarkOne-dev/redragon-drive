using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WindControls;

public static class GraphicsHelper
{
	public enum RoundStyle
	{
		None = 0,
		TopLeft = 1,
		TopRight = 2,
		BottomLeft = 4,
		BottomRight = 8,
		Top = 3,
		Bottom = 12,
		All = 15
	}

	public static void SetGDIHigh(this Graphics g)
	{
		g.SmoothingMode = (SmoothingMode)4;
		g.InterpolationMode = (InterpolationMode)7;
		g.CompositingQuality = (CompositingQuality)2;
		g.TextRenderingHint = (TextRenderingHint)4;
	}

	public static Region SetWindowRegion(PaintEventArgs e, int width, int height, int radius, Color borderColor, RoundStyle roundStyle)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		GraphicsPath roundedRectPath = GetRoundedRectPath(new Rectangle(0, 0, width, height), radius, roundStyle);
		Pen val = new Pen(borderColor, 1f);
		e.Graphics.DrawPath(val, roundedRectPath);
		return new Region(roundedRectPath);
	}

	public static Region GetWindowRegion(int width, int height, int radius, Color borderColor, RoundStyle roundStyle)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		return new Region(GetRoundedRectPath(new Rectangle(0, 0, width, height), radius, roundStyle));
	}

	public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius, RoundStyle roundStyle)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.Location, new Size(radius, radius));
		GraphicsPath val = new GraphicsPath();
		val.StartFigure();
		if ((roundStyle & RoundStyle.TopLeft) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 180f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Left, rect.Top), new Point(rect.Left, rect.Top + radius));
			val.AddLine(new Point(rect.Left, rect.Top), new Point(rect.Left + radius, rect.Top));
		}
		rectangle.X = rect.Right - radius;
		if ((roundStyle & RoundStyle.TopRight) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 270f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Right, rect.Top), new Point(rect.Right - radius, rect.Top));
			val.AddLine(new Point(rect.Right, rect.Top), new Point(rect.Right, rect.Top + radius));
		}
		rectangle.Y = rect.Bottom - radius;
		if ((roundStyle & RoundStyle.BottomRight) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 0f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Right, rect.Bottom), new Point(rect.Right, rect.Bottom - radius));
			val.AddLine(new Point(rect.Right, rect.Bottom), new Point(rect.Right + radius, rect.Bottom));
		}
		rectangle.X = rect.Left;
		if ((roundStyle & RoundStyle.BottomLeft) > RoundStyle.None && radius > 0)
		{
			val.AddArc(rectangle, 90f, 90f);
		}
		else
		{
			val.AddLine(new Point(rect.Left, rect.Bottom), new Point(rect.Left - radius, rect.Bottom));
			val.AddLine(new Point(rect.Left, rect.Bottom), new Point(rect.Left, rect.Bottom - radius));
		}
		val.CloseFigure();
		return val;
	}

	public static void FillRect(Graphics g, Rectangle frameRect, int Radius, Color backColor, int frameWidth, Color frameColor, RoundStyle roundStyle)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		g.SetGDIHigh();
		GraphicsPath val;
		if (Radius > 0)
		{
			val = GetRoundedRectPath(frameRect, Radius, roundStyle);
		}
		else
		{
			val = new GraphicsPath();
			val.AddRectangle(frameRect);
			val.CloseFigure();
		}
		g.FillPath((Brush)new SolidBrush(backColor), val);
		if (frameWidth > 0)
		{
			Pen val2 = new Pen(frameColor, (float)frameWidth);
			g.DrawPath(val2, val);
		}
	}

	public static int GetScreenWidth()
	{
		return Screen.PrimaryScreen.Bounds.Width;
	}

	public static int GetScreenHeight()
	{
		return Screen.PrimaryScreen.Bounds.Height;
	}

	public static GraphicsPath GetImageGraphicsPath(Bitmap image, byte alphaFilter)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		int height = ((Image)image).Height;
		int width = ((Image)image).Width;
		GraphicsPath val = new GraphicsPath();
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				for (; j < width && image.GetPixel(j, i).A < alphaFilter; j++)
				{
				}
				int num = j;
				for (; j < width && image.GetPixel(j, i).A > alphaFilter; j++)
				{
				}
				int num2 = j;
				if (num2 > num)
				{
					val.AddRectangle(new Rectangle(num, i, num2 - num, 1));
				}
			}
		}
		return val;
	}

	public static Region GetImageRegion(Bitmap bitmap, Size formSize, byte alphaFilter)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		return new Region(GetImageGraphicsPath(bitmap.ScaleToSize(formSize), alphaFilter));
	}

	public static Bitmap ScaleToSize(this Bitmap bitmap, int width, int height)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (((Image)bitmap).Width == width && ((Image)bitmap).Height == height)
		{
			return bitmap;
		}
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.InterpolationMode = (InterpolationMode)7;
			val2.DrawImage((Image)(object)bitmap, 0, 0, width, height);
			return val;
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	public static Bitmap ScaleToSize(this Bitmap bitmap, Size size)
	{
		return bitmap.ScaleToSize(size.Width, size.Height);
	}

	public static Bitmap FillBitmapWithColor(Bitmap bitmap, Color srcColor, Color dstColor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		Bitmap val = new Bitmap((Image)(object)bitmap);
		for (int i = 0; i < ((Image)bitmap).Height; i++)
		{
			for (int j = 0; j < ((Image)bitmap).Width; j++)
			{
				Color pixel = bitmap.GetPixel(j, i);
				if (pixel == srcColor)
				{
					val.SetPixel(j, i, dstColor);
				}
				else
				{
					val.SetPixel(j, i, pixel);
				}
			}
		}
		return val;
	}

	public static void FillBitmapWithColor(Bitmap bitmap, Rectangle fillRect, Color fillColor)
	{
		Color color = Color.FromArgb(0, 0, 0, 0);
		Rectangle rectangle = Rectangle.Intersect(fillRect, new Rectangle(0, 0, ((Image)bitmap).Width, ((Image)bitmap).Height));
		for (int i = rectangle.Y; i < rectangle.Height; i++)
		{
			for (int j = rectangle.X; j < rectangle.Width; j++)
			{
				if (bitmap.GetPixel(j, i) == color)
				{
					bitmap.SetPixel(j, i, fillColor);
				}
			}
		}
	}
}
