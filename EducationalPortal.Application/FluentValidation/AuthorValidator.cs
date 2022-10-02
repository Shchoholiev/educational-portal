using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.Application.FluentValidation
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }
}
