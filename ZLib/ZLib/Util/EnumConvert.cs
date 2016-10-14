using System;

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
		/// <param name="i"></param>
		/// <returns></returns>
		public static T ToEnum<T>(int i)
			where T : struct
		{
			Type _enumType = typeof(T);
			if (Enum.IsDefined(_enumType, i))
			{
				return (T)Convert.ChangeType(i, Enum.GetUnderlyingType(_enumType));
			}
			else
			{
				throw new Exception(i + " is not defined");
			}
		}

		/// <summary>
		/// 将字节转为枚举类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="b"></param>
		/// <returns></returns>
		public static T ToEnum<T>(byte b)
			where T : struct
		{
			Type _enumType = typeof(T);
			if (Enum.IsDefined(_enumType, b))
			{
				return (T)Convert.ChangeType(b, Enum.GetUnderlyingType(_enumType));
			}
			else
			{
				throw new Exception(b + " is not defined");
			}
		}
	}
}
