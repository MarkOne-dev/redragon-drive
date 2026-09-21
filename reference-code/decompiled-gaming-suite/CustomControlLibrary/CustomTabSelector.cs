using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomTabSelector : Control
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static TabControlEventHandler _003C_003E9__12_0;

		internal void _003Cset_TabControl_003Eb__12_0(object sender, TabControlEventArgs args)
		{
		}
	}

	private Size _ItemSize = new Size(60, 40);

	private TabControl _TabControl;

	private Image _NormalImage;

	private Image _MouseEnterImage;

	private Image _CheckedImage;

	private List<Rectangle> _tabRects;

	private MouseStatus[] mouseStatus = new MouseStatus[5]
	{
		MouseStatus.Leave,
		MouseStatus.Leave,
		MouseStatus.Leave,
		MouseStatus.Leave,
		MouseStatus.Leave
	};

	public override Color BackColor { get; set; } = Color.Transparent;

	[Category("自定义")]
	[Description("页头的大小")]
	public Size ItemSize
	{
		get
		{
			return _ItemSize;
		}
		set
		{
			if (_ItemSize != value)
			{
				_ItemSize = value;
				((Control)this).Invalidate();
			}
		}
	}

	[Category("自定义")]
	[Description("需要加载的导航栏")]
	public TabControl TabControl
	{
		get
		{
			return _TabControl;
		}
		set
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Expected O, but got Unknown
			if (_TabControl == value)
			{
				return;
			}
			_TabControl = value;
			((Control)this).Invalidate();
			if (_TabControl != null)
			{
				TabControl tabControl = _TabControl;
				object obj = _003C_003Ec._003C_003E9__12_0;
				if (obj == null)
				{
					TabControlEventHandler val = delegate
					{
					};
					_003C_003Ec._003C_003E9__12_0 = val;
					obj = (object)val;
				}
				tabControl.Deselected += (TabControlEventHandler)obj;
				_TabControl.SelectedIndexChanged += delegate
				{
					((Control)this).Invalidate();
				};
				((Control)_TabControl).ControlAdded += (ControlEventHandler)delegate
				{
					((Control)this).Invalidate();
				};
				((Control)_TabControl).ControlRemoved += (ControlEventHandler)delegate
				{
					((Control)this).Invalidate();
				};
			}
		}
	}

	public Image NormalImage
	{
		get
		{
			return _NormalImage;
		}
		set
		{
			_NormalImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image MouseEnterImage
	{
		get
		{
			return _MouseEnterImage;
		}
		set
		{
			_MouseEnterImage = value;
			((Control)this).Invalidate();
		}
	}

	public Image CheckedImage
	{
		get
		{
			return _CheckedImage;
		}
		set
		{
			_CheckedImage = value;
			((Control)this).Invalidate();
		}
	}

	public CustomTabSelector()
	{
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)196608, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		((Control)this).CreateControl();
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		int val = 0;
		int val2 = 0;
		val = Math.Max(val, width);
		val2 = Math.Max(val2, height);
		if (_TabControl != null && _TabControl.TabCount > 0)
		{
			_ItemSize.Width = val / _TabControl.TabCount;
			_ItemSize.Height = height;
		}
		((Control)this).SetBoundsCore(x, y, val, val2, specified);
	}

	protected override void OnResize(EventArgs e)
	{
		((Control)this).OnResize(e);
		if (_TabControl != null && _TabControl.TabCount > 0)
		{
			_ItemSize.Width = ((Control)this).Width / _TabControl.TabCount;
			_ItemSize.Height = ((Control)this).Height;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		((Control)this).OnPaint(e);
		e.Graphics.SmoothingMode = (SmoothingMode)2;
		Graphics graphics = e.Graphics;
		UpdateTabRects();
		PaintBackground(graphics);
	}

	private void PaintBackground(Graphics g)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00cc: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Expected O, but got Unknown
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_01fe: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_0176: Expected O, but got Unknown
		if (_TabControl == null || _NormalImage == null)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			Rectangle rectangle = new Rectangle(_ItemSize.Width * i, 0, _ItemSize.Width, _ItemSize.Height);
			if (_NormalImage != null && _MouseEnterImage != null && _CheckedImage != null)
			{
				Image val = _NormalImage;
				if (i == _TabControl.SelectedIndex)
				{
					val = _CheckedImage;
				}
				else if (mouseStatus[i] == MouseStatus.Enter)
				{
					val = _MouseEnterImage;
				}
				Bitmap val2 = new Bitmap(((Control)this).Width, ((Control)this).Height);
				Graphics obj = Graphics.FromImage((Image)val2);
				obj.InterpolationMode = (InterpolationMode)7;
				obj.DrawImage(val, 0, 0, ((Control)this).Width, ((Control)this).Height);
				obj.Dispose();
				val = (Image)val2;
				if (_TabControl.TabCount == 4 && i == 4)
				{
					val = null;
				}
				else if (_TabControl.TabCount == 4 && i == 3)
				{
					Image val3 = null;
					Bitmap val4 = new Bitmap(_ItemSize.Width, _ItemSize.Height, (PixelFormat)139273);
					Graphics obj2 = Graphics.FromImage((Image)val4);
					Rectangle rectangle2 = new Rectangle(((Control)this).Width / 5 * 4, 0, ((Control)this).Width / 5, val.Height);
					Rectangle rectangle3 = new Rectangle(0, 0, _ItemSize.Width, _ItemSize.Height);
					obj2.DrawImage(val, rectangle3, rectangle2, (GraphicsUnit)2);
					PixelProcess(val4);
					val3 = (Image)val4;
					g.DrawImage(val3, rectangle);
				}
				else
				{
					Image val5 = null;
					Bitmap val6 = new Bitmap(_ItemSize.Width, _ItemSize.Height, (PixelFormat)139273);
					Graphics obj3 = Graphics.FromImage((Image)val6);
					Rectangle rectangle4 = new Rectangle(((Control)this).Width / 5 * i, 0, ((Control)this).Width / 5, val.Height);
					Rectangle rectangle5 = new Rectangle(0, 0, _ItemSize.Width, _ItemSize.Height);
					obj3.DrawImage(val, rectangle5, rectangle4, (GraphicsUnit)2);
					PixelProcess(val6);
					val5 = (Image)val6;
					g.DrawImage(val5, rectangle);
				}
			}
		}
	}

	public static void PixelProcess(Bitmap bmp)
	{
		Color pixel = bmp.GetPixel(0, 0);
		bmp.MakeTransparent(pixel);
	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		((Control)this).OnMouseUp(e);
		if (_TabControl == null)
		{
			return;
		}
		for (int i = 0; i < _TabControl.TabCount; i++)
		{
			if (_tabRects[i].Contains(e.Location))
			{
				_TabControl.SelectedIndex = i;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		if (_TabControl == null || _tabRects == null)
		{
			return;
		}
		for (int i = 0; i < _TabControl.TabCount; i++)
		{
			if (_tabRects[i].Contains(e.Location))
			{
				mouseStatus[i] = MouseStatus.Enter;
				((Control)this).Invalidate();
			}
			else
			{
				mouseStatus[i] = MouseStatus.Leave;
				((Control)this).Invalidate();
			}
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		for (int i = 0; i < mouseStatus.Length; i++)
		{
			mouseStatus[i] = MouseStatus.Leave;
		}
		((Control)this).Invalidate();
	}

	private void UpdateTabRects()
	{
		_tabRects = new List<Rectangle>();
		if (_TabControl != null)
		{
			_tabRects.Clear();
			_tabRects.Add(new Rectangle(0, 0, _ItemSize.Width, _ItemSize.Height));
			for (int i = 1; i < _TabControl.TabCount; i++)
			{
				_tabRects.Add(new Rectangle(_tabRects[i - 1].Right, 0, _ItemSize.Width, _ItemSize.Height));
			}
		}
	}
}
