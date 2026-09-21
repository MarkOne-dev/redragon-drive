using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WindControls;

internal class ImageHelper
{
	public static bool IsRealImage(string path)
	{
		if (path == null || path == "")
		{
			return false;
		}
		try
		{
			Image.FromFile(path).Dispose();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static Image GetImage(string fileName)
	{
		Image result = null;
		if (File.Exists(fileName) && IsRealImage(fileName))
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open);
			result = Image.FromStream((Stream)fileStream);
			fileStream.Close();
		}
		return result;
	}

	public static byte[] GetBytesFromImagePath(string strFile)
	{
		byte[] result = null;
		if (IsRealImage(strFile))
		{
			using FileStream fileStream = new FileStream(strFile, FileMode.Open, FileAccess.Read);
			using BinaryReader binaryReader = new BinaryReader(fileStream);
			result = binaryReader.ReadBytes((int)fileStream.Length);
		}
		return result;
	}

	public static Image GetImageFromBytes(byte[] bytes)
	{
		Image val = null;
		using MemoryStream memoryStream = new MemoryStream(bytes);
		memoryStream.Write(bytes, 0, bytes.Length);
		return Image.FromStream((Stream)memoryStream, true);
	}

	public static Bitmap ReadImageFile(string path)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		Bitmap result = null;
		try
		{
			FileStream fileStream = File.OpenRead(path);
			int num = 0;
			num = (int)fileStream.Length;
			byte[] buffer = new byte[num];
			fileStream.Read(buffer, 0, num);
			Image val = Image.FromStream((Stream)fileStream);
			fileStream.Close();
			result = new Bitmap(val);
		}
		catch
		{
		}
		return result;
	}

	public static Bitmap GetImageWithRect(string fileName, Rectangle rect)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		Bitmap result = null;
		try
		{
			Bitmap val = ReadImageFile(fileName);
			if (val != null)
			{
				int num = 0;
				int num2 = 0;
				int width = ((Image)val).Width;
				int height = ((Image)val).Height;
				int width2 = rect.Size.Width;
				int height2 = rect.Size.Height;
				if (height > height2 || width > width2)
				{
					if (width * height2 > height * width2)
					{
						num = width2;
						num2 = width2 * height / width;
					}
					else
					{
						num2 = height2;
						num = width * height2 / height;
					}
				}
				else
				{
					num = width;
					num2 = height;
				}
				result = new Bitmap((Image)(object)val, new Size(num, num2));
				((Image)val).Dispose();
			}
		}
		catch
		{
		}
		return result;
	}

	public static Icon GetIcon(string ImageFileName, Size size)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		if (File.Exists(ImageFileName))
		{
			Bitmap val = new Bitmap(ImageFileName);
			try
			{
				string text = Path.GetExtension(ImageFileName).ToLower();
				if (((Image)val).Width == size.Width && ((Image)val).Height == size.Height && text.Equals(".ico"))
				{
					Bitmap val2 = new Bitmap((Image)(object)val);
					try
					{
						return Icon.FromHandle(val2.GetHicon());
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				Bitmap val3 = new Bitmap((Image)(object)val, size);
				try
				{
					return Icon.FromHandle(val3.GetHicon());
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return null;
	}

	public static byte[] GetIconBytes(string ImageFileName, Size size)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		if (File.Exists(ImageFileName))
		{
			Bitmap val = new Bitmap(ImageFileName);
			try
			{
				string text = Path.GetExtension(ImageFileName).ToLower();
				if (((Image)val).Width == 32 && ((Image)val).Height == 32 && text.Equals(".ico"))
				{
					using (MemoryStream memoryStream = new MemoryStream())
					{
						((Image)val).Save((Stream)memoryStream, ImageFormat.Bmp);
						return memoryStream.GetBuffer();
					}
				}
				Bitmap val2 = new Bitmap((Image)(object)val, size);
				try
				{
					using MemoryStream memoryStream2 = new MemoryStream();
					((Image)val2).Save((Stream)memoryStream2, ImageFormat.Bmp);
					return memoryStream2.GetBuffer();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		return null;
	}

	public static Bitmap ImageTailor(Bitmap src, Rectangle rect)
	{
		return src.Clone(rect, (PixelFormat)0);
	}
}
