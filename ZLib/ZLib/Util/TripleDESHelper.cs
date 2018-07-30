using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ZLib.Util
{
	public static class TripleDESHelper
	{
		/// <summary>
		/// 以默认模式对源解密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] clear, TripleDesKey desKey)
		{
			return Decrypt(clear, desKey.Key.ToArray(), desKey.IV.ToArray());
		}

		/// <summary>
		/// 以默认模式对源加密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] clear, byte[] rgbKey, byte[] rgbIV)
		{
			using (var _dsp = new TripleDESCryptoServiceProvider())
			{
				using (var _ms = new MemoryStream())
				{
					using (var _cs = new CryptoStream(_ms, _dsp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
					{
						_cs.Write(clear, 0, clear.Length);
						_cs.FlushFinalBlock();
						return _ms.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// 以默认模式对源加密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public static byte[] Encrypt(string clearText, TripleDesKey desKey)
		{
			return Encrypt(clearText, desKey.Key.ToArray(), desKey.IV.ToArray());
		}

		/// <summary>
		/// 将源文以 UTF-8 编码后加密
		/// </summary>
		/// <param name="clearText"></param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public static byte[] Encrypt(string clearText, byte[] rgbKey, byte[] rgbIV)
		{
			byte[] bs = Encoding.UTF8.GetBytes(clearText);
			return Encrypt(bs, rgbKey, rgbIV);
		}

		/// <summary>
		/// 以默认模式对源加密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] clear, byte[] rgbKey, byte[] rgbIV)
		{
			using (var _tdsp = new TripleDESCryptoServiceProvider())
			{
				using (var _ms = new MemoryStream())
				{
					using (var _cs = new CryptoStream(_ms, _tdsp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
					{
						_cs.Write(clear, 0, clear.Length);
						_cs.FlushFinalBlock();
						return _ms.ToArray();
					}
				}
			}
		}

		/// <summary>
		/// 将字符串转为 64bit 的密钥和 64bit 的初始化向量
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static TripleDesKey MakeKey(string key)
		{
			TripleDesKey _key = new TripleDesKey();
			string _sKey = Regex.Replace(key, "[^0-9a-zA-Z]", "$")
				.PadRight(16, '@');
			_key.Key = Encoding.ASCII.GetBytes(_sKey.Substring(0, 8));
			_key.IV = Encoding.ASCII.GetBytes(_sKey.Substring(8, 8));
			return _key;
		}

		/// <summary>
		/// 将字符串转为 168bit 的密钥和 64bit 的初始化向量
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static byte[] GetBytes(string key)
		{
			return Encoding.ASCII.GetBytes(key);
		}

		public class TripleDesKey
		{
			/// <summary>
			/// 64 bit 的密钥
			/// </summary>
			public IEnumerable<byte> Key { get; set; }
			/// <summary>
			/// 64 bit 的向量
			/// </summary>
			public IEnumerable<byte> IV { get; set; }
		}
	}
}
