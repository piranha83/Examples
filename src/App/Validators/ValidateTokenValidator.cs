using App.Models;
using FluentValidation;

namespace App.Validators
{
    public class ValidateTokenValidator : AbstractValidator<ValidateToken>
    {
        public ValidateTokenValidator()
        {
            RuleFor(m => m).NotNull();
            RuleFor(m=>m.Login).NotEmpty();
            RuleFor(m=>m.Refresh).NotEmpty();
            RuleFor(e => e.Token).NotEmpty();
        }
    }
}
