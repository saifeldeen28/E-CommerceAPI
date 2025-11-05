using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Order_Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Persentation.Controllers
{
    public class OrderController(IServiceManger _serviceManger) : APIBaseController
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _serviceManger.OrderService.CreateOrderAsync(orderDto, GetEmailFromToken());
            return Ok(order);
        }
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
        {
            var deliveryMethods = await _serviceManger.OrderService.GetAllDeliveryMethodsAsync();
            return Ok(deliveryMethods);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = GetEmailFromToken();
            var orders = await _serviceManger.OrderService.GetAllOrdersAsync(email);
            return Ok(orders);
        }
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(Guid id)
        {
            var order = await _serviceManger.OrderService.GetOrderByIdAsync(id);
            return Ok(order);
        }
    }
}
