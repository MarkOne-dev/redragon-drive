namespace DriverLib;

public class KeyboardCodes
{
	public static KeyboardCode[] KeyboardCodeDataBase = new KeyboardCode[146]
	{
		new KeyboardCode(27, 1, 0, 41, "Esc", KeysEnum.Escape),
		new KeyboardCode(112, 59, 0, 58, "F1", KeysEnum.F1),
		new KeyboardCode(113, 60, 0, 59, "F2", KeysEnum.F2),
		new KeyboardCode(114, 61, 0, 60, "F3", KeysEnum.F3),
		new KeyboardCode(115, 62, 0, 61, "F4", KeysEnum.F4),
		new KeyboardCode(116, 63, 0, 62, "F5", KeysEnum.F5),
		new KeyboardCode(117, 64, 0, 63, "F6", KeysEnum.F6),
		new KeyboardCode(118, 65, 0, 64, "F7", KeysEnum.F7),
		new KeyboardCode(119, 66, 0, 65, "F8", KeysEnum.F8),
		new KeyboardCode(120, 67, 0, 66, "F9", KeysEnum.F9),
		new KeyboardCode(121, 68, 0, 67, "F10", KeysEnum.F10),
		new KeyboardCode(122, 87, 0, 68, "F11", KeysEnum.F11),
		new KeyboardCode(123, 88, 0, 69, "F12", KeysEnum.F12),
		new KeyboardCode(192, 41, 0, 53, "`", KeysEnum.Oemtilde),
		new KeyboardCode(49, 2, 0, 30, "1", KeysEnum.D1),
		new KeyboardCode(50, 3, 0, 31, "2", KeysEnum.D2),
		new KeyboardCode(51, 4, 0, 32, "3", KeysEnum.D3),
		new KeyboardCode(52, 5, 0, 33, "4", KeysEnum.D4),
		new KeyboardCode(53, 6, 0, 34, "5", KeysEnum.D5),
		new KeyboardCode(54, 7, 0, 35, "6", KeysEnum.D6),
		new KeyboardCode(55, 8, 0, 36, "7", KeysEnum.D7),
		new KeyboardCode(56, 9, 0, 37, "8", KeysEnum.D8),
		new KeyboardCode(57, 10, 0, 38, "9", KeysEnum.D9),
		new KeyboardCode(48, 11, 0, 39, "0", KeysEnum.D0),
		new KeyboardCode(189, 12, 0, 45, "-", KeysEnum.OemMinus),
		new KeyboardCode(187, 13, 0, 46, "=", KeysEnum.Oemplus),
		new KeyboardCode(8, 14, 0, 42, "←", KeysEnum.Back),
		new KeyboardCode(9, 15, 0, 43, "Tab", KeysEnum.Tab),
		new KeyboardCode(81, 16, 0, 20, "Q", KeysEnum.Q),
		new KeyboardCode(87, 17, 0, 26, "W", KeysEnum.W),
		new KeyboardCode(69, 18, 0, 8, "E", KeysEnum.E),
		new KeyboardCode(82, 19, 0, 21, "R", KeysEnum.R),
		new KeyboardCode(84, 20, 0, 23, "T", KeysEnum.T),
		new KeyboardCode(89, 21, 0, 28, "Y", KeysEnum.Y),
		new KeyboardCode(85, 22, 0, 24, "U", KeysEnum.U),
		new KeyboardCode(73, 23, 0, 12, "I", KeysEnum.I),
		new KeyboardCode(79, 24, 0, 18, "O", KeysEnum.O),
		new KeyboardCode(80, 25, 0, 19, "P", KeysEnum.P),
		new KeyboardCode(219, 26, 0, 47, "[", KeysEnum.OemOpenBrackets),
		new KeyboardCode(221, 27, 0, 48, "]", KeysEnum.OemCloseBrackets),
		new KeyboardCode(220, 43, 0, 49, "\\", KeysEnum.OemPipe),
		new KeyboardCode(20, 58, 0, 57, "CapsLock", KeysEnum.Capital),
		new KeyboardCode(65, 30, 0, 4, "A", KeysEnum.A),
		new KeyboardCode(83, 31, 0, 22, "S", KeysEnum.S),
		new KeyboardCode(68, 32, 0, 7, "D", KeysEnum.D),
		new KeyboardCode(70, 33, 0, 9, "F", KeysEnum.F),
		new KeyboardCode(71, 34, 0, 10, "G", KeysEnum.G),
		new KeyboardCode(72, 35, 0, 11, "H", KeysEnum.H),
		new KeyboardCode(74, 36, 0, 13, "J", KeysEnum.J),
		new KeyboardCode(75, 37, 0, 14, "K", KeysEnum.K),
		new KeyboardCode(76, 38, 0, 15, "L", KeysEnum.L),
		new KeyboardCode(186, 39, 0, 51, ";", KeysEnum.OemSemicolon),
		new KeyboardCode(222, 40, 0, 52, "'", KeysEnum.OemQuotes),
		new KeyboardCode(13, 28, 0, 40, "Enter", KeysEnum.Enter),
		new KeyboardCode(160, 42, 0, 2, HID_CODE_TYPE.Modify, "LShift", KeysEnum.LShiftKey),
		new KeyboardCode(90, 44, 0, 29, "Z", KeysEnum.Z),
		new KeyboardCode(88, 45, 0, 27, "X", KeysEnum.X),
		new KeyboardCode(67, 46, 0, 6, "C", KeysEnum.C),
		new KeyboardCode(86, 47, 0, 25, "V", KeysEnum.V),
		new KeyboardCode(66, 48, 0, 5, "B", KeysEnum.B),
		new KeyboardCode(78, 49, 0, 17, "N", KeysEnum.N),
		new KeyboardCode(77, 50, 0, 16, "M", KeysEnum.M),
		new KeyboardCode(188, 51, 0, 54, ",", KeysEnum.Oemcomma),
		new KeyboardCode(190, 52, 0, 55, ".", KeysEnum.OemPeriod),
		new KeyboardCode(191, 53, 0, 56, "/", KeysEnum.OemQuestion),
		new KeyboardCode(161, 54, 1, 32, HID_CODE_TYPE.Modify, "RShift", KeysEnum.RShiftKey),
		new KeyboardCode(162, 29, 0, 1, HID_CODE_TYPE.Modify, "LCtrl", KeysEnum.LControlKey),
		new KeyboardCode(91, 91, 1, 8, HID_CODE_TYPE.Modify, "LWin", KeysEnum.LWin),
		new KeyboardCode(164, 56, 32, 4, HID_CODE_TYPE.Modify, "LAlt", KeysEnum.LAlt),
		new KeyboardCode(164, 56, 0, 4, HID_CODE_TYPE.Modify, "LAlt", KeysEnum.LAlt),
		new KeyboardCode(32, 57, 0, 44, "Space", KeysEnum.Space),
		new KeyboardCode(165, 56, 33, 64, HID_CODE_TYPE.Modify, "RAlt", KeysEnum.RAlt),
		new KeyboardCode(165, 56, 1, 64, HID_CODE_TYPE.Modify, "RAlt", KeysEnum.RAlt),
		new KeyboardCode(92, 92, 1, 128, HID_CODE_TYPE.Modify, "RWin", KeysEnum.RWin),
		new KeyboardCode(93, 93, 1, 101, "Apps", KeysEnum.Apps),
		new KeyboardCode(163, 29, 1, 16, HID_CODE_TYPE.Modify, "RCtrl", KeysEnum.RControlKey),
		new KeyboardCode(44, 55, 1, 70, "Screen", KeysEnum.PrintScreen),
		new KeyboardCode(145, 70, 0, 71, "Scroll", KeysEnum.Scroll),
		new KeyboardCode(19, 69, 0, 72, "Pause", KeysEnum.Pause),
		new KeyboardCode(45, 82, 1, 73, "Insert", KeysEnum.Insert),
		new KeyboardCode(36, 71, 1, 74, "Home", KeysEnum.Home),
		new KeyboardCode(33, 73, 1, 75, "PageUp", KeysEnum.PageUp),
		new KeyboardCode(46, 83, 1, 76, "Del", KeysEnum.Delete),
		new KeyboardCode(35, 79, 1, 77, "End", KeysEnum.End),
		new KeyboardCode(34, 81, 1, 78, "PageDn", KeysEnum.PageDown),
		new KeyboardCode(38, 72, 1, 82, "↑", KeysEnum.Up),
		new KeyboardCode(37, 75, 1, 80, "←", KeysEnum.Left),
		new KeyboardCode(40, 80, 1, 81, "↓", KeysEnum.Down),
		new KeyboardCode(39, 77, 1, 79, "→", KeysEnum.Right),
		new KeyboardCode(144, 69, 1, 83, "NumLock", KeysEnum.NumLock),
		new KeyboardCode(111, 53, 1, 84, "Num/", KeysEnum.Divide),
		new KeyboardCode(106, 55, 0, 85, "Num*", KeysEnum.Multiply),
		new KeyboardCode(109, 74, 0, 86, "Num-", KeysEnum.Subtract),
		new KeyboardCode(103, 71, 0, 95, "Num7", KeysEnum.NumPad7),
		new KeyboardCode(104, 72, 0, 96, "Num8", KeysEnum.NumPad8),
		new KeyboardCode(105, 73, 0, 97, "Num9", KeysEnum.NumPad9),
		new KeyboardCode(107, 78, 0, 87, "Num+", KeysEnum.Add),
		new KeyboardCode(100, 75, 0, 92, "Num4", KeysEnum.NumPad4),
		new KeyboardCode(101, 76, 0, 93, "Num5", KeysEnum.NumPad5),
		new KeyboardCode(102, 77, 0, 94, "Num6", KeysEnum.NumPad6),
		new KeyboardCode(97, 79, 0, 89, "Num1", KeysEnum.NumPad1),
		new KeyboardCode(98, 80, 0, 90, "Num2", KeysEnum.NumPad2),
		new KeyboardCode(99, 81, 0, 91, "Num3", KeysEnum.NumPad3),
		new KeyboardCode(96, 82, 0, 98, "Num0", KeysEnum.NumPad0),
		new KeyboardCode(110, 83, 0, 99, "Num.", KeysEnum.Decimal),
		new KeyboardCode(13, 28, 1, 88, "Enter", KeysEnum.Return),
		new KeyboardCode(45, 82, 0, 73, "Insert", KeysEnum.Insert),
		new KeyboardCode(36, 71, 0, 74, "Home", KeysEnum.Home),
		new KeyboardCode(33, 73, 0, 75, "PageUp", KeysEnum.PageUp),
		new KeyboardCode(46, 83, 0, 76, "Del", KeysEnum.Delete),
		new KeyboardCode(35, 79, 0, 77, "End", KeysEnum.End),
		new KeyboardCode(34, 81, 0, 78, "PageDn", KeysEnum.PageDown),
		new KeyboardCode(38, 72, 0, 82, "↑", KeysEnum.Up),
		new KeyboardCode(37, 75, 0, 80, "←", KeysEnum.Left),
		new KeyboardCode(40, 80, 0, 81, "↓", KeysEnum.Down),
		new KeyboardCode(39, 77, 0, 79, "→", KeysEnum.Right),
		new KeyboardCode(12, 89, 0, 103, "=", KeysEnum.Clear),
		new KeyboardCode(172, 0, 17, 547, HID_CODE_TYPE.Media, "BrowserHome", KeysEnum.BrowserHome),
		new KeyboardCode(170, 0, 17, 545, HID_CODE_TYPE.Media, "BrowserSearch", KeysEnum.BrowserSearch),
		new KeyboardCode(183, 0, 17, 402, HID_CODE_TYPE.Media, "Calc", KeysEnum.CUST_Calc),
		new KeyboardCode(180, 0, 17, 394, HID_CODE_TYPE.Media, "Email", KeysEnum.LaunchMail),
		new KeyboardCode(181, 0, 17, 387, HID_CODE_TYPE.Media, "Media", KeysEnum.SelectMedia),
		new KeyboardCode(178, 0, 17, 183, HID_CODE_TYPE.Media, "Stop", KeysEnum.MediaStop),
		new KeyboardCode(182, 0, 17, 404, HID_CODE_TYPE.Media, "Computer", KeysEnum.CUST_Computer),
		new KeyboardCode(171, 0, 17, 554, HID_CODE_TYPE.Media, "Favorite", KeysEnum.BrowserFavorites),
		new KeyboardCode(166, 0, 17, 548, HID_CODE_TYPE.Media, "BrowserBack", KeysEnum.BrowserBack),
		new KeyboardCode(167, 0, 17, 549, HID_CODE_TYPE.Media, "BrowserForward", KeysEnum.BrowserForward),
		new KeyboardCode(168, 0, 17, 551, HID_CODE_TYPE.Media, "BrowserRefresh", KeysEnum.BrowserRefresh),
		new KeyboardCode(169, 0, 17, 550, HID_CODE_TYPE.Media, "BrowserStop", KeysEnum.BrowserStop),
		new KeyboardCode(173, 0, 17, 226, HID_CODE_TYPE.Media, "Mute", KeysEnum.VolumeMute),
		new KeyboardCode(174, 0, 17, 234, HID_CODE_TYPE.Media, "Vol-", KeysEnum.VolumeDown),
		new KeyboardCode(175, 0, 17, 233, HID_CODE_TYPE.Media, "Vol+", KeysEnum.VolumeUp),
		new KeyboardCode(178, 0, 17, 183, HID_CODE_TYPE.Media, "Stop", KeysEnum.MediaStop),
		new KeyboardCode(179, 0, 17, 205, HID_CODE_TYPE.Media, "Play", KeysEnum.MediaPlayPause),
		new KeyboardCode(177, 0, 17, 182, HID_CODE_TYPE.Media, "Prtk", KeysEnum.MediaPreviousTrack),
		new KeyboardCode(176, 0, 17, 181, HID_CODE_TYPE.Media, "Netk", KeysEnum.MediaNextTrack),
		new KeyboardCode(0, 0, 0, 50, "K42", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 100, "K45", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 133, "K107", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 135, "K56 -\\ろ", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 136, "Roma 力夕力ナ", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 137, "K14 |    ¥", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 139, "K131 無変換", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 138, "K132 変換", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 145, "K150 한사 ", KeysEnum.None),
		new KeyboardCode(0, 0, 0, 144, "K151 한/영 かな", KeysEnum.None)
	};

	public static KeyboardCode FindKeyboardCode(string keyChar)
	{
		for (int i = 0; i < KeyboardCodeDataBase.Length; i++)
		{
			if (KeyboardCodeDataBase[i].keyChar == keyChar)
			{
				return new KeyboardCode(KeyboardCodeDataBase[i].vkCode, KeyboardCodeDataBase[i].scanCode, KeyboardCodeDataBase[i].flags, KeyboardCodeDataBase[i].hidCode, KeyboardCodeDataBase[i].hidCodeType, KeyboardCodeDataBase[i].keyChar, KeyboardCodeDataBase[i].keyCode);
			}
		}
		return new KeyboardCode(0, 0, 0, 0, "", KeysEnum.None);
	}

	public static KeyboardCode FindKeyboardCode(KeysEnum keyCode)
	{
		for (int i = 0; i < KeyboardCodeDataBase.Length; i++)
		{
			if (KeyboardCodeDataBase[i].keyCode == keyCode)
			{
				return new KeyboardCode(KeyboardCodeDataBase[i].vkCode, KeyboardCodeDataBase[i].scanCode, KeyboardCodeDataBase[i].flags, KeyboardCodeDataBase[i].hidCode, KeyboardCodeDataBase[i].keyChar, KeyboardCodeDataBase[i].keyCode);
			}
		}
		return new KeyboardCode(0, 0, 0, 0, "", KeysEnum.None);
	}

	public static KeyboardCode FindKeyboardCode(int hidKeyCode, int hidCodeType)
	{
		for (int i = 0; i < KeyboardCodeDataBase.Length; i++)
		{
			if (KeyboardCodeDataBase[i].hidCode == hidKeyCode && KeyboardCodeDataBase[i].hidCodeType == (HID_CODE_TYPE)hidCodeType)
			{
				return new KeyboardCode(KeyboardCodeDataBase[i].vkCode, KeyboardCodeDataBase[i].scanCode, KeyboardCodeDataBase[i].flags, KeyboardCodeDataBase[i].hidCode, KeyboardCodeDataBase[i].hidCodeType, KeyboardCodeDataBase[i].keyChar, KeyboardCodeDataBase[i].keyCode);
			}
		}
		return new KeyboardCode(0, 0, 0, 0, "", KeysEnum.None);
	}

	public static KeyboardCode FindKeyboardCode(GlobalHook.KeyboardHookStruct hookStruct)
	{
		return FindKeyboardCode(hookStruct.vkCode, hookStruct.scanCode, hookStruct.flags);
	}

	public static KeyboardCode FindKeyboardCode(int vkCode, int scanCode, int flags)
	{
		int num = flags & 0x7F;
		for (int i = 0; i < KeyboardCodeDataBase.Length; i++)
		{
			if (KeyboardCodeDataBase[i].vkCode == vkCode && KeyboardCodeDataBase[i].scanCode == scanCode && KeyboardCodeDataBase[i].flags == num)
			{
				return new KeyboardCode(vkCode, scanCode, num, KeyboardCodeDataBase[i].hidCode, KeyboardCodeDataBase[i].hidCodeType, KeyboardCodeDataBase[i].keyChar, KeyboardCodeDataBase[i].keyCode);
			}
		}
		return new KeyboardCode(vkCode, scanCode, flags, 0, "", KeysEnum.None);
	}
}
