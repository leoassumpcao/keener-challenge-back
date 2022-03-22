using System.Text.Json.Serialization;

namespace API.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public AddressViewModel Address { get; set; }
        [JsonPropertyName("Phone")]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime MemberSince { get; set; }

        public UserViewModel(Guid id, string name, string email, AddressViewModel address,
            DateTime birthDate, string phone, bool isActive, DateTime memberSince)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            BirthDate = birthDate;
            PhoneNumber = phone;
            IsActive = isActive;
            MemberSince = memberSince;
        }

        public UserViewModel()
        {
            Id = Guid.Empty;
            Name = String.Empty;
            Email = String.Empty;
            Address = new AddressViewModel("", "", "", "", "", "", "");
            BirthDate = DateTime.MinValue;
            PhoneNumber = String.Empty;
            IsActive = false;
            MemberSince = DateTime.MinValue;
        }
    }
}
