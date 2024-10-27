using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validation
{
	public  class CreateSubmissionWindowRequestValidator : AbstractValidator<CreateSubmissionWindowRequest>
	{
        public CreateSubmissionWindowRequestValidator()
        {
            RuleFor(x => x.ReportTypeId)
                .NotEmpty().WithMessage("Report Type Id is required.");

            RuleFor(x => x.StartingDate)
                .NotEmpty().WithMessage("Starting date is required.");

            RuleFor(x => x.EndingDate)
                .GreaterThan(x => x.StartingDate).WithMessage("Ending date must be after the starting date.");

            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

            RuleFor(x => x.Year)
                .GreaterThan(2000).WithMessage("Year must be greater than 2000.");

            RuleFor(x => x.IsLocked)
                .NotNull().WithMessage("IsLocked status must be provided.");
        }

        //private bool BeAValidStartingDate(DateTime startingDate)
        //{
        //    if (startingDate.Date > DateTime.Now.Date)
        //        return true;
        //    return startingDate.TimeOfDay >= DateTime.Now.TimeOfDay;
        //}
    }
}
 