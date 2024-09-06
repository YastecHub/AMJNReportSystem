using AMJNReportSystem.Application.Models.RequestModels;
using AMJNReportSystem.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Application.Validation
{
    public class ReportTypeRequestValidator : AbstractValidator<CreateReportTypeRequest>
    {
        public ReportTypeRequestValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MinimumLength(7).WithMessage("Name must be at least 7 characters long.");

            RuleFor(x => x.Description)
                 .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(7).WithMessage("Descriptionu must be at least 7 characters long.");

            RuleFor(x => x.Year)
           .NotEmpty().WithMessage("Year is required.")
           .InclusiveBetween(1000, 9999).WithMessage("Year must be a 4-digit number.");


            RuleFor(x => x.ReportTag)
                .IsInEnum().WithMessage("Invalid Report Tag provided.");


            //When(x => x.Options != null && x.Options.Count > 0, () =>
            //{
            //    RuleForEach(x => x.Options).SetValidator(new ReportTypeRequestValidator());
            //});
        }
    }
   
}
