using System.Collections.Generic;
using System;

namespace SharedKernel.Common
{
    public class PaginatedResult<TModel>
    {
        public PaginationModel Pagination { get; set; } = new PaginationModel();
        public List<TModel> Items { get; private set; }
        public PaginatedResult(List<TModel> items, int count, int pageNumber, int pageSize)
        {
            Pagination.TotalCount = count;
            Pagination.PageSize = pageSize;
            Pagination.CurrentPage = pageNumber;
            Pagination.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
        }
    }
}
