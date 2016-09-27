using System.Configuration;

namespace ZLib.Config
{
	public class IPFilterConfigElement : ConfigurationElement
	{
		/// <summary>
		/// 短信发送来源 Id
		/// </summary>
		[ConfigurationProperty("name"
		   , IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return this["name"].ToString();
			}
			set
			{
				this["name"] = value;
			}
		}

		/// <summary>
		/// 发送渠道，该来源通过什么通道发送
		/// </summary>
		[ConfigurationProperty("filterRules"
			, IsRequired = true)]
		public string FilterRules
		{
			get
			{
				return this["filterRules"].ToString();
			}
			set
			{
				this["filterRules"] = value;
			}
		}

		/// <summary>
		/// 来源描述
		/// </summary>
		[ConfigurationProperty("description"
			, IsRequired = false)]
		public string Description
		{
			get
			{
				return this["description"].ToString();
			}
			set
			{
				this["description"] = value;
			}
		}
	}
}
