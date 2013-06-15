using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BigEgg.Framework.Foundation.Validations
{
    /// <summary>
    /// Specifies that a string field value contains a root, seealso <seealso cref="Path.IsPathRooted"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    sealed public class IsPathRootedAttribute : ValidationAttribute
    {
        /// <summary>
        /// Get a value that indicates whether an empty string is allowed.
        /// </summary>
        public bool AllowEmptyString { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IsPathRootedAttribute class.
        /// </summary>
        /// <param name="allowEmptyString">Indicates whether an empty string is allowed.</param>
        public IsPathRootedAttribute(bool allowEmptyString = true)
        {
            this.AllowEmptyString = allowEmptyString;
        }
        
        /// <summary>
        /// Validation method.
        /// </summary>
        /// <param name="value">The data field.</param>
        /// <returns>True if the data field pass the validation. Otherwise, return false.</returns>
        public override bool IsValid(object value)
        {
            string path = value as String;
            if (String.IsNullOrWhiteSpace(path))
            {
                if (this.AllowEmptyString)
                    return true;
                else
                    return false;
            }

            try
            {
                return Path.IsPathRooted(path);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
