using System.Configuration;

namespace ZLib.Config
{
	public class IPFilterConfigCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new IPFilterConfigElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((IPFilterConfigElement)element).Name;
		}

		public IPFilterConfigElement this[int index]
		{
			get
			{
				return (IPFilterConfigElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}

		public new IPFilterConfigElement this[string name]
		{
			get
			{
				return (IPFilterConfigElement)BaseGet(name);
			}
		}
	}
}
