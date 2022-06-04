﻿using EducationalPortal.Application.Models.DTO;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class CourseValidator : AbstractValidator<CourseDto>
    {
        public CourseValidator()
        {
            RuleFor(c => c.Name).NotEmpty().Length(5, 100);
            RuleFor(c => c.Thumbnail).NotEmpty();
            RuleFor(c => c.ShortDescription).NotEmpty().Length(30, 150);
            RuleFor(c => c.Description).NotEmpty().Length(100, 3000);
            RuleFor(c => c.Price).NotEmpty().ExclusiveBetween(0, 1000);
            RuleFor(c => c.Skills).NotEmpty().WithMessage("Choose at least one skill!");
            RuleFor(c => c.Materials).NotEmpty().WithMessage("Choose at least one material!");
        }
    }
}
