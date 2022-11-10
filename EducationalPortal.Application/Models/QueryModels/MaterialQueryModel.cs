using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.QueryModels
{
    public class MaterialQueryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public int LearningMinutes { get; set; }

        public int Index { get; set; }

        public bool IsLearned { get; set; }

        public int PagesCount { get; set; }

        public Extension Extension { get; set; }

        public int PublicationYear { get; set; }

        public List<Author> Authors { get; set; }

        public Resource Resource { get; set; }

        public DateTime PublicationDate { get; set; }

        public DateTime Duration { get; set; }

        public Quality Quality { get; set; }
    }
}
