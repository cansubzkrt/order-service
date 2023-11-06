using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace OrderService.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; } // Foreign Key
        public string ProductName { get; set; }
        public int Quantity { get; set; } // Quantity
        public decimal UnitPrice { get; set; } // Unit price
        public decimal TotalPrice => UnitPrice * Quantity; // Total price, a property that can be automatically calculated
        public Order Order { get; set; } // Ref Order class
    }
}