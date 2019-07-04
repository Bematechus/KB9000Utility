// (c) 2004 Wout de Zeeuw

using System;

namespace KB9Utility
{
	/// <summary>
	/// Optional attribute for detailed specification of where
	/// <see cref="PropertiesDeluxeTypeConverter"/> should look for its resources.
	/// </summary>
	/// <remarks>
	/// See also <seealso cref="GlobalizedPropertyAttribute"/>
	/// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class GlobalizedTypeAttribute : Attribute
	{
	    private string baseName;

        /// <summary>
        /// Place where <see cref="ResourceManager"/> can find its resources.
        /// </summary>
        public string BaseName {
            get {
                return baseName;
            }
            set {
                baseName = value;
            }
        }
	}
}
