using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validators
{
    public class UpdateReportSectionRequestValidator : AbstractValidator<UpdateReportSectionRequest>
    {
        public UpdateReportSectionRequestValidator()
        {
            RuleFor(x => x.ReportSectionName)
                .NotEmpty().WithMessage("Report section name is required.")
                .MaximumLength(100).WithMessage("Report section name cannot exceed 100 characters.");

            RuleFor(x => x.ReportSectionValue)
                .GreaterThan(0).WithMessage("Report section value must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Report section value must be less than or equal to 1000.");

            RuleFor(x => x.Description)
               .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}



