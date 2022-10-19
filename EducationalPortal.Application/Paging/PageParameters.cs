namespace EducationalPortal.Application.Paging
{
    public class PageParameters
    {
        public int PageSize { get; set; } = 12;

        public int PageNumber { get; set; } = 1;

        public PageParameters() { }

        public PageParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
