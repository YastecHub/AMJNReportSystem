using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;
using System;

namespace AMJNReportSystem.Application.Validation
{
    public class ReportSubmissionValidator : AbstractValidator<CreateReportSubmissionRequest>
    {
        public ReportSubmissionValidator()
        {
           
            RuleFor(x => x.SubmissionWindowId)
                .NotEmpty()
                .WithMessage("Submission Window ID is required.");
        }
    }
}
