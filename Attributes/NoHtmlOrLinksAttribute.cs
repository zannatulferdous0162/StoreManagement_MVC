using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StoreManagement_Project.Attributes
{
    public class NoHtmlOrLinksAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string ?? "";

            // Check for any HTML tags
            if (Regex.IsMatch(input, "<.*?>"))
            {
                return new ValidationResult("HTML tags are not allowed.");
            }

            // Check for links
            if (Regex.IsMatch(input, @"(http[s]?://\S+|www\.\S+)"))
            {
                return new ValidationResult("Links are not allowed.");
            }

            return ValidationResult.Success;
        }
    }
}
