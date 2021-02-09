using App.Models;
using FluentValidation;

namespace App.Validators
{
    public class DepartmentValidator : AbstractValidator<Department>
    {
        public DepartmentValidator()
        {
            RuleFor(m=>m.Name).Length(1, 32);            
        }
    }
}
