using EducationalPortal.Application.Models.CreateDTO;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class VideoValidator : AbstractValidator<VideoCreateDto>
    {
        public VideoValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Duration).NotEmpty();
            RuleFor(b => b.Quality).NotEmpty();
        }
    }
}
