// (c) 2004 Wout de Zeeuw

using System;
using System.ComponentModel;

namespace KB9Utility
{
	/// <summary>
	/// Enhances the <see cref="PropertyDescriptor"/>.
	/// </summary>
	/// <remarks>
	/// All values are gotten from the <see cref="PropertyAttributes"/> 
	/// object passed to the constructor.
	/// </remarks>
	public class PropertyDescriptorEx : PropertyDescriptor
	{
		private PropertyDescriptor basePropertyDescriptor; 
		private PropertyAttributes propertyAttributes;

		/// <summary>
		/// Constructor.
		/// </summary>
		public PropertyDescriptorEx(
			PropertyDescriptor basePropertyDescriptor,
			PropertyAttributes propertyAttributes
		) : base(basePropertyDescriptor)
		{
			this.basePropertyDescriptor = basePropertyDescriptor;
			this.propertyAttributes = propertyAttributes;
		}

		public override bool CanResetValue(object component)
		{
			return basePropertyDescriptor.CanResetValue(component);
		}

		public override Type ComponentType
		{
			get { return basePropertyDescriptor.ComponentType; }
		}

		public override string DisplayName
		{
			get 
			{
				return propertyAttributes.DisplayName;
			}
		}

		public override string Description
		{
			get
			{
				return propertyAttributes.Description;
			}
		}

		public override string Category
		{
			get
			{
				return propertyAttributes.Category;
			}
		}

		public override object GetValue(object component)
		{
			return this.basePropertyDescriptor.GetValue(component);
		}

		public override bool IsReadOnly
		{
			get {
				return propertyAttributes.IsReadOnly;
			}
		}

		public override bool IsBrowsable
		{
			get {
				return propertyAttributes.IsBrowsable;
			}
		}

		public override string Name
		{
			get { return this.basePropertyDescriptor.Name; }
		}

		public override Type PropertyType
		{
			get { return this.basePropertyDescriptor.PropertyType; }
		}

		public override void ResetValue(object component)
		{
			this.basePropertyDescriptor.ResetValue(component);
		}

		public override bool ShouldSerializeValue(object component)
		{
			return this.basePropertyDescriptor.ShouldSerializeValue(component);
		}

		public override void SetValue(object component, object value)
		{
			this.basePropertyDescriptor.SetValue(component, value);
		}
	}
}
