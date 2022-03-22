using API.Core.ValueObjects;

namespace API.Core.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; protected set; }
        public virtual ApplicationUser User { get; protected set; } = null!;
        public decimal Total { get; protected set; }
        public decimal TotalPaid { get; protected set; }
        public Address DeliveryAddress { get; protected set; }
        public bool Delivered { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public virtual IEnumerable<OrderedProduct> OrderedProducts { get; protected set; } = null!;

        public Order(Guid userId, decimal total, decimal totalPaid, bool delivered, Address deliveryAddress, DateTime createdAt)
        {
            UserId = userId;
            Total = total;
            TotalPaid = totalPaid;
            DeliveryAddress = deliveryAddress;
            Delivered = delivered;
            CreatedAt = createdAt;
        }

        public Order()
        {
            UserId = Guid.Empty;
            Total = 0;
            TotalPaid = 0;
            DeliveryAddress = Address.Empty();
            Delivered = false;
            CreatedAt = DateTime.MinValue;
        }
    }
}
