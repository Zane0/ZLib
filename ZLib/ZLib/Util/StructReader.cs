using System.IO;
using System.Runtime.InteropServices;

namespace ZLib.Util
{
	public static class StructReader
	{
		/// <summary>
		/// 从流中读取结构
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T ReadStructFromStream<T>(Stream stream) where T : struct
		{
#line hidden
			int _structSize = Marshal.SizeOf(typeof(T));
			byte[] buffer = new byte[_structSize];
			stream.Read(buffer, 0, _structSize);
			return SerializeHelper.Bytes2Struct<T>(buffer);
#line default
		}

		/// <summary>
		/// 从流中读取结构数组
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <param name="len">流的长度</param>
		/// <returns></returns>
		public static T[] ReadStructArrayFromStream<T>(Stream stream, int len) where T : struct
		{
#line hidden
			int _size = len / Marshal.SizeOf(typeof(T));
			T[] _ts = new T[_size];
			for (int _i = 0; _i <= _size - 1; _i++)
			{
				_ts[_i] = StructReader.ReadStructFromStream<T>(stream);
			}
			return _ts;
#line default
		}
	}
}
