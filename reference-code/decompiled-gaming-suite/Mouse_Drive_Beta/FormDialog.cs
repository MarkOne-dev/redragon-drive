using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormDialog : Form
{
	public bool resault;

	public bool dontShowAgain;

	private FormResize formResize;

	private IContainer components;

	private Label label1;

	private CustomButton customButton_Confirm;

	private CustomButton customButton_Cancel;

	private CustomButton customButton_Close;

	private CustomCheckBox customCheckBox_DontShowAgain;

	private Label label_DontShowAgain;

	public FormDialog(string str)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		formResize = new FormResize();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)label1).Font = new Font("微软雅黑", 12f);
		((Control)label1).Text = str;
		((Control)customButton_Cancel).Visible = false;
		((Control)customButton_Confirm).Location = new Point((((Control)this).Width - ((Control)customButton_Confirm).Width) / 2, ((Control)customButton_Confirm).Location.Y);
		((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
	}

	public FormDialog(string str, DialogButtons buttons)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		formResize = new FormResize();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)label1).Font = new Font("微软雅黑", 12f);
		((Control)label1).Text = str;
		switch (buttons)
		{
		case DialogButtons.NO:
			((Control)customButton_Confirm).Visible = false;
			((Control)customButton_Close).Visible = false;
			((Control)customButton_Cancel).Visible = false;
			break;
		case DialogButtons.OK:
			((Control)customButton_Cancel).Visible = false;
			((Control)customButton_Confirm).Location = new Point((((Control)this).Width - ((Control)customButton_Confirm).Width) / 2, ((Control)customButton_Confirm).Location.Y);
			((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
			break;
		case DialogButtons.OKCanel:
			((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
			((Control)customButton_Cancel).Text = LanguageFile.Dialogs[38];
			break;
		}
	}

	public FormDialog(string str, DialogButtons buttons, float fontSize)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		formResize = new FormResize();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)label1).Font = new Font("微软雅黑", fontSize);
		((Control)label1).Text = str;
		switch (buttons)
		{
		case DialogButtons.NO:
			((Control)customButton_Confirm).Visible = false;
			((Control)customButton_Close).Visible = false;
			((Control)customButton_Cancel).Visible = false;
			break;
		case DialogButtons.OK:
			((Control)customButton_Cancel).Visible = false;
			((Control)customButton_Confirm).Location = new Point((((Control)this).Width - ((Control)customButton_Confirm).Width) / 2, ((Control)customButton_Confirm).Location.Y);
			((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
			break;
		case DialogButtons.OKCanel:
			((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
			((Control)customButton_Cancel).Text = LanguageFile.Dialogs[38];
			break;
		}
	}

	public FormDialog(string str, bool dontShowAgain)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		formResize = new FormResize();
		((Form)this)._002Ector();
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)label1).Font = new Font("微软雅黑", 12f);
		((Control)label1).Text = str;
		((Control)customButton_Cancel).Visible = false;
		((Control)customButton_Confirm).Location = new Point((((Control)this).Width - ((Control)customButton_Confirm).Width) / 2, ((Control)customButton_Confirm).Location.Y);
		((Control)customButton_Confirm).Text = LanguageFile.Dialogs[37];
		((Control)customCheckBox_DontShowAgain).Visible = true;
		((Control)label_DontShowAgain).Visible = true;
		((Control)label_DontShowAgain).Text = LanguageFile.Dialogs[94];
	}

	private void ImageInit()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0044: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_006d: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0096: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		((Control)this).BackgroundImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\message_bg.png");
		CustomButton customButton = customButton_Cancel;
		CustomButton customButton2 = customButton_Confirm;
		Bitmap val = new Bitmap(baseDirectory + "\\res\\7General\\ok_nr.png");
		Image normalImage = (Image)val;
		customButton2.NormalImage = (Image)val;
		customButton.NormalImage = normalImage;
		CustomButton customButton3 = customButton_Cancel;
		CustomButton customButton4 = customButton_Confirm;
		Bitmap val2 = new Bitmap(baseDirectory + "\\res\\7General\\ok_down.png");
		normalImage = (Image)val2;
		customButton4.MouseDownImage = (Image)val2;
		customButton3.MouseDownImage = normalImage;
		CustomButton customButton5 = customButton_Cancel;
		CustomButton customButton6 = customButton_Confirm;
		Bitmap val3 = new Bitmap(baseDirectory + "\\res\\7General\\ok_enter.png");
		normalImage = (Image)val3;
		customButton6.MouseEnterImage = (Image)val3;
		customButton5.MouseEnterImage = normalImage;
		customButton_Close.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_nr.png");
		customButton_Close.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_down.png");
		customButton_Close.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\setting_close_enter.png");
		customCheckBox_DontShowAgain.CheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_select.png");
		customCheckBox_DontShowAgain.UncheckImage = (Image)new Bitmap(baseDirectory + "\\res\\7General\\check_unselect.png");
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		dontShowAgain = customCheckBox_DontShowAgain.Checked;
		((Form)this).Close();
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		resault = true;
		dontShowAgain = customCheckBox_DontShowAgain.Checked;
		((Form)this).Close();
	}

	private void customButton_Cancel_Click(object sender, EventArgs e)
	{
		dontShowAgain = customCheckBox_DontShowAgain.Checked;
		((Form)this).Close();
	}

	private void FormDialog_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		if ((int)e.KeyCode == 13)
		{
			customButton_Confirm_Click(null, null);
		}
		else if ((int)e.KeyCode == 27)
		{
			customButton_Cancel_Click(null, null);
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormDialog));
		label1 = new Label();
		customButton_Confirm = new CustomButton();
		customButton_Cancel = new CustomButton();
		customButton_Close = new CustomButton();
		customCheckBox_DontShowAgain = new CustomCheckBox();
		label_DontShowAgain = new Label();
		((Control)this).SuspendLayout();
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(23, 21);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(325, 88);
		((Control)label1).TabIndex = 5;
		((Control)label1).Text = "label1";
		label1.TextAlign = (ContentAlignment)32;
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(70, 113);
		((Control)customButton_Confirm).Margin = new Padding(3, 4, 3, 4);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(100, 26);
		((Control)customButton_Confirm).TabIndex = 6;
		((Control)customButton_Confirm).Text = "确定";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)customButton_Cancel).ForeColor = Color.White;
		((Control)customButton_Cancel).Location = new Point(210, 113);
		((Control)customButton_Cancel).Margin = new Padding(3, 4, 3, 4);
		customButton_Cancel.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Cancel.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Cancel).Name = "customButton_Cancel";
		customButton_Cancel.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Cancel).Size = new Size(100, 26);
		((Control)customButton_Cancel).TabIndex = 7;
		((Control)customButton_Cancel).Text = "取消";
		((Control)customButton_Cancel).Click += customButton_Cancel_Click;
		((Control)customButton_Close).Location = new Point(354, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 26;
		((Control)customButton_Close).Click += customButton_Close_Click;
		customCheckBox_DontShowAgain.Checked = false;
		customCheckBox_DontShowAgain.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_DontShowAgain).Location = new Point(4, 142);
		((Control)customCheckBox_DontShowAgain).Name = "customCheckBox_DontShowAgain";
		((Control)customCheckBox_DontShowAgain).Size = new Size(16, 16);
		((Control)customCheckBox_DontShowAgain).TabIndex = 27;
		((Control)customCheckBox_DontShowAgain).Text = "customCheckBox1";
		customCheckBox_DontShowAgain.UncheckImage = (Image)(object)Resources.单选框未选择;
		((Control)customCheckBox_DontShowAgain).Visible = false;
		((Control)label_DontShowAgain).AutoSize = true;
		((Control)label_DontShowAgain).BackColor = Color.Transparent;
		((Control)label_DontShowAgain).Location = new Point(23, 141);
		((Control)label_DontShowAgain).Name = "label_DontShowAgain";
		((Control)label_DontShowAgain).Size = new Size(50, 20);
		((Control)label_DontShowAgain).TabIndex = 28;
		((Control)label_DontShowAgain).Text = "label2";
		((Control)label_DontShowAgain).Visible = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.消息窗背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(380, 164);
		((Control)this).Controls.Add((Control)(object)label_DontShowAgain);
		((Control)this).Controls.Add((Control)(object)customCheckBox_DontShowAgain);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).Controls.Add((Control)(object)customButton_Cancel);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).ImeMode = (ImeMode)3;
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(5, 6, 5, 6);
		((Control)this).Name = "FormDialog";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "FormDialog";
		((Control)this).KeyDown += new KeyEventHandler(FormDialog_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
