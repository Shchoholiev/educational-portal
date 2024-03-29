﻿using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.FinalTasks;

namespace EducationalPortal.Application.Models.CreateDTO
{
    public class CourseCreateDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public List<SkillDto> Skills { get; set; }

        public List<MaterialBaseDto> Materials { get; set; }

        public FinalTaskDto? FinalTask { get; set; }
    }
}
