using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Interfaces.EducationalMaterials;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class ResourcesController : ApiControllerBase
    {
        private readonly IResourcesService _resourcesService;

        public ResourcesController(IResourcesService resourcesService)
        {
            this._resourcesService = resourcesService;
        }

        [HttpGet]
        public async Task<IEnumerable<ResourceDto>> GetResourcesAsync([FromQuery] PageParameters pageParameters, 
                                                                      CancellationToken cancellationToken)
        {
            var resources = await this._resourcesService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(resources);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ResourceDto resourceDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._resourcesService.CreateAsync(resourceDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._resourcesService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
