using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class CapitalPriceVersionRepository : GenericRepository<CapitailPriceVersion, int>, ICapitalPriceVersionRepository
{
    public CapitalPriceVersionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<CapitailPriceVersion>> GetByInventoryId(long inventoryId)
    {
        return await DbSet.Where(x => x.InventoryId == inventoryId).ToListAsync();
    }
}