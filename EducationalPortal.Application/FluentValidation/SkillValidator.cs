using EducationalPortal.Application.Models.DTO;
using FluentValidation;

namespace EducationalPortal.Application.FluentValidation
{
    public class SkillValidator : AbstractValidator<SkillDto>
    {
        public SkillValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
