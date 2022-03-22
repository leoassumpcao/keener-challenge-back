namespace API.Core.Entities
{
    public class ProductImage : BaseEntity
    {
        public string Name { get; protected set; }
        public Guid ProductId { get; protected set; }
        public virtual Product Product { get; protected set; } = null!;

        public ProductImage(string name, Guid productId)
        {
            Name = name;
            ProductId = productId;
        }
    }
}
