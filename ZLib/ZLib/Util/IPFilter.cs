using System.Collections.Generic;
using System.Net;

namespace ZLib.Util
{
	/// <summary>
	/// IP 过滤功能
	/// </summary>
	public class IPFilter
	{
		/// <summary>
		/// 过滤规则列表
		/// </summary>
		private IList<IPFilterRule> _filterRules;

		public IPFilter(IList<IPFilterRule> filterRules)
		{
			_filterRules = filterRules;
		}

		public IPFilter(string filterRules)
		{
			_filterRules = ReadFilterRules(filterRules);
		}

		/// <summary>
		/// 检查 IP 是否符合一条过滤规则
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="rule"></param>
		/// <returns></returns>
		private static bool CheckFilter(IPAddress ip, IPFilterRule rule)
		{
			byte[] _ipByte = ip.GetAddressBytes();
			byte[] _filterIpByte = rule.NetAddress.GetAddressBytes();
			byte[] _filterMaskByte = rule.Mask.GetAddressBytes();
			for (int _i = 0; _i < _ipByte.Length; _i++)
			{
				if ((_ipByte[_i] & _filterMaskByte[_i]) != _filterIpByte[_i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 检查 IP 是否符合过滤规则列表
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public bool CheckFilter(IPAddress ip)
		{
			for (int _i = 0; _i < _filterRules.Count; _i++)
			{
				if (CheckFilter(ip, _filterRules[_i]))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 读取过滤规则
		/// </summary>
		/// <param name="stringRules"></param>
		/// <returns></returns>
		private static IList<IPFilterRule> ReadFilterRules(string stringRules)
		{
			IList<IPFilterRule> _filterSettings = new List<IPFilterRule>();
			string filterString = stringRules;
			string[] _ss = filterString.Split(';');
			for (int _i = 0; _i < _ss.Length; _i++)
			{
				_filterSettings.Add(ReadFilterRule(_ss[_i]));
			}
			return _filterSettings;
		}

		/// <summary>
		/// 读取一条过滤规则
		/// </summary>
		/// <param name="stringRule"></param>
		/// <returns></returns>
		private static IPFilterRule ReadFilterRule(string stringRule)
		{
			int _pos = stringRule.IndexOf(',');
			string _address = stringRule;
			string _mask = "255.255.255.255";
			if (_pos > -1)
			{
				_address = stringRule.Substring(0, _pos);
				_mask = stringRule.Substring(_pos + 1);
			}
			return new IPFilterRule() { NetAddress = IPAddress.Parse(_address), Mask = IPAddress.Parse(_mask) };
		}
	}

	/// <summary>
	/// 过滤规则，包括 IP 地址和子网掩码
	/// </summary>
	public class IPFilterRule
	{
		/// <summary>
		/// 网络地址
		/// </summary>
		public IPAddress NetAddress { get; set; }
		/// <summary>
		/// 掩码
		/// </summary>
		public IPAddress Mask { get; set; }
	}
}
