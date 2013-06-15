using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BigEgg.Framework.Foundation.Validations
{
    /// <summary>
    /// Specifies that a data field value is required if other Property is is "EqualTo" (or other operator which set) to another dependent data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class RequiredIfAttribute : IsAttribute
    {
        /// <summary>
        /// The property name which need to be compared.
        /// </summary>
        public string DependentPropertyName { get; protected set; }


        /// <summary>
        /// Initializes a new instance of the RequiredIfAttribute class with the specified property name and object to compare and the Operator value.
        /// </summary>
        /// <param name="dependentPropertyName">The specified object name which need to be compared.</param>
        /// <param name="operator">The compare operator.</param>
        /// <param name="dependentObject">The specified object or property to compare</param>
        /// <param name="validationType">The type of the validation, see <seealso cref="ValidationType"/>. By data field value or by property value</param>
        public RequiredIfAttribute(
            string dependentPropertyName,
            object dependentObject,
            Operator @operator = Operator.EqualTo,
            ValidationType validationType = ValidationType.ByValue)
            : base(dependentObject, validationType, @operator)
        {
            if (string.IsNullOrWhiteSpace(dependentPropertyName)) { throw new ArgumentException("dependentPropertyName"); }

            this.DependentPropertyName = dependentPropertyName;
        }


        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>An instance of the ValidationResult class.</returns>
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            Type type = validationContext.ObjectType;
            PropertyInfo dependentProperty = type.GetProperty(DependentPropertyName);

            if (dependentProperty == null) { throw new ArgumentException("DependentPropertyName"); }

            ValidationResult check = base.IsValid(dependentProperty.GetValue(validationContext.ObjectInstance, null), validationContext);
            if (check != ValidationResult.Success)     //  DependentPropertyName is not "Operator" to the DependentObject
                return ValidationResult.Success;

            if (value is string)
            {
                return (!string.IsNullOrWhiteSpace(value as string)) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
            else
            {
                return (value != null) ? ValidationResult.Success : new ValidationResult(ErrorMessageString);
            }
        }
    }
}
