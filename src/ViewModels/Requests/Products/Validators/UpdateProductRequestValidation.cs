using FluentValidation;

namespace API.ViewModels.Requests.Products.Validators
{
    public class UpdateProductRequestValidation : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidation()
        {
            RuleFor(x => x.ProductId)
                    .NotEmpty()
                    .NotEqual(Guid.Empty);

            RuleFor(x => x.Name)
                    .NotEmpty();

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.InStock)
                .NotEmpty();
        }
    }
}