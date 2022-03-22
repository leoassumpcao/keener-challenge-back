namespace API.Core.Interfaces.Services
{
    public interface IBaseService
    {
        bool HasAdministratorPermission();
        bool HasOwnershipPermission(Guid userId);
        Guid GetCurrentUserId();
    }
}
