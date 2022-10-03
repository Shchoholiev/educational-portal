using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class SkillsService : ISkillsService
    {
        private readonly IGenericRepository<Skill> _skillsRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public SkillsService(IGenericRepository<Skill> skillsRepository, ILogger<SkillsService> logger)
        {
            this._skillsRepository = skillsRepository;
            this._logger = logger;
        }

        public async Task CreateAsync(SkillDto skillDto, CancellationToken cancellationToken)
        {
            if (await this._skillsRepository.ExistsAsync(s => s.Name == skillDto.Name, cancellationToken))
            {
                throw new AlreadyExistsException("skill name", skillDto.Name);
            }

            var skill = this._mapper.Map(skillDto);
            await this._skillsRepository.AddAsync(skill, cancellationToken);

            this._logger.LogInformation($"Created skill with id: {skill.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._skillsRepository.ExistsAsync(s => s.CoursesSkills.Any(cs => cs.SkillId == id),
                                                         cancellationToken))
            {
                throw new DeleteEntityException("This skill is used in other courses!");
            }

            var skill = await this._skillsRepository.GetOneAsync(id, cancellationToken);
            if (skill == null)
            {
                throw new NotFoundException("Skill");
            }

            await this._skillsRepository.DeleteAsync(skill, cancellationToken);

            this._logger.LogInformation($"Deleted skill with id: {skill.Id}.");
        }

        public async Task<PagedList<SkillDto>> GetPageAsync(PageParameters pageParameters, 
                                                            CancellationToken cancellationToken)
        {
            var skills = await this._skillsRepository.GetPageAsync(pageParameters, cancellationToken);
            var dtos = this._mapper.Map(skills);

            this._logger.LogInformation($"Returned skills page {skills.PageNumber} from database.");

            return dtos;
        }
    }
}
