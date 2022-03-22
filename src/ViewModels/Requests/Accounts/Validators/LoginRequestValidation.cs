using FluentValidation;

namespace API.ViewModels.Requests.Accounts.Validators
{
    public class LoginRequestValidation : AbstractValidator<LoginRequest>
    {

        public LoginRequestValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}