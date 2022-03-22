using API.Core.Interfaces.Notifications;
using API.Core.Interfaces.Services;
using AutoMapper;
using System.Security.Claims;

namespace API.Services
{
    public abstract class BaseService : IBaseService
    {
        protected readonly IMapper _mapper;
        protected readonly INotificator _notifications;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(
            IMapper mapper,
            INotificator notifications,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _notifications = notifications;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasAdministratorPermission()
        {
            if (_httpContextAccessor.HttpContext is null)
                return false;

            return _httpContextAccessor.HttpContext.User.Claims.Any(c =>
                c.Type == ClaimTypes.Role &&
                c.Value.Equals("administrator", StringComparison.OrdinalIgnoreCase));
        }

        public bool HasOwnershipPermission(Guid userId)
        {
            if (_httpContextAccessor.HttpContext is null)
                return false;

            bool isAdministrator = _httpContextAccessor.HttpContext.User.Claims.Any(c =>
                c.Type == ClaimTypes.Role &&
                c.Value.Equals("administrator", StringComparison.OrdinalIgnoreCase));

            if (isAdministrator)
                return true;

            var claimUserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimUserId is not null)
            {
                return claimUserId.Value.Equals(userId.ToString(), StringComparison.Ordinal);
            }

            return false;
        }

        public Guid GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext is null)
                return Guid.Empty;

            var claimUserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claimUserId is null ||
                !Guid.TryParse(claimUserId.Value, out Guid result))
            {
                return Guid.Empty;
            }

            return result;
        }
    }
}
