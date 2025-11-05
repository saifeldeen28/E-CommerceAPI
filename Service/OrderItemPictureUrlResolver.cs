using AutoMapper;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Order_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrderItemPictureUrlResolver(IConfiguration configuration) : IValueResolver<OrderItem, OrderItemsDto, string>
    {
        public string Resolve(OrderItem source, OrderItemsDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return String.Empty;
            }
            return $"{configuration.GetSection("URLS")["BaseUrl"]}{source.Product.PictureUrl}";
        }
    }
}
