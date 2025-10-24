using AutoMapper;
using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManger(IUnitOfWork unitOfWork,IMapper mapper,IBasketRepository basketRepository) : IServiceManger
    {
        private readonly Lazy<IProductService> _LazyproductService=new Lazy<IProductService>(()=>new ProductServices(unitOfWork,mapper));
        private readonly Lazy<IBasketServices> _LazybasketService=new Lazy<IBasketServices>(()=>new BasketService(basketRepository,mapper));
        public IProductService ProductService => _LazyproductService.Value;
        public IBasketServices BasketService => _LazybasketService.Value;
    }
}
