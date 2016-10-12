using System;

namespace ZLib.Util
{
	public class Sampling
	{
		/// <summary>
		/// 按几率返回 true，如果 100 则 100% 返回 true，0 则 100% 返回 false
		/// </summary>
		/// <param name="percent"></param>
		/// <returns></returns>
		public bool GetTrue(int percent)
		{
			Random _ra = new Random();
			int _r = _ra.Next(100);
			return percent > _r;
		}
	}
}
