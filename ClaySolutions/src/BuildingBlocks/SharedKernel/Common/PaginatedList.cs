using System.Collections.Generic;

namespace SharedKernel.Common
{
    public class PaginatedList<TModel>
    {
        public PaginationModel Pagination { get; set; }
        public List<TModel> Items { get; set; }
    }
}
