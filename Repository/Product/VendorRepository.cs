using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class VendorRepository : GenericRepository<Vendor, long>, IVendorRepository
{
    public VendorRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByCode(string code, long? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.VendorId != id)
            && EF.Functions.ILike(x.Code, code));
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.VendorId == id);
    }
}