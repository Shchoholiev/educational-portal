using EducationalPortal.Application.Paging;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/skills")]
    public class SkillsController : Controller
    {
        private readonly IGenericRepository<Skill> _skillsRepository;

        public SkillsController(IGenericRepository<Skill> skillsRepository)
        {
            this._skillsRepository = skillsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills([FromQuery]PageParameters pageParameters)
        {
            var skills = await this._skillsRepository.GetPageAsync(pageParameters);
            var metadata = new
            {
                skills.PageSize,
                skills.PageNumber,
                skills.TotalPages,
                skills.HasNextPage,
                skills.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return skills;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]SkillDto skillDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._skillsRepository.Exists(s => s.Name == skillDTO.Name))
                {
                    ModelState.AddModelError("Name", "Skill already exists!");
                }
                else
                {
                    var skill = new Skill { Id = skillDTO.Id, Name = skillDTO.Name };
                    await this._skillsRepository.AddAsync(skill);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._skillsRepository.Exists(s => s.Courses.Any(c => c.Skills.Any(s => s.Id == id))))
            {
                return BadRequest("This skill is used in other courses!");
            }
            else
            {
                var skill = await this._skillsRepository.GetOneAsync(id);
                if (skill == null)
                {
                    return NotFound();
                }

                await this._skillsRepository.DeleteAsync(skill);
                return NoContent();
            }
        }
    }
}
