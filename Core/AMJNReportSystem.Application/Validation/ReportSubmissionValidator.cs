using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;
using System;

namespace AMJNReportSystem.Application.Validation
{
    public class ReportSubmissionValidator : AbstractValidator<CreateReportSubmissionRequest>
    {
        public ReportSubmissionValidator()
        {
            
            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .GreaterThanOrEqualTo(2020)
                .LessThanOrEqualTo(DateTime.Now.Year)
                .WithMessage($"Year must be between 2020 and {DateTime.Now.Year}.");
           
            RuleFor(x => x.ReportTypeId)
                .NotEmpty()
                .WithMessage("Report Type ID is required.");

            RuleFor(x => x.ReportSubmissionStatus)
                .IsInEnum()
                .WithMessage("Invalid Report Submission Status.");

            RuleFor(x => x.ReportTag)
                .IsInEnum()
                .WithMessage("Invalid Report Tag.");

            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .WithMessage("Creator's name is required.");

            RuleFor(x => x.SubmissionWindowId)
                .NotEmpty()
                .WithMessage("Submission Window ID is required.");
        }
    }
}
