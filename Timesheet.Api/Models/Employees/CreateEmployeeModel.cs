using FluentValidation;
using FluentValidation.Attributes;

namespace Timesheet.Api.Models.Employees
{
    [Validator(typeof(CreateEmployeeModelValidator))]
    public class CreateEmployeeModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
    }

    public class CreateEmployeeModelValidator : AbstractValidator<CreateEmployeeModel>
    {
        public CreateEmployeeModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 300);
            RuleFor(x => x.FirstName).NotEmpty().Length(2, 300);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}