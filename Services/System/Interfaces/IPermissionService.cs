using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Services.interfaces;

public interface IPermissionService
{
    Task<long> CreateBy(PermissionReq req);

    Task<Permission> GetById(long id);
}