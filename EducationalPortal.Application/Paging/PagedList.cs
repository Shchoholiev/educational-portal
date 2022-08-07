namespace EducationalPortal.Application.Paging
{
    public class PagedList<T> : List<T>, IPagedList
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PagedList() { }

        public PagedList(IEnumerable<T> items, PageParameters pageParameters, int totalItems)
        {
            this.PageNumber = pageParameters.PageNumber;
            this.PageSize = pageParameters.PageSize;
            this.TotalPages = (int)Math.Ceiling(totalItems / (double)pageParameters.PageSize);

            this.AddRange(items);
        }

        public void MapList(IPagedList list)
        {
            this.PageNumber = list.PageNumber;
            this.PageSize = list.PageSize;
            this.TotalPages = list.TotalPages;
        }
    }
}
