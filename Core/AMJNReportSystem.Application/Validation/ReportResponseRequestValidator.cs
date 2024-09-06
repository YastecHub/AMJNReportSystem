using AMJNReportSystem.Application.Models.DTOs;
using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validators
{
    public class ReportResponseRequestValidator : AbstractValidator<CreateReportResponseRequest>
    {
        public ReportResponseRequestValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("Question Id is required.")
                .Must(x => x != Guid.Empty).WithMessage("Invalid Question Id.");

            RuleFor(x => x.TextAnswer)
                .NotEmpty().WithMessage("Text Answer is required.")
                .MaximumLength(500).WithMessage("Text Answer cannot exceed 500 characters.");

            RuleFor(x => x.QuestionOptionId)
                .NotEmpty().WithMessage("Question Option Id is required.")
                .Must(x => x != Guid.Empty).WithMessage("Invalid Question Option Id.");

            RuleFor(x => x.Report)
                .NotEmpty().WithMessage("Report is required.")
                .MaximumLength(1000).WithMessage("Report cannot exceed 1000 characters.");
        }
    }

    public class UpdateReportResponseRequestValidator : AbstractValidator<UpdateReportResponseRequest>
    {
        public UpdateReportResponseRequestValidator()
        {
            RuleFor(x => x.TextAnswer)
                .NotEmpty().WithMessage("Text Answer is required.")
                .MaximumLength(500).WithMessage("Text Answer cannot exceed 500 characters.");

            RuleFor(x => x.Report)
                .NotEmpty().WithMessage("Report is required.")
                .MaximumLength(1000).WithMessage("Report cannot exceed 1000 characters.");
        }
    }
}
