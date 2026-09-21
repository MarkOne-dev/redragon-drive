using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_System : UserControl
{
	private IContainer components;

	private PictureBox pictureBox_WinMouse;

	private LinkLabel linkLabel_SystemMouse;

	private CustomCheckBox customCheckBox_RunPCBoot;

	private Label label_RunPCBoot;

	private PictureBox pictureBox_Title;

	private Label label_SystemSetting;

	private string systemStartPath => Environment.GetFolderPath(Environment.SpecialFolder.Startup);

	private string appAllPath => Process.GetCurrentProcess().MainModule.FileName;

	public UC_System()
	{
		InitializeComponent();
		LanguageChange();
		customCheckBox_RunPCBoot.Checked = RegeditManager.GetRunBoot(FormMain.driveParam.DriveName, "\"" + ((object)this).GetType().Assembly.Location + "\"");
	}

	private void pictureBox_WinMouse_Click(object sender, EventArgs e)
	{
		Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL main.cpl @0");
	}

	private void linkLabel_SystemMouse_Click(object sender, EventArgs e)
	{
		Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL main.cpl @0");
	}

	private void customCheckBox_RunPCBoot_CheckChange(object sender, CustomEventArgs e)
	{
		RegeditManager.SetRunBoot(customCheckBox_RunPCBoot.Checked, FormMain.driveParam.DriveName, "\"" + ((object)this).GetType().Assembly.Location + "\"");
	}

	public void LanguageChange()
	{
		((Control)linkLabel_SystemMouse).Text = LanguageFile.Dialogs[69];
		((Control)label_RunPCBoot).Text = LanguageFile.Dialogs[85];
		((Control)label_SystemSetting).Text = LanguageFile.Dialogs[83];
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		linkLabel_SystemMouse = new LinkLabel();
		label_RunPCBoot = new Label();
		label_SystemSetting = new Label();
		pictureBox_WinMouse = new PictureBox();
		customCheckBox_RunPCBoot = new CustomCheckBox();
		pictureBox_Title = new PictureBox();
		((ISupportInitialize)pictureBox_WinMouse).BeginInit();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)linkLabel_SystemMouse).AutoSize = true;
		((Control)linkLabel_SystemMouse).BackColor = Color.Transparent;
		linkLabel_SystemMouse.LinkColor = Color.White;
		((Control)linkLabel_SystemMouse).Location = new Point(44, 86);
		((Control)linkLabel_SystemMouse).Name = "linkLabel_SystemMouse";
		((Control)linkLabel_SystemMouse).Size = new Size(127, 20);
		((Control)linkLabel_SystemMouse).TabIndex = 122;
		linkLabel_SystemMouse.TabStop = true;
		((Control)linkLabel_SystemMouse).Text = "Windows鼠标属性";
		((Control)linkLabel_SystemMouse).Click += linkLabel_SystemMouse_Click;
		((Control)label_RunPCBoot).AutoSize = true;
		((Control)label_RunPCBoot).Location = new Point(57, 44);
		((Control)label_RunPCBoot).Name = "label_RunPCBoot";
		((Control)label_RunPCBoot).Size = new Size(93, 20);
		((Control)label_RunPCBoot).TabIndex = 120;
		((Control)label_RunPCBoot).Text = "开机自动启动";
		((Control)label_SystemSetting).AutoSize = true;
		((Control)label_SystemSetting).ForeColor = Color.White;
		((Control)label_SystemSetting).Location = new Point(57, 10);
		((Control)label_SystemSetting).Name = "label_SystemSetting";
		((Control)label_SystemSetting).Size = new Size(37, 20);
		((Control)label_SystemSetting).TabIndex = 118;
		((Control)label_SystemSetting).Text = "其他";
		((Control)pictureBox_WinMouse).BackColor = Color.Transparent;
		((Control)pictureBox_WinMouse).BackgroundImage = (Image)(object)Resources.win系统鼠标;
		((Control)pictureBox_WinMouse).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_WinMouse).Cursor = Cursors.Hand;
		((Control)pictureBox_WinMouse).Location = new Point(26, 85);
		((Control)pictureBox_WinMouse).Name = "pictureBox_WinMouse";
		((Control)pictureBox_WinMouse).Size = new Size(20, 20);
		pictureBox_WinMouse.TabIndex = 123;
		pictureBox_WinMouse.TabStop = false;
		((Control)pictureBox_WinMouse).Click += pictureBox_WinMouse_Click;
		customCheckBox_RunPCBoot.Checked = true;
		customCheckBox_RunPCBoot.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_RunPCBoot).Location = new Point(26, 46);
		((Control)customCheckBox_RunPCBoot).Name = "customCheckBox_RunPCBoot";
		((Control)customCheckBox_RunPCBoot).Size = new Size(16, 16);
		((Control)customCheckBox_RunPCBoot).TabIndex = 121;
		customCheckBox_RunPCBoot.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_RunPCBoot.CheckChange += customCheckBox_RunPCBoot_CheckChange;
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 16);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 119;
		pictureBox_Title.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox_WinMouse);
		((Control)this).Controls.Add((Control)(object)linkLabel_SystemMouse);
		((Control)this).Controls.Add((Control)(object)customCheckBox_RunPCBoot);
		((Control)this).Controls.Add((Control)(object)label_RunPCBoot);
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Controls.Add((Control)(object)label_SystemSetting);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_System";
		((Control)this).Size = new Size(857, 123);
		((ISupportInitialize)pictureBox_WinMouse).EndInit();
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
