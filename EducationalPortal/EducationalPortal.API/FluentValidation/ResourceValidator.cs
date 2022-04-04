using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class ResourceValidator : AbstractValidator<Resource>
    {
        public ResourceValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
