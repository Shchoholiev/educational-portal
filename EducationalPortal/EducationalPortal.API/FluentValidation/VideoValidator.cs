using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class VideoValidator : AbstractValidator<VideoDto>
    {
        public VideoValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Duration).NotEmpty();
            RuleFor(b => b.Quality).NotEmpty();
        }
    }
}
