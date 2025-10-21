using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductServices(IUnitOfWork _unitOfWork,IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo= _unitOfWork.GetRepository<ProductBrand,int>();
            var brands=await repo.GetAllAsync();
            var brandDtos = _mapper.Map< IEnumerable < ProductBrand > ,IEnumerable <BrandDto>>(brands);
            return brandDtos;
        }

        public async Task<PaginatedResult<ProductDto>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var spec=new ProductSpecification(queryParams);
            var Products = await _unitOfWork.GetRepository<Product, int>().GetAllAsync(spec);
            var productDtos = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(Products);
            var productcount =productDtos.Count();
            var CountSpec = new ProductSpecificationCount(queryParams);
            var totalCount = await _unitOfWork.GetRepository<Product, int>().CountAsync(CountSpec);
            return new PaginatedResult<ProductDto>(queryParams.PageIndex,productcount,totalCount,productDtos);
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typeDtos = _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(Types);
            return typeDtos;
        }

        public async Task<ProductDto>? GetProductByIdAsync(int id)
        {
            var spec = new ProductSpecification(id);
            var product =await  _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);
            var productDto = _mapper.Map<Product, ProductDto>(product);
            if (productDto == null)
            {
                throw new ProductNotFoundException(id);
            }
            return productDto;
        }

        
    }
}
