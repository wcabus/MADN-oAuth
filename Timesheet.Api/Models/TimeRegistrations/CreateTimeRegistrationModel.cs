using System;
using FluentValidation;
using FluentValidation.Attributes;

namespace Timesheet.Api.Models.TimeRegistrations
{
    [Validator(typeof(CreateTimeRegistrationModelValidator))]
    public class CreateTimeRegistrationModel
    {
        public Guid TaskId { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public string Remarks { get; set; }
    }

    public class CreateTimeRegistrationModelValidator : AbstractValidator<CreateTimeRegistrationModel>
    {
        public CreateTimeRegistrationModelValidator()
        {
            RuleFor(x => x.TaskId).NotEqual(Guid.Empty);
            RuleFor(x => x.End).GreaterThan(x => x.Start);
            RuleFor(x => x.Remarks).Length(0, 32 * 1024);
        }
    }
}