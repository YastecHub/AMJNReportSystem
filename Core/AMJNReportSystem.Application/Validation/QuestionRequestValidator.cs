using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;

namespace AMJNReportSystem.Application.Validation
{
	public class QuestionRequestValidator : AbstractValidator<CreateQuestionRequest>
	{
		public QuestionRequestValidator()
		{

			RuleFor(x => x.QuestionName)
				.NotEmpty().WithMessage("Question Name is required.")
				.MinimumLength(7).WithMessage("Question Name must be at least 7 characters long.");

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
			 
			When(x => x.Options != null && x.Options.Count > 0, () =>
			{
				RuleForEach(x => x.Options).SetValidator(new QuestionOptionRequestValidator());
			});
		}
	}
}
