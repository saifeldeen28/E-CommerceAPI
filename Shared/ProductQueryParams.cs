using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParams
    {
        private const int MaxPageSize = 10; 
        private const int DefualtPageSize = 5;
        public int? Brands { get; set; }
        public int? Types { get; set; }
        public Enums.ProductSortingOptions? SortingOptions { get; set; }
        public string? Search { get; set; }
        public int PageIndex { get; set; } = 1;
        private int pageSize = DefualtPageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
    }
}
