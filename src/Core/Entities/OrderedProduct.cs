namespace API.Core.Entities
{
    public class OrderedProduct : BaseEntity
    {
        public Guid OrderId { get; protected set; }
        public Guid ProductId { get; protected set; }
        public uint Quantity { get; protected set; }
        public decimal UnitPrice { get; protected set; }
        public virtual Product Product { get; protected set; } = null!;
        public virtual Order Order { get; protected set; } = null!;

        public OrderedProduct(Guid orderId, Guid productId, uint quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
