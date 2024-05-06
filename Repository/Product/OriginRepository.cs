using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class OriginRepository : GenericRepository<Origin, long>, IOriginRepository
{
    public OriginRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByCode(string code, long? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.OriginId != id)
            && EF.Functions.ILike(x.Code!, code));
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.OriginId == id);
    }
}