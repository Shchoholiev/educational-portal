using EducationalPortal.Core.Entities;
using FluentValidation;

namespace EducationalPortal.Web.FluentValidation
{
    public class SkillValidator : AbstractValidator<Skill>
    {
        public SkillValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
