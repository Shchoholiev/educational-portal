﻿using EducationalPortal.Application.DTO;
using FluentValidation;

namespace EducationalPortal.Web.FluentValidation
{
    public class VideoValidator : AbstractValidator<VideoDTO>
    {
        public VideoValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.File).NotEmpty();
            RuleFor(b => b.Duration).NotEmpty();
            RuleFor(b => b.Quality).NotEmpty();
        }
    }
}
