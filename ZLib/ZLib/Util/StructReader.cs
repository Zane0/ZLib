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
	}
}
