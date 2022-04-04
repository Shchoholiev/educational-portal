using EducationalPortal.Application.DTO;
using FluentValidation;

namespace EducationalPortal.API.FluentValidation
{
    public class ArticleValidator : AbstractValidator<ArticleDTO>
    {
        public ArticleValidator()
        {
            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.Link).NotEmpty();
            RuleFor(b => b.PublicationDate).NotEmpty();
            RuleFor(b => b.Resource).NotEmpty();
        }
    }
}
