namespace EducationalPortal.Application.Paging
{
    public interface IPagedList
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; }

        public bool HasNextPage { get; }
    }
}
