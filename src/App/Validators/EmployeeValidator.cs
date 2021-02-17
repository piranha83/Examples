using App.Models;
using FluentValidation;

namespace App.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(m => m).NotNull();
            RuleFor(m=>m.FirstName).Length(1, 32);
            RuleFor(e => e.Salary).GreaterThan(0.0m).ScalePrecision(2, 10);
        }
    }
}
