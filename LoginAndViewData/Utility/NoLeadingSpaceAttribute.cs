using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NoLeadingSpaceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null && value is string stringValue)
        {
            if (Regex.IsMatch(stringValue, @"^\s"))
            {
                return new ValidationResult(ErrorMessage ?? "The value cannot start with a space.");
            }
        }

        return ValidationResult.Success;
    }
}
