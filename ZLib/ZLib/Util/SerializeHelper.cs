using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ZLib.Util
{
	/// <summary>
	/// 结构对象字节序列化工具
	/// </summary>
	public static class SerializeHelper
	{
		/// <summary>
		/// 结构体序列化为字节数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static byte[] Struct2Bytes<T>(T obj) where T : struct
		{
			int size = Marshal.SizeOf(obj);
			byte[] bytes = new byte[size];
			IntPtr arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
			Marshal.StructureToPtr(obj, arrPtr, true);
			return bytes;
		}

		/// <summary>
		/// 字节数组反序列化为结构体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Bytes2Struct<T>(byte[] value) where T : struct
		{
#line hidden
			return Bytes2Struct<T>(value, 0);
#line default
		}

		/// <summary>
		/// 将字节数组反序列化成结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static T Bytes2Struct<T>(byte[] value, int position) where T : struct
		{
#line hidden
			int _size = Marshal.SizeOf(typeof(T));
			if (_size > value.Length - position)
			{
				throw new ArgumentOutOfRangeException("position", position, "剩余字节不足结构大小");
			}

			IntPtr _ip = Marshal.AllocHGlobal(_size);
			Marshal.Copy(value, position, _ip, _size);
			try
			{
				return (T)Marshal.PtrToStructure(_ip, typeof(T));
			}
			finally
			{
				Marshal.FreeHGlobal(_ip);
			}
#line default
		}

		/// <summary>
		/// 将字节数组连续反序列化成结构数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="position"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<T> Bytes2Structs<T>(byte[] value, int position, int count) where T : struct
		{
#line hidden
			int _structSize = Marshal.SizeOf(typeof(T));
			int _allocSize = _structSize * count;
			if (_allocSize > value.Length - position)
			{
				throw new ArgumentOutOfRangeException("count", count, "剩余字节不足结构大小");
			}
			IntPtr _ip = Marshal.AllocHGlobal(_allocSize);
			try
			{
				Marshal.Copy(value, position, _ip, _allocSize);
				for (int i = 0; i < count; i++)
				{
					yield return (T)Marshal.PtrToStructure((IntPtr)(_ip.ToInt32() + _structSize * i), typeof(T));
				}
			}
			finally
			{
				Marshal.FreeHGlobal(_ip);
			}
#line default
		}
	}
}
