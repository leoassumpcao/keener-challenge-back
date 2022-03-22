namespace API.ViewModels.Requests.Users
{
    public class UpdateUserRequest
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string Email { get; protected set; }
        public string AddressLine1 { get; protected set; }
        public string AddressLine2 { get; protected set; }
        public string City { get; protected set; }
        public string State { get; protected set; }
        public string Neighborhood { get; protected set; }
        public string ZipCode { get; protected set; }
        public string Country { get; protected set; }
        public string BirthDate { get; protected set; }
        public bool IsActive { get; protected set; }

        public UpdateUserRequest(Guid id, string name, string email, string addressLine1, string addressLine2,
            string city, string state, string neighborhood, string zipCode,
            string country, string birthDate, bool isActive)
        {
            Id = id;
            Name = name;
            Email = email;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            Country = country;
            BirthDate = birthDate;
            IsActive = isActive;
        }
    }
}