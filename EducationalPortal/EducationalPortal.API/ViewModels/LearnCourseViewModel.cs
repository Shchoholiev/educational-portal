namespace EducationalPortal.API.ViewModels
{
    public class LearnCourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Progress { get; set; }

        public List<MaterialsBaseViewModel> Materials { get; set; }
    }
}
