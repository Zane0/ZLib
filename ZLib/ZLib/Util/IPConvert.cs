using System;
using System.Net;

namespace ZLib.Util
{
	/// <summary>
	/// IP 地址类型转换
	/// </summary>
	public class IPConvert
	{
		/// <summary>
		/// 将 IP 字符串转为 long
		/// </summary>
		/// <param name="ipstr"></param>
		/// <returns></returns>
		public static long ConvertToLong(string ipstr)
		{
			return ConvertToLong(IPAddress.Parse((ipstr)));
		}

		/// <summary>
		/// 将 IPAddress 类型的对象转为 long
		/// </summary>
		/// <param name="ipadd"></param>
		/// <returns></returns>
		public static long ConvertToLong(IPAddress ipadd)
		{
			return ConvertToLong(ipadd.GetAddressBytes());
		}

		/// <summary>
		/// 将字节数组转为 long
		/// </summary>
		/// <param name="ipbytes"></param>
		/// <returns></returns>
		public static long ConvertToLong(byte[] ipbytes)
		{
			long l = 0;
			for (int i = 0; i < ipbytes.Length; i++)
			{
				l = (l << 8) + (ushort)ipbytes[i];
			}
			return l;
		}

		/// <summary>
		/// 将长整形转为字节数组
		/// </summary>
		/// <param name="ladd"></param>
		/// <returns></returns>
		public static byte[] ConvertToBytes(long ladd)
		{
			return BitConverter.GetBytes(ladd);
		}

		/// <summary>
		/// 将 IP 字符串转为字节数组
		/// </summary>
		/// <param name="ladd"></param>
		/// <returns></returns>
		public static byte[] ConvertToBytes(string ipstr)
		{
			return BitConverter.GetBytes(ConvertToLong(ipstr));
		}

		/// <summary>
		/// 将长整形转为 IP 字符串
		/// </summary>
		/// <param name="ladd"></param>
		/// <returns></returns>
		public static string ConvertToString(long ladd)
		{
			byte[] bs = IPConvert.ConvertToBytes(ladd);
			return string.Format("{0}.{1}.{2}.{3}", bs[3], bs[2], bs[1], bs[0]);
		}
	}
}
