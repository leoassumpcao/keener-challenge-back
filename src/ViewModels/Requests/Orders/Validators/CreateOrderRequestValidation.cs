using FluentValidation;

namespace API.ViewModels.Requests.Orders.Validators
{
    public class CreateOrderRequestValidation : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidation()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.OrderedProducts)
               .NotEmpty()
               .Must(op => HasOrderedProducts(op));

            RuleFor(x => x.Total)
                .NotEmpty()
                .GreaterThan(0);
        }

        private bool HasOrderedProducts(IEnumerable<OrderedProductViewModel> orderedProducts)
        {
            return orderedProducts.Any();
        }
    }
}