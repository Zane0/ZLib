using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ZLib.Util
{
	public class TripleDESHelper
	{
		/// <summary>
		/// 以默认模式对源解密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public byte[] Decrypt(byte[] clear, TripleDESKey desKey)
		{
			return Decrypt(clear, desKey.Key, desKey.IV);
		}

		/// <summary>
		/// 以默认模式对源加密，默认运算模式（CBC）默认填充模式（PKCS7）
		/// </summary>
		/// <param name="clear">源</param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public byte[] Decrypt(byte[] clear, byte[] rgbKey, byte[] rgbIV)
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
		public byte[] Encrypt(string clearText, TripleDESKey desKey)
		{
			return Encrypt(clearText, desKey.Key, desKey.IV);
		}

		/// <summary>
		/// 将源文以 UTF-8 编码后加密
		/// </summary>
		/// <param name="clearText"></param>
		/// <param name="rgbKey">密钥</param>
		/// <returns></returns>
		public byte[] Encrypt(string clearText, byte[] rgbKey, byte[] rgbIV)
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
		public byte[] Encrypt(byte[] clear, byte[] rgbKey, byte[] rgbIV)
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
		public TripleDESKey MakeKey(string key)
		{
			TripleDESKey _key = new TripleDESKey();
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
		public byte[] GetBytes(string key)
		{
			return Encoding.ASCII.GetBytes(key);
		}

		public class TripleDESKey
		{
			/// <summary>
			/// 64 bit 的密钥
			/// </summary>
			public byte[] Key { get; set; }
			/// <summary>
			/// 64 bit 的向量
			/// </summary>
			public byte[] IV { get; set; }
		}
	}
}
