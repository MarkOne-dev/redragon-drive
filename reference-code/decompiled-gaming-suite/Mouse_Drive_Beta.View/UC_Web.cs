using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FileManager;
using Mouse_Drive_Beta.FileManager;

namespace Mouse_Drive_Beta.View;

public class UC_Web : UserControl
{
	private LanguageFile languageFile;

	private IContainer components;

	private LinkLabel linkLabel_Web;

	private Label label_Web;

	public UC_Web(LanguageFile language)
	{
		InitializeComponent();
		languageFile = language;
		LanguageChange();
	}

	private void linkLabel_Web_Click(object sender, EventArgs e)
	{
		Process.Start(((Control)linkLabel_Web).Text);
	}

	public void LanguageChange()
	{
		((Control)linkLabel_Web).Text = DriveConfig.GetWeb(languageFile.LanguageType[languageFile.LanguageIndex] + "-Web");
		((Control)linkLabel_Web).Location = new Point(((Control)this).Width - ((Control)linkLabel_Web).Width - 3, ((Control)linkLabel_Web).Top);
		((Control)label_Web).Text = DriveConfig.GetDescription(languageFile.LanguageType[languageFile.LanguageIndex]);
		((Control)label_Web).Location = new Point(((Control)this).Width - ((Control)label_Web).Width - 3, ((Control)label_Web).Top);
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
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Expected O, but got Unknown
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		linkLabel_Web = new LinkLabel();
		label_Web = new Label();
		((Control)this).SuspendLayout();
		((Control)linkLabel_Web).AutoSize = true;
		((Control)linkLabel_Web).BackColor = Color.Transparent;
		((Control)linkLabel_Web).Font = new Font("微软雅黑", 10.5f, (FontStyle)2, (GraphicsUnit)3, (byte)134);
		((Control)linkLabel_Web).ForeColor = Color.White;
		linkLabel_Web.LinkBehavior = (LinkBehavior)3;
		linkLabel_Web.LinkColor = Color.White;
		((Control)linkLabel_Web).Location = new Point(711, 9);
		((Control)linkLabel_Web).Name = "linkLabel_Web";
		((Control)linkLabel_Web).Size = new Size(141, 20);
		((Control)linkLabel_Web).TabIndex = 4;
		linkLabel_Web.TabStop = true;
		((Control)linkLabel_Web).Text = "www.compx.com.cn";
		((Control)linkLabel_Web).Click += linkLabel_Web_Click;
		((Control)label_Web).AutoSize = true;
		((Control)label_Web).BackColor = Color.Transparent;
		((Control)label_Web).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)label_Web).ForeColor = Color.White;
		((Control)label_Web).Location = new Point(653, 36);
		((Control)label_Web).Name = "label_Web";
		((Control)label_Web).Size = new Size(199, 20);
		((Control)label_Web).TabIndex = 5;
		((Control)label_Web).Text = "版权所有Compx.保留所有权利";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)linkLabel_Web);
		((Control)this).Controls.Add((Control)(object)label_Web);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).ForeColor = Color.White;
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UC_Web";
		((Control)this).Size = new Size(855, 64);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
