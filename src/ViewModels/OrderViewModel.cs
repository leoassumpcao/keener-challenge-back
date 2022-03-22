using System.Text.Json.Serialization;

namespace API.ViewModels
{
    public class OrderViewModel
    {
        [JsonPropertyName("orderId")]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPaid { get; set; }
        public AddressViewModel DeliveryAddress { get; set; }
        public bool Delivered { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<OrderedProductViewModel> OrderedProducts { get; set; }


        public OrderViewModel(Guid id, Guid userId, decimal total,
            decimal totalPaid, bool delivered, AddressViewModel deliveryAddress, DateTime createdAt,
            IEnumerable<OrderedProductViewModel> orderedProducts)
        {
            Id = id;
            UserId = userId;
            Total = total;
            TotalPaid = totalPaid;
            DeliveryAddress = deliveryAddress;
            Delivered = delivered;
            CreatedAt = createdAt;
            OrderedProducts = orderedProducts;
        }
    }
}
