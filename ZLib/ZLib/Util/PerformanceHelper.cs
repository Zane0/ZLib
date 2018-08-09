using System;
using System.Diagnostics;

namespace ZLib.Util
{
	/// <summary>
	/// 用于计算执行方法花费的时间
	/// </summary>
	public static class PerformanceHelper
	{
		/// <summary>
		/// 获取方法执行花费的时间
		///	TimeSpan _ts = PerformanceHelper.CheckTime(() =>
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
