using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.API.Mapping;
using EducationalPortal.API.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class ResourcesController : Controller
    {
        private readonly IGenericRepository<Resource> _resourcesRepository;

        private readonly Mapper _mapper = new();

        public ResourcesController(IGenericRepository<Resource> resourcesRepository)
        {
            this._resourcesRepository = resourcesRepository;
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters, Resource chosenResource)
        {
            pageParameters.PageSize = 3;
            var resources = await this._resourcesRepository.GetPageAsync(pageParameters);
            var resourceCreateModels = this._mapper.Map(resources, chosenResource);
            var totalCount = await this._resourcesRepository.GetCountAsync(s => true);
            var pagedResources = new PagedList<ResourceCreateModel>(resourceCreateModels, pageParameters, totalCount);

            return PartialView("_AddResources", pagedResources);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                if (await this._resourcesRepository.Exists(r => r.Name == resource.Name))
                {
                    ModelState.AddModelError(string.Empty, "Resource already exists!");
                }
                else
                {
                    await this._resourcesRepository.AddAsync(resource);
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateResource", resource);
        }

        [HttpGet]
        public async Task<IActionResult> Choose(int id)
        {
            var resource = await this._resourcesRepository.GetOneAsync(id);
            return PartialView("_Resource", resource);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._resourcesRepository.Exists(r => r.Articles.Any(a => a.Resource.Id == id)))
            {
                return Json(new { success = false, message = "This resource is used in other courses!" });
            }
            else
            {
                var resource = await this._resourcesRepository.GetOneAsync(id);
                await this._resourcesRepository.DeleteAsync(resource);
                return Json(new { success = true });
            }
        }
    }
}
