using DomainLayer.Models;
using Shared;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class ProductSpecification : BaseSpecifications<Product, int>
    {
        public ProductSpecification(ProductQueryParams queryParams) :
            base(p=>(!queryParams.Brands.HasValue || p.BrandId==queryParams.Brands)&&(!queryParams.Types.HasValue 
            || p.TypeId == queryParams.Types)&&(string.IsNullOrWhiteSpace(queryParams.Search)
            ||p.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p=>p.ProductType);
            if(queryParams.SortingOptions.HasValue)
            {
                switch (queryParams.SortingOptions.Value)
                {
                    case ProductSortingOptions.PriceAsc:
                        AddOrderBy(p => p.Price);
                        break;
                    case ProductSortingOptions.PriceDesc:
                        AddOrderByDescending(p => p.Price);
                        break;
                    case ProductSortingOptions.NameDesc:
                        AddOrderByDescending(p => p.Name);
                        break;
                    case ProductSortingOptions.NameAsc:
                        AddOrderBy(p => p.Name);
                        break;
         
                }
            }
            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        public ProductSpecification(int id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
