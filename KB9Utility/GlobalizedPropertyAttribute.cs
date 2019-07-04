// Original ripped from Gerd Klevesaat,
// http://www.codeguru.com/Csharp/Csharp/cs_controls/propertygrid/comments.php/c4795
//
// Made minor improvements.
//
// (c) 2004 Wout de Zeeuw

using System;
using System.Resources;

namespace KB9Utility
{
	/// <summary>
	/// Optional attribute for detailed specification of where
	/// <see cref="PropertiesDeluxeTypeConverter"/> should look for its resources.
	/// </summary>
	/// <remarks>
	/// See also <seealso cref="GlobalizedTypeAttribute"/>
	/// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
	public class GlobalizedPropertyAttribute : Attribute
	{
	    private string baseName;
	    private string displayNameId;
	    private string descriptionId;
	    private string categoryId;

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

        /// <summary>
        /// Resource name for a property's DisplayName.
        /// </summary>
        public string DisplayNameId {
            get {
                return displayNameId;
            }
            set {
                displayNameId = value;
            }
        }

        /// <summary>
        /// Resource name for a property's Description.
        /// </summary>
        public string DescriptionId {
            get {
                return descriptionId;
            }
            set {
                descriptionId = value;
            }
        }

        /// <summary>
        /// Resource name for a property's Category.
        /// </summary>
        public string CategoryId {
            get {
                return categoryId;
            }
            set {
                categoryId = value;
            }
        }
	}
}
