namespace EducationalPortal.Application.Paging
{
    public class PagedList<T> : List<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int TotalItems { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PagedList() { }

        public PagedList(IEnumerable<T> items, PageParameters pageParameters, int totalItems)
        {
            this.PageNumber = pageParameters.PageNumber;
            this.PageSize = pageParameters.PageSize;
            this.TotalItems = totalItems;
            this.TotalPages = (int)Math.Ceiling(totalItems / (double)pageParameters.PageSize);

            this.AddRange(items);
        }
    }
}
