using EducationalPortal.Application.DTO;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class SkillValidator : AbstractValidator<SkillDTO>
    {
        public SkillValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
