using System.Runtime.InteropServices;

namespace Mouse_Drive_Beta;

public class AesEncryption
{
	[DllImport("AES.dll", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]
	private static extern byte[] CS_AESEncrypt(byte[] binFile, int binArraySize);

	public static byte[] AESEncrypt(byte[] binFile)
	{
		return CS_AESEncrypt(binFile, binFile.Length);
	}

	[DllImport("AES.dll", CallingConvention = CallingConvention.Cdecl)]
	[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UI1)]
	private static extern byte[] CS_AESDecrypt(byte[] binFile, int binArraySize);

	public static byte[] AESDecrypt(byte[] binFile)
	{
		return CS_AESDecrypt(binFile, binFile.Length);
	}
}
