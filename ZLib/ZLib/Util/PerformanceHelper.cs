using System;
using System.Diagnostics;

namespace ZLib.Util
{
	public static class Performance
	{
		/// <summary>
		/// 获取方法执行花费的时间
		///	TimeSpan _ts = CheckTime(() =>
		///	{
		///	    for (int _i = 0; _i < 100000000; _i++)
		///	    {
		///	        int _j = (int)123456789L;
		///	    }
		///	});
		///	Console.WriteLine("执行方法共花费{0}毫秒", _ts.TotalMilliseconds);
		/// </summary>
		/// <param name="act"></param>
		/// <returns></returns>
		public static TimeSpan CheckTime(Action act)
		{
			Stopwatch _st = Stopwatch.StartNew();
			act();
			return _st.Elapsed;
		}
	}
}
