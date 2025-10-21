using DomainLayer.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductSpecificationCount:BaseSpecifications<Product,int>
    {
        public ProductSpecificationCount(ProductQueryParams queryParams) : 
            base(p => (!queryParams.Brands.HasValue || p.BrandId == queryParams.Brands) && (!queryParams.Types.HasValue
            || p.TypeId == queryParams.Types) && (string.IsNullOrWhiteSpace(queryParams.Search)
            || p.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
        }
    }
}
