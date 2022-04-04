using EducationalPortal.Core.Entities;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class SkillValidator : AbstractValidator<Skill>
    {
        public SkillValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
