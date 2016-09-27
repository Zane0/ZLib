using System;
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
		/// <returns> </returns> 
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
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static T Bytes2Struct<T>(byte[] bytes) where T : struct
		{
#line hidden
			return Bytes2Struct<T>(bytes, 0);
#line default
		}

		/// <summary>
		/// 字节数组反序列化为结构体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static T Bytes2Struct<T>(byte[] bytes, int iPos) where T : struct
		{
#line hidden
			int _size = Marshal.SizeOf(typeof(T));
			if (_size > bytes.Length - iPos)
			{
				throw new ApplicationException();
			}

			IntPtr _ptr = Marshal.AllocHGlobal(_size);
			Marshal.Copy(bytes, iPos, _ptr, _size);
			try
			{
				return (T)Marshal.PtrToStructure(_ptr, typeof(T));
			}
			finally
			{
				Marshal.FreeHGlobal(_ptr);
			}
#line default
		}

		/// <summary>
		/// 将字节数组反序列化成对象数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="bytes"></param>
		/// <param name="iPos"></param>
		/// <param name="itemCount"></param>
		/// <returns></returns>
		public static T[] Bytes2Structs<T>(byte[] bytes, int iPos, int itemCount) where T : struct
		{
			int rawsize = Marshal.SizeOf(typeof(T));
			if (rawsize * itemCount > bytes.Length)
				return null;
			IntPtr buffer = Marshal.AllocHGlobal(rawsize * itemCount);
			Marshal.Copy(bytes, iPos, buffer, rawsize * itemCount);
			T[] retobjs = new T[itemCount];
			for (int i = 0; i < itemCount; i++)
			{
				retobjs[i] = (T)Marshal.PtrToStructure((IntPtr)(buffer.ToInt32() + rawsize * i), typeof(T));//+
			}
			Marshal.FreeHGlobal(buffer);
			return retobjs;
		}
	}
}
