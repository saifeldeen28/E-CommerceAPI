using AutoMapper;
using DomainLayer.Models;
using Microsoft.Extensions.Configuration;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Service
{
    public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                                return null!;
            }
            var baseUrl = $"{configuration.GetSection("URLS")["BaseUrl"]}{source.PictureUrl}";
            return baseUrl;
        }
    }
}
