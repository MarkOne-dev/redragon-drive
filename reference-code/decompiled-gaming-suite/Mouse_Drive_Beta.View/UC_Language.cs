using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using FileManager;
using Mouse_Drive_Beta.FileManager;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.View;

public class UC_Language : UserControl
{
	public delegate void LanguageChangeEventHandler(object sender, CustomEventArgs e);

	public LanguageFile languageFile;

	private IContainer components;

	private PictureBox pictureBox_Title;

	private CustomComboBox customComboBox_Language;

	private Label label_Language;

	public event LanguageChangeEventHandler LanguageChange;

	public UC_Language(LanguageFile language)
	{
		InitializeComponent();
		languageFile = language;
		((Control)label_Language).Text = LanguageFile.Dialogs[81];
		CustomComboBox customComboBox = customComboBox_Language;
		Color unselectItemColor = (((Control)customComboBox_Language).BackColor = FormMain.driveParam.CbbBgClr);
		customComboBox.UnselectItemColor = unselectItemColor;
		customComboBox_Language.SelectItemColor = FormMain.driveParam.CbbItemClr;
		((Control)customComboBox_Language).ForeColor = FormMain.driveParam.CbbForeClr;
		customComboBox_Language.ArrowImageNoraml = (Image)(object)ResourcesFile.GetComboxBoxBitmap();
		customComboBox_Language.Item.Clear();
		for (int i = 0; i < languageFile.LanguageType.Length; i++)
		{
			customComboBox_Language.Item.Add((object)languageFile.LanguageType[i]);
		}
		customComboBox_Language.SelectIndex = language.LanguageIndex;
	}

	private void customComboBox_Language_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (customComboBox_Language.SelectIndex != FormMain.languageFile.LanguageIndex)
		{
			languageFile.SetSettingLanguage(customComboBox_Language.SelectIndex);
			((Control)label_Language).Text = LanguageFile.Dialogs[81];
			RegeditManager.SaveLanguageIndex(customComboBox_Language.SelectIndex);
			CustomEventArgs e2 = new CustomEventArgs(customComboBox_Language.SelectIndex);
			LanguageChange?.Invoke(this, e2);
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(UC_Language));
		label_Language = new Label();
		pictureBox_Title = new PictureBox();
		customComboBox_Language = new CustomComboBox();
		((ISupportInitialize)pictureBox_Title).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label_Language).AutoSize = true;
		((Control)label_Language).ForeColor = Color.White;
		((Control)label_Language).Location = new Point(57, 11);
		((Control)label_Language).Name = "label_Language";
		((Control)label_Language).Size = new Size(37, 20);
		((Control)label_Language).TabIndex = 126;
		((Control)label_Language).Text = "语言";
		((Control)pictureBox_Title).BackgroundImage = (Image)(object)Resources.标题符号;
		((Control)pictureBox_Title).BackgroundImageLayout = (ImageLayout)2;
		((Control)pictureBox_Title).Location = new Point(26, 16);
		((Control)pictureBox_Title).Name = "pictureBox_Title";
		((Control)pictureBox_Title).Size = new Size(16, 10);
		pictureBox_Title.TabIndex = 128;
		pictureBox_Title.TabStop = false;
		customComboBox_Language.ArrowDirection = CustomComboBox.ArrowDirectionEnum.Down;
		customComboBox_Language.ArrowImageNoraml = (Image)componentResourceManager.GetObject("customComboBox_Language.ArrowImageNoraml");
		((Control)customComboBox_Language).BackColor = Color.FromArgb(57, 57, 57);
		((Control)customComboBox_Language).Font = new Font("微软雅黑", 10.5f);
		((Control)customComboBox_Language).Location = new Point(25, 46);
		((Control)customComboBox_Language).Name = "customComboBox_Language";
		customComboBox_Language.SelectIndex = -1;
		customComboBox_Language.SelectItemColor = Color.FromArgb(119, 119, 119);
		((Control)customComboBox_Language).Size = new Size(162, 30);
		((Control)customComboBox_Language).TabIndex = 127;
		customComboBox_Language.UnselectItemColor = Color.FromArgb(63, 63, 63);
		customComboBox_Language.OnSelectedIndexChanged += customComboBox_Language_OnSelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pictureBox_Title);
		((Control)this).Controls.Add((Control)(object)customComboBox_Language);
		((Control)this).Controls.Add((Control)(object)label_Language);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_Language";
		((Control)this).Size = new Size(857, 93);
		((ISupportInitialize)pictureBox_Title).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
