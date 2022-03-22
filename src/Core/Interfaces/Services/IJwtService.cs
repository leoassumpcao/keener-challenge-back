using API.Core.Entities;

namespace API.Core.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser applicationUser, IList<string> roles);
    }
}
