using API.Core.Entities;
using API.Core.Enums;
using API.ViewModels;
using API.ViewModels.Requests.Accounts;
using API.ViewModels.Requests.Users;

namespace API.Core.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(Guid userId);
        Task<PaginatedList<UserViewModel>> GetPaginatedAsync(GetUsersWithPaginationRequest paginationQuery);
        Task<UserViewModel?> GetByIdAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<(string, UserViewModel)?> AuthenticateAsync(LoginRequest loginRequest);
        Task<bool> AuthorizeAsync(Guid userId, string policyName);
        Task<UserViewModel?> CreateUserAsync(RegisterRequest createUserRequest);
        Task<UserViewModel?> CreateUserAsync(ApplicationUser applicationUser, string password, Roles role);
        Task<bool> UpdateUserAsync(UpdateUserRequest updateProductRequest);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}
