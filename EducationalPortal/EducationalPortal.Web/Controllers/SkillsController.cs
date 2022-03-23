using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EducationalPortal.Web.Controllers
{
    [Authorize(Roles = "Creator")]
    public class SkillsController : Controller
    {
        private readonly IGenericRepository<Skill> _skillsRepository;

        public SkillsController(IGenericRepository<Skill> skillsRepository)
        {
            this._skillsRepository = skillsRepository;
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters)
        {
            var skills = await this._skillsRepository
                                   .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber);
            var totalCount = await this._skillsRepository.GetCountAsync(s => true);
            var pagedSkills = new PagedList<Skill>(skills, pageParameters, totalCount);

            return PartialView("_AddSkillsPanel", pagedSkills);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Skill skill)
        {
            if ((await this._skillsRepository.GetAllAsync(s => s.Name == skill.Name)).Count() > 0)
            {
                ModelState.AddModelError(string.Empty, "Skill already exists!");
                return PartialView("_CreateSkill", skill);
            }
            else
            {
                await this._skillsRepository.AddAsync(skill);
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(int skillId, List<Skill> skills)
        {
            var skill = await this._skillsRepository.GetOneAsync(skillId);
            skills.Add(skill);
            return PartialView("_Skills", skills);
        }
    }
}
