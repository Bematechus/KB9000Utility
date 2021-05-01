/*
 * Created by SharpDevelop.
 * User: wout
 * Date: 9-10-2004
 * Time: 17:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Reflection;

namespace KB9Utility
{
	/// <summary>
	/// Delegate that allows a class to change property attributes that
	/// define a property's behaviour in 
	/// a <see cref="System.Windows.Forms.PropertyGrid"/>.
	/// </summary>
	public delegate void PropertyAttributesProvider(PropertyAttributes propertyAttributes);
	
	/// <summary>
	/// Use this attribute on a property to set a property's
	/// <see cref="PropertyAttributesProvider"/> delegate.
	/// </summary>
    [AttributeUsage(AttributeTargets.Property)]
	public class PropertyAttributesProviderAttribute : Attribute
	{
		string propertyAttributesProviderName;

		/// <summary>
		/// Constructor.
		/// </summary>
		public PropertyAttributesProviderAttribute(string propertyAttributesProviderName)
		{
			this.propertyAttributesProviderName = propertyAttributesProviderName;
		}
		
		/// <summary>
		/// Get the <see cref="PropertyAttributesProvider"/> specified by the
		/// <see cref="PropertyAttributesProviderAttribute"/> on given target object.
		/// </summary>
		public MethodInfo GetPropertyAttributesProvider(object target) {
			return target.GetType().GetMethod(propertyAttributesProviderName);
		}

		public string Name
		{
			get { return propertyAttributesProviderName; }
		}
	}
}
