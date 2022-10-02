using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using FluentValidation;

namespace EducationalPortal.Web.FluentValidation
{
    public class ResourceValidator : AbstractValidator<Resource>
    {
        public ResourceValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
