using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validation
{
	public class UpdateQuestionRequestValidator : AbstractValidator<UpdateQuestionRequest>
	{
		public UpdateQuestionRequestValidator()
		{

			RuleFor(x => x.QuestionName)
				.NotEmpty().WithMessage("Question Name is required.")
				.MinimumLength(4).WithMessage("Question Name must be at least 4 characters long.");

			RuleFor(x => x.ReportSectionId)
				.NotEqual(Guid.Empty).WithMessage("Report Section Id must be a valid non-empty GUID.");

			RuleFor(x => x.QuestionType)
				.IsInEnum().WithMessage("Invalid Question Type provided.");

	
			RuleFor(x => x.ResponseType)
				.IsInEnum().WithMessage("Invalid Response Type provided.");

			RuleFor(x => x.IsRequired)
				.NotNull().WithMessage("IsRequired must be specified.");
		
			RuleFor(x => x.IsActive)
				.NotNull().WithMessage("IsActive must be specified.");

		}
	}
}
