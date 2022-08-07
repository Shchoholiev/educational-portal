using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IBooksService
    {
        Task<PagedList<BookDto>> GetPageAsync(PageParameters pageParameters, 
                                              CancellationToken cancellationToken);

        Task CreateAsync(BookCreateDto bookDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
