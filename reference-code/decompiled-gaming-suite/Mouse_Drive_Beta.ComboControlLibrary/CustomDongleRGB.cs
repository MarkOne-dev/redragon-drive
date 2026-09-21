using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class CustomDongleRGB : UserControl
{
	public delegate void LEDChangeEventHandler(object sender, int value);

	private IContainer components;

	private PictureBox pictureBox1;

	private Label label_DongleLED;

	private Panel panel_Dongle4KLED;

	private CustomRadioButton customRadioButton_OnlyBattery;

	private CustomRadioButton customRadioButton_ConnectionState;

	private CustomRadioButton customRadioButton_BatteryState;

	private Label label_ConnectionState;

	private Label label_BatteryState;

	private Label label_OnlyBattery;

	public byte[] Data { get; set; }

	public Image CheckImage
	{
		get
		{
			return customRadioButton_BatteryState.CheckImage;
		}
		set
		{
			customRadioButton_BatteryState.CheckImage = value;
			customRadioButton_ConnectionState.CheckImage = value;
			customRadioButton_OnlyBattery.CheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image UncheckImage
	{
		get
		{
			return customRadioButton_BatteryState.UncheckImage;
		}
		set
		{
			customRadioButton_OnlyBattery.UncheckImage = value;
			customRadioButton_ConnectionState.UncheckImage = value;
			customRadioButton_BatteryState.UncheckImage = value;
			((Control)this).Invalidate();
		}
	}

	public event LEDChangeEventHandler LEDChange;

	public CustomDongleRGB(byte[] value)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		((UserControl)this)._002Ector();
		InitializeComponent();
		Data = new byte[value.Length];
		for (int i = 0; i < value.Length; i++)
		{
			Data[i] = value[i];
		}
		((Control)label_DongleLED).Text = LanguageFile.Dialogs[77];
		((Control)label_ConnectionState).Text = LanguageFile.Dialogs[78];
		((Control)label_BatteryState).Text = LanguageFile.Dialogs[79];
		((Control)label_OnlyBattery).Text = LanguageFile.Dialogs[80];
		foreach (Control item in (ArrangedElementCollection)((Control)panel_Dongle4KLED).Controls)
		{
			Control val = item;
			if (val is CustomRadioButton)
			{
				CustomRadioButton customRadioButton = (CustomRadioButton)(object)val;
				((RadioButton)customRadioButton).CheckedChanged += Button_CheckedChanged;
				if (int.Parse(((Control)customRadioButton).Tag.ToString()) == value[0])
				{
					((RadioButton)customRadioButton).Checked = true;
				}
			}
		}
	}

	private void Button_CheckedChanged(object sender, EventArgs e)
	{
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		if (((RadioButton)customRadioButton).Checked)
		{
			LEDChange?.Invoke(this, int.Parse(((Control)customRadioButton).Tag.ToString()));
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
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		pictureBox1 = new PictureBox();
		label_DongleLED = new Label();
		panel_Dongle4KLED = new Panel();
		label_ConnectionState = new Label();
		label_BatteryState = new Label();
		label_OnlyBattery = new Label();
		customRadioButton_OnlyBattery = new CustomRadioButton();
		customRadioButton_ConnectionState = new CustomRadioButton();
		customRadioButton_BatteryState = new CustomRadioButton();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)panel_Dongle4KLED).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)pictureBox1).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox1).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox1).Location = new Point(12, 3);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(33, 20);
		pictureBox1.TabIndex = 8;
		pictureBox1.TabStop = false;
		((Control)label_DongleLED).AutoSize = true;
		((Control)label_DongleLED).Location = new Point(56, 5);
		((Control)label_DongleLED).Name = "label_DongleLED";
		((Control)label_DongleLED).Size = new Size(119, 20);
		((Control)label_DongleLED).TabIndex = 9;
		((Control)label_DongleLED).Text = "接收器LED指示灯";
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_OnlyBattery);
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_ConnectionState);
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_BatteryState);
		((Control)panel_Dongle4KLED).Location = new Point(60, 37);
		((Control)panel_Dongle4KLED).Name = "panel_Dongle4KLED";
		((Control)panel_Dongle4KLED).Size = new Size(18, 129);
		((Control)panel_Dongle4KLED).TabIndex = 86;
		((Control)label_ConnectionState).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_ConnectionState).Location = new Point(84, 37);
		((Control)label_ConnectionState).Name = "label_ConnectionState";
		((Control)label_ConnectionState).Size = new Size(472, 51);
		((Control)label_ConnectionState).TabIndex = 87;
		((Control)label_ConnectionState).Text = "连接状态（1K白色常亮，4K混色常亮）";
		((Control)label_BatteryState).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_BatteryState).Location = new Point(84, 97);
		((Control)label_BatteryState).Name = "label_BatteryState";
		((Control)label_BatteryState).Size = new Size(472, 34);
		((Control)label_BatteryState).TabIndex = 88;
		((Control)label_BatteryState).Text = "电池状态（根据鼠标电池电量，分别指示：绿色>80%，黄色>60%，橙色>20%和红色<20%）";
		((Control)label_OnlyBattery).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_OnlyBattery).Location = new Point(81, 143);
		((Control)label_OnlyBattery).Name = "label_OnlyBattery";
		((Control)label_OnlyBattery).Size = new Size(475, 34);
		((Control)label_OnlyBattery).TabIndex = 89;
		((Control)label_OnlyBattery).Text = "仅电池警告（始终保持熄灭，仅鼠标低电量时红灯闪烁）";
		((RadioButton)customRadioButton_OnlyBattery).Appearance = (Appearance)1;
		customRadioButton_OnlyBattery.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_OnlyBattery.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_OnlyBattery.DisableImage = null;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_OnlyBattery).Location = new Point(3, 107);
		customRadioButton_OnlyBattery.MouseEnterImage = null;
		((Control)customRadioButton_OnlyBattery).Name = "customRadioButton_OnlyBattery";
		((Control)customRadioButton_OnlyBattery).Size = new Size(12, 12);
		((Control)customRadioButton_OnlyBattery).TabIndex = 75;
		((RadioButton)customRadioButton_OnlyBattery).TabStop = true;
		((Control)customRadioButton_OnlyBattery).Tag = "3";
		customRadioButton_OnlyBattery.TextString = null;
		customRadioButton_OnlyBattery.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_OnlyBattery).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_ConnectionState).Appearance = (Appearance)1;
		customRadioButton_ConnectionState.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_ConnectionState.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_ConnectionState.DisableImage = null;
		((ButtonBase)customRadioButton_ConnectionState).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_ConnectionState).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_ConnectionState).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_ConnectionState).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_ConnectionState).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_ConnectionState).Location = new Point(3, 6);
		customRadioButton_ConnectionState.MouseEnterImage = null;
		((Control)customRadioButton_ConnectionState).Name = "customRadioButton_ConnectionState";
		((Control)customRadioButton_ConnectionState).Size = new Size(12, 12);
		((Control)customRadioButton_ConnectionState).TabIndex = 72;
		((RadioButton)customRadioButton_ConnectionState).TabStop = true;
		((Control)customRadioButton_ConnectionState).Tag = "1";
		customRadioButton_ConnectionState.TextString = null;
		customRadioButton_ConnectionState.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_ConnectionState).UseVisualStyleBackColor = true;
		((RadioButton)customRadioButton_BatteryState).Appearance = (Appearance)1;
		customRadioButton_BatteryState.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_BatteryState.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_BatteryState.DisableImage = null;
		((ButtonBase)customRadioButton_BatteryState).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_BatteryState).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_BatteryState).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_BatteryState).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_BatteryState).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_BatteryState).Location = new Point(3, 61);
		customRadioButton_BatteryState.MouseEnterImage = null;
		((Control)customRadioButton_BatteryState).Name = "customRadioButton_BatteryState";
		((Control)customRadioButton_BatteryState).Size = new Size(12, 12);
		((Control)customRadioButton_BatteryState).TabIndex = 73;
		((RadioButton)customRadioButton_BatteryState).TabStop = true;
		((Control)customRadioButton_BatteryState).Tag = "2";
		customRadioButton_BatteryState.TextString = null;
		customRadioButton_BatteryState.UncheckImage = (Image)(object)Resources.单选按钮未选择;
		((ButtonBase)customRadioButton_BatteryState).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)label_OnlyBattery);
		((Control)this).Controls.Add((Control)(object)label_BatteryState);
		((Control)this).Controls.Add((Control)(object)label_ConnectionState);
		((Control)this).Controls.Add((Control)(object)panel_Dongle4KLED);
		((Control)this).Controls.Add((Control)(object)label_DongleLED);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "CustomDongleRGB";
		((Control)this).Size = new Size(600, 207);
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)panel_Dongle4KLED).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
