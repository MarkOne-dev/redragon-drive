using System;

namespace CustomControlLibrary;

public class CustomEventArgs : EventArgs
{
	public object Value { get; set; }

	public CustomEventArgs(object value)
	{
		Value = value;
	}
}
