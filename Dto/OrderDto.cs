namespace OrderService.Dto
{
    public class OrderDTO
    {
        public string CustomerName { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }

}