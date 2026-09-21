using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormCalibration : Form
{
	private FormResize formResize;

	private Timer DongleCheckTimer;

	public bool CalPass;

	private IContainer components;

	private CustomButton customButton_Close;

	private Label label_Title;

	private PictureBox pictureBox_Main;

	private PictureBox pictureBox_Loading;

	private CustomButton customButton_Start;

	private CustomButton customButton_Back;

	public FormCalibration()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		formResize = new FormResize();
		CalPass = true;
		((Form)this)._002Ector();
		InitializeComponent();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\setting_bg.png");
		((Control)pictureBox_Main).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\8Calibration\\cal_mouse.png");
		pictureBox_Loading.Image = (Image)new Bitmap(baseDirectory + "\\res\\8Calibration\\caling.gif");
		formResize.Resize((Form)(object)this);
		((Control)label_Title).Text = LanguageFile.Dialogs[70];
		((Control)customButton_Start).Text = LanguageFile.Dialogs[74];
		((Control)label_Title).Location = new Point((((Control)this).Width - ((Control)label_Title).Width) / 2, ((Control)label_Title).Top);
		((Control)pictureBox_Main).Location = new Point((((Control)this).Width - ((Control)pictureBox_Main).Width) / 2, ((Control)pictureBox_Main).Top);
		((Control)pictureBox_Loading).Location = new Point((((Control)this).Width - ((Control)pictureBox_Loading).Width) / 2, ((Control)pictureBox_Loading).Top);
		((Control)customButton_Start).Location = new Point((((Control)this).Width - ((Control)customButton_Start).Width) / 2, ((Control)customButton_Start).Top);
		((Control)customButton_Back).Visible = false;
		((Control)customButton_Back).Text = LanguageFile.Dialogs[75];
		((Control)customButton_Back).Location = new Point(((Control)this).Width / 2 - ((Control)customButton_Back).Width - 20, ((Control)customButton_Back).Top);
		((Control)pictureBox_Loading).Visible = false;
		DongleCheckTimer = new Timer();
		DongleCheckTimer.Interval = 3000;
		DongleCheckTimer.Enabled = false;
		DongleCheckTimer.Tick += DongleCheckTimer_Tick;
	}

	private void DongleCheckTimer_Tick(object sender, EventArgs e)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		DongleCheckTimer.Enabled = false;
		if (!CalPass)
		{
			((Control)pictureBox_Loading).Visible = false;
			((Control)label_Title).Text = LanguageFile.Dialogs[72];
			((Control)label_Title).Location = new Point((((Control)this).Width - ((Control)label_Title).Width) / 2, ((Control)label_Title).Top);
			((Control)pictureBox_Main).BackgroundImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\8Calibration\\pass.png");
			((Control)customButton_Back).Location = new Point((((Control)this).Width - ((Control)customButton_Back).Width) / 2, ((Control)customButton_Back).Top);
			((Control)customButton_Back).Visible = true;
			CalPass = true;
		}
		else
		{
			((Control)pictureBox_Loading).Visible = false;
			((Control)label_Title).Text = LanguageFile.Dialogs[73];
			((Control)label_Title).Location = new Point((((Control)this).Width - ((Control)label_Title).Width) / 2, ((Control)label_Title).Top);
			((Control)pictureBox_Main).BackgroundImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\8Calibration\\fail.png");
			((Control)customButton_Back).Visible = true;
			((Control)customButton_Start).Text = LanguageFile.Dialogs[76];
			((Control)customButton_Start).Location = new Point(((Control)customButton_Back).Right + 40, ((Control)customButton_Start).Top);
			((Control)customButton_Start).Visible = true;
			CalPass = false;
		}
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void customButton_Back_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void customButton_Start_Click(object sender, EventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		((Control)customButton_Back).Visible = false;
		((Control)pictureBox_Loading).Visible = true;
		((Control)label_Title).Text = LanguageFile.Dialogs[71];
		((Control)label_Title).Location = new Point((((Control)this).Width - ((Control)label_Title).Width) / 2, ((Control)label_Title).Top);
		((Control)pictureBox_Main).BackgroundImage = (Image)new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "\\res\\8Calibration\\cal_mouse.png");
		((Control)customButton_Start).Visible = false;
		DongleCheckTimer.Enabled = true;
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormCalibration));
		customButton_Close = new CustomButton();
		label_Title = new Label();
		pictureBox_Main = new PictureBox();
		pictureBox_Loading = new PictureBox();
		customButton_Start = new CustomButton();
		customButton_Back = new CustomButton();
		((ISupportInitialize)pictureBox_Main).BeginInit();
		((ISupportInitialize)pictureBox_Loading).BeginInit();
		((Control)this).SuspendLayout();
		((Control)customButton_Close).Location = new Point(874, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 27;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)label_Title).AutoSize = true;
		((Control)label_Title).BackColor = Color.Transparent;
		((Control)label_Title).Font = new Font("微软雅黑", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Title).ForeColor = Color.White;
		((Control)label_Title).Location = new Point(282, 73);
		((Control)label_Title).Name = "label_Title";
		((Control)label_Title).Size = new Size(73, 28);
		((Control)label_Title).TabIndex = 28;
		((Control)label_Title).Text = "label1";
		((Control)pictureBox_Main).BackColor = Color.Transparent;
		((Control)pictureBox_Main).BackgroundImage = (Image)(object)Resources.校准鼠标;
		((Control)pictureBox_Main).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Main).Location = new Point(348, 137);
		((Control)pictureBox_Main).Name = "pictureBox_Main";
		((Control)pictureBox_Main).Size = new Size(264, 346);
		pictureBox_Main.TabIndex = 29;
		pictureBox_Main.TabStop = false;
		((Control)pictureBox_Loading).BackColor = Color.Transparent;
		((Control)pictureBox_Loading).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Loading).Location = new Point(454, 178);
		((Control)pictureBox_Loading).Name = "pictureBox_Loading";
		((Control)pictureBox_Loading).Size = new Size(50, 50);
		pictureBox_Loading.SizeMode = (PictureBoxSizeMode)1;
		pictureBox_Loading.TabIndex = 30;
		pictureBox_Loading.TabStop = false;
		((Control)customButton_Start).ForeColor = Color.White;
		((Control)customButton_Start).Location = new Point(498, 510);
		customButton_Start.MouseDownImage = (Image)(object)Resources.按钮点击状态;
		customButton_Start.MouseEnterImage = (Image)(object)Resources.按钮默认状态;
		((Control)customButton_Start).Name = "customButton_Start";
		customButton_Start.NormalImage = (Image)(object)Resources.按钮默认状态;
		((Control)customButton_Start).Size = new Size(128, 42);
		((Control)customButton_Start).TabIndex = 31;
		((Control)customButton_Start).Text = "开始";
		((Control)customButton_Start).Click += customButton_Start_Click;
		((Control)customButton_Back).ForeColor = Color.White;
		((Control)customButton_Back).Location = new Point(319, 510);
		customButton_Back.MouseDownImage = (Image)(object)Resources.按钮点击状态;
		customButton_Back.MouseEnterImage = (Image)(object)Resources.按钮默认状态;
		((Control)customButton_Back).Name = "customButton_Back";
		customButton_Back.NormalImage = (Image)(object)Resources.按钮默认状态;
		((Control)customButton_Back).Size = new Size(128, 42);
		((Control)customButton_Back).TabIndex = 32;
		((Control)customButton_Back).Text = "返回";
		((Control)customButton_Back).Click += customButton_Back_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.设置界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(900, 600);
		((Control)this).Controls.Add((Control)(object)customButton_Back);
		((Control)this).Controls.Add((Control)(object)customButton_Start);
		((Control)this).Controls.Add((Control)(object)pictureBox_Loading);
		((Control)this).Controls.Add((Control)(object)pictureBox_Main);
		((Control)this).Controls.Add((Control)(object)label_Title);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormCalibration";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormCalibration";
		((ISupportInitialize)pictureBox_Main).EndInit();
		((ISupportInitialize)pictureBox_Loading).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
