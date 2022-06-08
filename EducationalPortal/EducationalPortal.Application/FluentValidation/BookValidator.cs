using EducationalPortal.Application.Models.CreateDTO;
using FluentValidation;

namespace EducationalPortal.Application.FluentValidation
{
    public class BookValidator : AbstractValidator<BookCreateDto>
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
