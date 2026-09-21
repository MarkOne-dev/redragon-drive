using System.Collections.Generic;
using DriverLib;

namespace Mouse_Drive_Beta;

public struct DeviceAllInfo
{
	public string deviceString;

	public string VID;

	public string PID;

	public DeviceInfo deviceInfo;

	public int deviceIndex;

	public bool online;

	public bool isUSB;

	public string DongleVersion;

	public string MouseVersion;

	public List<byte> address;
}
