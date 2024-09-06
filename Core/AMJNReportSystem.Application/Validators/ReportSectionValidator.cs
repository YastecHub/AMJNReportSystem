using AMJNReportSystem.Application.Models.DTOs;
using FluentValidation;

namespace AMJNReportSystem.Application.Validators
{
    public class ReportSectionValidator : AbstractValidator<ReportSectionDto>
    {
        public ReportSectionValidator()
        {
            RuleFor(x => x.ReportSectionName)
                .NotEmpty().WithMessage("Report Section Name is required.")
                .MaximumLength(10).WithMessage("Report Section Name cannot be longer than 10 characters.");

            
            RuleFor(x => x.ReportSectionValue)
                .GreaterThan(0).WithMessage("Report Section Value must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Report Section Value must be less than or equal to 100.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");

            RuleFor(x => x.ReportTypeId)
                .NotEmpty().WithMessage("Report Type Id is required.");
        }
    }
}
