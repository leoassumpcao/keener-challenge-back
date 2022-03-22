using API.ViewModels.Requests.Accounts.Validators;

namespace API.ViewModels.Requests.Accounts
{
    public class RegisterRequest : Request
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string Password { get; protected set; }
        public string AddressLine1 { get; protected set; }
        public string AddressLine2 { get; protected set; }
        public string City { get; protected set; }
        public string State { get; protected set; }
        public string Neighborhood { get; protected set; }
        public string ZipCode { get; protected set; }
        public string Country { get; protected set; }
        public string BirthDate { get; protected set; }
        public string? Role { get; protected set; }

        public RegisterRequest(string name, string email, string password,
            string addressLine1, string addressLine2, string city,
            string state, string neighborhood, string zipCode,
            string country, string birthDate, string? role)
        {
            Name = name;
            Email = email;
            Password = password;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            Country = country;
            BirthDate = birthDate;
            Role = role;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterRequestValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}