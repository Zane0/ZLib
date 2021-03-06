﻿using System;
using System.Globalization;
using System.Net;

namespace ZLib.Util
{
	/// <summary>
	/// IP 地址类型转换
	/// </summary>
	public static class IPConvert
	{
		/// <summary>
		/// 将 IP 字符串转为 long
		/// </summary>
		/// <param name="ipstr"></param>
		/// <returns></returns>
		public static long ConvertToInt64(string ipstr)
		{
			return ConvertToInt64(IPAddress.Parse((ipstr)));
		}

		/// <summary>
		/// 将 IPAddress 类型的对象转为 long
		/// </summary>
		/// <param name="ipadd"></param>
		/// <returns></returns>
		public static long ConvertToInt64(IPAddress ipadd)
		{
			return ConvertToInt64(ipadd.GetAddressBytes());
		}

		/// <summary>
		/// 将 IPv4 字节数组转为 long
		/// </summary>
		/// <param name="ipbytes"></param>
		/// <returns></returns>
		public static long ConvertToInt64(byte[] ipbytes)
		{
			long l = 0;
			for (int i = 0; i < ipbytes.Length; i++)
			{
				l = (l << 8) | ipbytes[i];
			}
			return l;
		}

		/// <summary>
		/// 将长整形转为字节数组，相当于 new IPAddress(ladd).GetAddressBytes()
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
			return BitConverter.GetBytes(ConvertToInt64(ipstr));
		}

		/// <summary>
		/// 将长整形转为 IP 字符串
		/// </summary>
		/// <param name="ladd"></param>
		/// <returns></returns>
		public static string ConvertToString(long ladd)
		{
			byte[] bs = IPConvert.ConvertToBytes(ladd);
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", bs[3], bs[2], bs[1], bs[0]);
		}
	}
}
