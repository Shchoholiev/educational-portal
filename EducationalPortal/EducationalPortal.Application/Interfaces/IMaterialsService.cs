using EducationalPortal.Core.Entities.EducationalMaterials;

namespace EducationalPortal.Application.Interfaces
{
    public interface IMaterialsService<TMaterials> where TMaterials : MaterialsBase
    {
        Task AddAsync(TMaterials materials);

        Task<TMaterials> GetOneAsync(int id);

        Task<IEnumerable<TMaterials>> GetAllAsync();

        Task<IEnumerable<TMaterials>> GetPageAsync(int pageSize, int pageNumber);

        Task UpdateAsync(TMaterials materials);

        Task DeleteAsync(int id);
    }
}
