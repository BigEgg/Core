using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BigEgg.Framework.Foundation.Validations
{
    /// <summary>
    /// Specifies that a data field value or property value is "EqualTo" (or other operator which set) to another dependent data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IsAttribute : ValidationAttribute
    {
        /// <summary>
        /// The operator to compare, see <seealso cref="Operator"/>.
        /// </summary>
        public Operator Operator { get; protected set; }
        /// <summary>
        /// An object to compare.
        /// </summary>
        public object DependentObject { get; protected set; }
        /// <summary>
        /// The property name which need to be compared, see <seealso cref="ValidationType"/>.
        /// </summary>
        public ValidationType ValidationType { get; protected set; }
        /// <summary>
        /// Specifies that if the data field pass the validation when it is null.
        /// </summary>
        public bool PassOnNull { get; set; }


        /// <summary>
        /// Initializes a new instance of the IsAttribute class with the specified object or property to compare by the Operator value.
        /// </summary>
        /// <param name="dependentObject">The specified object or property to compare</param>
        /// <param name="validationType">The type of the validation, see <seealso cref="ValidationType"/>. By data field value or by property value</param>
        /// <param name="operator">The compare operator, see <seealso cref="Operator"/></param>
        public IsAttribute(
            object dependentObject, 
            ValidationType validationType = ValidationType.ByValue, 
            Operator @operator = Operator.EqualTo)
        {
            if (dependentObject == null) { throw new ArgumentException("dependentObject"); }
            if ((validationType == ValidationType.ByProperty) && !(dependentObject is string)) { throw new ArgumentException("validationType"); }
            if (!TypeCheck(dependentObject)) { throw new ArgumentException("The dependentObject type is not a comparable type."); }

            this.Operator = @operator;
            this.DependentObject = dependentObject;
            this.ValidationType = validationType;
            this.PassOnNull = false;
        }


        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="validationContext">The context information about the validation operation</param>
        /// <returns>An instance of the ValidationResult class</returns>
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            //  Check the value
            if (value == null) 
            {
                if (this.PassOnNull) { return ValidationResult.Success; }
                else { return new ValidationResult(ErrorMessageString); }
            }

            if (!TypeCheck(value)) { throw new ArgumentException("The value type is not a comparable type."); }

            //  Get the compare value
            object dependentValue = null;
            if (this.ValidationType == ValidationType.ByProperty)
            {
                //  Get property value
                Type type = validationContext.ObjectType;
                PropertyInfo dependentProperty = type.GetProperty(this.DependentObject as string);

                if (dependentProperty == null) { throw new ArgumentException("DependentPropertyName"); }

                dependentValue = dependentProperty.GetValue(validationContext.ObjectInstance, null);
            }
            else
            {
                dependentValue = this.DependentObject;
            }

            if (dependentValue == null)
            {
                if (this.PassOnNull) { return ValidationResult.Success; }
                else { return new ValidationResult(ErrorMessageString); }
            }

            if (value.GetType() != dependentValue.GetType())
                return new ValidationResult(this.ErrorMessage);

            //  Start validation.
            int result = (value as IComparable).CompareTo(dependentValue);
            if (result < 0)
            {
                if ((Operator == Operator.LessThan)
                    || (Operator == Operator.LessThanOrEqualTo)
                    || (Operator == Operator.NotEqualTo))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(base.ErrorMessageString);
            }
            else if (result == 0)
            {
                if ((Operator == Operator.EqualTo)
                    || (Operator == Operator.LessThanOrEqualTo)
                    || (Operator == Operator.GreaterThanOrEqualTo))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(base.ErrorMessageString);
            }
            else
            {
                if ((Operator == Operator.GreaterThan)
                    || (Operator == Operator.GreaterThanOrEqualTo)
                    || (Operator == Operator.NotEqualTo))
                    return ValidationResult.Success;
                else
                    return new ValidationResult(base.ErrorMessageString);
            }
        }

        /// <summary>
        /// The methods for the type check. Use to check if the type is support by the the validation.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>True if the validation is support. Otherwise, return false.</returns>
        protected virtual bool TypeCheck(object value)
        {
            return (value is IComparable);
        }
    }
}
