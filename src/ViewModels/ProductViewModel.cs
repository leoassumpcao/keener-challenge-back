namespace API.ViewModels
{
    public class ProductViewModel
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; protected set; }
        public string Thumb { get; set; }

        public Guid? CategoryId { get; set; }

        public ProductViewModel(Guid productId, string name, string description, decimal price, bool inStock, string thumb, Guid? categoryId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            InStock = inStock;
            Thumb = thumb;
        }

        protected ProductViewModel()
        {
            ProductId = Guid.Empty;
            Name = "";
            Description = "";
            Price = 0;
            CategoryId = Guid.Empty;
            InStock = false;
            Thumb = "";
        }
    }
}
