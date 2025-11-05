using DomainLayer.Contracts;
using Service;

namespace ServiceAbstraction
{
    public interface IServiceManger
    {
        public IProductService ProductService { get; }
        public IBasketServices BasketService { get; }
        public IAuthenticationServices AuthenticationService { get; }
        public IOrderServices OrderService { get; }
    }
}
