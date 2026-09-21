using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using FileManager;

namespace Mouse_Drive_Beta.FileManager;

public class ResourcesFile
{
	public class GeneralBitmap
	{
		public Bitmap RadioBtnCheck;

		public Bitmap RadioBtnUncheck;

		public Bitmap CheckBoxCheck;

		public Bitmap CheckBoxUncheck;

		public Bitmap SwitchOn;

		public Bitmap SwitchOff;

		public Bitmap BtnCloseNr;

		public Bitmap BtnCloseDown;

		public Bitmap BtnCloseEnter;

		public Bitmap BtnAdd;

		public Bitmap BtnDec;

		public Bitmap BtnOKNr;

		public Bitmap BtnOKDown;

		public Bitmap BtnOKEnter;

		public Bitmap BtnSettingNr;

		public Bitmap BtnSettingDown;

		public Bitmap BtnSettingEnter;

		public Bitmap BtnDlgCloseNr;

		public Bitmap BtnDlgCloseDown;

		public Bitmap BtnDlgCloseEnter;

		public Bitmap BtnComboBox;

		public Bitmap DialogBg;

		public Bitmap BtnMiniNr;

		public Bitmap BtnMiniDown;

		public Bitmap BtnMiniEnter;

		public Bitmap PictureTitle;

		public Bitmap PictureTips;

		public Bitmap PictureWinMouse;
	}

	public GeneralBitmap resourcesBitmap = new GeneralBitmap();

	public static GeneralBitmap GetResourcesPicture()
	{
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		GeneralBitmap generalBitmap = new GeneralBitmap();
		try
		{
			string text = AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\";
			generalBitmap.RadioBtnCheck = new Bitmap(text + "radio_select.png");
			generalBitmap.RadioBtnUncheck = new Bitmap(text + "radio_unselect.png");
			generalBitmap.CheckBoxCheck = new Bitmap(text + "check_select.png");
			generalBitmap.CheckBoxUncheck = new Bitmap(text + "check_unselect.png");
			generalBitmap.SwitchOn = new Bitmap(text + "on.png");
			generalBitmap.SwitchOff = new Bitmap(text + "off.png");
			generalBitmap.BtnAdd = new Bitmap(text + "add.png");
			generalBitmap.BtnDec = new Bitmap(text + "sub.png");
			generalBitmap.BtnCloseNr = new Bitmap(text + "close_nr.png");
			generalBitmap.BtnCloseDown = new Bitmap(text + "close_down.png");
			generalBitmap.BtnCloseEnter = new Bitmap(text + "close_enter.png");
			generalBitmap.BtnMiniNr = new Bitmap(text + "mini_nr.png");
			generalBitmap.BtnMiniDown = new Bitmap(text + "mini_down.png");
			generalBitmap.BtnMiniEnter = new Bitmap(text + "mini_enter.png");
			generalBitmap.BtnSettingNr = new Bitmap(text + "setting_nr.png");
			generalBitmap.BtnSettingDown = new Bitmap(text + "setting_down.png");
			generalBitmap.BtnSettingEnter = new Bitmap(text + "setting_enter.png");
			generalBitmap.BtnDlgCloseNr = new Bitmap(text + "setting_close_nr.png");
			generalBitmap.BtnDlgCloseDown = new Bitmap(text + "setting_close_down.png");
			generalBitmap.BtnDlgCloseEnter = new Bitmap(text + "setting_close_enter.png");
			generalBitmap.BtnOKNr = new Bitmap(text + "ok_nr.png");
			generalBitmap.BtnOKDown = new Bitmap(text + "ok_down.png");
			generalBitmap.BtnOKEnter = new Bitmap(text + "ok_enter.png");
			generalBitmap.BtnComboBox = new Bitmap(text + "btn_dropdown.png");
			generalBitmap.DialogBg = new Bitmap(text + "message_bg.png");
			generalBitmap.PictureTitle = new Bitmap(text + "title.png");
			generalBitmap.PictureTips = new Bitmap(text + "tips.png");
			generalBitmap.PictureWinMouse = new Bitmap(text + "win_mouse.png");
		}
		catch
		{
			((Form)new FormDialog(LanguageFile.Dialogs[1])).ShowDialog();
		}
		return generalBitmap;
	}

	private static void ForeachSetControlBitmap(Control father, GeneralBitmap resourcesBitmap)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		foreach (Control item in (ArrangedElementCollection)father.Controls)
		{
			Control val = item;
			if (val is PictureBox)
			{
				PictureBox val2 = (PictureBox)val;
				if (val.Name.Contains("Title"))
				{
					((Control)val2).BackgroundImage = (Image)(object)resourcesBitmap.PictureTitle;
				}
				else if (val.Name.Contains("Tips"))
				{
					((Control)val2).BackgroundImage = (Image)(object)resourcesBitmap.PictureTips;
				}
				else if (val.Name.Contains("WinMouse"))
				{
					((Control)val2).BackgroundImage = (Image)(object)resourcesBitmap.PictureWinMouse;
				}
			}
			else if (val is CustomRadioButton)
			{
				CustomRadioButton obj = (CustomRadioButton)(object)val;
				obj.CheckImage = (Image)(object)resourcesBitmap.RadioBtnCheck;
				obj.UncheckImage = (Image)(object)resourcesBitmap.RadioBtnUncheck;
			}
			else if (val is CustomCheckBox)
			{
				CustomCheckBox obj2 = (CustomCheckBox)(object)val;
				obj2.CheckImage = (Image)(object)resourcesBitmap.CheckBoxCheck;
				obj2.UncheckImage = (Image)(object)resourcesBitmap.CheckBoxUncheck;
			}
			else if (val is CustomButton)
			{
				CustomButton obj3 = (CustomButton)(object)val;
				obj3.NormalImage = (Image)(object)resourcesBitmap.BtnOKNr;
				obj3.MouseEnterImage = (Image)(object)resourcesBitmap.BtnOKEnter;
				obj3.MouseDownImage = (Image)(object)resourcesBitmap.BtnOKDown;
			}
			else if (!(val is CustomComboBox) && val.HasChildren)
			{
				SetControlBitmap(val);
			}
		}
	}

	public static void SetControlBitmap(Control father)
	{
		GeneralBitmap resourcesPicture = GetResourcesPicture();
		ForeachSetControlBitmap(father, resourcesPicture);
	}

	public static Bitmap GetComboxBoxBitmap()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		return new Bitmap(string.Concat(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\", "btn_dropdown.png"));
	}
}
