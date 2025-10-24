using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController(IServiceManger _serviceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBasket(string key)
        {
            var basket = await _serviceManger.BasketService.GetBasketAsync(key);
            return Ok(basket);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateBasket(BasketDto basket)
        {
            var updatedBasket = await _serviceManger.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(updatedBasket);
        }
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteBasket(string key)
        {
            var res=await _serviceManger.BasketService.DeleteBasketAsync(key);
            return Ok(res);
        }
    }
}
