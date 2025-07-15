namespace SolYSalEcommerce.DTOs.Orders
{
    public class CreateOrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string WompiCheckoutUrl { get; set; } = string.Empty;
    }
}