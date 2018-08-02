using System;

namespace ZLib.Util
{
	/// <summary>
	/// 枚举与数值类型转换工具
	/// </summary>
	public static class EnumConvert
	{
		/// <summary>
		/// 将整形转为枚举类型，在转换过程中会检测数值是否在枚举中定义过。如果没有定义将抛出异常。
		/// 如果不需要做值检测，可以直接强制转换，效率会更高
		/// </summary>
		/// <typeparam name="TEnum">需要转换成的目标枚举</typeparam>
		/// <param name="value">待转换的数值</param>
		/// <returns></returns>
		public static TEnum ToEnum<TEnum>(int value)
			where TEnum : struct
		{
			Type _enumType = typeof(TEnum);
			if (!_enumType.IsEnum)
			{
				throw new InvalidCastException("只能转换为枚举类型");
			}
			if (Enum.IsDefined(_enumType, value))
			{
				return (TEnum)Enum.ToObject(_enumType, value);
			}
			else
			{
				throw new ArgumentException("指定枚举中不存在具有指定值的常数", "value");
			}
		}

		/// <summary>
		/// 将字节转为枚举类型，在转换过程中会检测数值是否在枚举中定义过。如果没有定义将抛出异常。
		/// 如果不需要做值检测，可以直接强制转换，效率会更高
		/// </summary>
		/// <typeparam name="TEnum">需要转换成的目标枚举</typeparam>
		/// <param name="value">待转换的数值</param>
		/// <returns></returns>
		public static TEnum ToEnum<TEnum>(byte value)
			where TEnum : struct
		{
			Type _enumType = typeof(TEnum);
			if (!_enumType.IsEnum)
			{
				throw new InvalidCastException("只能转换为枚举类型");
			}
			if (Enum.IsDefined(_enumType, value))
			{
				return (TEnum)Enum.ToObject(_enumType, value);
			}
			else
			{
				throw new ArgumentException("指定枚举中不存在具有指定值的常数", "value");
			}
		}
	}
}
