using FluentValidation;
using FluentValidation.Attributes;
using System;

namespace Timesheet.Api.Models.Tasks
{
    [Validator(typeof(CreateTaskModelValidator))]
    public class CreateTaskModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan? AvailableTime { get; set; }
    }

    public class CreateTaskModelValidator : AbstractValidator<CreateTaskModel>
    {
        public CreateTaskModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(3, 100);
            RuleFor(x => x.Description).Length(0, 32 * 1024);
        }
    }
}