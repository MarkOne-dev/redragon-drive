using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.Properties;
using Mouse_Drive_Beta.SkinFormLib;

namespace Mouse_Drive_Beta;

public class FormEditDialog : Form
{
	private FormResize formResize = new FormResize();

	public string text = "";

	public Keys keys;

	public int delay;

	public bool NeedUpdate;

	public string ComboString = "";

	public List<byte> ComboData = new List<byte>();

	private bool CheckBoxVisible;

	private bool InsertKey;

	private bool InsertDelay;

	private int time;

	private KeyboardHook keyboardHook = new KeyboardHook();

	public KeyboardCode keyboardCode = new KeyboardCode();

	private List<KeyboardCode> keyboardCodes = new List<KeyboardCode>();

	private IContainer components;

	private Label label1;

	private CustomButton customButton_Close;

	private TextBox Dlg_textBox1;

	private CustomCheckBox customCheckBox_Shift;

	private CustomCheckBox customCheckBox_Ctrl;

	private CustomCheckBox customCheckBox_Alt;

	private CustomCheckBox customCheckBox_Win;

	private Label label_Shift;

	private Label label_Ctrl;

	private Label label_Alt;

	private Label label_Win;

	private CustomButton customButton_Confirm;

	private CustomButton customButton_Cancel;

	public FormEditDialog(string str, string[] keys, FormEditDialogType type)
	{
		InitializeComponent();
		ImageInit();
		formResize.Resize((Form)(object)this);
		LanguageFile.FormSetColor((Control)(object)this);
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)label1).Text = str;
		switch (type)
		{
		case FormEditDialogType.MacroName:
			((Control)Dlg_textBox1).ImeMode = (ImeMode)1;
			break;
		case FormEditDialogType.KeyEvent:
			((TextBoxBase)Dlg_textBox1).MaxLength = 1;
			Dlg_textBox1.CharacterCasing = (CharacterCasing)1;
			InsertKey = true;
			break;
		case FormEditDialogType.Delay:
			InsertDelay = true;
			break;
		case FormEditDialogType.ComboKey:
			CheckBoxVisible = true;
			((TextBoxBase)Dlg_textBox1).MaxLength = 1;
			Dlg_textBox1.CharacterCasing = (CharacterCasing)1;
			InsertKey = true;
			break;
		}
		((Control)customCheckBox_Alt).Visible = CheckBoxVisible;
		((Control)label_Alt).Visible = CheckBoxVisible;
		((Control)customCheckBox_Ctrl).Visible = CheckBoxVisible;
		((Control)label_Ctrl).Visible = CheckBoxVisible;
		((Control)customCheckBox_Shift).Visible = CheckBoxVisible;
		((Control)label_Shift).Visible = CheckBoxVisible;
		((Control)customCheckBox_Win).Visible = CheckBoxVisible;
		((Control)label_Win).Visible = CheckBoxVisible;
		if (InsertKey)
		{
			keyboardHook.KeyDown += KeyboardHook_KeyDown;
			keyboardHook.KeyUp += KeyboardHook_KeyUp;
			keyboardHook.Start(Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]));
		}
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_011c: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Expected O, but got Unknown
		//IL_0161: Expected O, but got Unknown
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
		CustomCheckBox customCheckBox = customCheckBox_Alt;
		CustomCheckBox customCheckBox2 = customCheckBox_Ctrl;
		CustomCheckBox customCheckBox3 = customCheckBox_Shift;
		CustomCheckBox customCheckBox4 = customCheckBox_Win;
		Bitmap val4 = new Bitmap(baseDirectory + "\\res\\7General\\check_select.png");
		Image val5 = (Image)val4;
		customCheckBox4.CheckImage = (Image)val4;
		Image val6 = (customCheckBox3.CheckImage = val5);
		normalImage = (customCheckBox2.CheckImage = val6);
		customCheckBox.CheckImage = normalImage;
		CustomCheckBox customCheckBox5 = customCheckBox_Alt;
		CustomCheckBox customCheckBox6 = customCheckBox_Ctrl;
		CustomCheckBox customCheckBox7 = customCheckBox_Shift;
		CustomCheckBox customCheckBox8 = customCheckBox_Win;
		Bitmap val9 = new Bitmap(baseDirectory + "\\res\\7General\\check_unselect.png");
		val5 = (Image)val9;
		customCheckBox8.UncheckImage = (Image)val9;
		val6 = (customCheckBox7.UncheckImage = val5);
		normalImage = (customCheckBox6.UncheckImage = val6);
		customCheckBox5.UncheckImage = normalImage;
	}

	private void KeyboardHook_KeyDown(object sender, KeyEventArgs e, GlobalHook.KeyboardHookStruct hookStruct)
	{
		if (keyboardCode.vkCode != hookStruct.vkCode || keyboardCode.scanCode != hookStruct.scanCode || keyboardCode.flags != hookStruct.flags)
		{
			keyboardCode = KeyboardCodes.FindKeyboardCode(hookStruct);
			keyboardCode.flags = hookStruct.flags;
		}
		e.Handled = false;
	}

	private void KeyboardHook_KeyUp(object sender, KeyEventArgs e, GlobalHook.KeyboardHookStruct hookStruct)
	{
		if (keyboardCode.vkCode != hookStruct.vkCode || keyboardCode.scanCode != hookStruct.scanCode || keyboardCode.flags != hookStruct.flags)
		{
			keyboardCode = KeyboardCodes.FindKeyboardCode(hookStruct);
			keyboardCode.flags = hookStruct.flags;
			if ((CheckBoxVisible && keyboardCode.hidCodeType != HID_CODE_TYPE.Modify && keyboardCode.hidCodeType != HID_CODE_TYPE.Media) || (!CheckBoxVisible && keyboardCode.hidCodeType != HID_CODE_TYPE.Media))
			{
				((Control)Dlg_textBox1).Text = keyboardCode.keyChar;
			}
			else
			{
				keyboardCode.keyChar = "";
			}
			if (CheckBoxVisible)
			{
				if (hookStruct.time - time > 2000 || keyboardCodes.Count >= 3)
				{
					keyboardCodes.Clear();
				}
				time = hookStruct.time;
				((Control)Dlg_textBox1).Text = "";
				bool flag = false;
				for (int i = 0; i < keyboardCodes.Count; i++)
				{
					if (keyboardCode.keyChar == keyboardCodes[i].keyChar)
					{
						flag = true;
						break;
					}
				}
				if (!flag && keyboardCode.keyChar != "")
				{
					keyboardCodes.Add(keyboardCode);
				}
				for (int j = 0; j < keyboardCodes.Count; j++)
				{
					TextBox dlg_textBox = Dlg_textBox1;
					((Control)dlg_textBox).Text = ((Control)dlg_textBox).Text + keyboardCodes[j].keyChar;
					if (j < keyboardCodes.Count - 1)
					{
						TextBox dlg_textBox2 = Dlg_textBox1;
						((Control)dlg_textBox2).Text = ((Control)dlg_textBox2).Text + "+";
					}
				}
			}
			((TextBoxBase)Dlg_textBox1).SelectionStart = ((TextBoxBase)Dlg_textBox1).TextLength;
			((TextBoxBase)Dlg_textBox1).Select(((TextBoxBase)Dlg_textBox1).SelectionStart, 0);
		}
		e.Handled = false;
	}

	private void customButton_Close_Click(object sender, EventArgs e)
	{
		if (InsertKey)
		{
			keyboardHook.Stop();
		}
		((Form)this).Close();
	}

	private void customButton_Confirm_Click(object sender, EventArgs e)
	{
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		ComboString = "";
		ComboData.Clear();
		if (customCheckBox_Ctrl.Checked)
		{
			ComboData.Add(0);
			ComboData.Add(1);
			ComboData.Add(0);
			ComboString += "Ctrl";
		}
		if (customCheckBox_Shift.Checked)
		{
			if (ComboData.Count != 0)
			{
				ComboString += "+";
			}
			ComboString += "Shift";
			ComboData.Add(0);
			ComboData.Add(2);
			ComboData.Add(0);
		}
		if (customCheckBox_Alt.Checked)
		{
			if (ComboData.Count != 0)
			{
				ComboString += "+";
			}
			ComboString += "Alt";
			ComboData.Add(0);
			ComboData.Add(4);
			ComboData.Add(0);
		}
		if (customCheckBox_Win.Checked)
		{
			if (ComboData.Count != 0)
			{
				ComboString += "+";
			}
			ComboString += "Win";
			ComboData.Add(0);
			ComboData.Add(8);
			ComboData.Add(0);
		}
		if (ComboData.Count != 0 && ((Control)Dlg_textBox1).Text != "")
		{
			ComboString = ComboString + "+" + ((Control)Dlg_textBox1).Text;
		}
		else
		{
			ComboString += ((Control)Dlg_textBox1).Text;
		}
		for (int i = 0; i < keyboardCodes.Count; i++)
		{
			ComboData.Add(1);
			ComboData.Add((byte)keyboardCodes[i].hidCode);
			ComboData.Add(0);
		}
		if (ComboData.Count > 9)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[7])).ShowDialog();
			return;
		}
		if (InsertDelay)
		{
			if (((Control)Dlg_textBox1).Text == "")
			{
				((Control)Dlg_textBox1).Text = 10.ToString();
			}
			delay = Convert.ToInt32(((Control)Dlg_textBox1).Text);
			delay = ((delay < 10) ? 10 : delay);
			text = delay.ToString();
		}
		else
		{
			text = ((Control)Dlg_textBox1).Text;
		}
		if (text != "" || ComboData.Count > 0)
		{
			NeedUpdate = true;
		}
		if (InsertKey)
		{
			keyboardHook.Stop();
		}
		((Form)this).Close();
	}

	private void customButton_Cancel_Click(object sender, EventArgs e)
	{
		if (InsertKey)
		{
			keyboardHook.Stop();
		}
		((Form)this).Close();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		if (InsertDelay)
		{
			if (!(((Control)Dlg_textBox1).Text == "") && int.Parse(((Control)Dlg_textBox1).Text) > 65535)
			{
				((Control)Dlg_textBox1).Text = 65535.ToString();
			}
			return;
		}
		byte[] bytes = Encoding.Default.GetBytes(((Control)Dlg_textBox1).Text);
		if (bytes.Length > 30)
		{
			((Control)Dlg_textBox1).Text = Encoding.Default.GetString(bytes, 0, 30);
		}
		((Control)Dlg_textBox1).Focus();
		((TextBoxBase)Dlg_textBox1).Select(((TextBoxBase)Dlg_textBox1).TextLength, 0);
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (InsertDelay && e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void FormEditDialog_KeyDown(object sender, KeyEventArgs e)
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_096a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Expected O, but got Unknown
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FormEditDialog));
		label1 = new Label();
		customButton_Close = new CustomButton();
		Dlg_textBox1 = new TextBox();
		customCheckBox_Shift = new CustomCheckBox();
		customCheckBox_Ctrl = new CustomCheckBox();
		customCheckBox_Alt = new CustomCheckBox();
		customCheckBox_Win = new CustomCheckBox();
		label_Shift = new Label();
		label_Ctrl = new Label();
		label_Alt = new Label();
		label_Win = new Label();
		customButton_Confirm = new CustomButton();
		customButton_Cancel = new CustomButton();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).BackColor = Color.Transparent;
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(32, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(50, 20);
		((Control)label1).TabIndex = 17;
		((Control)label1).Text = "label1";
		((Control)customButton_Close).Location = new Point(274, 12);
		customButton_Close.MouseDownImage = (Image)(object)Resources.设置关闭按键按下;
		customButton_Close.MouseEnterImage = (Image)(object)Resources.设置关闭按键鼠标进入;
		((Control)customButton_Close).Name = "customButton_Close";
		customButton_Close.NormalImage = (Image)(object)Resources.设置关闭按键;
		((Control)customButton_Close).Size = new Size(14, 14);
		((Control)customButton_Close).TabIndex = 18;
		((Control)customButton_Close).Click += customButton_Close_Click;
		((Control)Dlg_textBox1).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)Dlg_textBox1).ImeMode = (ImeMode)11;
		((Control)Dlg_textBox1).Location = new Point(36, 32);
		((Control)Dlg_textBox1).Name = "Dlg_textBox1";
		((Control)Dlg_textBox1).Size = new Size(234, 23);
		((Control)Dlg_textBox1).TabIndex = 0;
		Dlg_textBox1.TextAlign = (HorizontalAlignment)2;
		((Control)Dlg_textBox1).TextChanged += textBox1_TextChanged;
		((Control)Dlg_textBox1).KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
		customCheckBox_Shift.Checked = false;
		customCheckBox_Shift.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_Shift).Location = new Point(36, 59);
		((Control)customCheckBox_Shift).Name = "customCheckBox_Shift";
		((Control)customCheckBox_Shift).Size = new Size(16, 16);
		((Control)customCheckBox_Shift).TabIndex = 66;
		customCheckBox_Shift.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_Ctrl.Checked = false;
		customCheckBox_Ctrl.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_Ctrl).Location = new Point(96, 59);
		((Control)customCheckBox_Ctrl).Name = "customCheckBox_Ctrl";
		((Control)customCheckBox_Ctrl).Size = new Size(16, 16);
		((Control)customCheckBox_Ctrl).TabIndex = 67;
		customCheckBox_Ctrl.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_Alt.Checked = false;
		customCheckBox_Alt.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_Alt).Location = new Point(160, 59);
		((Control)customCheckBox_Alt).Name = "customCheckBox_Alt";
		((Control)customCheckBox_Alt).Size = new Size(16, 16);
		((Control)customCheckBox_Alt).TabIndex = 68;
		customCheckBox_Alt.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_Win.Checked = false;
		customCheckBox_Win.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_Win).Location = new Point(218, 59);
		((Control)customCheckBox_Win).Name = "customCheckBox_Win";
		((Control)customCheckBox_Win).Size = new Size(16, 16);
		((Control)customCheckBox_Win).TabIndex = 69;
		customCheckBox_Win.UncheckImage = (Image)(object)Resources.单选框未选择;
		((Control)label_Shift).AutoSize = true;
		((Control)label_Shift).BackColor = Color.Transparent;
		((Control)label_Shift).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Shift).ForeColor = Color.White;
		((Control)label_Shift).Location = new Point(52, 59);
		((Control)label_Shift).Name = "label_Shift";
		((Control)label_Shift).Size = new Size(32, 17);
		((Control)label_Shift).TabIndex = 70;
		((Control)label_Shift).Text = "shift";
		((Control)label_Ctrl).AutoSize = true;
		((Control)label_Ctrl).BackColor = Color.Transparent;
		((Control)label_Ctrl).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Ctrl).ForeColor = Color.White;
		((Control)label_Ctrl).Location = new Point(118, 58);
		((Control)label_Ctrl).Name = "label_Ctrl";
		((Control)label_Ctrl).Size = new Size(28, 17);
		((Control)label_Ctrl).TabIndex = 71;
		((Control)label_Ctrl).Text = "Ctrl";
		((Control)label_Alt).AutoSize = true;
		((Control)label_Alt).BackColor = Color.Transparent;
		((Control)label_Alt).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Alt).ForeColor = Color.White;
		((Control)label_Alt).Location = new Point(182, 59);
		((Control)label_Alt).Name = "label_Alt";
		((Control)label_Alt).Size = new Size(23, 17);
		((Control)label_Alt).TabIndex = 72;
		((Control)label_Alt).Text = "Alt";
		((Control)label_Win).AutoSize = true;
		((Control)label_Win).BackColor = Color.Transparent;
		((Control)label_Win).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Win).ForeColor = Color.White;
		((Control)label_Win).Location = new Point(240, 59);
		((Control)label_Win).Name = "label_Win";
		((Control)label_Win).Size = new Size(30, 17);
		((Control)label_Win).TabIndex = 73;
		((Control)label_Win).Text = "Win";
		((Control)customButton_Confirm).ForeColor = Color.White;
		((Control)customButton_Confirm).Location = new Point(40, 78);
		customButton_Confirm.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Confirm.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Confirm).Name = "customButton_Confirm";
		customButton_Confirm.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Confirm).Size = new Size(100, 26);
		((Control)customButton_Confirm).TabIndex = 74;
		((Control)customButton_Confirm).Text = "确认";
		((Control)customButton_Confirm).Click += customButton_Confirm_Click;
		((Control)customButton_Cancel).ForeColor = Color.White;
		((Control)customButton_Cancel).Location = new Point(160, 78);
		customButton_Cancel.MouseDownImage = (Image)(object)Resources.恢复默认按键按下;
		customButton_Cancel.MouseEnterImage = (Image)(object)Resources.恢复默认按键鼠标进入;
		((Control)customButton_Cancel).Name = "customButton_Cancel";
		customButton_Cancel.NormalImage = (Image)(object)Resources.恢复默认按键;
		((Control)customButton_Cancel).Size = new Size(100, 26);
		((Control)customButton_Cancel).TabIndex = 75;
		((Control)customButton_Cancel).Text = "取消";
		((Control)customButton_Cancel).Click += customButton_Cancel_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)(object)Resources.消息窗背景;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(300, 110);
		((Control)this).Controls.Add((Control)(object)customButton_Cancel);
		((Control)this).Controls.Add((Control)(object)customButton_Confirm);
		((Control)this).Controls.Add((Control)(object)label_Win);
		((Control)this).Controls.Add((Control)(object)label_Alt);
		((Control)this).Controls.Add((Control)(object)label_Ctrl);
		((Control)this).Controls.Add((Control)(object)label_Shift);
		((Control)this).Controls.Add((Control)(object)customCheckBox_Win);
		((Control)this).Controls.Add((Control)(object)customCheckBox_Alt);
		((Control)this).Controls.Add((Control)(object)customCheckBox_Ctrl);
		((Control)this).Controls.Add((Control)(object)customCheckBox_Shift);
		((Control)this).Controls.Add((Control)(object)Dlg_textBox1);
		((Control)this).Controls.Add((Control)(object)customButton_Close);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).DoubleBuffered = true;
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Form)this).KeyPreview = true;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "FormEditDialog";
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "FormEditDialog";
		((Control)this).KeyDown += new KeyEventHandler(FormEditDialog_KeyDown);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
