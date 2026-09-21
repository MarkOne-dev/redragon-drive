using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormMoveXY : Form
{
	private FormResize formResize = new FormResize();

	public bool NeedUpdate;

	public uint x;

	public uint y;

	public string xString;

	public string yString;

	private IContainer components;

	private Label label_X;

	private Label label_Y;

	private TextBox Dlg_textBox_X;

	private TextBox Dlg_textBox_Y;

	private CustomRadioButton customRadioButton_Left;

	private Label label_Left;

	private Label label_Right;

	private CustomRadioButton customRadioButton_Right;

	private Label label_Up;

	private CustomRadioButton customRadioButton_Up;

	private Label label_Down;

	private CustomRadioButton customRadioButton_Down;

	private CustomButton customButton_Close;

	private CustomButton customButton_Cancel;

	private CustomButton customButton_Confirm;

	private Panel panel_X;

	private Panel panel_Y;

	public FormMoveXY(string[] text, string[] keys)
	{
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Control)label_Up).Text = text[0];
		((Control)label_Down).Text = text[1];
		((Control)label_Left).Text = text[2];
		((Control)label_Right).Text = text[3];
		((Control)customButton_Confirm).Text = keys[0];
		((Control)customButton_Cancel).Text = keys[1];
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

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		if (((Control)Dlg_textBox_X).Text == "")
		{
			((Control)Dlg_textBox_X).Text = 0.ToString();
		}
		if (((RadioButton)customRadioButton_Left).Checked)
		{
			xString = ((Control)label_Left).Text;
			x = (uint)(-Convert.ToInt32(((Control)Dlg_textBox_X).Text));
		}
		else if (((RadioButton)customRadioButton_Right).Checked)
		{
			xString = ((Control)label_Right).Text;
			x = (uint)Convert.ToInt32(((Control)Dlg_textBox_X).Text);
		}
		if (((Control)Dlg_textBox_Y).Text == "")
		{
			((Control)Dlg_textBox_Y).Text = 0.ToString();
		}
		if (((RadioButton)customRadioButton_Up).Checked)
		{
			yString = ((Control)label_Up).Text;
			y = (uint)(-Convert.ToInt32(((Control)Dlg_textBox_Y).Text));
		}
		else if (((RadioButton)customRadioButton_Down).Checked)
		{
			yString = ((Control)label_Down).Text;
			y = (uint)Convert.ToInt32(((Control)Dlg_textBox_Y).Text);
		}
		if (x == 0 && y == 0)
		{
			NeedUpdate = false;
		}
		else
		{
			NeedUpdate = true;
		}
		((Form)this).Close();
	}

	private void customButton_Cancel_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void textBox_X_TextChanged(object sender, EventArgs e)
	{
		if (((Control)Dlg_textBox_X).Text != "" && int.Parse(((Control)Dlg_textBox_X).Text) > 127)
		{
			((Control)Dlg_textBox_X).Text = 127.ToString();
		}
	}

	private void textBox_X_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void textBox_Y_TextChanged(object sender, EventArgs e)
	{
		if (((Control)Dlg_textBox_Y).Text != "" && int.Parse(((Control)Dlg_textBox_Y).Text) > 127)
		{
			((Control)Dlg_textBox_Y).Text = 127.ToString();
		}
	}

	private void textBox_Y_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Expected O, but got Unknown
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Expected O, but got Unknown
		//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e90: Expected O, but got Unknown
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Expected O, but got Unknown
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormMoveXY));
		label_X = new Label();
		label_Y = new Label();
		Dlg_textBox_X = new TextBox();
		Dlg_textBox_Y = new TextBox();
		customRadioButton_Left = new CustomRadioButton();
		label_Left = new Label();
		label_Right = new Label();
		customRadioButton_Right = new CustomRadioButton();
		label_Up = new Label();
		customRadioButton_Up = new CustomRadioButton();
		label_Down = new Label();
		customRadioButton_Down = new CustomRadioButton();
		customButton_Close = new CustomButton();
		customButton_Cancel = new CustomButton();
		customButton_Confirm = new CustomButton();
		panel_X = new Panel();
		panel_Y = new Panel();
		((Control)panel_X).SuspendLayout();
		((Control)panel_Y).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)label_X).AutoSize = true;
		((Control)label_X).BackColor = Color.Transparent;
		((Control)label_X).Font = new Font("微软雅黑", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_X).Location = new Point(3, 29);
		((Control)label_X).Name = "label_X";
		((Control)label_X).Size = new Size(31, 28);
		((Control)label_X).TabIndex = 0;
		((Control)label_X).Text = "X:";
		((Control)label_Y).AutoSize = true;
		((Control)label_Y).BackColor = Color.Transparent;
		((Control)label_Y).Font = new Font("微软雅黑", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Y).Location = new Point(3, 28);
		((Control)label_Y).Name = "label_Y";
		((Control)label_Y).Size = new Size(30, 28);
		((Control)label_Y).TabIndex = 1;
		((Control)label_Y).Text = "Y:";
		((Control)Dlg_textBox_X).Location = new Point(42, 29);
		((Control)Dlg_textBox_X).Name = "Dlg_textBox_X";
		((Control)Dlg_textBox_X).Size = new Size(179, 26);
		((Control)Dlg_textBox_X).TabIndex = 2;
		((Control)Dlg_textBox_X).Text = "0";
		Dlg_textBox_X.TextAlign = (HorizontalAlignment)2;
		((Control)Dlg_textBox_X).TextChanged += textBox_X_TextChanged;
		((Control)Dlg_textBox_X).KeyPress += new KeyPressEventHandler(textBox_X_KeyPress);
		((Control)Dlg_textBox_Y).Location = new Point(39, 28);
		((Control)Dlg_textBox_Y).Name = "Dlg_textBox_Y";
		((Control)Dlg_textBox_Y).Size = new Size(186, 26);
		((Control)Dlg_textBox_Y).TabIndex = 3;
		((Control)Dlg_textBox_Y).Text = "0";
		Dlg_textBox_Y.TextAlign = (HorizontalAlignment)2;
		((Control)Dlg_textBox_Y).TextChanged += textBox_Y_TextChanged;
		((Control)Dlg_textBox_Y).KeyPress += new KeyPressEventHandler(textBox_Y_KeyPress);
		((RadioButton)customRadioButton_Left).Appearance = (Appearance)1;
		((Control)customRadioButton_Left).BackColor = Color.Transparent;
		((RadioButton)customRadioButton_Left).Checked = true;
		customRadioButton_Left.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_Left.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Left.DisableImage = null;
		((ButtonBase)customRadioButton_Left).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Left).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Left).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Left).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Left).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Left).Location = new Point(42, 10);
		customRadioButton_Left.MouseEnterImage = null;
		((Control)customRadioButton_Left).Name = "customRadioButton_Left";
		((Control)customRadioButton_Left).Size = new Size(12, 12);
		((Control)customRadioButton_Left).TabIndex = 4;
		((RadioButton)customRadioButton_Left).TabStop = true;
		customRadioButton_Left.TextString = null;
		customRadioButton_Left.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_Left).UseVisualStyleBackColor = false;
		((Control)label_Left).AutoSize = true;
		((Control)label_Left).BackColor = Color.Transparent;
		((Control)label_Left).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Left).Location = new Point(56, 6);
		((Control)label_Left).Name = "label_Left";
		((Control)label_Left).Size = new Size(32, 17);
		((Control)label_Left).TabIndex = 5;
		((Control)label_Left).Text = "左移";
		((Control)label_Right).AutoSize = true;
		((Control)label_Right).BackColor = Color.Transparent;
		((Control)label_Right).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Right).Location = new Point(136, 6);
		((Control)label_Right).Name = "label_Right";
		((Control)label_Right).Size = new Size(32, 17);
		((Control)label_Right).TabIndex = 7;
		((Control)label_Right).Text = "右移";
		((RadioButton)customRadioButton_Right).Appearance = (Appearance)1;
		((Control)customRadioButton_Right).BackColor = Color.Transparent;
		customRadioButton_Right.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_Right.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Right.DisableImage = null;
		((ButtonBase)customRadioButton_Right).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Right).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Right).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Right).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Right).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Right).Location = new Point(122, 10);
		customRadioButton_Right.MouseEnterImage = null;
		((Control)customRadioButton_Right).Name = "customRadioButton_Right";
		((Control)customRadioButton_Right).Size = new Size(12, 12);
		((Control)customRadioButton_Right).TabIndex = 6;
		customRadioButton_Right.TextString = null;
		customRadioButton_Right.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_Right).UseVisualStyleBackColor = false;
		((Control)label_Up).AutoSize = true;
		((Control)label_Up).BackColor = Color.Transparent;
		((Control)label_Up).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Up).Location = new Point(53, 6);
		((Control)label_Up).Name = "label_Up";
		((Control)label_Up).Size = new Size(32, 17);
		((Control)label_Up).TabIndex = 9;
		((Control)label_Up).Text = "上移";
		((RadioButton)customRadioButton_Up).Appearance = (Appearance)1;
		((Control)customRadioButton_Up).BackColor = Color.Transparent;
		((RadioButton)customRadioButton_Up).Checked = true;
		customRadioButton_Up.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_Up.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Up.DisableImage = null;
		((ButtonBase)customRadioButton_Up).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Up).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Up).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Up).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Up).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Up).Location = new Point(39, 10);
		customRadioButton_Up.MouseEnterImage = null;
		((Control)customRadioButton_Up).Name = "customRadioButton_Up";
		((Control)customRadioButton_Up).Size = new Size(12, 12);
		((Control)customRadioButton_Up).TabIndex = 8;
		((RadioButton)customRadioButton_Up).TabStop = true;
		customRadioButton_Up.TextString = null;
		customRadioButton_Up.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_Up).UseVisualStyleBackColor = false;
		((Control)label_Down).AutoSize = true;
		((Control)label_Down).BackColor = Color.Transparent;
		((Control)label_Down).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Down).Location = new Point(138, 6);
		((Control)label_Down).Name = "label_Down";
		((Control)label_Down).Size = new Size(32, 17);
		((Control)label_Down).TabIndex = 11;
		((Control)label_Down).Text = "下移";
		((RadioButton)customRadioButton_Down).Appearance = (Appearance)1;
		((Control)customRadioButton_Down).BackColor = Color.Transparent;
		customRadioButton_Down.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_Down.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_Down.DisableImage = null;
		((ButtonBase)customRadioButton_Down).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_Down).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Down).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Down).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_Down).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_Down).Location = new Point(124, 10);
		customRadioButton_Down.MouseEnterImage = null;
		((Control)customRadioButton_Down).Name = "customRadioButton_Down";
		((Control)customRadioButton_Down).Size = new Size(12, 12);
		((Control)customRadioButton_Down).TabIndex = 10;
		customRadioButton_Down.TextString = null;
		customRadioButton_Down.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_Down).UseVisualStyleBackColor = false;
		((Control)customButton_Close).Location = new Point(453, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 19;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)customButton_Cancel).ForeColor = Color.White;
		((Control)customButton_Cancel).Location = new Point(239, 131);
		customButton_Cancel.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Cancel.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Cancel).Name = "customButton_Cancel";
		customButton_Cancel.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Cancel).Size = new Size(78, 26);
		((Control)customButton_Cancel).TabIndex = 77;
		((Control)customButton_Cancel).Text = "取消";
		((Control)customButton_Cancel).Click += customButton_Cancel_Click;
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(120, 131);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(78, 26);
		((Control)customButton_Confirm).TabIndex = 76;
		((Control)customButton_Confirm).Text = "确认";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)panel_X).BackColor = Color.Transparent;
		((Control)panel_X).Controls.Add((Control)(object)Dlg_textBox_X);
		((Control)panel_X).Controls.Add((Control)(object)customRadioButton_Left);
		((Control)panel_X).Controls.Add((Control)(object)label_Left);
		((Control)panel_X).Controls.Add((Control)(object)customRadioButton_Right);
		((Control)panel_X).Controls.Add((Control)(object)label_Right);
		((Control)panel_X).Controls.Add((Control)(object)label_X);
		((Control)panel_X).Location = new Point(12, 46);
		((Control)panel_X).Name = "panel_X";
		((Control)panel_X).Size = new Size(224, 65);
		((Control)panel_X).TabIndex = 78;
		((Control)panel_Y).BackColor = Color.Transparent;
		((Control)panel_Y).Controls.Add((Control)(object)label_Up);
		((Control)panel_Y).Controls.Add((Control)(object)Dlg_textBox_Y);
		((Control)panel_Y).Controls.Add((Control)(object)customRadioButton_Up);
		((Control)panel_Y).Controls.Add((Control)(object)customRadioButton_Down);
		((Control)panel_Y).Controls.Add((Control)(object)label_Down);
		((Control)panel_Y).Controls.Add((Control)(object)label_Y);
		((Control)panel_Y).Location = new Point(239, 46);
		((Control)panel_Y).Name = "panel_Y";
		((Control)panel_Y).Size = new Size(228, 65);
		((Control)panel_Y).TabIndex = 79;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.设置界面背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(479, 183);
		((Control)this).Controls.Add((Control)(object)panel_Y);
		((Control)this).Controls.Add((Control)(object)panel_X);
		((Control)this).Controls.Add((Control)(object)customButton_Cancel);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormMoveXY";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormMoveXY";
		((Control)panel_X).ResumeLayout(false);
		((Control)panel_X).PerformLayout();
		((Control)panel_Y).ResumeLayout(false);
		((Control)panel_Y).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
