using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
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
        public async Task<ActionResult<IEnumerable<ResourceDto>>> GetResources([FromQuery]PageParameters pageParameters)
        {
            var resources = await this._resourcesService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(resources);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ResourceDto resourceDto)
        {
            await this._resourcesService.CreateAsync(resourceDto);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._resourcesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
