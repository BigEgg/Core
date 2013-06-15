using System;

namespace BigEgg.Framework.Foundation.Validations
{
    /// <summary>
    /// Specifies that a data field value is "LessThan" (or other operator which set) to another dependent data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LessThanAttribute : IsAttribute
    {
        /// <summary>
        /// Initializes a new instance of the LessThanAttribute class with the specified object to compare.
        /// </summary>
        /// <param name="dependentObject">The specified object to compare</param>
        /// <param name="validationType">The type of the validation, see <seealso cref="ValidationType"/>. By data field value or by property value</param>
        public LessThanAttribute(object dependentObject, ValidationType validationType = ValidationType.ByValue)
            : base(dependentObject, validationType, Operator.LessThan)
        {
        }
    }
}
