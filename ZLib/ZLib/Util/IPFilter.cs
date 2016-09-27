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
		private IList<FilterRule> _filterRules;

		public IPFilter(IList<FilterRule> filterRules)
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
		private bool CheckFilter(IPAddress ip, FilterRule rule)
		{
			byte[] _ipByte = ip.GetAddressBytes();
			byte[] _filterIpByte = rule.IP.GetAddressBytes();
			byte[] _filterMaskByte = rule.SubnetMask.GetAddressBytes();
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
		private IList<FilterRule> ReadFilterRules(string stringRules)
		{
			IList<FilterRule> _filterSettings = new List<FilterRule>();
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
		private FilterRule ReadFilterRule(string stringRule)
		{
			FilterRule _fs = new FilterRule();
			int _pos = stringRule.IndexOf(',');
			string _ip = stringRule;
			string _subnetMask = "255.255.255.255";
			if (_pos > -1)
			{
				_ip = stringRule.Substring(0, _pos);
				_subnetMask = stringRule.Substring(_pos + 1);
			}
			return new FilterRule() { IP = IPAddress.Parse(_ip), SubnetMask = IPAddress.Parse(_subnetMask) };
		}
	}

	/// <summary>
	/// 过滤规则，包括 IP 地址和子网掩码
	/// </summary>
	public class FilterRule
	{
		/// <summary>
		/// IP 地址
		/// </summary>
		public IPAddress IP { get; set; }
		/// <summary>
		/// 子网掩码
		/// </summary>
		public IPAddress SubnetMask { get; set; }
	}
}
