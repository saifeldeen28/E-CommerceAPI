using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public class OrderWithPaymentIntentSpecification:BaseSpecifications<Order,Guid>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId)
            :base(o=>o.PaymentIntentId==paymentIntentId)
        {
       
        }   
    }
}
