namespace EducationalPortal.API.ViewModels
{
    public class BookViewModel : MaterialsBaseViewModel
    {
        public int PagesCount { get; set; }

        public string Extension { get; set; }

        public int PublicationYear { get; set; }

        public List<string> Authors { get; set; }
    }
}
