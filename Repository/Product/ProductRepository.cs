using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class ProductRepository : GenericRepository<Product, long>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByCode(string code, long? id)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.ProductId != id)
            && EF.Functions.ILike(x.Code, code));
    }

    public new async Task<List<Product>> GetAll()
    {
        return await DbSet
            .Include(x => x.Inventories)
            .ThenInclude(x => x.CapitailPriceVersions)
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.Origin)
            .Include(x => x.Unit)
            .Include(x => x.Vendor)
            .Include(p => p.Images)
            .ThenInclude(x => x.File)
            .ToListAsync();
    }

    public new async Task<Product?> GetByIdAsync(long id)
    {
        return await DbSet
            .Where(x => x.ProductId == id)
            .Include(x => x.Inventories)
            .ThenInclude(x => x.CapitailPriceVersions)
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.Origin)
            .Include(x => x.Unit)
            .Include(x => x.Vendor)
            .Include(p => p.Images)
            .ThenInclude(x => x.File)
            .FirstOrDefaultAsync();
    }
}