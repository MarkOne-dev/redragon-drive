using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormPair : Form
{
	private FormResize formResize;

	private Timer PairingTimer;

	private Timer CountTimer;

	private bool CanStartPair;

	private bool IsPairingFlag;

	private string PairingText;

	private string PairDevice;

	private bool FormMainFlag;

	public ButtonStateEnum PairState;

	private DeviceAllInfo gDeviceAllInfo;

	private int CID;

	private IContainer components;

	private PictureBox pictureBox2;

	private Label label_PairTips;

	private CustomButton customButton_Close;

	private CustomStateButton customStateButton_Pair;

	private ToolTip toolTip;

	private CustomButton customButton_Confirm;

	public FormPair(int cid)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		formResize = new FormResize();
		PairingTimer = new Timer();
		CountTimer = new Timer();
		PairingText = "";
		PairDevice = "";
		FormMainFlag = true;
		((Form)this)._002Ector();
		InitializeComponent();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_bg.png");
		pictureBox2.Image = (Image)new Bitmap(baseDirectory + "\\res\\7General\\tips.png");
		customStateButton_Pair.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_nr.png");
		customStateButton_Pair.FailImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_fail.png");
		customStateButton_Pair.SuccessImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_success.png");
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
		customButton_Confirm.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_nr.png");
		customButton_Confirm.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_down.png");
		customButton_Confirm.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_enter.png");
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		CID = cid;
		DriveConfig.DriveParam driveParam = DriveConfig.GetDriveParam();
		((Control)label_PairTips).ForeColor = driveParam.TipsClr;
		((Control)label_PairTips).Text = LanguageFile.Dialogs[43];
		PairingText = LanguageFile.Dialogs[48];
		customStateButton_Pair.NormalText = LanguageFile.Dialogs[42];
		customStateButton_Pair.DoingText = LanguageFile.Dialogs[48];
		((Control)customStateButton_Pair).Text = LanguageFile.Dialogs[42];
		((Control)customStateButton_Pair).Location = new Point((((Control)this).Width - ((Control)customStateButton_Pair).Width) / 2, ((Control)customStateButton_Pair).Location.Y);
		((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
		PairingTimer.Enabled = false;
		PairingTimer.Interval = 1000;
		PairingTimer.Tick += PairingTimer_Tick;
		CountTimer.Enabled = true;
		CountTimer.Interval = 100;
		CountTimer.Tick += CountTimer_Tick;
		PairDevice = FormMain.SelectedDeviceInfo.deviceString;
		gDeviceAllInfo = FormMain.SelectedDeviceInfo;
	}

	public FormPair(DeviceAllInfo deviceAllInfo, int cid)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		formResize = new FormResize();
		PairingTimer = new Timer();
		CountTimer = new Timer();
		PairingText = "";
		PairDevice = "";
		FormMainFlag = true;
		((Form)this)._002Ector();
		InitializeComponent();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_bg.png");
		pictureBox2.Image = (Image)new Bitmap(baseDirectory + "\\res\\7General\\tips.png");
		customStateButton_Pair.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_nr.png");
		customStateButton_Pair.FailImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_fail.png");
		customStateButton_Pair.SuccessImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_success.png");
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
		customButton_Confirm.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_nr.png");
		customButton_Confirm.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_down.png");
		customButton_Confirm.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\ok_enter.png");
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		CID = cid;
		DriveConfig.DriveParam driveParam = DriveConfig.GetDriveParam();
		((Control)label_PairTips).ForeColor = driveParam.TipsClr;
		((Control)label_PairTips).Text = LanguageFile.Dialogs[43];
		PairingText = LanguageFile.Dialogs[48];
		customStateButton_Pair.NormalText = LanguageFile.Dialogs[42];
		customStateButton_Pair.DoingText = LanguageFile.Dialogs[48];
		((Control)customStateButton_Pair).Text = LanguageFile.Dialogs[42];
		((Control)customStateButton_Pair).Location = new Point((((Control)this).Width - ((Control)customStateButton_Pair).Width) / 2, ((Control)customStateButton_Pair).Location.Y);
		((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
		PairingTimer.Enabled = false;
		PairingTimer.Interval = 1000;
		PairingTimer.Tick += PairingTimer_Tick;
		PairDevice = deviceAllInfo.deviceString;
		gDeviceAllInfo = deviceAllInfo;
		CanStartPair = true;
		FormMainFlag = false;
	}

	private void CountTimer_Tick(object sender, EventArgs e)
	{
		if (!IsPairingFlag)
		{
			if (!gDeviceAllInfo.isUSB)
			{
				CanStartPair = true;
				((Control)label_PairTips).Text = LanguageFile.Dialogs[43];
			}
			else
			{
				CanStartPair = false;
				((Control)label_PairTips).Text = LanguageFile.Dialogs[46];
			}
		}
		((Control)customStateButton_Pair).Enabled = CanStartPair;
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		if (!IsPairingFlag)
		{
			((Form)this).Close();
		}
	}

	private void PairingInit()
	{
		IsPairingFlag = false;
		DongleEnterPairing();
	}

	private void DongleEnterPairing()
	{
		if (!IsPairingFlag)
		{
			IsPairingFlag = true;
			if (FormMainFlag)
			{
				FormMain.IntervalTimer.Enabled = false;
				FormMain.DeviceStateTimer.Enabled = false;
			}
			PairingTimer.Enabled = true;
			UsbServer.Start(PairDevice, PairDevice, PairingUsbDataReceived);
			UsbServer.EnterDonglePairOnlyCid((byte)CID);
		}
		else
		{
			UsbServer.EnterDonglePairOnlyCid((byte)CID);
		}
	}

	private void PairingTimer_Tick(object sender, EventArgs e)
	{
		bool flag = false;
		UsbServer.ReadDonglePairStatus();
		string[] array = UsbFinder.FindHidDevicesByDefaultDeviceId(gDeviceAllInfo.VID, gDeviceAllInfo.PID);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == gDeviceAllInfo.deviceString)
			{
				flag = true;
				break;
			}
		}
		if ((array.Length != 0 && flag) || !IsPairingFlag)
		{
			return;
		}
		PairingTimer.Enabled = false;
		IsPairingFlag = false;
		((Control)this).Invoke((Delegate)(EventHandler)delegate
		{
			if (FormMainFlag)
			{
				FormMain.IntervalTimer.Enabled = true;
				FormMain.DeviceStateTimer.Enabled = true;
			}
			PairState = ButtonStateEnum.Fail;
			customStateButton_Pair.ButtonState = ButtonStateEnum.Fail;
			((Control)customStateButton_Pair).Text = LanguageFile.Dialogs[50];
		});
	}

	private void PairingUsbDataReceived(UsbCommand command)
	{
		if (command.id != 6 || command.receivedData[0] == 1)
		{
			return;
		}
		if (command.receivedData[0] == 2)
		{
			PairingTimer.Enabled = false;
			IsPairingFlag = false;
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				if (FormMainFlag)
				{
					FormMain.IntervalTimer.Enabled = true;
					FormMain.DeviceStateTimer.Enabled = true;
				}
				PairState = ButtonStateEnum.Fail;
				customStateButton_Pair.ButtonState = ButtonStateEnum.Fail;
				((Control)customStateButton_Pair).Text = LanguageFile.Dialogs[50];
				((Control)customButton_Confirm).Visible = true;
			});
		}
		else
		{
			if (command.receivedData[0] != 3)
			{
				return;
			}
			PairingTimer.Enabled = false;
			IsPairingFlag = false;
			((Control)this).Invoke((Delegate)(EventHandler)delegate
			{
				if (FormMainFlag)
				{
					FormMain.IntervalTimer.Enabled = true;
					FormMain.DeviceStateTimer.Enabled = true;
				}
				PairState = ButtonStateEnum.Success;
				customStateButton_Pair.ButtonState = ButtonStateEnum.PairSuccess;
				((Control)customStateButton_Pair).Text = LanguageFile.Dialogs[49];
				((Control)customStateButton_Pair).Enabled = false;
				((Control)customButton_Confirm).Visible = true;
			});
		}
	}

	private void customStateButton_Pair_Click(object sender, EventArgs e)
	{
		if (customStateButton_Pair.ButtonState != ButtonStateEnum.PairSuccess && CanStartPair)
		{
			customStateButton_Pair.ButtonState = ButtonStateEnum.Doing;
			((Control)customButton_Confirm).Visible = false;
			PairingInit();
		}
	}

	private void FormPair_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 32)
		{
			customStateButton_Pair_Click(null, null);
		}
		else if ((int)e.KeyCode == 27)
		{
			customButton_Close_Click(null, null);
		}
		else if ((int)e.KeyCode == 13)
		{
			customButton_Close_Click(null, null);
		}
	}

	private void customStateButton_Pair_MouseEnter(object sender, EventArgs e)
	{
		CustomStateButton customStateButton = (CustomStateButton)sender;
		if (Convert.ToInt32(((Control)this).CreateGraphics().MeasureString(((Control)customStateButton).Text, ((Control)customStateButton).Font).Width) >= ((Control)customStateButton).Width)
		{
			toolTip.SetToolTip((Control)(object)customStateButton, ((Control)customStateButton).Text);
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormPair));
		pictureBox2 = new PictureBox();
		label_PairTips = new Label();
		toolTip = new ToolTip(components);
		customStateButton_Pair = new CustomStateButton();
		customButton_Close = new CustomButton();
		customButton_Confirm = new CustomButton();
		((ISupportInitialize)pictureBox2).BeginInit();
		((Control)this).SuspendLayout();
		((Control)pictureBox2).BackColor = Color.Transparent;
		((Control)pictureBox2).BackgroundImageLayout = (ImageLayout)0;
		pictureBox2.Image = (Image)(object)Resources.提示符号;
		((Control)pictureBox2).Location = new Point(12, 72);
		((Control)pictureBox2).Name = "pictureBox2";
		((Control)pictureBox2).Size = new Size(27, 24);
		pictureBox2.SizeMode = (PictureBoxSizeMode)3;
		pictureBox2.TabIndex = 2;
		pictureBox2.TabStop = false;
		((Control)label_PairTips).BackColor = Color.Transparent;
		((Control)label_PairTips).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_PairTips).ForeColor = Color.FromArgb(255, 106, 0);
		((Control)label_PairTips).Location = new Point(44, 26);
		((Control)label_PairTips).MaximumSize = new Size(326, 140);
		((Control)label_PairTips).Name = "label_PairTips";
		((Control)label_PairTips).Size = new Size(326, 140);
		((Control)label_PairTips).TabIndex = 3;
		((Control)label_PairTips).Text = "让鼠标进入配对状态，并靠近接收器";
		label_PairTips.TextAlign = (ContentAlignment)32;
		customStateButton_Pair.ButtonState = ButtonStateEnum.Normal;
		customStateButton_Pair.DoingText = "正在配对";
		customStateButton_Pair.FailImage = (Image)(object)Resources.开始配对失败状态;
		((Control)customStateButton_Pair).Location = new Point(123, 183);
		((Control)customStateButton_Pair).MaximumSize = new Size(500, 200);
		((Control)customStateButton_Pair).Name = "customStateButton_Pair";
		customStateButton_Pair.NormalImage = (Image)(object)Resources.开始配对默认状态;
		customStateButton_Pair.NormalText = "";
		customStateButton_Pair.Percent = 0;
		((Control)customStateButton_Pair).Size = new Size(154, 32);
		customStateButton_Pair.SuccessImage = (Image)(object)Resources.开始配对成功状态;
		((Control)customStateButton_Pair).TabIndex = 26;
		((Control)customStateButton_Pair).Text = "开始配对";
		customStateButton_Pair.UpdatedImage = null;
		((Control)customStateButton_Pair).Click += customStateButton_Pair_Click;
		((Control)customStateButton_Pair).MouseEnter += customStateButton_Pair_MouseEnter;
		((Control)customButton_Close).Location = new Point(398, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 25;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(312, 188);
		((Control)customButton_Confirm).Margin = new Padding(3, 4, 3, 4);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(100, 26);
		((Control)customButton_Confirm).TabIndex = 27;
		((Control)customButton_Confirm).Text = "确定";
		((Control)customButton_Confirm).Visible = false;
		((Control)customButton_Confirm).Click += customButton_Close_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.配对背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(424, 227);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)customStateButton_Pair);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).Controls.Add((Control)(object)label_PairTips);
		((Control)this).Controls.Add((Control)(object)pictureBox2);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormPair";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormPair";
		((Control)this).KeyDown += new KeyEventHandler(FormPair_KeyDown);
		((ISupportInitialize)pictureBox2).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
