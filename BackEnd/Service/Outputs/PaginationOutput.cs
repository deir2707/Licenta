using System.Collections.Generic;

namespace Service.Outputs
{
    public class PaginationOutput<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}