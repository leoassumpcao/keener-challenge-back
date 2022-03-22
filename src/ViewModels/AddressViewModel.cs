namespace API.ViewModels
{
    public class AddressViewModel
    {
        public string AddressLine1 { get; protected set; }
        public string AddressLine2 { get; protected set; }
        public string City { get; protected set; }
        public string State { get; protected set; }
        public string Neighborhood { get; protected set; }
        public string ZipCode { get; protected set; }
        public string Country { get; protected set; }

        public AddressViewModel(string addressLine1, string addressLine2,
            string city, string state, string neighborhood, string zipCode, string country)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            Country = country;
        }
    }
}
