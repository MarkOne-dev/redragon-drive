using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormFireKey : Form
{
	private FormResize formResize = new FormResize();

	public byte times = 3;

	public byte interval = 10;

	public bool NeedUpdate;

	private IContainer components;

	private CustomButton customButton_Close;

	private Label label_Times;

	private Label label_Interval;

	private Label label_Tips;

	private TextBox Dlg_textBox_Times;

	private TextBox Dlg_textBox_Interval;

	private CustomButton customButton_Confirm;

	private CustomButton customButton_Cancel;

	public FormFireKey(string[] labels, byte itimes, byte inter)
	{
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		times = itimes;
		interval = inter;
		((Control)Dlg_textBox_Times).Focus();
		((Control)Dlg_textBox_Times).Text = times.ToString();
		((Control)Dlg_textBox_Interval).Text = interval.ToString();
		((Control)label_Times).Text = labels[0];
		((Control)label_Interval).Text = labels[1];
		((Control)label_Tips).Text = labels[2];
		((Control)customButton_Confirm).Text = labels[3];
		((Control)customButton_Cancel).Text = labels[4];
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
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		if (((Control)Dlg_textBox_Times).Text == "")
		{
			((Control)Dlg_textBox_Times).Text = 3.ToString();
		}
		times = Convert.ToByte(((Control)Dlg_textBox_Times).Text);
		if (((Control)Dlg_textBox_Interval).Text == "")
		{
			((Control)Dlg_textBox_Interval).Text = 10.ToString();
		}
		interval = Convert.ToByte(((Control)Dlg_textBox_Interval).Text);
		interval = (byte)((interval < 10) ? 10 : interval);
		NeedUpdate = true;
		((Form)this).Close();
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void customButton_Cancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void textBox_Times_TextChanged(object sender, EventArgs e)
	{
		if (!(((Control)Dlg_textBox_Times).Text == "") && int.Parse(((Control)Dlg_textBox_Times).Text) > 3)
		{
			((Control)Dlg_textBox_Times).Text = 3.ToString();
		}
	}

	private void textBox_Times_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void textBox_Interval_TextChanged(object sender, EventArgs e)
	{
		if (!(((Control)Dlg_textBox_Interval).Text == "") && int.Parse(((Control)Dlg_textBox_Interval).Text) > 255)
		{
			((Control)Dlg_textBox_Interval).Text = 255.ToString();
		}
	}

	private void textBox_Interval_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void FormFireKey_KeyDown(object sender, KeyEventArgs e)
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Expected O, but got Unknown
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormFireKey));
		label_Times = new Label();
		label_Interval = new Label();
		label_Tips = new Label();
		Dlg_textBox_Times = new TextBox();
		Dlg_textBox_Interval = new TextBox();
		customButton_Cancel = new CustomButton();
		customButton_Confirm = new CustomButton();
		customButton_Close = new CustomButton();
		((Control)this).SuspendLayout();
		((Control)label_Times).AutoSize = true;
		((Control)label_Times).BackColor = Color.Transparent;
		((Control)label_Times).Location = new Point(65, 22);
		((Control)label_Times).Name = "label_Times";
		((Control)label_Times).Size = new Size(87, 20);
		((Control)label_Times).TabIndex = 20;
		((Control)label_Times).Text = "次数（0-3）";
		((Control)label_Interval).AutoSize = true;
		((Control)label_Interval).BackColor = Color.Transparent;
		((Control)label_Interval).Location = new Point(229, 22);
		((Control)label_Interval).Name = "label_Interval";
		((Control)label_Interval).Size = new Size(111, 20);
		((Control)label_Interval).TabIndex = 21;
		((Control)label_Interval).Text = "间隔（10-255）";
		((Control)label_Tips).BackColor = Color.Transparent;
		((Control)label_Tips).Location = new Point(55, 74);
		((Control)label_Tips).Name = "label_Tips";
		((Control)label_Tips).Size = new Size(311, 60);
		((Control)label_Tips).TabIndex = 22;
		((Control)label_Tips).Text = "次数设置为0时，按下按键一直发，松开按键结束";
		((Control)Dlg_textBox_Times).Location = new Point(69, 45);
		((Control)Dlg_textBox_Times).Name = "Dlg_textBox_Times";
		((Control)Dlg_textBox_Times).Size = new Size(72, 26);
		((Control)Dlg_textBox_Times).TabIndex = 23;
		Dlg_textBox_Times.TextAlign = (HorizontalAlignment)2;
		((Control)Dlg_textBox_Times).TextChanged += textBox_Times_TextChanged;
		((Control)Dlg_textBox_Times).KeyPress += new KeyPressEventHandler(textBox_Times_KeyPress);
		((Control)Dlg_textBox_Interval).Location = new Point(238, 45);
		((Control)Dlg_textBox_Interval).Name = "Dlg_textBox_Interval";
		((Control)Dlg_textBox_Interval).Size = new Size(85, 26);
		((Control)Dlg_textBox_Interval).TabIndex = 24;
		Dlg_textBox_Interval.TextAlign = (HorizontalAlignment)2;
		((Control)Dlg_textBox_Interval).TextChanged += textBox_Interval_TextChanged;
		((Control)Dlg_textBox_Interval).KeyPress += new KeyPressEventHandler(textBox_Interval_KeyPress);
		((Control)customButton_Cancel).Location = new Point(241, 141);
		customButton_Cancel.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Cancel.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Cancel).Name = "customButton_Cancel";
		customButton_Cancel.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Cancel).Size = new Size(100, 26);
		((Control)customButton_Cancel).TabIndex = 26;
		((Control)customButton_Cancel).Text = "取消";
		((Control)customButton_Cancel).Click += customButton_Cancel_Click;
		((Control)customButton_Confirm).Location = new Point(66, 141);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(100, 26);
		((Control)customButton_Confirm).TabIndex = 25;
		((Control)customButton_Confirm).Text = "确定";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)customButton_Close).Location = new Point(380, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 19;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.设置界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(408, 177);
		((Control)this).Controls.Add((Control)(object)customButton_Cancel);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)Dlg_textBox_Interval);
		((Control)this).Controls.Add((Control)(object)Dlg_textBox_Times);
		((Control)this).Controls.Add((Control)(object)label_Tips);
		((Control)this).Controls.Add((Control)(object)label_Interval);
		((Control)this).Controls.Add((Control)(object)label_Times);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormFireKey";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormFireKey";
		((Control)this).KeyDown += new KeyEventHandler(FormFireKey_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
