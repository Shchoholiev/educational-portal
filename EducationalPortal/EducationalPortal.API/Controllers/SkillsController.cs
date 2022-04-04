using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.API.Mapping;
using EducationalPortal.API.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
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
            var skills = await this._skillsRepository.GetPageAsync(pageParameters);
            var skillCreateModels = this._mapper.Map(skills, chosenSkills);
            var totalCount = await this._skillsRepository.GetCountAsync(s => true);
            var pagedSkills = new PagedList<SkillCreateModel>(skillCreateModels, pageParameters, totalCount);

            return PartialView("_AddSkillsPanel", pagedSkills);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (ModelState.IsValid)
            {
                if (await this._skillsRepository.Exists(s => s.Name == skill.Name))
                {
                    ModelState.AddModelError(string.Empty, "Skill already exists!");
                }
                else
                {
                    await this._skillsRepository.AddAsync(skill);
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateSkill", skill);
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
            if (await this._skillsRepository.Exists(s => s.CoursesSkills.Any(cs => cs.SkillId == idSkills)))
            {
                return Json(new { success = false, message = "This skill is used in other courses!" });
            }
            else
            {
                var skill = await this._skillsRepository.GetOneAsync(idSkills);
                await this._skillsRepository.DeleteAsync(skill);
                return Json(new { success = true });
            }
        }
    }
}
