using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using System;
using System.Threading.Tasks;
using OrderService.Dto;

namespace OrderService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderQueueService _orderQueueService;

        public OrdersController(OrderQueueService orderQueueService)
        {
            _orderQueueService = orderQueueService;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] OrderDTO orderDto)
        {
            var order = new Order 
            {
                CustomerName = orderDto.CustomerName,
                ShippingAddress = orderDto.ShippingAddress,
                TotalAmount = orderDto.TotalAmount,
                PaymentMethod = orderDto.PaymentMethod,
                IsPaid = orderDto.IsPaid,
                EstimatedDeliveryDate = orderDto.EstimatedDeliveryDate
            };

            order.OrderNumber = Guid.NewGuid().ToString();
            
            // order send to rabbitmq
            _orderQueueService.Publish(order);

            // success response
            return Ok(new { Message = "Order has been sent to the queue." });
        }

        // get order by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
}

}
