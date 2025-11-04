using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using Shared.Dtos.Order_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderServices(IBasketRepository _basketRepository, IMapper _mapper,IUnitOfWork _unitOfWork) : IOrderServices
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            var orderAddress = _mapper.Map<ShippingAddressDto,ShippingAddress>(orderDto.Address);
            var basket =await _basketRepository.GetBasketAsync(orderDto.BasketId)
                ?? throw new BasketNotFoundException(orderDto.BasketId);
            List<OrderItem> orderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items) 
            { 
                var product =await ProductRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                var orderItem = new OrderItem
                {/////////////////////////////////////////////////////////////////////////////////////////////////////// why no id
                    Product = new ProductItemOrdered
                    {
                        PictureUrl = product.PictureUrl,
                        ProductId = product.Id,
                        ProductName = product.Name,
                    },
                    Price = product.Price,
                    Quantity=item.Quantity,
                };
                orderItems.Add(orderItem);
            }
            var deliveryMethod =await  _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);
            var subTotal = orderItems.Sum(i=>i.Quantity*i.Price);
            var order = new Order (email,orderAddress,deliveryMethod,orderItems,subTotal);
            await _unitOfWork.GetRepository<Order,Guid>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<Order,OrderToReturnDto>(order);
        }
    }
}
