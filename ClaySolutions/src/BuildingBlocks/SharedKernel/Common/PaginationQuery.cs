namespace SharedKernel.Common
{
    public class PaginationQuery
    {
        private const int MAXPAGESIZE = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize { get { return _pageSize; } set { _pageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value; } }
    }
}
