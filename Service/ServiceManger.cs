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
    public class ServiceManger(IUnitOfWork unitOfWork,IMapper mapper) : IServiceManger
    {
        private readonly Lazy<IProductService> _LazyproductService=new Lazy<IProductService>(()=>new ProductServices(unitOfWork,mapper));
        public IProductService ProductService => _LazyproductService.Value;
    }
}
