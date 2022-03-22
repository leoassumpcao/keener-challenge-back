using FluentValidation;

namespace API.ViewModels.Requests.Accounts.Validators
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .Must(bd => IsBirthDateValid(bd));

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.AddressLine1)
                .NotEmpty();

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.State)
                .NotEmpty();

            RuleFor(x => x.Neighborhood)
                .NotEmpty();

            RuleFor(x => x.ZipCode)
                .NotEmpty();

            RuleFor(x => x.Country)
                .NotEmpty();


        }

        private bool IsBirthDateValid(string birthDate)
        {
            return DateTime.TryParse(birthDate, out _);
        }
    }
}