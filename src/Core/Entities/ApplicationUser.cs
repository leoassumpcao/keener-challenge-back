using API.Core.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace API.Core.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime MemberSince { get; set; }
        public virtual IEnumerable<Order> Orders { get; protected set; } = null!;

        public ApplicationUser(string name, Address address, DateTime birthDate, bool isActive, DateTime memberSince) : base()
        {
            Name = name;
            Address = address;
            BirthDate = birthDate;
            IsActive = isActive;
            MemberSince = memberSince;
        }

        protected ApplicationUser() : base()
        {
            Name = String.Empty;
            Address = new Address("", "", "", "", "", "", "");
            BirthDate = DateTime.MinValue;
            IsActive = false;
            MemberSince = DateTime.MinValue;
        }
    }
}
