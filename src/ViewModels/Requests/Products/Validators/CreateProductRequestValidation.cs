using FluentValidation;

namespace API.ViewModels.Requests.Products.Validators
{
    public class CreateProductRequestValidation : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidation()
        {
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