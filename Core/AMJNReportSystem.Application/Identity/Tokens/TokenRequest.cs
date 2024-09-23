using AMJNReportSystem.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AMJNReportSystem.Application.Identity.Tokens
{
    public record TokenRequest(string ChandaNo, string Password);

    public class TokenRequestValidator : CustomValidator<TokenRequest>
    {
        public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> T)
        {
            RuleFor(p => p.ChandaNo).Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(T["Invalid Email Address."]);

            RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
                .NotEmpty();
        }
    }
}