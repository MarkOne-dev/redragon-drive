using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DriverLib;

public abstract class GlobalHook
{
	[StructLayout(LayoutKind.Sequential)]
	public class KeyboardHookStruct
	{
		public int vkCode;

		public int scanCode;

		public int flags;

		public int time;

		public int dwExtraInfo;
	}

	protected delegate int HookProc(int nCode, int wParam, IntPtr lParam);

	protected const int WH_MOUSE_LL = 14;

	protected const int WH_KEYBOARD_LL = 13;

	protected const int WH_MOUSE = 7;

	protected const int WH_KEYBOARD = 2;

	protected const int WM_MOUSEMOVE = 512;

	protected const int WM_LBUTTONDOWN = 513;

	protected const int WM_RBUTTONDOWN = 516;

	protected const int WM_MBUTTONDOWN = 519;

	protected const int WM_LBUTTONUP = 514;

	protected const int WM_RBUTTONUP = 517;

	protected const int WM_MBUTTONUP = 520;

	protected const int WM_LBUTTONDBLCLK = 515;

	protected const int WM_RBUTTONDBLCLK = 518;

	protected const int WM_MBUTTONDBLCLK = 521;

	protected const int WM_MOUSEWHEEL = 522;

	protected const int WM_KEYDOWN = 256;

	protected const int WM_KEYUP = 257;

	protected const int WM_SYSKEYDOWN = 260;

	protected const int WM_SYSKEYUP = 261;

	protected const byte VK_SHIFT = 16;

	protected const byte VK_CAPITAL = 20;

	protected const byte VK_NUMLOCK = 144;

	protected const byte VK_LSHIFT = 160;

	protected const byte VK_RSHIFT = 161;

	protected const byte VK_LCONTROL = 162;

	protected const byte VK_RCONTROL = 3;

	protected const byte VK_LALT = 164;

	protected const byte VK_RALT = 165;

	protected const byte LLKHF_ALTDOWN = 32;

	protected int _hookType;

	protected int _handleToHook;

	protected bool _isStarted;

	protected HookProc _hookCallback;

	public bool IsStarted => _isStarted;

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	protected static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
	protected static extern int UnhookWindowsHookEx(int idHook);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	protected static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

	[DllImport("user32")]
	protected static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

	[DllImport("user32")]
	protected static extern int GetKeyboardState(byte[] pbKeyState);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	protected static extern short GetKeyState(int vKey);

	public GlobalHook()
	{
		Application.ApplicationExit += Application_ApplicationExit;
	}

	public void Start(IntPtr hMod)
	{
		if (!_isStarted && _hookType != 0)
		{
			_hookCallback = HookCallbackProcedure;
			_handleToHook = SetWindowsHookEx(_hookType, _hookCallback, hMod, 0);
			if (_handleToHook != 0)
			{
				_isStarted = true;
			}
		}
	}

	public void Stop()
	{
		if (_isStarted)
		{
			UnhookWindowsHookEx(_handleToHook);
			_isStarted = false;
		}
	}

	protected virtual int HookCallbackProcedure(int nCode, int wParam, IntPtr lParam)
	{
		return 0;
	}

	protected void Application_ApplicationExit(object sender, EventArgs e)
	{
		if (_isStarted)
		{
			Stop();
		}
	}
}
