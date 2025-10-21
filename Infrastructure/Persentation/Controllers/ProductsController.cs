using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared;
using Shared.Dtos;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManger _serviceManger) : ControllerBase

    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams)
        {
            var products = await _serviceManger.ProductService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductDto?>> GetProduct(int Id)
        {
            var product = await _serviceManger.ProductService.GetProductByIdAsync(Id);
            return Ok(product);
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypes()
        {
            var types = await _serviceManger.ProductService.GetAllTypesAsync();
            return Ok(types);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await _serviceManger.ProductService.GetAllBrandsAsync();
            return Ok(brands);
        }

    } 
}
