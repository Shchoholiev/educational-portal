using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class AuthorValidator : AbstractValidator<AuthorDTO>
    {
        public AuthorValidator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }
}
