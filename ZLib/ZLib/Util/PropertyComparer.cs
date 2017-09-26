using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace ZLib.Util
{
	/// <summary>
	/// 属性比较器
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PropertyComparer<T> : IComparer<T>
	{
		private readonly IComparer comparer;
		private PropertyDescriptor propertyDescriptor;
		private int reverse;

		/// <summary>
		///   Constructs a new property comparer.
		/// </summary>
		public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
		{
			this.propertyDescriptor = property;
			Type comparerForPropertyType = typeof(Comparer<>).MakeGenericType(property.PropertyType);
			this.comparer = (IComparer)comparerForPropertyType.InvokeMember("Default",
				BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public,
				null, null, null, CultureInfo.InvariantCulture);
			this.SetListSortDirection(direction);
		}

		#region IComparer<T> Members

		/// <summary>
		///   Compares two values.
		/// </summary>
		public int Compare(T x, T y)
		{
			return this.reverse * this.comparer.Compare(this.propertyDescriptor.GetValue(x), this.propertyDescriptor.GetValue(y));
		}

		#endregion

		private void SetPropertyDescriptor(PropertyDescriptor descriptor)
		{
			this.propertyDescriptor = descriptor;
		}

		private void SetListSortDirection(ListSortDirection direction)
		{
			this.reverse = direction == ListSortDirection.Ascending ? 1 : -1;
		}

		/// <summary>
		///   Sets the property being sorted and the sorting direction.
		/// </summary>
		public void SetPropertyAndDirection(PropertyDescriptor descriptor, ListSortDirection direction)
		{
			this.SetPropertyDescriptor(descriptor);
			this.SetListSortDirection(direction);
		}
	}
}
