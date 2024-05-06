using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class RoleRepository : GenericRepository<Role, long>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role> GetById(long id)
    {
        return await DbSet.Where(x => x.RoleId == id).Include(x => x.Permissions).FirstAsync();
    }
    
    public async Task<bool> ExistsByUserName(string name, long? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.RoleId != id)
            && EF.Functions.ILike(x.Name, name));
    }
}