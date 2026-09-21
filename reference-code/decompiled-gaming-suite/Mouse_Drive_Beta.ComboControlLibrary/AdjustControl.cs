using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomControlLibrary;
using Mouse_Drive_Beta.Properties;

namespace Mouse_Drive_Beta.ComboControlLibrary;

public class AdjustControl : UserControl
{
	public delegate void ValueChangeEventHandler(object sender, CustomEventArgs e);

	private int _MaxValue = 255;

	private int _MinValue;

	private int _Value;

	private int _Step = 50;

	private IContainer components;

	private CustomButton customButton_Sub;

	private TextBox textBox1;

	private CustomButton customButton_Add;

	[Category("自定义")]
	[Description("输入框的背景色")]
	public Color TextBoxBackColor
	{
		set
		{
			((Control)textBox1).BackColor = value;
		}
	}

	[Category("自定义")]
	[Description("输入框限制最大值")]
	public int MaxValue
	{
		get
		{
			return _MaxValue;
		}
		set
		{
			_MaxValue = value;
		}
	}

	[Category("自定义")]
	[Description("输入框限制最小值")]
	public int MinValue
	{
		get
		{
			return _MinValue;
		}
		set
		{
			_MinValue = value;
		}
	}

	[Category("自定义")]
	[Description("输入框限制的值")]
	public int Value
	{
		get
		{
			return _Value;
		}
		set
		{
			_Value = value;
			((Control)textBox1).Text = _Value.ToString();
			UpdateText();
		}
	}

	[Category("自定义")]
	[Description("步进")]
	public int Step
	{
		get
		{
			return _Step;
		}
		set
		{
			_Step = value;
		}
	}

	public Image SubImage
	{
		get
		{
			return customButton_Sub.NormalImage;
		}
		set
		{
			customButton_Sub.NormalImage = value;
			customButton_Sub.MouseDownImage = value;
			customButton_Sub.MouseEnterImage = value;
		}
	}

	public Image AddImage
	{
		get
		{
			return customButton_Add.NormalImage;
		}
		set
		{
			customButton_Add.NormalImage = value;
			customButton_Add.MouseDownImage = value;
			customButton_Add.MouseEnterImage = value;
		}
	}

	public event ValueChangeEventHandler ValueChange;

	public AdjustControl()
	{
		InitializeComponent();
	}

	private void UpdateText()
	{
		CustomEventArgs e = new CustomEventArgs(_Value);
		ValueChange?.Invoke(this, e);
	}

	private void customButton_Sub_Click(object sender, EventArgs e)
	{
		if (_Value > _MinValue)
		{
			_Value -= _Step;
		}
		((Control)textBox1).Text = _Value.ToString();
	}

	private void customButton_Add_Click(object sender, EventArgs e)
	{
		if (_Value < _MaxValue)
		{
			_Value += _Step;
		}
		((Control)textBox1).Text = _Value.ToString();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		if (((Control)textBox1).Text == "")
		{
			_Value = 0;
		}
		else
		{
			_Value = int.Parse(((Control)textBox1).Text);
			if (_Value > _MaxValue)
			{
				_Value = _MaxValue;
				((Control)textBox1).Text = _MaxValue.ToString();
			}
		}
		UpdateText();
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (e.KeyChar != '\b' && !char.IsNumber(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		textBox1 = new TextBox();
		customButton_Add = new CustomButton();
		customButton_Sub = new CustomButton();
		((Control)this).SuspendLayout();
		((Control)textBox1).BackColor = Color.FromArgb(57, 57, 57);
		((TextBoxBase)textBox1).BorderStyle = (BorderStyle)1;
		((Control)textBox1).Font = new Font("微软雅黑", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)textBox1).ForeColor = Color.White;
		((Control)textBox1).Location = new Point(25, 2);
		((Control)textBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(58, 23);
		((Control)textBox1).TabIndex = 1;
		textBox1.TextAlign = (HorizontalAlignment)2;
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)textBox1).KeyPress += new KeyPressEventHandler(textBox1_KeyPress);
		((Control)customButton_Add).Location = new Point(90, 7);
		((Control)customButton_Add).Margin = new Padding(4, 5, 4, 5);
		customButton_Add.MouseDownImage = (Image)(object)Resources.加键使能;
		customButton_Add.MouseEnterImage = (Image)(object)Resources.加键使能;
		((Control)customButton_Add).Name = "customButton_Add";
		customButton_Add.NormalImage = (Image)(object)Resources.加键使能;
		((Control)customButton_Add).Size = new Size(14, 14);
		((Control)customButton_Add).TabIndex = 2;
		((Control)customButton_Add).Click += customButton_Add_Click;
		((Control)customButton_Sub).Location = new Point(4, 12);
		((Control)customButton_Sub).Margin = new Padding(4, 5, 4, 5);
		customButton_Sub.MouseDownImage = (Image)(object)Resources.减键使能;
		customButton_Sub.MouseEnterImage = (Image)(object)Resources.减键使能;
		((Control)customButton_Sub).Name = "customButton_Sub";
		customButton_Sub.NormalImage = (Image)(object)Resources.减键使能;
		((Control)customButton_Sub).Size = new Size(14, 4);
		((Control)customButton_Sub).TabIndex = 0;
		((Control)customButton_Sub).Click += customButton_Sub_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)customButton_Add);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)customButton_Sub);
		((Control)this).Font = new Font("微软雅黑", 10.5f, (FontStyle)0, (GraphicsUnit)3, (byte)134);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "AdjustControl";
		((Control)this).Size = new Size(108, 27);
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
