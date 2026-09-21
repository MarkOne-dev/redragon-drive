using System.Windows.Forms;

namespace Mouse_Drive_Beta.ComboControlLibrary;

internal class DropDownBoxMessager : IMessageFilter
{
	private const int WM_LBUTTONDOWN = 513;

	private const int WM_LBUTTONUP = 514;

	private const int WM_RBUTTONDOWN = 516;

	private const int WM_RBUTTONUP = 517;

	private const int WM_MBUTTONDOWN = 519;

	private const int WM_MBUTTONUP = 520;

	private const int WM_XBUTTONDOWN = 523;

	private const int WM_XBUTTONUP = 524;

	private CustomKeyFunction windDropDownBox;

	public DropDownBoxMessager(CustomKeyFunction _windDropDownBox)
	{
		windDropDownBox = _windDropDownBox;
	}

	public bool PreFilterMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 514 && !windDropDownBox.isInClientRect() && !windDropDownBox.isDilog)
		{
			windDropDownBox.HideActiveWindows();
		}
		return false;
	}
}
