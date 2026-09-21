using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.ComboControlLibrary;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;
using WindControls;

namespace Mouse_Drive_Beta;

public class FormAdvancedSetting : Form
{
	public delegate void AdvancedChangeEventHandler(object sender, int index, int value);

	private FormMover formMover;

	private FormResize formResize;

	public bool CalPass;

	public bool longRangeMode;

	public int dongleLEDMode;

	private bool isUsb;

	private string sensor;

	private CustomDongleRGB dongleRGB;

	private IContainer components;

	private CustomButton customButton_Close;

	private LinkLabel linkLabel_SystemMouse;

	private PictureBox pictureBox_WinMouse;

	private PictureBox pictureBox2;

	private PictureBox pictureBox3;

	private Label label_PowerModeSelect;

	private Label label_PowerModeSelectTips;

	private Label label_MousepadSurfaceCalTips;

	private Label label_MousepadSurfaceCal;

	private Label label_Title;

	private CustomCheckBox customCheckBox_LongRange;

	private CustomButton customButton_ManualCal;

	public event AdvancedChangeEventHandler AdvancedChange;

	public FormAdvancedSetting(bool isUSB, string sen, bool setting, bool calPass, bool dongleLED, byte[] dongleled)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0078: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		formMover = new FormMover();
		formResize = new FormResize();
		sensor = "";
		((Form)this)._002Ector();
		InitializeComponent();
		sensor = sen;
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\setting_bg.png");
		PictureBox obj = pictureBox3;
		PictureBox obj2 = pictureBox2;
		Bitmap val = new Bitmap(baseDirectory + "\\res\\7General\\title.png");
		Image backgroundImage = (Image)val;
		((Control)obj2).BackgroundImage = (Image)val;
		((Control)obj).BackgroundImage = backgroundImage;
		((Control)pictureBox_WinMouse).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\win_mouse.png");
		SetManualCal(calPass);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		isUsb = isUSB;
		customCheckBox_LongRange.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\on.png");
		customCheckBox_LongRange.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\off.png");
		formResize.Resize((Form)(object)this);
		formMover.AddForm((Form)(object)this);
		((Control)label_Title).Text = LanguageFile.Dialogs[61];
		((Control)label_Title).Location = new Point((((Control)this).Width - ((Control)label_Title).Width) / 2, ((Control)label_Title).Location.Y);
		((Control)label_PowerModeSelect).Text = LanguageFile.Dialogs[62];
		((Control)label_PowerModeSelectTips).Text = "(" + LanguageFile.Dialogs[64] + ")";
		((Control)label_PowerModeSelectTips).Location = new Point(((Control)label_PowerModeSelect).Right, ((Control)label_PowerModeSelectTips).Location.Y);
		customCheckBox_LongRange.Checked = (longRangeMode = setting);
		((Control)label_MousepadSurfaceCal).Text = LanguageFile.Dialogs[65];
		((Control)label_MousepadSurfaceCalTips).Text = "(" + LanguageFile.Dialogs[68] + ")";
		((Control)label_MousepadSurfaceCalTips).Location = new Point(((Control)label_MousepadSurfaceCal).Right, ((Control)label_MousepadSurfaceCalTips).Location.Y);
		((Control)customButton_ManualCal).Text = LanguageFile.Dialogs[66];
		int right = ((Control)linkLabel_SystemMouse).Right;
		((Control)linkLabel_SystemMouse).Text = LanguageFile.Dialogs[69];
		((Control)linkLabel_SystemMouse).Location = new Point(right - ((Control)linkLabel_SystemMouse).Size.Width, ((Control)linkLabel_SystemMouse).Location.Y);
		((Control)pictureBox_WinMouse).Location = new Point(((Control)linkLabel_SystemMouse).Left - ((Control)pictureBox_WinMouse).Size.Width - 3, ((Control)pictureBox_WinMouse).Location.Y);
		if (dongleLED)
		{
			dongleRGB = new CustomDongleRGB(dongleled);
			dongleRGB.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_select.png");
			dongleRGB.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\radio_unselect.png");
			((Control)dongleRGB).Location = new Point(0, 133);
			dongleRGB.LEDChange += DongleRGB_LEDChange;
			((Control)this).Controls.Add((Control)(object)dongleRGB);
		}
	}

	private void DongleRGB_LEDChange(object sender, int value)
	{
		DongleRGB dongleRGB = new DongleRGB
		{
			color1 = new byte[3],
			color2 = new byte[3],
			color3 = new byte[3],
			mode = (byte)value
		};
		dongleRGB.color1[0] = this.dongleRGB.Data[1];
		dongleRGB.color1[1] = this.dongleRGB.Data[2];
		dongleRGB.color1[2] = this.dongleRGB.Data[3];
		dongleRGB.color2[0] = this.dongleRGB.Data[4];
		dongleRGB.color2[1] = this.dongleRGB.Data[5];
		dongleRGB.color2[2] = this.dongleRGB.Data[6];
		dongleRGB.color3[0] = this.dongleRGB.Data[7];
		dongleRGB.color3[1] = this.dongleRGB.Data[8];
		dongleRGB.color3[2] = this.dongleRGB.Data[9];
		AdvancedChange?.Invoke(this, 1, value);
	}

	public void SetUSBMode(bool mode)
	{
		isUsb = mode;
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void linkLabel_SystemMouse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL main.cpl @0");
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL main.cpl @0");
	}

	private void customCheckBox_LongRange_Click(object sender, EventArgs e)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (isUsb)
		{
			if (!longRangeMode)
			{
				((Form)new FormDialog(LanguageFile.Dialogs[63], DialogButtons.OK, 12f)).ShowDialog();
				customCheckBox_LongRange.Checked = true;
			}
			else
			{
				longRangeMode = false;
			}
		}
		else if (!longRangeMode)
		{
			FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[64], DialogButtons.OKCanel, 8f);
			((Form)formDialog).ShowDialog();
			if (formDialog.resault)
			{
				longRangeMode = true;
			}
			else
			{
				customCheckBox_LongRange.Checked = true;
			}
		}
		else
		{
			longRangeMode = false;
		}
	}

	private void SetManualCal(bool calPass)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0043: Expected O, but got Unknown
		CalPass = calPass;
		CustomButton customButton = customButton_ManualCal;
		CustomButton customButton2 = customButton_ManualCal;
		CustomButton customButton3 = customButton_ManualCal;
		Bitmap val = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + (calPass ? "\\res\\2Button\\adv_ld.png" : "\\res\\2Button\\adv_nr.png"));
		Image val2 = (Image)val;
		customButton3.MouseDownImage = (Image)val;
		Image normalImage = (customButton2.MouseEnterImage = val2);
		customButton.NormalImage = normalImage;
	}

	private void customButton_ManualCal_Click(object sender, EventArgs e)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if (SensorDPIFile.SupportLODCal(sensor))
		{
			FormCalibration formCalibration = new FormCalibration();
			((Form)formCalibration).ShowDialog();
			if (formCalibration.CalPass != CalPass)
			{
				SetManualCal(formCalibration.CalPass);
			}
		}
		else
		{
			((Form)new FormDialog(LanguageFile.Dialogs[67])).ShowDialog();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormAdvancedSetting));
		linkLabel_SystemMouse = new LinkLabel();
		pictureBox_WinMouse = new PictureBox();
		pictureBox2 = new PictureBox();
		pictureBox3 = new PictureBox();
		label_PowerModeSelect = new Label();
		label_PowerModeSelectTips = new Label();
		label_MousepadSurfaceCalTips = new Label();
		label_MousepadSurfaceCal = new Label();
		label_Title = new Label();
		customButton_Close = new CustomButton();
		customCheckBox_LongRange = new CustomCheckBox();
		customButton_ManualCal = new CustomButton();
		((ISupportInitialize)pictureBox_WinMouse).BeginInit();
		((ISupportInitialize)pictureBox2).BeginInit();
		((ISupportInitialize)pictureBox3).BeginInit();
		((Control)this).SuspendLayout();
		((Control)linkLabel_SystemMouse).AutoSize = true;
		((Control)linkLabel_SystemMouse).BackColor = Color.Transparent;
		linkLabel_SystemMouse.LinkColor = Color.White;
		((Control)linkLabel_SystemMouse).Location = new Point(461, 367);
		((Control)linkLabel_SystemMouse).Name = "linkLabel_SystemMouse";
		((Control)linkLabel_SystemMouse).Size = new Size(127, 20);
		((Control)linkLabel_SystemMouse).TabIndex = 29;
		linkLabel_SystemMouse.TabStop = true;
		((Control)linkLabel_SystemMouse).Text = "Windows鼠标属性";
		linkLabel_SystemMouse.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabel_SystemMouse_LinkClicked);
		((Control)pictureBox_WinMouse).BackColor = Color.Transparent;
		((Control)pictureBox_WinMouse).BackgroundImage = (Image)(object)Resources.win系统鼠标;
		((Control)pictureBox_WinMouse).BackgroundImageLayout = (ImageLayout)3;
		((Control)pictureBox_WinMouse).Cursor = Cursors.Hand;
		((Control)pictureBox_WinMouse).Location = new Point(443, 366);
		((Control)pictureBox_WinMouse).Name = "pictureBox_WinMouse";
		((Control)pictureBox_WinMouse).Size = new Size(14, 20);
		pictureBox_WinMouse.TabIndex = 30;
		pictureBox_WinMouse.TabStop = false;
		((Control)pictureBox_WinMouse).Click += pictureBox1_Click;
		((Control)pictureBox2).BackColor = Color.Transparent;
		((Control)pictureBox2).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox2).Location = new Point(12, 53);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(33, 20);
		pictureBox2.TabIndex = 31;
		pictureBox2.TabStop = false;
		((Control)pictureBox3).BackColor = Color.Transparent;
		((Control)pictureBox3).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox3).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox3).Location = new Point(12, 133);
		((Control)pictureBox3).Name = "pictureBox3";
		((Control)pictureBox3).Size = new Size(33, 20);
		pictureBox3.TabIndex = 32;
		pictureBox3.TabStop = false;
		((Control)pictureBox3).Visible = false;
		((Control)label_PowerModeSelect).AutoSize = true;
		((Control)label_PowerModeSelect).BackColor = Color.Transparent;
		((Control)label_PowerModeSelect).Location = new Point(51, 53);
		((Control)label_PowerModeSelect).Name = "label_PowerModeSelect";
		((Control)label_PowerModeSelect).Size = new Size(65, 20);
		((Control)label_PowerModeSelect).TabIndex = 33;
		((Control)label_PowerModeSelect).Text = "模式选择";
		((Control)label_PowerModeSelectTips).BackColor = Color.Transparent;
		((Control)label_PowerModeSelectTips).Font = new Font("微软雅黑", 7.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_PowerModeSelectTips).Location = new Point(125, 55);
		((Control)label_PowerModeSelectTips).Name = "label_PowerModeSelectTips";
		((Control)label_PowerModeSelectTips).Size = new Size(393, 48);
		((Control)label_PowerModeSelectTips).TabIndex = 34;
		((Control)label_PowerModeSelectTips).Text = " (远距离模式下，距离会更远抗干扰能力更强，相应工作电流会加大，使用时间减少)";
		((Control)label_MousepadSurfaceCalTips).BackColor = Color.Transparent;
		((Control)label_MousepadSurfaceCalTips).Font = new Font("微软雅黑", 7.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_MousepadSurfaceCalTips).Location = new Point(164, 137);
		((Control)label_MousepadSurfaceCalTips).Name = "label_MousepadSurfaceCalTips";
		((Control)label_MousepadSurfaceCalTips).Size = new Size(354, 32);
		((Control)label_MousepadSurfaceCalTips).TabIndex = 36;
		((Control)label_MousepadSurfaceCalTips).Text = "(找到鼠标与鼠标垫的最佳搭配参数，体验更高级别的追踪精度)";
		((Control)label_MousepadSurfaceCalTips).Visible = false;
		((Control)label_MousepadSurfaceCal).AutoSize = true;
		((Control)label_MousepadSurfaceCal).BackColor = Color.Transparent;
		((Control)label_MousepadSurfaceCal).Location = new Point(51, 133);
		((Control)label_MousepadSurfaceCal).Name = "label_MousepadSurfaceCal";
		((Control)label_MousepadSurfaceCal).Size = new Size(107, 20);
		((Control)label_MousepadSurfaceCal).TabIndex = 35;
		((Control)label_MousepadSurfaceCal).Text = "鼠标垫表面校准";
		((Control)label_MousepadSurfaceCal).Visible = false;
		((Control)label_Title).AutoSize = true;
		((Control)label_Title).BackColor = Color.Transparent;
		((Control)label_Title).Font = new Font("微软雅黑", 14.25f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Title).Location = new Point(211, 12);
		((Control)label_Title).Name = "label_Title";
		((Control)label_Title).Size = new Size(88, 25);
		((Control)label_Title).TabIndex = 41;
		((Control)label_Title).Text = "高级设置";
		((Control)customButton_Close).Location = new Point(574, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 27;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)customCheckBox_LongRange).BackColor = Color.Transparent;
		customCheckBox_LongRange.Checked = false;
		customCheckBox_LongRange.CheckImage = (Image)(object)Resources.开;
		((Control)customCheckBox_LongRange).Cursor = Cursors.Default;
		((Control)customCheckBox_LongRange).Location = new Point(55, 90);
		((Control)customCheckBox_LongRange).Name = "customCheckBox_LongRange";
		((Control)customCheckBox_LongRange).Size = new Size(54, 22);
		((Control)customCheckBox_LongRange).TabIndex = 42;
		((Control)customCheckBox_LongRange).Text = "customCheckBox1";
		customCheckBox_LongRange.UncheckImage = (Image)(object)Resources.关;
		((Control)customCheckBox_LongRange).Click += customCheckBox_LongRange_Click;
		((Control)customButton_ManualCal).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)customButton_ManualCal).Location = new Point(55, 166);
		customButton_ManualCal.MouseDownImage = (Image)(object)Resources.远距离模式按键按下;
		customButton_ManualCal.MouseEnterImage = (Image)(object)Resources.普通模式按键按下;
		((Control)customButton_ManualCal).Name = "customButton_ManualCal";
		customButton_ManualCal.NormalImage = (Image)(object)Resources.普通模式按键按下;
		((Control)customButton_ManualCal).Size = new Size(66, 26);
		((Control)customButton_ManualCal).TabIndex = 43;
		((Control)customButton_ManualCal).Text = "手动校准";
		((Control)customButton_ManualCal).Visible = false;
		((Control)customButton_ManualCal).Click += customButton_ManualCal_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.设置界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(600, 396);
		((Control)this).Controls.Add((Control)(object)customButton_ManualCal);
		((Control)this).Controls.Add((Control)(object)customCheckBox_LongRange);
		((Control)this).Controls.Add((Control)(object)label_Title);
		((Control)this).Controls.Add((Control)(object)label_MousepadSurfaceCalTips);
		((Control)this).Controls.Add((Control)(object)label_MousepadSurfaceCal);
		((Control)this).Controls.Add((Control)(object)label_PowerModeSelectTips);
		((Control)this).Controls.Add((Control)(object)label_PowerModeSelect);
		((Control)this).Controls.Add((Control)(object)pictureBox3);
		((Control)this).Controls.Add((Control)(object)pictureBox2);
		((Control)this).Controls.Add((Control)(object)pictureBox_WinMouse);
		((Control)this).Controls.Add((Control)(object)linkLabel_SystemMouse);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormAdvancedSetting";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormAdvancedSetting";
		((ISupportInitialize)pictureBox_WinMouse).EndInit();
		((ISupportInitialize)pictureBox2).EndInit();
		((ISupportInitialize)pictureBox3).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
