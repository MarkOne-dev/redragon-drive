using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_Pair : UserControl
{
	public delegate void ExitPairingEventHandler(object sender);

	private FormPair formPair;

	private IContainer components;

	private PictureBox pictureBox_Title;

	private CustomButton customButton_Pairing;

	private Label label_PairTool;

	public event ExitPairingEventHandler ExitPairing;

	public UC_Pair()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		((UserControl)this)._002Ector();
		InitializeComponent();
		string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
		customButton_Pairing.NormalImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		customButton_Pairing.MouseDownImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		customButton_Pairing.MouseEnterImage = (Image)new Bitmap(baseDirectory + "\\res\\6Setting\\pair_tool.png");
		((Control)customButton_Pairing).ForeColor = FormMain.driveParam.BtnForeClr;
		LanguageChange();
	}

	private void customButton_Pairing_Click(object sender, EventArgs e)
	{
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		DeviceAllInfo selectedDeviceInfo = FormMain.SelectedDeviceInfo;
		bool flag = true;
		bool flag2 = false;
		string mcu = FormMain.driveParam.DeviceParams[selectedDeviceInfo.deviceIndex].DM;
		int num = 1;
		string mouseVersion = RegeditManager.GetMouseVersion(selectedDeviceInfo.deviceInfo.CID, selectedDeviceInfo.deviceInfo.MID);
		bool flag3 = false;
		if (mouseVersion != null)
		{
			flag3 = DeviceUpdateFile.HasNewVersion(210, mouseVersion, FormMain.driveParam.DeviceParams[selectedDeviceInfo.deviceIndex].MM, selectedDeviceInfo.deviceInfo.CID, selectedDeviceInfo.deviceInfo.MID);
			DeviceType deviceType = (DeviceType)selectedDeviceInfo.deviceInfo.DeviceType;
			if (deviceType.ToString().Contains("Wireless"))
			{
				if (deviceType.ToString().Contains("2K"))
				{
					num = 2;
					mcu = FormMain.driveParam.DeviceParams[selectedDeviceInfo.deviceIndex].D2M;
				}
				if (deviceType.ToString().Contains("4K"))
				{
					num = 4;
					mcu = FormMain.driveParam.DeviceParams[selectedDeviceInfo.deviceIndex].D4M;
				}
			}
		}
		bool flag4 = DeviceUpdateFile.HasNewVersion(211, "v" + selectedDeviceInfo.DongleVersion, mcu, selectedDeviceInfo.deviceInfo.CID, selectedDeviceInfo.deviceInfo.MID);
		if (flag3 != flag4)
		{
			if (!flag3 & flag4)
			{
				FormDialog formDialog = new FormDialog(LanguageFile.Dialogs[92], DialogButtons.OKCanel);
				((Form)formDialog).ShowDialog();
				flag2 = formDialog.resault;
				flag = formDialog.resault;
			}
			else if (!flag4 & flag3)
			{
				((Form)new FormDialog(LanguageFile.Dialogs[91], DialogButtons.OK)).ShowDialog();
				flag = false;
			}
		}
		if (flag2)
		{
			byte[] buffers = new byte[1024];
			DeviceUpdateFile.HasDeviceUpdateFile(211, "v" + selectedDeviceInfo.DongleVersion, mcu, selectedDeviceInfo.deviceInfo.CID, selectedDeviceInfo.deviceInfo.MID, out buffers);
			((Form)new FormUpdate(buffers, (byte)num)).ShowDialog();
			flag = true;
		}
		if (flag)
		{
			formPair = new FormPair(FormMain.driveParam.CID);
			((Form)formPair).ShowDialog();
		}
		ExitPairing?.Invoke(this);
	}

	public void EnterPair()
	{
		customButton_Pairing_Click(null, null);
	}

	public void ExitPair()
	{
		if (formPair != null)
		{
			((Form)formPair).Close();
			((Component)(object)formPair).Dispose();
		}
	}

	public void LanguageChange()
	{
		((Control)label_PairTool).Text = LanguageFile.Dialogs[39];
		((Control)customButton_Pairing).Text = LanguageFile.Dialogs[84];
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
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		label_PairTool = new Label();
		pictureBox_Title = new PictureBox();
		customButton_Pairing = new CustomButton();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_PairTool).AutoSize = true;
		((Control)label_PairTool).ForeColor = Color.White;
		((Control)label_PairTool).Location = new Point(57, 10);
		((Control)label_PairTool).Name = "label_PairTool";
		((Control)label_PairTool).Size = new Size(65, 20);
		((Control)label_PairTool).TabIndex = 124;
		((Control)label_PairTool).Text = "配对工具";
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 16);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 126;
		pictureBox_Title.TabStop = false;
		((Control)customButton_Pairing).ForeColor = Color.White;
		((Control)customButton_Pairing).Location = new Point(26, 52);
		((Control)customButton_Pairing).Margin = new Padding(3, 4, 3, 4);
		customButton_Pairing.MouseDownImage = (Image)(object)Resources.配对工具;
		customButton_Pairing.MouseEnterImage = (Image)(object)Resources.配对工具;
		((Control)customButton_Pairing).Name = "customButton_Pairing";
		customButton_Pairing.NormalImage = (Image)(object)Resources.配对工具;
		((Control)customButton_Pairing).Size = new Size(112, 26);
		((Control)customButton_Pairing).TabIndex = 125;
		((Control)customButton_Pairing).Text = "配对";
		((Control)customButton_Pairing).Click += customButton_Pairing_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Controls.Add((Control)(object)customButton_Pairing);
		((Control)this).Controls.Add((Control)(object)label_PairTool);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_Pair";
		((Control)this).Size = new Size(857, 97);
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
