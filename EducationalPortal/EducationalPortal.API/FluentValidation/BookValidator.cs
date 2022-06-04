using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class BookValidator : AbstractValidator<BookDto>
    {
        public BookValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.PagesCount).NotEmpty().GreaterThan(0);
            RuleFor(b => b.PublicationYear).NotEmpty().GreaterThan(0);
            RuleFor(b => b.Authors).NotEmpty();
        }
    }
}
