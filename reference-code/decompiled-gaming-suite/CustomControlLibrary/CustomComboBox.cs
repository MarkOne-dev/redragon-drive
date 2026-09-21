using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace CustomControlLibrary;

public class CustomComboBox : UserControl
{
	public enum ArrowDirectionEnum
	{
		Down,
		Right
	}

	private ArrowDirectionEnum _ArrowDirection;

	private Image _ArrowImageNoraml;

	private Color _SelectItemColor;

	private Color _UnselectItemColor;

	private ComboBox _ComboBox;

	private Label _Label;

	private Button _Button;

	public override Color BackColor
	{
		get
		{
			return ((Control)_ComboBox).BackColor;
		}
		set
		{
			((Control)_Button).BackColor = value;
			((Control)_Label).BackColor = value;
			((Control)_ComboBox).BackColor = value;
			((Control)this).Invalidate();
		}
	}

	public override Color ForeColor
	{
		get
		{
			return ((Control)_ComboBox).ForeColor;
		}
		set
		{
			((Control)_Button).ForeColor = value;
			((Control)_Label).ForeColor = value;
			((Control)_ComboBox).ForeColor = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("箭头方向\r\nDown：向下，下拉框；Right：向右：右拉框")]
	public ArrowDirectionEnum ArrowDirection
	{
		get
		{
			return _ArrowDirection;
		}
		set
		{
			_ArrowDirection = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("箭头图片")]
	public Image ArrowImageNoraml
	{
		get
		{
			return _ArrowImageNoraml;
		}
		set
		{
			_ArrowImageNoraml = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[Description("选项框选中的颜色")]
	public Color SelectItemColor
	{
		get
		{
			return _SelectItemColor;
		}
		set
		{
			_SelectItemColor = value;
		}
	}

	[Category("自定义")]
	[Description("选项框选中的颜色")]
	public Color UnselectItemColor
	{
		get
		{
			return _UnselectItemColor;
		}
		set
		{
			_UnselectItemColor = value;
		}
	}

	public int SelectIndex
	{
		get
		{
			return ((ListControl)_ComboBox).SelectedIndex;
		}
		set
		{
			((ListControl)_ComboBox).SelectedIndex = value;
			((Control)this).Invalidate();
		}
	}

	[Category("自定义")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Localizable(true)]
	[MergableProperty(false)]
	public ObjectCollection Item => _ComboBox.Items;

	public event EventHandler OnSelectedIndexChanged;

	public CustomComboBox()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		_SelectItemColor = Color.FromArgb(119, 119, 119);
		_UnselectItemColor = Color.FromArgb(63, 63, 63);
		((UserControl)this)._002Ector();
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)2048, true);
		_ComboBox = new ComboBox();
		_Label = new Label();
		_Button = new Button();
		DrawControl(falg: true);
		AdjustComboBoxLocation();
	}

	private void DrawControl(bool falg)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		((Control)this).SuspendLayout();
		((Control)_ComboBox).BackColor = ((Control)this).BackColor;
		((Control)_ComboBox).Width = ((Control)this).Width;
		((Control)_ComboBox).Font = new Font(((Control)this).Font.Name, ((Control)this).Font.Size);
		((Control)_ComboBox).ForeColor = ((Control)this).ForeColor;
		((Control)_ComboBox).Size = ((Control)this).Size;
		_ComboBox.DropDownStyle = (ComboBoxStyle)2;
		if (falg)
		{
			_ComboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
			((Control)_ComboBox).TextChanged += ComboBox_TextChanged;
		}
		_ComboBox.DrawMode = (DrawMode)2;
		_ComboBox.DrawItem += new DrawItemEventHandler(ComboBox_DrawItem);
		((Control)_Button).Dock = (DockStyle)4;
		((ButtonBase)_Button).FlatStyle = (FlatStyle)0;
		((ButtonBase)_Button).FlatAppearance.BorderSize = 0;
		((ButtonBase)_Button).FlatAppearance.MouseDownBackColor = ((Control)this).BackColor;
		((ButtonBase)_Button).FlatAppearance.MouseOverBackColor = ((Control)this).BackColor;
		((ButtonBase)_Button).FlatAppearance.CheckedBackColor = ((Control)this).BackColor;
		((Control)_Button).BackColor = ((Control)this).BackColor;
		((Control)_Button).Size = new Size(30, 30);
		((Control)_Button).Cursor = Cursors.Default;
		if (falg)
		{
			((Control)_Button).Click += Icon_Click;
			((Control)_Button).Paint += new PaintEventHandler(Icon_Paint);
		}
		((Control)_Label).Dock = (DockStyle)5;
		((Control)_Label).AutoSize = false;
		((Control)_Label).BackColor = ((Control)this).BackColor;
		_Label.TextAlign = (ContentAlignment)32;
		((Control)_Label).Padding = new Padding(8, 0, 0, 0);
		((Control)_Label).Font = new Font(((Control)this).Font.Name, ((Control)this).Font.Size);
		((Control)_Label).ForeColor = ((Control)this).ForeColor;
		((Control)_Label).Size = new Size(((Control)this).Width, ((Control)this).Height);
		if (falg)
		{
			((Control)_Label).Click += Surface_Click;
			((Control)_Label).MouseEnter += Surface_MouseEnter;
			((Control)_Label).MouseLeave += Surface_MouseLeave;
		}
		((Control)this).Controls.Add((Control)(object)_Label);
		((Control)this).Controls.Add((Control)(object)_Button);
		((Control)this).Controls.Add((Control)(object)_ComboBox);
		((Control)this).Size = new Size(((Control)this).Width, ((Control)this).Height);
		((Control)this).ForeColor = ((Control)this).ForeColor;
		((Control)this).Font = new Font(((Control)this).Font.Name, ((Control)this).Font.Size);
		((Control)this).BackColor = ((Control)this).BackColor;
		((Control)this).ResumeLayout();
	}

	private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		e.DrawBackground();
		e.DrawFocusRectangle();
		if (e.Index >= 0)
		{
			Color color = _UnselectItemColor;
			if ((e.State & 1) != 0)
			{
				color = _SelectItemColor;
			}
			Rectangle bounds = e.Bounds;
			SolidBrush val = new SolidBrush(color);
			Graphics graphics = e.Graphics;
			graphics.DrawRectangle(new Pen(Color.Transparent), bounds);
			graphics.FillRectangle((Brush)(object)val, bounds);
			graphics.DrawString(_ComboBox.Items[e.Index].ToString(), ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (float)bounds.X, (float)bounds.Y);
		}
	}

	private void Icon_Paint(object sender, PaintEventArgs e)
	{
		if (_ArrowImageNoraml != null)
		{
			int num = ((Control)this).Height - _ArrowImageNoraml.Height;
			Rectangle rectangle = new Rectangle(e.ClipRectangle.X + _ArrowImageNoraml.Width, num / 2, _ArrowImageNoraml.Width, _ArrowImageNoraml.Height);
			e.Graphics.DrawImage(_ArrowImageNoraml, rectangle);
		}
	}

	protected override void OnResize(EventArgs e)
	{
		((UserControl)this).OnResize(e);
		DrawControl(falg: false);
		AdjustComboBoxLocation();
	}

	private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (OnSelectedIndexChanged != null)
		{
			OnSelectedIndexChanged(sender, e);
		}
		((Control)_Label).Text = ((Control)_ComboBox).Text;
	}

	private void ComboBox_TextChanged(object sender, EventArgs e)
	{
		((Control)_Label).ForeColor = ((Control)this).ForeColor;
		((Control)_Label).Text = ((Control)_ComboBox).Text;
	}

	private void Icon_Click(object sender, EventArgs e)
	{
		((Control)_ComboBox).Select();
		_ComboBox.DroppedDown = true;
	}

	private void Surface_Click(object sender, EventArgs e)
	{
		((Control)this).OnClick(e);
		((Control)_ComboBox).Select();
		_ComboBox.DroppedDown = true;
	}

	private void Surface_MouseLeave(object sender, EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
	}

	private void Surface_MouseEnter(object sender, EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
	}

	private void AdjustComboBoxLocation()
	{
		if (_ArrowDirection == ArrowDirectionEnum.Down)
		{
			((Control)_ComboBox).Location = new Point
			{
				X = 0,
				Y = ((Control)this).Height - ((Control)this).Font.Height - 8
			};
		}
		else
		{
			((Control)_ComboBox).Location = new Point
			{
				X = ((Control)_ComboBox).Width + 1,
				Y = 0
			};
		}
	}
}
