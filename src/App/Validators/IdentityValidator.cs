using App.Models;
using FluentValidation;

namespace App.Validators
{
    public class IdentityValidator : AbstractValidator<Identity>
    {
        public IdentityValidator()
        {
            RuleFor(m => m).NotNull();
            RuleFor(m=>m.Login).NotEmpty();
            RuleFor(e => e.Password).MinimumLength(5);
        }
    }
}
