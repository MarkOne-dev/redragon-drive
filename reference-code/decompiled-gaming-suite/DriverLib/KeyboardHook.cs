using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DriverLib;

public class KeyboardHook : GlobalHook
{
	public delegate void HookKeyEventHandler(object sender, KeyEventArgs e, KeyboardHookStruct hookStruct);

	[CompilerGenerated]
	private KeyPressEventHandler m_KeyPress;

	public event HookKeyEventHandler KeyDown;

	public event HookKeyEventHandler KeyUp;

	public event KeyPressEventHandler KeyPress
	{
		[CompilerGenerated]
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = this.m_KeyPress;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyPress, value2, val2);
			}
			while (val != val2);
		}
		[CompilerGenerated]
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			KeyPressEventHandler val = this.m_KeyPress;
			KeyPressEventHandler val2;
			do
			{
				val2 = val;
				KeyPressEventHandler value2 = (KeyPressEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref this.m_KeyPress, value2, val2);
			}
			while (val != val2);
		}
	}

	public KeyboardHook()
	{
		_hookType = 13;
	}

	protected override int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Expected O, but got Unknown
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		bool flag = false;
		if (nCode > -1 && (KeyDown != null || KeyUp != null))
		{
			KeyboardHookStruct keyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
			bool flag2 = (GlobalHook.GetKeyState(162) & 0x80) != 0 || (GlobalHook.GetKeyState(3) & 0x80) != 0;
			bool flag3 = (GlobalHook.GetKeyState(160) & 0x80) != 0 || (GlobalHook.GetKeyState(161) & 0x80) != 0;
			bool flag4 = (GlobalHook.GetKeyState(164) & 0x80) != 0 || (GlobalHook.GetKeyState(165) & 0x80) != 0;
			bool flag5 = GlobalHook.GetKeyState(20) != 0;
			KeyEventArgs e = new KeyEventArgs((Keys)(keyboardHookStruct.vkCode | (flag2 ? 131072 : 0) | (flag3 ? 65536 : 0) | (flag4 ? 262144 : 0)));
			switch (wParam)
			{
			case 256:
			case 260:
				if (KeyDown != null)
				{
					KeyDown(this, e, keyboardHookStruct);
					flag = flag || e.Handled;
				}
				break;
			case 257:
			case 261:
				if (KeyUp != null)
				{
					KeyUp(this, e, keyboardHookStruct);
					flag = flag || e.Handled;
				}
				break;
			}
			if (wParam == 256 && !flag && !e.SuppressKeyPress && this.m_KeyPress != null)
			{
				byte[] array = new byte[256];
				byte[] array2 = new byte[2];
				GlobalHook.GetKeyboardState(array);
				if (GlobalHook.ToAscii(keyboardHookStruct.vkCode, keyboardHookStruct.scanCode, array, array2, keyboardHookStruct.flags) == 1)
				{
					char c = (char)array2[0];
					if ((flag5 ^ flag3) && char.IsLetter(c))
					{
						c = char.ToUpper(c);
					}
					KeyPressEventArgs e2 = new KeyPressEventArgs(c);
					this.m_KeyPress.Invoke((object)this, e2);
					flag = flag || e.Handled;
				}
			}
		}
		if (flag)
		{
			return 1;
		}
		return GlobalHook.CallNextHookEx(_handleToHook, nCode, wParam, lParam);
	}
}
