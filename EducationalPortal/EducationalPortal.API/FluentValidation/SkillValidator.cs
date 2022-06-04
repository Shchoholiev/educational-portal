using EducationalPortal.Application.Models.DTO;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class SkillValidator : AbstractValidator<SkillDto>
    {
        public SkillValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
        }
    }
}
