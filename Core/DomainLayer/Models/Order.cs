using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Order:BaseEntity<Guid>
    {
        public Order() { }
        public Order(string userEmail, ShippingAddress address, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            UserEmail = userEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }

        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus OrderStatus { get; set; }
        public ShippingAddress Address { get; set; } = null!;
        public DeliveryMethod DeliveryMethod { get; set; }=null!;
        public int DeliveryMethodId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
        public decimal SubTotal {  get; set; }
        public decimal GetTotal()
            => SubTotal+DeliveryMethod.Price;

    }
}
