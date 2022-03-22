using API.ViewModels.Requests.Accounts.Validators;

namespace API.ViewModels.Requests.Accounts
{
    public class LoginRequest : Request
    {
        public string Email { get; protected set; }
        public string Password { get; protected set; }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public override bool IsValid()
        {
            ValidationResult = new LoginRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}