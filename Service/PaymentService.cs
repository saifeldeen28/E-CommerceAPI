using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService(IConfiguration _configuration,IBasketRepository _basketRepository,IUnitOfWork _unitOfWork,IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basket =await _basketRepository.GetBasketAsync(basketId)?? throw new BasketNotFoundException(basketId);
            var productRepository = _unitOfWork.GetRepository<DomainLayer.Models.Product,int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepository.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
                
                
            }
            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethod =await _unitOfWork.GetRepository<DomainLayer.Models.DeliveryMethod,int>().GetByIdAsync(basket.DeliveryMethodId.Value )
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = deliveryMethod.Price;
            var basketTotal = (long)basket.Items.Sum(x => (x.Quantity * x.Price))+basket.DeliveryMethodId;
            var service = new PaymentIntentService();
            if(basket.PaymentIntentId==null)
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = basketTotal,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                var intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = basketTotal
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }
            await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return mapper.Map<BasketDto>(basket);
        }
    }
}
