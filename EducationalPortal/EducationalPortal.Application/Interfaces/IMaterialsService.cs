using EducationalPortal.Core.Entities.EducationalMaterials;

namespace EducationalPortal.Application.Interfaces
{
    public interface IMaterialsService<TMaterials> where TMaterials : MaterialsBase
    {
        Task Add(TMaterials materials);

        Task<TMaterials> GetOne(int id);

        Task<IEnumerable<TMaterials>> GetAll();

        Task<IEnumerable<TMaterials>> GetPage(int pageSize, int pageNumber);

        Task Update(TMaterials materials);

        Task Delete(int id);
    }
}
