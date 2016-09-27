using System.Text.RegularExpressions;

namespace ZLib.Util
{
	/// <summary>
	/// 手机号码运营商判断
	/// </summary>
	public static class PhoneNumber
	{
		/// <summary>
		/// 是否手机号码
		/// </summary>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		public static bool IsCellPhoneNo(string phoneNo)
		{
			return _cellPhoneNoPattern.IsMatch(phoneNo);
		}

		/// <summary>
		/// 是否移动手机号码
		/// </summary>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		public static bool IsMobileNo(string phoneNo)
		{
			return _mobileNoPattern.IsMatch(phoneNo);
		}

		/// <summary>
		/// 是否联通手机号码
		/// </summary>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		public static bool IsUnicomNo(string phoneNo)
		{
			return _unicomNoPattern.IsMatch(phoneNo);
		}

		/// <summary>
		/// 是否电信手机号码
		/// </summary>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		public static bool IsTelecomNo(string phoneNo)
		{
			return _telecomNoPattern.IsMatch(phoneNo);
		}

		/// <summary>
		/// 获取手机号码运营商，非手机号码返回 0，移动手机返回 1，联通手机返回 2，电信手机返回 3
		/// </summary>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		public static TelecomsOperator GetPhoneNoType(string phoneNo)
		{
			if (!IsCellPhoneNo(phoneNo))
			{
				return 0;
			}
			if (IsMobileNo(phoneNo))
			{
				return TelecomsOperator.ChinaMobile;
			}
			if (IsUnicomNo(phoneNo))
			{
				return TelecomsOperator.ChinaUnicom;
			}
			if (IsTelecomNo(phoneNo))
			{
				return TelecomsOperator.ChinaTelecom;
			}
			return TelecomsOperator.Other;
		}

		private static Regex _cellPhoneNoPattern = new Regex("^1[3458][0-9]{9}$");
		private static Regex _mobileNoPattern = new Regex("^1(3[4-9]|47|5[0-27-9]|8[278])[0-9]{8}$");
		private static Regex _unicomNoPattern = new Regex("^1(3[0-2]|45|5[356]|8[56])[0-9]{8}$");
		private static Regex _telecomNoPattern = new Regex("^1(33|80|89)[0-9]{8}$");
	}
}
