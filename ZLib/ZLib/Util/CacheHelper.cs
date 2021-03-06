﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ZLib.Util
{
	/// <summary>
	/// 缓存工具类
	/// </summary>
	public static class CacheHelper
	{
		/// <summary>
		/// 检查缓存，若存在则从缓存中获取结果，若不存在，则使用 handler 方法获取，并将结果缓存一定时间
		/// 示例： string _s = CacheHelper.GetObjectWithCache<string>(HttpContext.Current.Cache, "CACHEKEY_Test", delegate { return "Sample"; }, 3 * 1000);
		/// </summary>
		/// <typeparam name="T">结果对象的类型</typeparam>
		/// <param name="cache">缓存</param>
		/// <param name="cacheKey">缓存的键值</param>
		/// <param name="handler">获取结果的方法</param>
		/// <param name="milliseconds">结果缓存的毫秒数</param>
		/// <returns></returns>
		public static T GetObjectWithCache<T>(ObjectCache cache
			, string cacheKey
			, Func<T> func
			, double milliseconds) where T : class
		{
			T _t = null;
			lock (GetLockByCacheKey(cacheKey))
			{
				if (cache[cacheKey] == null)
				{
					_t = func() as T;
					cache.Add(cacheKey
						, _t
						, DateTime.UtcNow.AddMilliseconds(milliseconds));
				}
				else
				{
					_t = cache[cacheKey] as T;
				}
			}
			return _t;
		}

		/// <summary>
		/// 根据缓存键名获取锁，防止获取结果的时候重复获取
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <returns></returns>
		private static object GetLockByCacheKey(string cacheKey)
		{
			lock ((_locks as ICollection).SyncRoot)
			{
				if (_locks.ContainsKey(cacheKey))
				{
					return _locks[cacheKey];
				}

				object _lock = new object();
				_locks.Add(cacheKey, _lock);
				return _lock;
			}
		}

		/// <summary>
		/// 缓存此值，可以确保在获取结果失败后的缓存时效内不再重复获取
		/// </summary>
		public static readonly object Null = new object();

		private static IDictionary<string, object> _locks = new Dictionary<string, object>();
	}
}
