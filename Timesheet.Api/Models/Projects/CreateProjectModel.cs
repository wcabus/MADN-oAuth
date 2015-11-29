using FluentValidation;
using FluentValidation.Attributes;

namespace Timesheet.Api.Models.Projects
{
    [Validator(typeof(CreateProjectModelValidator))]
    public class CreateProjectModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateProjectModelValidator : AbstractValidator<CreateProjectModel>
    {
        public CreateProjectModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 100);
            RuleFor(x => x.Name).Length(0, 32 * 1024);
        }
    }
}