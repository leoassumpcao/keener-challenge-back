namespace API.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public decimal Price { get; protected set; }
        public uint InStock { get; protected set; }

        public Guid? CategoryId { get; protected set; }
        public virtual Category Category { get; protected set; } = null!;

        public virtual ICollection<ProductImage> Images { get; protected set; } = null!;

        public Product(string name, string description, decimal price, uint inStock, Guid? categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            InStock = inStock;
            CategoryId = categoryId;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdatePrice(decimal price)
        {
            Price = price;
        }

        public void UpdateCategoryId(Guid? categoryId)
        {
            CategoryId = categoryId;
        }

        public void UpdateStock(uint quantity)
        {
            InStock = quantity;
        }

        public bool RemoveFromStock(uint quantity)
        {
            if (InStock > 0 && InStock >= quantity)
            {
                InStock -= quantity;
                return true;
            }
            return false;
        }
    }
}
