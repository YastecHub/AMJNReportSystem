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
				.LessThan(x => x.EndingDate).WithMessage("Starting date must be before the ending date.")
				.GreaterThanOrEqualTo(DateTime.Now).WithMessage("Starting date must be in the future.");

			RuleFor(x => x.EndingDate)
				.GreaterThan(x => x.StartingDate).WithMessage("Ending date must be after the starting date.");

			RuleFor(x => x.Month)
				.InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

			RuleFor(x => x.Year)
				.GreaterThan(2000).WithMessage("Year must be greater than 2000.");

			RuleFor(x => x.IsLocked)
				.NotNull().WithMessage("IsLocked status must be provided.");
		}
	}
}
 