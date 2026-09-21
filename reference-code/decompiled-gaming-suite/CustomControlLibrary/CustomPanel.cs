using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomPanel : Panel
{
	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Panel)this).CreateParams;
			createParams.ExStyle |= 0x2000000;
			return createParams;
		}
	}

	public CustomPanel()
	{
		((Control)this).SetStyle((ControlStyles)141330, true);
	}
}
