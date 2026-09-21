using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_Advanced : UserControl
{
	private UC_LongDistance uc_LongDistance;

	private UC_DongleLED uc_DongleLED;

	private IContainer components;

	private PictureBox pictureBox_Title;

	private Label label_AdvancedSetting;

	public UC_Advanced()
	{
		InitializeComponent();
		LanguageChange();
	}

	public void UpdateLongDistance(bool longDistance)
	{
		if (uc_LongDistance != null)
		{
			uc_LongDistance.LongDistance = longDistance;
		}
	}

	public void UpdateDongleLED(int value)
	{
		if (uc_DongleLED != null)
		{
			uc_DongleLED.DongleLed = value;
		}
	}

	public void AddLongDistance()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		if (uc_LongDistance != null)
		{
			return;
		}
		uc_LongDistance = new UC_LongDistance();
		((Control)uc_LongDistance).Location = new Point(0, ((Control)label_AdvancedSetting).Bottom + 10);
		((Control)this).Controls.Add((Control)(object)uc_LongDistance);
		int num = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			num += val.Height;
			((Control)this).Size = new Size(((Control)this).Width, num);
		}
	}

	public void AddDongleLED()
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		if (uc_DongleLED != null)
		{
			return;
		}
		uc_DongleLED = new UC_DongleLED();
		if (uc_LongDistance != null)
		{
			((Control)uc_DongleLED).Location = new Point(0, ((Control)uc_LongDistance).Bottom);
		}
		else
		{
			((Control)uc_DongleLED).Location = new Point(0, ((Control)label_AdvancedSetting).Bottom + 3);
		}
		((Control)this).Controls.Add((Control)(object)uc_DongleLED);
		int num = 0;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			num += val.Height;
			((Control)this).Size = new Size(((Control)this).Width, num);
		}
	}

	public void LanguageChange()
	{
		((Control)label_AdvancedSetting).Text = LanguageFile.Dialogs[61];
		if (uc_LongDistance != null)
		{
			uc_LongDistance.LanguageChange();
		}
		if (uc_DongleLED != null)
		{
			uc_DongleLED.LanguageChange();
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
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		label_AdvancedSetting = new Label();
		pictureBox_Title = new PictureBox();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_AdvancedSetting).AutoSize = true;
		((Control)label_AdvancedSetting).ForeColor = Color.White;
		((Control)label_AdvancedSetting).Location = new Point(57, 10);
		((Control)label_AdvancedSetting).Name = "label_AdvancedSetting";
		((Control)label_AdvancedSetting).Size = new Size(65, 20);
		((Control)label_AdvancedSetting).TabIndex = 127;
		((Control)label_AdvancedSetting).Text = "高级设置";
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 16);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 128;
		pictureBox_Title.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Controls.Add((Control)(object)label_AdvancedSetting);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.Transparent;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_Advanced";
		((Control)this).Size = new Size(857, 249);
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
