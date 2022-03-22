namespace API.ViewModels
{
    public class OrderedProductViewModel
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }

        public Guid ProductId { get; set; }

        public ProductViewModel? Product { get; set; } = null;

        public uint Quantity { get; set; }
        public decimal UnitPrice { get; set; }


        public OrderedProductViewModel(
            Guid? id,
            Guid? orderId,
            Guid productId,
            uint quantity,
            decimal unitPrice)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Product = null;
        }

        public OrderedProductViewModel(
            Guid? id,
            Guid? orderId,
            Guid productId,
            uint quantity,
            decimal unitPrice,
            ProductViewModel? product)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Product = product;
        }

        public OrderedProductViewModel()
        {
            Id = Guid.Empty;
            OrderId = Guid.Empty;
            ProductId = Guid.Empty;
            Quantity = 0;
            UnitPrice = 0;
            Product = null;
        }
    }
}
