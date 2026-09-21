using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.View;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class CustomSetting : UserControl
{
	public delegate void LanguageChangeEventHandler(object sender, CustomEventArgs e);

	public delegate void DeviceUpdateFailEventHandler(object sender);

	private List<Point> ControlsPoints = new List<Point>();

	private List<int> LockPointIndexs = new List<int>();

	public bool updating;

	public bool pairing;

	private int lockControlCount;

	private bool addLongDistance;

	private bool addDongleRGB;

	private LanguageFile languageFile;

	private UC_Language uc_Language;

	private UC_DeviceUpdate uc_DeviceUpdate;

	private UC_Pair uc_Pair;

	private UC_System uc_System;

	private UC_Advanced uc_Advanced;

	private UC_Web uc_Web;

	private bool longDistance;

	private IContainer components;

	private Panel panel1;

	private Label label_DriveVersionValue;

	private Label label_DriveVersion;

	private CustomScrollBar customScrollBar1;

	public bool LongDistance
	{
		get
		{
			return longDistance;
		}
		set
		{
			longDistance = value;
			if (uc_Advanced != null)
			{
				uc_Advanced.UpdateLongDistance(longDistance);
			}
		}
	}

	public string DongleVersion
	{
		get
		{
			return "";
		}
		set
		{
			if (uc_DeviceUpdate != null)
			{
				uc_DeviceUpdate.DongleVersion = value;
			}
		}
	}

	public byte DongleType
	{
		get
		{
			return 0;
		}
		set
		{
			if (uc_DeviceUpdate != null)
			{
				uc_DeviceUpdate.DongleType = value;
			}
		}
	}

	public string MouseVersion
	{
		get
		{
			return "";
		}
		set
		{
			if (uc_DeviceUpdate != null)
			{
				uc_DeviceUpdate.MouseVersion = value;
			}
		}
	}

	public event LanguageChangeEventHandler LanguageChange;

	public event DeviceUpdateFailEventHandler DeviceUpdateFail;

	public CustomSetting()
	{
		InitializeComponent();
	}

	public void UpdateUI(LanguageFile language, DriveConfig.DeviceParam deviceParam)
	{
		languageFile = language;
		int y = 0;
		lockControlCount = ((ArrangedElementCollection)((Control)this).Controls).Count;
		SetLabel(languageFile.LanguageIndex);
		uc_Language = new UC_Language(languageFile);
		((Control)uc_Language).Location = new Point(0, y);
		uc_Language.LanguageChange += Language_LanguageChange;
		((Control)panel1).Controls.Add((Control)(object)uc_Language);
		y = ((Control)uc_Language).Bottom;
		uc_DeviceUpdate = new UC_DeviceUpdate(language, deviceParam);
		((Control)uc_DeviceUpdate).Location = new Point(0, y);
		uc_DeviceUpdate.DeviceUpdating += DeviceUpdate_DeviceUpdating;
		uc_DeviceUpdate.DeviceUpdateFail += DeviceUpdate_DeviceUpdateFail;
		((Control)panel1).Controls.Add((Control)(object)uc_DeviceUpdate);
		y = ((Control)uc_DeviceUpdate).Bottom;
		uc_Pair = new UC_Pair();
		((Control)uc_Pair).Location = new Point(0, y);
		uc_Pair.ExitPairing += Uc_Pair_ExitPairing;
		((Control)panel1).Controls.Add((Control)(object)uc_Pair);
		y = ((Control)uc_Pair).Bottom;
		uc_System = new UC_System();
		((Control)uc_System).Location = new Point(0, y);
		((Control)panel1).Controls.Add((Control)(object)uc_System);
		y = ((Control)uc_System).Bottom;
		uc_Web = new UC_Web(language);
		((Control)uc_Web).Location = new Point(0, y);
		((Control)panel1).Controls.Add((Control)(object)uc_Web);
		ResourcesFile.SetControlBitmap((Control)(object)this);
		AdjustWebLocation(y);
	}

	private void Uc_Pair_ExitPairing(object sender)
	{
		ExitPair();
	}

	private void DeviceUpdate_DeviceUpdateFail(object sender)
	{
		DeviceUpdateFail?.Invoke(this);
	}

	public void EnterPair()
	{
		if (uc_Pair != null)
		{
			pairing = true;
			uc_Pair.EnterPair();
		}
	}

	public void ExitPair()
	{
		if (uc_Pair != null && pairing)
		{
			pairing = false;
			uc_Pair.ExitPair();
		}
	}

	public void AddAdvanced()
	{
		int bottom = ((Control)uc_System).Bottom;
		uc_Advanced = new UC_Advanced();
		((Control)uc_Advanced).Location = new Point(0, bottom);
		((Control)panel1).Controls.Add((Control)(object)uc_Advanced);
		bottom = ((Control)uc_Advanced).Bottom;
		((Control)uc_Web).Location = new Point(0, bottom);
		AdjustWebLocation(bottom);
	}

	private void AdjustWebLocation(int y)
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		int num = y;
		((Control)uc_Web).Location = new Point(0, y);
		num = ((Control)uc_Web).Bottom;
		ResourcesFile.SetControlBitmap((Control)(object)this);
		ControlsPoints.Clear();
		LockPointIndexs.Clear();
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel1).Controls).Count; i++)
		{
			ControlsPoints.Add(((Control)panel1).Controls[i].Location);
			if (((Control)panel1).Controls[i].TabIndex < lockControlCount)
			{
				LockPointIndexs.Add(i);
				continue;
			}
			((Control)panel1).Controls[i].BackgroundImageLayout = (ImageLayout)3;
			((Control)panel1).Controls[i].BackgroundImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\7General\\split.png");
		}
		((Control)panel1).Height = num + 3;
		int height = ((Control)customScrollBar1).Size.Height;
		if (height >= num)
		{
			((Control)customScrollBar1).Visible = false;
			((Control)panel1).Height = height;
			return;
		}
		((Control)customScrollBar1).Visible = true;
		customScrollBar1.DocSize = ((Control)panel1).Height;
		customScrollBar1.PageSize = height;
		customScrollBar1.ScrollInterval = SystemInformation.MouseWheelScrollLines * 15;
		customScrollBar1.DocPosition = 0f;
	}

	public void SetDongleRGB(byte[] rgb)
	{
		if (uc_Advanced != null)
		{
			uc_Advanced.UpdateDongleLED(rgb[0]);
		}
	}

	public byte[] GetDongleRGB()
	{
		return new byte[10];
	}

	public void AddDongleRGB()
	{
		if (uc_Advanced == null)
		{
			AddAdvanced();
		}
		if (!addDongleRGB)
		{
			addDongleRGB = true;
			uc_Advanced.AddDongleLED();
			AdjustWebLocation(((Control)uc_Advanced).Bottom);
		}
	}

	public void AddLongDistance()
	{
		if (uc_Advanced == null)
		{
			AddAdvanced();
		}
		if (!addLongDistance)
		{
			addLongDistance = true;
			uc_Advanced.AddLongDistance();
			AdjustWebLocation(((Control)uc_Advanced).Bottom);
		}
	}

	public void RemoteAdvanced()
	{
		if (uc_Advanced != null)
		{
			int bottom = ((Control)uc_System).Bottom;
			((Control)panel1).Controls.Remove((Control)(object)uc_Advanced);
			uc_Advanced = null;
			((Control)uc_Web).Location = new Point(0, bottom);
			AdjustWebLocation(bottom);
		}
	}

	private void Language_LanguageChange(object sender, CustomEventArgs e)
	{
		SetLabel((int)e.Value);
		LanguageChange?.Invoke(this, e);
		if (uc_DeviceUpdate != null)
		{
			uc_DeviceUpdate.LanguageChange();
		}
		if (uc_Pair != null)
		{
			uc_Pair.LanguageChange();
		}
		if (uc_System != null)
		{
			uc_System.LanguageChange();
		}
		if (uc_Advanced != null)
		{
			uc_Advanced.LanguageChange();
		}
		if (uc_Web != null)
		{
			uc_Web.LanguageChange();
		}
	}

	private void SetLabel(int index)
	{
		((Control)label_DriveVersionValue).Text = FormMain.driveParam.Version;
		((Control)label_DriveVersionValue).Location = new Point(((Control)panel1).Width - ((Control)label_DriveVersionValue).Width, ((Control)label_DriveVersionValue).Top);
		((Control)label_DriveVersion).Text = languageFile.FromSettingString[0];
		((Control)label_DriveVersion).Location = new Point(((Control)label_DriveVersionValue).Left - ((Control)label_DriveVersion).Width, ((Control)label_DriveVersion).Top);
	}

	private void DeviceUpdate_DeviceUpdating(object sender, bool updating)
	{
		this.updating = updating;
	}

	public void RestDongleButton()
	{
		if (uc_DeviceUpdate != null)
		{
			uc_DeviceUpdate.ResetDongleButton();
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		if (((Control)customScrollBar1).Visible)
		{
			bool num = e.Delta <= 0;
			float docPosition = customScrollBar1.DocPosition;
			docPosition = (num ? ((docPosition + customScrollBar1.ScrollInterval > customScrollBar1.DocSize - customScrollBar1.PageSize) ? (customScrollBar1.DocSize - customScrollBar1.PageSize) : (docPosition + customScrollBar1.ScrollInterval)) : ((docPosition - customScrollBar1.ScrollInterval < 0f) ? 0f : (docPosition - customScrollBar1.ScrollInterval)));
			customScrollBar1.DocPosition = docPosition;
			customScrollBar1_CustomScroll(null, null);
		}
	}

	private void customScrollBar1_CustomScroll(object sender, CustomEventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < ((ArrangedElementCollection)((Control)panel1).Controls).Count; i++)
		{
			flag = false;
			for (int j = 0; j < LockPointIndexs.Count; j++)
			{
				if (i == LockPointIndexs[j])
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				((Control)panel1).Controls[i].Location = new Point(ControlsPoints[i].X, ControlsPoints[i].Y - Convert.ToInt32(customScrollBar1.DocPosition));
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		panel1 = new Panel();
		label_DriveVersion = new Label();
		label_DriveVersionValue = new Label();
		customScrollBar1 = new CustomScrollBar();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)label_DriveVersion);
		((Control)panel1).Controls.Add((Control)(object)label_DriveVersionValue);
		((Control)panel1).Location = new Point(0, 0);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(855, 591);
		((Control)panel1).TabIndex = 0;
		((Control)label_DriveVersion).AutoSize = true;
		((Control)label_DriveVersion).Location = new Point(716, 5);
		((Control)label_DriveVersion).Name = "label_DriveVersion";
		((Control)label_DriveVersion).Size = new Size(68, 20);
		((Control)label_DriveVersion).TabIndex = 1;
		((Control)label_DriveVersion).Text = "驱动版本:";
		((Control)label_DriveVersionValue).AutoSize = true;
		((Control)label_DriveVersionValue).Location = new Point(790, 5);
		((Control)label_DriveVersionValue).Name = "label_DriveVersionValue";
		((Control)label_DriveVersionValue).Size = new Size(50, 20);
		((Control)label_DriveVersionValue).TabIndex = 0;
		((Control)label_DriveVersionValue).Text = "1.0.0.4";
		customScrollBar1.BarColor = Color.FromArgb(30, 30, 30);
		customScrollBar1.BarRadius = 16;
		customScrollBar1.BarSize = 10;
		customScrollBar1.DocPosition = 0f;
		customScrollBar1.DocSize = 10f;
		customScrollBar1.Interval = 2;
		customScrollBar1.IsRound = true;
		((Control)customScrollBar1).Location = new Point(861, 3);
		((Control)customScrollBar1).Name = "customScrollBar1";
		customScrollBar1.Orientation = (Orientation)1;
		customScrollBar1.PageSize = 1f;
		customScrollBar1.ScrollInterval = 10f;
		((Control)customScrollBar1).Size = new Size(10, 588);
		customScrollBar1.SliderColor = Color.FromArgb(70, 70, 70);
		customScrollBar1.SliderMiniSize = 20f;
		customScrollBar1.SliderPosition = 0f;
		((Control)customScrollBar1).TabIndex = 1;
		customScrollBar1.CustomScroll += customScrollBar1_CustomScroll;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)customScrollBar1);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "CustomSetting";
		((Control)this).Size = new Size(877, 595);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
