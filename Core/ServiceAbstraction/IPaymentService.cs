using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IPaymentService
    {
        public Task<BasketDto> CreateOrUpdateBasketAsync(string basketId);
        //public Task UpdateOrderPaymentStatusAsync(string request, string StripeHeader);
        //it wasnt included in the implementation that was sent on whatsapp
    }
}
