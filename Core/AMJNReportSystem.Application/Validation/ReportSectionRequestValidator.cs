using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validators
{
    public class ReportSectionRequestValidator : AbstractValidator<CreateReportSectionRequest>
    {
        public ReportSectionRequestValidator()
        {
            RuleFor(x => x.ReportSectionName)
                .NotEmpty().WithMessage("Report Section Name is required.")
                .MinimumLength(1).WithMessage("Report Section Name must be Greater than 1")
                .MaximumLength(100).WithMessage("Report Section Name cannot be longer than 100 characters.");

            RuleFor(x => x.ReportSectionValue)
                .NotEmpty().WithMessage("Report Section Value is required.")
                .GreaterThan(0).WithMessage("Report Section Value must be greater than 0.")
                .LessThanOrEqualTo(1000).WithMessage("Report Section Value must be less than or equal to 1000.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.ReportTypeId)
                .NotEmpty().WithMessage("Report Type Id is required.");
        }
    }
}



