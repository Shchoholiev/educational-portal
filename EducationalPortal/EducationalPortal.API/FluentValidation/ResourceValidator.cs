using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class ResourceValidator : AbstractValidator<ResourceDTO>
    {
        public ResourceValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
