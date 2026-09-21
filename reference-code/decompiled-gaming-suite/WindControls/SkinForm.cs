using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace WindControls;

public class SkinForm : Form
{
	private FormMover formMover = new FormMover();

	public Form ControlsForm;

	private int formLocationOffset;

	private bool Movable = true;

	private IContainer components;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 0x80000;
			return createParams;
		}
	}

	public SkinForm(bool movable)
	{
		InitializeComponent();
		SetStyles();
		Movable = movable;
	}

	public void AddControl(Control control)
	{
		if (Movable)
		{
			formMover.AddControl(control);
		}
	}

	public void InitSkin(Form form, int reduce, int radius)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		if (Movable)
		{
			formMover.AddForm(form);
		}
		ControlsForm = form;
		ControlsForm.Owner = (Form)(object)this;
		((Form)this).ShowInTaskbar = true;
		ControlsForm.ShowInTaskbar = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).Icon = ControlsForm.Icon;
		((Form)this).ShowIcon = ControlsForm.ShowIcon;
		((Control)this).Text = ((Control)ControlsForm).Text;
		((Control)this).Font = ((Control)ControlsForm).Font;
		((Form)this).Size = ((Control)ControlsForm).BackgroundImage.Size;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).BackgroundImage = (Image)new Bitmap(((Control)ControlsForm).BackgroundImage, ((Form)this).Size);
		((Control)ControlsForm).Width = ((Form)this).Size.Width - reduce * 2;
		((Control)ControlsForm).Height = ((Form)this).Size.Height - reduce * 2;
		formLocationOffset = reduce;
		Rectangle rect = new Rectangle(reduce, reduce, ((Control)ControlsForm).Width, ((Control)ControlsForm).Height);
		Bitmap backgroundImage = ImageHelper.ImageTailor(new Bitmap(((Control)ControlsForm).BackgroundImage), rect);
		((Control)ControlsForm).BackgroundImage = (Image)(object)backgroundImage;
		((Control)ControlsForm).Region = GraphicsHelper.GetWindowRegion(rect.Width, rect.Height, radius, Color.Transparent, GraphicsHelper.RoundStyle.All);
		ControlsForm.FormClosing += new FormClosingEventHandler(Form_FormClosing);
		((Control)ControlsForm).LocationChanged += ControlsForm_LocationChanged;
		ControlsForm.Load += ControlsForm_Load;
		foreach (Control item in (ArrangedElementCollection)((Control)ControlsForm).Controls)
		{
			Control val = item;
			val.Location = new Point(val.Location.X - reduce, val.Location.Y - reduce);
		}
		((Form)this).Activated += SkinForm_Activated;
	}

	private void SkinForm_Activated(object sender, EventArgs e)
	{
		ControlsForm.Activate();
	}

	private void ControlsForm_Load(object sender, EventArgs e)
	{
		((Form)this).Location = new Point(ControlsForm.Location.X - formLocationOffset, ControlsForm.Location.Y - formLocationOffset);
		((Control)this).Show();
	}

	private void ControlsForm_LocationChanged(object sender, EventArgs e)
	{
		((Form)this).Location = new Point(ControlsForm.Location.X - formLocationOffset, ControlsForm.Location.Y - formLocationOffset);
	}

	private void Form_FormClosing(object sender, FormClosingEventArgs e)
	{
		ControlsForm.Owner = null;
		((Form)this).Close();
	}

	public void AllHide()
	{
		((Control)this).Hide();
		((Control)ControlsForm).Hide();
	}

	public void AllShow()
	{
		((Control)this).Show();
		((Control)ControlsForm).Show();
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)204818, true);
		((Control)this).UpdateStyles();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
	}

	public void SetBits()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)this).BackgroundImage == null)
		{
			return;
		}
		Bitmap val = new Bitmap(((Control)this).BackgroundImage, ((Control)this).Width, ((Control)this).Height);
		if (!Image.IsCanonicalPixelFormat(((Image)val).PixelFormat) || !Image.IsAlphaPixelFormat(((Image)val).PixelFormat))
		{
			throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
		}
		IntPtr hObj = IntPtr.Zero;
		IntPtr dC = Win32.GetDC(IntPtr.Zero);
		IntPtr intPtr = IntPtr.Zero;
		IntPtr intPtr2 = Win32.CreateCompatibleDC(dC);
		try
		{
			Win32.Point pptDst = new Win32.Point(((Control)this).Left, ((Control)this).Top);
			Win32.Size psize = new Win32.Size(((Control)this).Width, ((Control)this).Height);
			Win32.BLENDFUNCTION pblend = default(Win32.BLENDFUNCTION);
			Win32.Point pptSrc = new Win32.Point(0, 0);
			intPtr = val.GetHbitmap(Color.FromArgb(0));
			hObj = Win32.SelectObject(intPtr2, intPtr);
			pblend.BlendOp = 0;
			pblend.SourceConstantAlpha = byte.MaxValue;
			pblend.AlphaFormat = 1;
			pblend.BlendFlags = 0;
			Win32.UpdateLayeredWindow(((Control)this).Handle, dC, ref pptDst, ref psize, intPtr2, ref pptSrc, 0, ref pblend, 2);
		}
		finally
		{
			if (intPtr != IntPtr.Zero)
			{
				Win32.SelectObject(intPtr2, hObj);
				Win32.DeleteObject(intPtr);
			}
			Win32.ReleaseDC(IntPtr.Zero, dC);
			Win32.DeleteDC(intPtr2);
		}
	}

	protected override void OnBackgroundImageChanged(EventArgs e)
	{
		((Form)this).OnBackgroundImageChanged(e);
		SetBits();
	}

	protected override void OnResize(EventArgs e)
	{
		((Form)this).OnResize(e);
		SetBits();
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
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 12f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImageLayout = (ImageLayout)3;
		((Form)this).ClientSize = new Size(259, 271);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "SkinForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "SkinForm";
		((Control)this).ResumeLayout(false);
	}
}
