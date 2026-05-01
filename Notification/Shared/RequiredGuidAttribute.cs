using System.ComponentModel.DataAnnotations;

namespace Zuhid.Notification.Shared;

public class RequiredGuidAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || value is not Guid || (Guid)value == Guid.Empty)
        {
            return new ValidationResult($"{validationContext.MemberName} must be a valid Guid.");
        }
        return ValidationResult.Success;
    }
}
