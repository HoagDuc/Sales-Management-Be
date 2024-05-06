using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Services.System.Interfaces;

public interface IRoleService
{
    Task<Role> GetById(long id);
    
    Task<RoleResp> GetByIdAsync(long id);
    
    Task<List<RoleResp>> GetAll();
    
    Task<long> CreateBy(RoleReq req);
    
    Task<long> UpdateBy(long roleId, RoleReq req);
}