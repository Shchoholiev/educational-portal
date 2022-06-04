using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class ResourceValidator : AbstractValidator<ResourceDto>
    {
        public ResourceValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
