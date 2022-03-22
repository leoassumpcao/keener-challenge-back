using API.ViewModels.Requests.Products.Validators;

namespace API.ViewModels.Requests.Products
{
    public class CreateProductRequest : Request
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public uint InStock { get; set; }
        public Guid? CategoryId { get; set; }

        public CreateProductRequest(string name, string description, decimal price, uint inStock, Guid? categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            InStock = inStock;
            CategoryId = categoryId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateProductRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}