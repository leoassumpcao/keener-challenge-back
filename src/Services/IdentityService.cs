using API.Core.Entities;
using API.Core.Enums;
using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using API.Core.ValueObjects;
using API.Mappings;
using API.ViewModels;
using API.ViewModels.Requests.Accounts;
using API.ViewModels.Requests.Users;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class IdentityService : BaseService, IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IJwtService _jwtService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IMapper mapper,
            INotificator notifications,
            IHttpContextAccessor httpContextAccessor,
            IJwtService jwtService) : base(mapper, notifications, httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _jwtService = jwtService;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await _userManager.Users.AsNoTracking().FirstAsync(u => u.Id == userId);
            return user.UserName;
        }

        public async Task<PaginatedList<UserViewModel>> GetPaginatedAsync(GetUsersWithPaginationRequest paginationQuery)
        {
            if (!HasAdministratorPermission())
            {
                _notifications.AddNotification("You are not allowed to GET those Users.");
                return new PaginatedList<UserViewModel>(new List<UserViewModel>(), 0, 1, 0);
            }

            return await _userManager.Users.AsNoTracking()
                .OrderBy(prod => prod.Name)
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(paginationQuery.PageNumber, paginationQuery.PageSize);
        }

        public async Task<UserViewModel?> GetByIdAsync(Guid userId)
        {
            if (!HasOwnershipPermission(userId))
            {
                _notifications.AddNotification("You are not allowed to GET this User.");
                return null;
            }

            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == userId);
            if (user is null)
                return null;

            return _mapper.Map<ApplicationUser, UserViewModel>(user);
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = _userManager.Users.AsNoTracking().SingleOrDefault(u => u.Id == userId);

            return user is not null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<(string, UserViewModel)?> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == loginRequest.Email);
            if (user is null)
            {
                _notifications.AddNotification("Incorrect user or password.");
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, true);
            if (result.IsLockedOut)
            {
                _notifications.AddNotification("This user is temporarily blocked.");
                return null;
            }
            else if (!result.Succeeded)
            {
                _notifications.AddNotification("Incorrect user or password.");
                return null;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return (_jwtService.GenerateToken(user, userRoles),
                    _mapper.Map<ApplicationUser, UserViewModel>(user));
        }

        public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            var user = _userManager.Users.AsNoTracking().SingleOrDefault(u => u.Id == userId);
            if (user is null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<UserViewModel?> CreateUserAsync(ApplicationUser applicationUser, string password, Roles role = Roles.User)
        {
            var result = await _userManager.CreateAsync(applicationUser, password);
            if (!result.Succeeded)
            {
                _notifications.AddNotifications(result.Errors);
                return null;
            }

            if (role != Roles.User && !HasAdministratorPermission())
                role = Roles.User;

            await _userManager.AddToRoleAsync(applicationUser, role.ToString());

            return _mapper.Map<ApplicationUser, UserViewModel>(applicationUser);
        }

        public async Task<UserViewModel?> CreateUserAsync(RegisterRequest request)
        {
            var address = new Address(request.AddressLine1, request.AddressLine2,
                request.City, request.State, request.Neighborhood,
                request.ZipCode, request.Country);

            if (!DateTime.TryParse(request.BirthDate, out DateTime birthDate))
                birthDate = DateTime.MinValue;

            var applicationUser = new ApplicationUser(request.Name, address,
                birthDate, true, DateTime.UtcNow)
            {
                Email = request.Email,
                UserName = request.Email,
                EmailConfirmed = true
            };

            if (Enum.TryParse<Roles>(request.Role, out Roles role) == false)
                role = Roles.User;

            return await CreateUserAsync(applicationUser, request.Password, role);
        }

        public async Task<bool> UpdateUserAsync(UpdateUserRequest request)
        {
            if (!HasOwnershipPermission(request.Id))
            {
                _notifications.AddNotification("You are not allowed to UPDATE this User.");
                return false;
            }

            var address = new Address(request.AddressLine1, request.AddressLine2,
                request.City, request.State, request.Neighborhood,
                request.ZipCode, request.Country);

            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.Id);
            if (existingUser is null)
            {
                _notifications.AddNotification("User doesn't exists.");
                return false;
            }

            if (!DateTime.TryParse(request.BirthDate, out DateTime birthDate))
                birthDate = DateTime.MinValue;

            if (!existingUser.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var setEmailResult = await this._userManager.SetEmailAsync(existingUser, request.Email);
                if (!setEmailResult.Succeeded)
                {
                    _notifications.AddNotifications(setEmailResult.Errors);
                    return false;
                }
            }

            existingUser.Name = request.Name;
            existingUser.Address = address;
            existingUser.BirthDate = birthDate;
            existingUser.IsActive = request.IsActive;

            await _userManager.UpdateAsync(existingUser);

            return true;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user is null || await DeleteUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            if (!HasAdministratorPermission())
            {
                _notifications.AddNotification("You are not allowed to DELETE this User.");
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _notifications.AddNotifications(result.Errors);
                return false;
            }

            return true;
        }
    }
}
