using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace ZLib.Util
{
	public static class StructReader
	{
		/// <summary>
		/// 从流中读取一个结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T ReadStructFromStream<T>(Stream stream) where T : struct
		{
#line hidden
			int _size = Marshal.SizeOf(typeof(T));
			byte[] _bs = new byte[_size];
			int _len = stream.Read(_bs, 0, _size);
			if (_size > _len)
			{
				throw new ArgumentOutOfRangeException("stream", stream, "超出流的剩余长度");
			}
			return SerializeHelper.Bytes2Struct<T>(_bs);
#line default
		}

		/// <summary>
		/// 从流中读取指定个结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<T> ReadStructFromStream<T>(Stream stream, int count) where T : struct
		{
#line hidden
			for (int _i = 0; _i <= count - 1; _i++)
			{
				yield return ReadStructFromStream<T>(stream);
			}
#line default
		}

		/// <summary>
		/// 从剩余流中读取多个结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static IEnumerable<T> ReadStructToEndFromStreamAll<T>(Stream stream) where T : struct
		{
#line hidden
			long _streamRemainLength = stream.Length - stream.Position;
			int _size = Marshal.SizeOf(typeof(T));
			int _count = (int)(_streamRemainLength / _size);
			return ReadStructFromStream<T>(stream, _count);
#line default
		}
	}
}
