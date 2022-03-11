namespace EducationalPortal.Core.Entities.EducationalMaterials.Properties
{
    public class Author : EntityBase
    {
        public string FullName { get; set; }

        public List<Book> Books { get; set; }
    }
}
