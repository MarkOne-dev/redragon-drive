using System.Drawing;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomListView : ListView
{
	private Color _ItemSelectColor = Color.FromArgb(57, 57, 57);

	public Color ItemSelectColor
	{
		get
		{
			return _ItemSelectColor;
		}
		set
		{
			_ItemSelectColor = value;
			((Control)this).Invalidate();
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((ListView)this).CreateParams;
			createParams.ExStyle |= 0x2000000;
			return createParams;
		}
	}

	private void SetStyles()
	{
		((Control)this).SetStyle((ControlStyles)198656, true);
		((Control)this).UpdateStyles();
		((ListView)this).OwnerDraw = true;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		e.Graphics.Clear(((Control)this).BackColor);
	}

	protected override void OnDrawItem(DrawListViewItemEventArgs e)
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		((ListView)this).OnDrawItem(e);
		if (((Control)this).Enabled)
		{
			if (e.Item.Selected)
			{
				e.Graphics.FillRectangle((Brush)new SolidBrush(_ItemSelectColor), e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
			}
			else
			{
				e.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), e.Bounds.X, e.Bounds.Y, ((Control)this).Width, e.Bounds.Height);
			}
		}
		else
		{
			e.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), 0, 0, ((Control)this).Width, ((ListView)this).Items[0].Bounds.Top);
			e.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).BackColor), 0, e.Bounds.Height * ((ListView)this).Items.Count, ((Control)this).Width, ((Control)this).Height - e.Bounds.Height * ((ListView)this).Items.Count);
			e.Graphics.FillRectangle((Brush)new SolidBrush(e.Item.Selected ? _ItemSelectColor : ((Control)this).BackColor), e.Bounds.X, e.Bounds.Y, ((Control)this).Width, e.Bounds.Height);
		}
		e.Graphics.DrawString(e.Item.Text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (RectangleF)e.Bounds);
	}

	public CustomListView()
	{
		SetStyles();
	}
}
