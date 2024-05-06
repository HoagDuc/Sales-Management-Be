namespace ptdn_net.Services.interfaces;

public interface IRolePermissionService
{
    Task CreateOrUpdateBy(long roleId, ICollection<long> permissionId);
}