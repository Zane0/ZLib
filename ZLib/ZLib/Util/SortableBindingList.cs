using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ZLib.Util
{
	/// <summary>
	/// 支持排序的列表，按默认规则，可以绑定到 dataGridView 的数据源
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SortableBindingList<T> : BindingList<T>
	{
		private readonly Dictionary<Type, PropertyComparer<T>> comparers;
		private bool isSorted;
		private ListSortDirection listSortDirection;
		private PropertyDescriptor propertyDescriptor;

		/// <summary>
		///   Constructs a new SortableBindingList.
		/// </summary>
		public SortableBindingList()
			: base(new List<T>())
		{
			this.comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		/// <summary>
		///   Constructs a new SortableBindingList.
		/// </summary>
		public SortableBindingList(IEnumerable<T> enumeration)
			: base(new List<T>(enumeration))
		{
			this.comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		/// <summary>
		///   Returns true.
		/// </summary>
		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		/// <summary>
		///   Gets whether this list is sorted.
		/// </summary>
		protected override bool IsSortedCore
		{
			get { return this.isSorted; }
		}

		/// <summary>
		///   Gets the current property being sorted.
		/// </summary>
		protected override PropertyDescriptor SortPropertyCore
		{
			get { return this.propertyDescriptor; }
		}

		/// <summary>
		///   Gets the sort order direction.
		/// </summary>
		protected override ListSortDirection SortDirectionCore
		{
			get { return this.listSortDirection; }
		}

		/// <summary>
		///   Returns true.
		/// </summary>
		protected override bool SupportsSearchingCore
		{
			get { return true; }
		}

		/// <summary>
		///   Sorts the items.
		/// </summary>
		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			List<T> itemsList = (List<T>)this.Items;

			Type propertyType = prop.PropertyType;
			PropertyComparer<T> comparer;
			if (!this.comparers.TryGetValue(propertyType, out comparer))
			{
				comparer = new PropertyComparer<T>(prop, direction);
				this.comparers.Add(propertyType, comparer);
			}

			comparer.SetPropertyAndDirection(prop, direction);
			itemsList.Sort(comparer);

			this.propertyDescriptor = prop;
			this.listSortDirection = direction;
			this.isSorted = true;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		/// <summary>
		///   Removes any sort applied. 
		/// </summary>
		protected override void RemoveSortCore()
		{
			this.isSorted = false;
			this.propertyDescriptor = base.SortPropertyCore;
			this.listSortDirection = base.SortDirectionCore;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		/// <summary>
		///   Searches for the index of a item with a specific property descriptor and value
		/// </summary>
		protected override int FindCore(PropertyDescriptor prop, object key)
		{
			int count = this.Count;
			for (int i = 0; i < count; ++i)
			{
				T element = this[i];
				if (prop.GetValue(element).Equals(key))
				{
					return i;
				}
			}

			return -1;
		}
	}
}
