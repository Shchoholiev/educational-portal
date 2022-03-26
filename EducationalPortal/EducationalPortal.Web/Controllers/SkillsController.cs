using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels;
using EducationalPortal.Web.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.Web.Controllers
{
    [Authorize(Roles = "Creator")]
    public class SkillsController : Controller
    {
        private readonly IGenericRepository<Skill> _skillsRepository;

        private readonly Mapper _mapper = new();

        public SkillsController(IGenericRepository<Skill> skillsRepository)
        {
            this._skillsRepository = skillsRepository;
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Skill> chosenSkills)
        {
            var skills = await this._skillsRepository
                                   .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber);
            var skillCreateModels = this._mapper.Map(skills, chosenSkills);
            var totalCount = await this._skillsRepository.GetCountAsync(s => true);
            var pagedSkills = new PagedList<SkillCreateModel>(skillCreateModels, pageParameters, totalCount);

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
        public async Task<IActionResult> Add(int idSkills, List<Skill> skills)
        {
            var skill = await this._skillsRepository.GetOneAsync(idSkills);
            skills.Add(skill);
            return PartialView("_Skills", skills);
        }

        [HttpPost]
        public IActionResult Remove(int idSkills, List<Skill> skills)
        {
            skills.Remove(skills.FirstOrDefault(s => s.Id == idSkills));
            return PartialView("_Skills", skills);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idSkills)
        {
            if ((await this._skillsRepository.GetCountAsync(s => s.Courses.Any(c => c.Skills.Any(s => s.Id == idSkills)))) == 0)
            {
                var skill = await this._skillsRepository.GetOneAsync(idSkills);
                await this._skillsRepository.DeleteAsync(skill);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "This skill is used in other courses!" });
            }
        }
    }
}
