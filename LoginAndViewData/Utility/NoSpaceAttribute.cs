using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;

namespace LoginAndViewData.Utility
{
    public class NoSpaceAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is string str) {
                if (str.Trim().Length == 0)
                {
                    return new ValidationResult(ErrorMessage ?? "Only whitespaces are not allowed");
                }
            }
            return ValidationResult.Success;
        }
    }
}
