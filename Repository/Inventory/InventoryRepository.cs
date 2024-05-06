using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class InventoryRepository : GenericRepository<Inventory, long>, IInventoryRepository
{
    public InventoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Inventory?> GetByProductId(long productId)
    {
        return await DbSet.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
    }

    public async Task<List<Inventory>> GetAll()
    {
        return await DbSet.Include(x => x.Product)
            .ThenInclude(x => x.Unit)
            .Include(x => x.CapitailPriceVersions)
            .ToListAsync();
    }
}