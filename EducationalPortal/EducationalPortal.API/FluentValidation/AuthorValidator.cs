using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class AuthorValidator : AbstractValidator<AuthorDto>
    {
        public AuthorValidator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }
}
