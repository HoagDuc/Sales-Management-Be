using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.LocationEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class ProvinceRepository : GenericRepository<Province, int>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.ProvinceId == id);
    }
}