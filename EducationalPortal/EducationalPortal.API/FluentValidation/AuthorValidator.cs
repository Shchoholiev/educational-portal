using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(c => c.FullName).NotEmpty();
        }
    }
}
