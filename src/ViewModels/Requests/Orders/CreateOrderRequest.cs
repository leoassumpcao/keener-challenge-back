using API.ViewModels.Requests.Orders.Validators;

namespace API.ViewModels.Requests.Orders
{
    public class CreateOrderRequest : Request
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public IEnumerable<OrderedProductViewModel> OrderedProducts { get; set; } = Enumerable.Empty<OrderedProductViewModel>();
        public decimal Total { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateOrderRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}