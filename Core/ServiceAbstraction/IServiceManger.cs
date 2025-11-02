using DomainLayer.Contracts;

namespace ServiceAbstraction
{
    public interface IServiceManger
    {
        public IProductService ProductService { get; }
        public IBasketServices BasketService { get; }
        public IAuthenticationServices AuthenticationService { get; }
    }
}
