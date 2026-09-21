using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindControls;

internal class FormMover
{
	private Point mPoint = new Point(0, 0);

	private bool mouseDown;

	private List<Form> forms = new List<Form>();

	public void AddForm(Form form)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		((Control)form).MouseDown += new MouseEventHandler(Form_MouseDown);
		((Control)form).MouseUp += new MouseEventHandler(Form_MouseUp);
		((Control)form).MouseMove += new MouseEventHandler(Form_MouseMove);
		forms.Add(form);
	}

	public void AddControl(Control control)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		control.MouseDown += new MouseEventHandler(Form_MouseDown);
		control.MouseUp += new MouseEventHandler(Form_MouseUp);
		control.MouseMove += new MouseEventHandler(Form_MouseMove);
	}

	private void Form_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		mPoint = new Point(e.X, e.Y);
	}

	private void Form_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void Form_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576 && mouseDown)
		{
			_ = mPoint;
			for (int i = 0; i < forms.Count; i++)
			{
				forms[i].Location = new Point(forms[i].Location.X + e.X - mPoint.X, forms[i].Location.Y + e.Y - mPoint.Y);
			}
		}
	}
}
