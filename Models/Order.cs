using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using OrderService.Enums;

namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now
        public string OrderNumber { get; set; }  // Unique order number
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string ShippingAddress { get; set; } // Shipping address
        public decimal TotalAmount { get; set; } // Total amount of the order
        public string PaymentMethod { get; set; } // Payment method (Credit Card, Cash)
        public bool IsPaid { get; set; }          // Payment status
        public DateTime? EstimatedDeliveryDate { get; set; } // Estimated delivery date
    }
}
