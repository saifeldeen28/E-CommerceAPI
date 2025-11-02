using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManger(IUnitOfWork unitOfWork,IMapper mapper,IBasketRepository basketRepository,UserManager<ApplicationUser> userManager,IConfiguration configuration) : IServiceManger
    {
        private readonly Lazy<IProductService> _LazyproductService=new Lazy<IProductService>(()=>new ProductServices(unitOfWork,mapper));
        private readonly Lazy<IBasketServices> _LazybasketService=new Lazy<IBasketServices>(()=>new BasketService(basketRepository,mapper));
        private readonly Lazy<IAuthenticationServices> _LazyauthenticationService=new Lazy<IAuthenticationServices>(()=>new AuthenticationServices(userManager,configuration));
        public IProductService ProductService => _LazyproductService.Value; 
        public IBasketServices BasketService => _LazybasketService.Value;

        public IAuthenticationServices AuthenticationService => _LazyauthenticationService.Value;
    }
}
