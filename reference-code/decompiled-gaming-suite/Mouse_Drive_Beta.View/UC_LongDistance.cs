using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using DriverLib;
using FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_LongDistance : UserControl
{
	private IContainer components;

	private Label label_LongDistanceDis;

	private CustomCheckBox customCheckBox_LongDistance;

	private Label label_LongDistance;

	public bool LongDistance
	{
		get
		{
			return customCheckBox_LongDistance.Checked;
		}
		set
		{
			customCheckBox_LongDistance.Checked = value;
		}
	}

	public UC_LongDistance()
	{
		InitializeComponent();
		LanguageChange();
	}

	private void customCheckBox_LongDistance_CheckChange(object sender, CustomEventArgs e)
	{
		if (FormMain.ConnectedDeviceInfo.deviceString == null)
		{
			return;
		}
		if (!FormMain.ConnectedDeviceInfo.isUSB)
		{
			if (FormMain.GetDeviceOnlineFlag())
			{
				UsbServer.SetLongRangeMode(customCheckBox_LongDistance.Checked);
			}
		}
		else if (!customCheckBox_LongDistance.Checked)
		{
			if (FormMain.GetDeviceOnlineFlag())
			{
				UsbServer.SetLongRangeMode(customCheckBox_LongDistance.Checked);
			}
		}
		else
		{
			customCheckBox_LongDistance.Checked = false;
		}
	}

	private void customCheckBox_LongDistance_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (FormMain.GetDeviceOnlineFlag() && FormMain.ConnectedDeviceInfo.isUSB && !customCheckBox_LongDistance.Checked)
		{
			((Form)new FormDialog(LanguageFile.Dialogs[63])).ShowDialog();
		}
	}

	public void LanguageChange()
	{
		((Control)label_LongDistance).Text = LanguageFile.Dialogs[62];
		((Control)label_LongDistanceDis).Text = "(" + LanguageFile.Dialogs[64] + ")";
		((Control)label_LongDistanceDis).Location = new Point(((Control)label_LongDistance).Right, ((Control)label_LongDistanceDis).Top);
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		label_LongDistanceDis = new Label();
		label_LongDistance = new Label();
		customCheckBox_LongDistance = new CustomCheckBox();
		((Control)this).SuspendLayout();
		((Control)label_LongDistanceDis).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_LongDistanceDis).Location = new Point(142, 5);
		((Control)label_LongDistanceDis).Name = "label_LongDistanceDis";
		((Control)label_LongDistanceDis).Size = new Size(594, 34);
		((Control)label_LongDistanceDis).TabIndex = 125;
		((Control)label_LongDistanceDis).Text = "（远距离模式下，距离会更远/抗干扰能力更强，相应工作电流会加大，使用时间减少）";
		((Control)label_LongDistance).AutoSize = true;
		((Control)label_LongDistance).Location = new Point(57, 2);
		((Control)label_LongDistance).Name = "label_LongDistance";
		((Control)label_LongDistance).Size = new Size(79, 20);
		((Control)label_LongDistance).TabIndex = 123;
		((Control)label_LongDistance).Text = "远距离模式";
		customCheckBox_LongDistance.Checked = false;
		customCheckBox_LongDistance.CheckImage = (Image)(object)Resources.单选框选择;
		((Control)customCheckBox_LongDistance).Location = new Point(26, 5);
		((Control)customCheckBox_LongDistance).Name = "customCheckBox_LongDistance";
		((Control)customCheckBox_LongDistance).Size = new Size(16, 16);
		((Control)customCheckBox_LongDistance).TabIndex = 124;
		customCheckBox_LongDistance.UncheckImage = (Image)(object)Resources.单选框未选择;
		customCheckBox_LongDistance.CheckChange += customCheckBox_LongDistance_CheckChange;
		((Control)customCheckBox_LongDistance).Click += customCheckBox_LongDistance_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)label_LongDistanceDis);
		((Control)this).Controls.Add((Control)(object)customCheckBox_LongDistance);
		((Control)this).Controls.Add((Control)(object)label_LongDistance);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_LongDistance";
		((Control)this).Size = new Size(857, 48);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
