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
                .MaximumLength(50).WithMessage("Report section name cannot exceed 50 characters.");

            RuleFor(x => x.ReportSectionValue)
                .GreaterThan(0).WithMessage("Report section value must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Report section value must be less than or equal to 100.");

            RuleFor(x => x.Description)
               .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");
        }
    }
}



