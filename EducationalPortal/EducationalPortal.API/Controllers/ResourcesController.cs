using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/resources")]
    public class ResourcesController : Controller
    {
        private readonly IGenericRepository<Resource> _resourcesRepository;

        public ResourcesController(IGenericRepository<Resource> resourcesRepository)
        {
            this._resourcesRepository = resourcesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Resource>>> GetResources([FromQuery]PageParameters pageParameters)
        {
            var resources = await this._resourcesRepository.GetPageAsync(pageParameters);
            var metadata = new
            {
                resources.TotalItems,
                resources.PageSize,
                resources.PageNumber,
                resources.TotalPages,
                resources.HasNextPage,
                resources.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ResourceDTO resourceDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._resourcesRepository.Exists(r => r.Name == resourceDTO.Name))
                {
                    ModelState.AddModelError(string.Empty, "Resource already exists!");
                }
                else
                {
                    var resource = new Resource { Id = resourceDTO.Id, Name = resourceDTO.Name };
                    await this._resourcesRepository.AddAsync(resource);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._resourcesRepository.Exists(r => r.Articles.Any(a => a.Resource.Id == id)))
            {
                return BadRequest("This resource is used in other courses!");
            }
            else
            {
                var resource = await this._resourcesRepository.GetOneAsync(id);
                if (resource == null)
                {
                    return NotFound();
                }

                await this._resourcesRepository.DeleteAsync(resource);
                return NoContent();
            }
        }
    }
}
