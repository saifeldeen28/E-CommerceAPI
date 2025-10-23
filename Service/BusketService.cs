using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BusketService(IBasketRepository _basketRepository, IMapper _mapper) : IBasketServices
    {
        public async Task<BasketDto?> CreateOrUpdateBasketAsync(BasketDto basket)
        {
             var customerbasket = _mapper.Map<BasketDto, CustomerBasket>(basket);
             var created =  _basketRepository.CreateOrUpdateBasketAsync(customerbasket);
            if(created == null) throw new Exception("Failed to create or update basket");
             return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string key)
        => await _basketRepository.DeleteBasketAsync(key);

        public async Task<BasketDto?> GetBasketAsync(string key)
        {
            var basket =  await _basketRepository.GetBasketAsync(key);
            if(basket == null) throw new BasketNotFoundException(key);
            return _mapper.Map<CustomerBasket,BasketDto>(basket);
        }
    }
}
