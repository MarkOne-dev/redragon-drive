using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;

namespace Mouse_Drive_Beta.SkinFormLib;

public class FormResize
{
	private const int HORZRES = 8;

	private const int VERTRES = 10;

	private const int LOGPIXELSX = 88;

	private const int LOGPIXELSY = 90;

	private const int DESKTOPVERTRES = 117;

	private const int DESKTOPHORZRES = 118;

	private const int DEFAULTDPI = 96;

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr ptr);

	[DllImport("gdi32.dll")]
	private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

	[DllImport("user32.dll")]
	private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

	public void Resize(Form form)
	{
		IntPtr dC = GetDC(IntPtr.Zero);
		int deviceCaps = GetDeviceCaps(dC, 88);
		deviceCaps = GetDeviceCaps(dC, 90);
		ReleaseDC(IntPtr.Zero, dC);
		((Control)form).Width = ((Control)form).Width * deviceCaps / 96;
		((Control)form).Height = ((Control)form).Height * deviceCaps / 96;
		Resize((Control)(object)form, deviceCaps);
	}

	private static void Resize(Control father, int scale)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			val.Width = val.Width * scale / 96;
			val.Height = val.Height * scale / 96;
			val.Location = new Point(val.Location.X * scale / 96, val.Location.Y * scale / 96);
			if (!(val is CustomComboBox) && val.HasChildren)
			{
				Resize(val, scale);
			}
		}
	}

	public static void ResizeInit(Control control)
	{
		IntPtr dC = GetDC(IntPtr.Zero);
		int deviceCaps = GetDeviceCaps(dC, 88);
		deviceCaps = GetDeviceCaps(dC, 90);
		ReleaseDC(IntPtr.Zero, dC);
		Resize(control, deviceCaps);
	}

	public static int GetDpi()
	{
		IntPtr dC = GetDC(IntPtr.Zero);
		GetDeviceCaps(dC, 88);
		int deviceCaps = GetDeviceCaps(dC, 90);
		ReleaseDC(IntPtr.Zero, dC);
		return deviceCaps;
	}
}
