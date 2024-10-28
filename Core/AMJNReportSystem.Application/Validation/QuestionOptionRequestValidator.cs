using AMJNReportSystem.Application.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Validation
{
	internal class QuestionOptionRequestValidator : AbstractValidator<CreateQuestionOptionRequest>
	{
		public QuestionOptionRequestValidator()
		{
			RuleFor(x => x.Text)
				.NotEmpty().WithMessage("Option text is required.")
				.MinimumLength(2).WithMessage("Option text must be at least 2 characters long.");
		}
	}
}
