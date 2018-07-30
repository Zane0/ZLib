using System;
using System.Globalization;

namespace ZLib.Util
{
	/// <summary>
	/// 枚举与数值类型转换工具
	/// </summary>
	public static class EnumConvert
	{
		/// <summary>
		/// 将整形转为枚举类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ToEnum<T>(int value)
			where T : struct
		{
			Type _enumType = typeof(T);
			if (Enum.IsDefined(_enumType, value))
			{
				return (T)Convert.ChangeType(value, Enum.GetUnderlyingType(_enumType), CultureInfo.InvariantCulture);
			}
			else
			{
				throw new ArgumentException(value + " is not defined");
			}
		}

		/// <summary>
		/// 将字节转为枚举类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ToEnum<T>(byte value)
			where T : struct
		{
			Type _enumType = typeof(T);
			if (Enum.IsDefined(_enumType, value))
			{
				return (T)Convert.ChangeType(value, Enum.GetUnderlyingType(_enumType), CultureInfo.InvariantCulture);
			}
			else
			{
				throw new ArgumentException(value + " is not defined");
			}
		}
	}
}
