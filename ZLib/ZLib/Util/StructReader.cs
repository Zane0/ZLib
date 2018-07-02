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
			int _size = Marshal.SizeOf(typeof(T));
			byte[] _bs = ReadBytes(stream, _size);
			return SerializeHelper.Bytes2Struct<T>(_bs);
		}

		/// <summary>
		/// 从流中读取多个结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static IEnumerable<T> ReadStructFromStream<T>(Stream stream, int count) where T : struct
		{
			for (int _i = 0; _i <= count - 1; _i++)
			{
				yield return ReadStructFromStream<T>(stream);
			}
		}

		/// <summary>
		/// 从剩余流中读取多个结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static IEnumerable<T> ReadStructToEndFromStreamAll<T>(Stream stream) where T : struct
		{
			long _streamRemainLength = stream.Length - stream.Position;
			int _size = Marshal.SizeOf(typeof(T));
			int _count = (int)(_streamRemainLength / _size);
			return ReadStructFromStream<T>(stream, _count);
		}

		/// <summary>
		/// 从流中读取字节
		/// </summary>
		/// <param name="stream">流</param>
		/// <param name="count">需要读取的字节数</param>
		/// <returns></returns>
		private static byte[] ReadBytes(Stream stream, int count)
		{
			byte[] _buffer = new byte[count];
			for (int _offset = 0; count > 0; )
			{
				int _num = stream.Read(_buffer, _offset, count);
				if (_num == 0)
				{
					throw new ArgumentOutOfRangeException("stream", stream, "超出流的剩余长度");
				}
				_offset += _num;
				count -= _num;
			}
			return _buffer;
		}
	}
}
