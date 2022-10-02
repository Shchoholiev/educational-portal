using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Interfaces;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class SkillsController : ApiControllerBase
    {
        private readonly ISkillsService _skillsService;

        public SkillsController(ISkillsService skillsService)
        {
            this._skillsService = skillsService;
        }

        [HttpGet]
        public async Task<IEnumerable<SkillDto>> GetSkillsAsync([FromQuery] PageParameters pageParameters, 
                                                                CancellationToken cancellationToken)
        {
            var skills = await this._skillsService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(skills);
            return skills;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]SkillDto skillDto, CancellationToken cancellationToken)
        {
            await this._skillsService.CreateAsync(skillDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._skillsService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
