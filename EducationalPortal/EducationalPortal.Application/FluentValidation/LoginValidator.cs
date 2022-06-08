using EducationalPortal.Application.Models;
using FluentValidation;

namespace EducationalPortal.Application.FluentValidation
{
    public class LoginValidator : AbstractValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty()
                                    .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,20}$")
                                    .WithMessage("Password must contain at least one upper and lower case " +
                                                 "letter, at least one digit, one special letter and " +
                                                 "minimum 6 symbols length.");
        }
    }
}
