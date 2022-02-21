namespace EducationalPortal.Core.Entities.EducationalMaterials.Properties
{
    public class Author
    {
        public string FullName { get; set; }

        public List<Books> Books { get; set; }
    }
}
