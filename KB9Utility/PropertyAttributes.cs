// (c) 2004 Wout de Zeeuw

using System;

namespace KB9Utility
{
	/// <summary>
	/// Has some attributes defining how a property behaves in 
	/// a <see cref="System.Windows.Forms.PropertyGrid"/>.
	/// </summary>
	public class PropertyAttributes : IComparable
	{
		private string name;
		private bool isBrowsable;
		private bool isReadOnly;
	    private string displayName;
	    private string description;
	    private string category;
	    private int order = 0;
		
		public PropertyAttributes(string name) {
			this.name = name;
		}
		
		public string Name {
			get {
				return name;
			}
		}

		/// <summary>
		/// Is the property visible in a property grid.
		/// </summary>
		public bool IsBrowsable {	
			get {
				return isBrowsable;
			}
			set {
				isBrowsable = value;
			}
		}

		public bool IsReadOnly {
			get {
				return isReadOnly;
			}
			set {
				isReadOnly = value;
			}
		}
		public string DisplayName {
			get {
				return displayName;
			}
			set {
				displayName = value;
			}
		}
		public string Description {
			get {
				return description;
			}
			set {
				description = value;
			}
		}
		public string Category {
			get {
				return category;
			}
			set {
				category = value;
			}
		}
		public int Order {
			get {
				return order;
			}
			set {
				order = value;
			}
		}

		#region IComparable
		
		public int CompareTo(object obj)
		{
			// Compare this pair's order to another.  If the numeric order is the same, sort by display name.
			PropertyAttributes other = (PropertyAttributes)obj;
			if (order == other.order) {
				return string.Compare(displayName, other.displayName);
			} else {
				return (order < other.order) ? -1 : 1;
			}
		}
		
		#endregion
	}
}
