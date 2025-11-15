using AutoMapper;
using DomainLayer.Models;
using Shared.Dtos;
using Shared.Dtos.Identity_dtos;
using Shared.Dtos.Order_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dist=>dist.productBrand,options=>options.MapFrom(src=>src.ProductBrand.Name))
                .ForMember(dist => dist.productType, options => options.MapFrom(src => src.ProductType.Name))
                .ForMember(dist=>dist.PictureUrl,options=>options.MapFrom<PictureUrlResolver>());
            CreateMap<ProductType,TypeDto>();
            CreateMap<ProductBrand, BrandDto>();

            CreateMap<CustomerBasket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<ShippingAddressDto, ShippingAddress>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dist=>dist.DeliveryMethod,options=>options.MapFrom(src=>src.DeliveryMethod.ShortName))
                .ForMember(d=>d.Total,o=>o.MapFrom(s=>s.GetTotal())).ReverseMap();
            CreateMap<OrderItem, OrderItemsDto>()
                .ForMember(dist => dist.ProductName, options => options.MapFrom(src => src.Product.ProductName))
                .ForMember(dist => dist.Price, options => options.MapFrom(src => src.Price))
                .ForMember(dist => dist.ProductId, options => options.MapFrom(src => src.Product.ProductId))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
