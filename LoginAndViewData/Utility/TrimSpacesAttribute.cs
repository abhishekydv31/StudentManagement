using System;
using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class TrimSpacesAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null && value is string stringValue)
        {
            // Trim leading and trailing spaces
            var trimmedValue = stringValue.Trim();

            if (trimmedValue.Length < stringValue.Length)
            {
                // Value had leading or trailing spaces, update the model
                var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);
                propertyInfo?.SetValue(validationContext.ObjectInstance, trimmedValue);
            }
        }

        return ValidationResult.Success;
    }
}