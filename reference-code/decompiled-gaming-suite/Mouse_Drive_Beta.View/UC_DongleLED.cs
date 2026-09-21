using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_DongleLED : UserControl
{
	private bool updatingUI;

	private IContainer components;

	private Label label_OnlyBattery;

	private Label label_BatteryState;

	private Label label_ConnectionState;

	private Panel panel_Dongle4KLED;

	private CustomRadioButton customRadioButton_OnlyBattery;

	private CustomRadioButton customRadioButton_ConnectionState;

	private CustomRadioButton customRadioButton_BatteryState;

	private Label label_DongleLED;

	private PictureBox pictureBox_Title;

	public int DongleLed
	{
		get
		{
			return 0;
		}
		set
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			updatingUI = true;
			foreach (Control item in (ArrangedElementCollection)((Control)panel_Dongle4KLED).Controls)
			{
				Control val = item;
				if (val is CustomRadioButton)
				{
					CustomRadioButton customRadioButton = (CustomRadioButton)(object)val;
					if (int.Parse(((Control)customRadioButton).Tag.ToString()) == value)
					{
						((RadioButton)customRadioButton).Checked = true;
					}
				}
			}
			updatingUI = false;
		}
	}

	public UC_DongleLED()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		((UserControl)this)._002Ector();
		InitializeComponent();
		LanguageChange();
		foreach (Control item in (ArrangedElementCollection)((Control)panel_Dongle4KLED).Controls)
		{
			Control val = item;
			if (val is CustomRadioButton)
			{
				((RadioButton)(CustomRadioButton)(object)val).CheckedChanged += Button_CheckedChanged;
			}
		}
	}

	private void Button_CheckedChanged(object sender, EventArgs e)
	{
		CustomRadioButton customRadioButton = (CustomRadioButton)sender;
		if (((RadioButton)customRadioButton).Checked && !updatingUI && FormMain.GetDeviceOnlineFlag())
		{
			DongleRGB dongleRGB = new DongleRGB
			{
				mode = byte.Parse(((Control)customRadioButton).Tag.ToString())
			};
			UsbServer.Set4KDongleRGB(ref dongleRGB);
		}
	}

	public void LanguageChange()
	{
		((Control)label_DongleLED).Text = LanguageFile.Dialogs[77];
		((Control)label_ConnectionState).Text = LanguageFile.Dialogs[78];
		((Control)label_BatteryState).Text = LanguageFile.Dialogs[79];
		((Control)label_OnlyBattery).Text = LanguageFile.Dialogs[80];
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
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Expected O, but got Unknown
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		label_OnlyBattery = new Label();
		label_BatteryState = new Label();
		label_ConnectionState = new Label();
		panel_Dongle4KLED = new Panel();
		label_DongleLED = new Label();
		pictureBox_Title = new PictureBox();
		customRadioButton_OnlyBattery = new CustomRadioButton();
		customRadioButton_ConnectionState = new CustomRadioButton();
		customRadioButton_BatteryState = new CustomRadioButton();
		((Control)panel_Dongle4KLED).SuspendLayout();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_OnlyBattery).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_OnlyBattery).Location = new Point(73, 105);
		((Control)label_OnlyBattery).Name = "label_OnlyBattery";
		((Control)label_OnlyBattery).Size = new Size(726, 34);
		((Control)label_OnlyBattery).TabIndex = 95;
		((Control)label_OnlyBattery).Text = "仅电池警告（始终保持熄灭，仅鼠标低电量时红灯闪烁）";
		((Control)label_BatteryState).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_BatteryState).Location = new Point(76, 65);
		((Control)label_BatteryState).Name = "label_BatteryState";
		((Control)label_BatteryState).Size = new Size(723, 34);
		((Control)label_BatteryState).TabIndex = 94;
		((Control)label_BatteryState).Text = "电池状态（根据鼠标电池电量，分别指示：绿色>80%，黄色>60%，橙色>20%和红色<20%）";
		((Control)label_ConnectionState).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_ConnectionState).Location = new Point(76, 27);
		((Control)label_ConnectionState).Name = "label_ConnectionState";
		((Control)label_ConnectionState).Size = new Size(723, 34);
		((Control)label_ConnectionState).TabIndex = 93;
		((Control)label_ConnectionState).Text = "连接状态（1K白色常亮，4K混色常亮）";
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_OnlyBattery);
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_ConnectionState);
		((Control)panel_Dongle4KLED).Controls.Add((Control)(object)customRadioButton_BatteryState);
		((Control)panel_Dongle4KLED).Location = new Point(52, 27);
		((Control)panel_Dongle4KLED).Name = "panel_Dongle4KLED";
		((Control)panel_Dongle4KLED).Size = new Size(18, 112);
		((Control)panel_Dongle4KLED).TabIndex = 92;
		((Control)label_DongleLED).AutoSize = true;
		((Control)label_DongleLED).Location = new Point(49, 1);
		((Control)label_DongleLED).Name = "label_DongleLED";
		((Control)label_DongleLED).Size = new Size(119, 20);
		((Control)label_DongleLED).TabIndex = 91;
		((Control)label_DongleLED).Text = "接收器LED指示灯";
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 6);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 90;
		pictureBox_Title.TabStop = false;
		((RadioButton)customRadioButton_OnlyBattery).Appearance = (Appearance)1;
		customRadioButton_OnlyBattery.CheckImage = (Image)(object)Resources.单选按钮选择;
		customRadioButton_OnlyBattery.DisableColor = Color.FromArgb(57, 57, 57);
		customRadioButton_OnlyBattery.DisableImage = null;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.BorderSize = 0;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.CheckedBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)customRadioButton_OnlyBattery).FlatStyle = (FlatStyle)0;
		((Control)customRadioButton_OnlyBattery).Location = new Point(3, 80);
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
		((Control)customRadioButton_BatteryState).Location = new Point(3, 39);
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
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_DongleLED";
		((Control)this).Size = new Size(857, 159);
		((Control)panel_Dongle4KLED).ResumeLayout(false);
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
