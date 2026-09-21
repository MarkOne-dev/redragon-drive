using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomTabControl : TabControl
{
	private Color _TabPageSelectBackColor = Color.Transparent;

	private Color _TabPageUnselectBackColor = Color.Transparent;

	private Color _TabPageSelectFontColor = Color.Orange;

	private Color _TabPageUnselectFontColor = Color.White;

	public override Color BackColor { get; set; } = Color.Transparent;

	[Category("自定义")]
	[Description("TabPage选中时背景色")]
	public Color TabPageSelectBackColor
	{
		get
		{
			return _TabPageSelectBackColor;
		}
		set
		{
			_TabPageSelectBackColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("TabPage未选中时背景色")]
	public Color TabPageUnselectBackColor
	{
		get
		{
			return _TabPageUnselectBackColor;
		}
		set
		{
			_TabPageUnselectBackColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("TabPage选中时字体颜色")]
	public Color TabPageSelectFontColor
	{
		get
		{
			return _TabPageSelectFontColor;
		}
		set
		{
			_TabPageSelectFontColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("TabPage未选中时字体颜色")]
	public Color TabPageUnselectFontColor
	{
		get
		{
			return _TabPageUnselectFontColor;
		}
		set
		{
			_TabPageUnselectFontColor = value;
			((Control)this).Invalidate();
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((TabControl)this).CreateParams;
			createParams.ExStyle |= 0x2000000;
			return createParams;
		}
	}

	public CustomTabControl()
	{
		((Control)this).SetStyle((ControlStyles)206866, true);
		((Control)this).UpdateStyles();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		PaintBackground(e.Graphics, ((Control)this).ClientRectangle);
		PaintAllTheTabs(e);
		PaintTheSelectedTab(e);
	}

	protected void PaintBackground(Graphics g, Rectangle clipRect)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (((Control)this).Parent != null)
		{
			clipRect.Offset(((Control)this).Location);
			PaintEventArgs e = new PaintEventArgs(g, clipRect);
			GraphicsState val = g.Save();
			try
			{
				g.TranslateTransform((float)(-((Control)this).Location.X), (float)(-((Control)this).Location.Y));
				((Control)this).InvokePaintBackground(((Control)this).Parent, e);
				((Control)this).InvokePaint(((Control)this).Parent, e);
				return;
			}
			finally
			{
				g.Restore(val);
				clipRect.Offset(-((Control)this).Location.X, -((Control)this).Location.Y);
			}
		}
		clipRect.Offset(((Control)this).Location);
		Brush val2 = (Brush)new SolidBrush(_TabPageUnselectBackColor);
		g.FillRectangle(val2, clipRect);
		val2.Dispose();
	}

	private void PaintTheSelectedTab(PaintEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		if (((TabControl)this).SelectedIndex != -1)
		{
			int num = 0;
			Rectangle tabRect = ((TabControl)this).GetTabRect(((TabControl)this).SelectedIndex);
			num = tabRect.Right;
			e.Graphics.DrawLine(new Pen(_TabPageSelectBackColor), tabRect.Left, tabRect.Bottom + 1, num, tabRect.Bottom + 1);
		}
	}

	private void PaintAllTheTabs(PaintEventArgs e)
	{
		if (((TabControl)this).TabCount > 0)
		{
			int num = 0;
			for (num = 0; num < ((TabControl)this).TabCount; num++)
			{
				PaintTab(e, num);
			}
		}
	}

	private GraphicsPath GetTabPath(int index)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected I4, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.Reset();
		if (!((Control)this).IsHandleCreated)
		{
			return val;
		}
		Rectangle tabRect = ((TabControl)this).GetTabRect(index);
		TabAlignment alignment = ((TabControl)this).Alignment;
		switch ((int)alignment)
		{
		default:
			val.AddLine(tabRect.Left, tabRect.Top, tabRect.Left, tabRect.Bottom + 1);
			val.AddLine(tabRect.Left, tabRect.Top, tabRect.Right, tabRect.Top);
			val.AddLine(tabRect.Right, tabRect.Top, tabRect.Right, tabRect.Bottom + 1);
			val.AddLine(tabRect.Right, tabRect.Bottom + 1, tabRect.Left, tabRect.Bottom + 1);
			return val;
		}
	}

	private void PaintTab(PaintEventArgs e, int index)
	{
		GraphicsPath tabPath = GetTabPath(index);
		PaintTabBackground(e.Graphics, index, tabPath);
		PaintTabText(e.Graphics, index);
	}

	private void PaintTabBackground(Graphics graph, int index, GraphicsPath path)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		((TabControl)this).GetTabRect(index);
		Color transparent = Color.Transparent;
		transparent = ((index != ((TabControl)this).SelectedIndex) ? _TabPageUnselectBackColor : _TabPageSelectBackColor);
		Brush val = (Brush)new SolidBrush(transparent);
		graph.FillPath(val, path);
		val.Dispose();
	}

	private void PaintTabText(Graphics graph, int index)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		string text = ((Control)((TabControl)this).TabPages[index]).Text;
		StringFormat val = new StringFormat();
		val.Alignment = (StringAlignment)0;
		val.LineAlignment = (StringAlignment)1;
		val.Trimming = (StringTrimming)3;
		Brush val2 = null;
		val2 = (((TabControl)this).TabPages[index].Enabled ? SystemBrushes.ControlText : SystemBrushes.ControlDark);
		Font font = ((Control)this).Font;
		if (index == ((TabControl)this).SelectedIndex)
		{
			if (((TabControl)this).TabPages[index].Enabled)
			{
				val2 = (Brush)new SolidBrush(_TabPageSelectFontColor);
			}
		}
		else if (((TabControl)this).TabPages[index].Enabled)
		{
			val2 = (Brush)new SolidBrush(_TabPageUnselectFontColor);
		}
		Rectangle tabRect = ((TabControl)this).GetTabRect(index);
		int num = Convert.ToInt32(((Control)this).CreateGraphics().MeasureString(((Control)((TabControl)this).TabPages[index]).Text, font).Width);
		Rectangle rectangle = new Rectangle(tabRect.Left + (((TabControl)this).ItemSize.Width - num) / 2, tabRect.Top, tabRect.Width, tabRect.Height);
		graph.DrawString(text, font, val2, (RectangleF)rectangle, val);
	}
}
