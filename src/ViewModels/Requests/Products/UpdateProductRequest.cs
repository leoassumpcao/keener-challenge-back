using API.ViewModels.Requests.Products.Validators;

namespace API.ViewModels.Requests.Products
{
    public class UpdateProductRequest : Request
    {
        public Guid ProductId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public decimal Price { get; protected set; }
        public uint InStock { get; protected set; }
        public Guid? CategoryId { get; protected set; }

        public UpdateProductRequest(Guid productId, string name, string description, decimal price, uint inStock, Guid? categoryId)
        {
            ProductId = productId;
            Name = name;
            Description = description;
            Price = price;
            InStock = inStock;
            CategoryId = categoryId;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateProductRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}