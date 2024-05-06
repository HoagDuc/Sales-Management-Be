using ptdn_net.Data;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class PermissionRepository : GenericRepository<Permission, long>, IPermissionRepository
{
    public PermissionRepository(ApplicationDbContext context) : base(context)
    {
    }
}