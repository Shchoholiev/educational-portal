using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Models.QueryModels
{
    public class CourseQueryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public int LearningTime { get; set; }

        public bool IsBought { get; set; }

        public IEnumerable<SkillQueryModel> Skills { get; set; }

        public IEnumerable<MaterialQueryModel> Materials { get; set; }

        public User Author { get; set; }
    }
}
