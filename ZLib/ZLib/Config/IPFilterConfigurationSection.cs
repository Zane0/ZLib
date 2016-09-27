using System.Configuration;

namespace ZLib.Config
{
	/// <summary>
	/// IP 过滤规则配置项
	/// </summary>
	public class IPFilterConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("IPFilterRule", IsDefaultCollection = false)]
		[ConfigurationCollection(typeof(IPFilterConfigCollection),
			AddItemName = "add",
			ClearItemsName = "clear",
			RemoveItemName = "remove")]
		public IPFilterConfigCollection IPFilterRule
		{
			get
			{
				IPFilterConfigCollection aucss =
					(IPFilterConfigCollection)base["IPFilterRule"];
				return aucss;
			}
		}
	}
}
