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
        public async Task<ActionResult<IEnumerable<SkillDto>>> GetSkills([FromQuery]PageParameters pageParameters)
        {
            var skills = await this._skillsService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(skills);
            return skills;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]SkillDto skillDto)
        {
            await this._skillsService.CreateAsync(skillDto);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._skillsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
