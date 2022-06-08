using EducationalPortal.Application.Models.CreateDTO;
using FluentValidation;

namespace EducationalPortal.Application.FluentValidation
{
    public class ArticleValidator : AbstractValidator<ArticleCreateDto>
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
