using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.LocationEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class DistrictRepository : GenericRepository<District, int>, IDistrictRepository
{
    public DistrictRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<District>> GetByProvinceCode(string provinceCode)
    {
        return await DbSet
            .Where(x => x.ProvinceCode == provinceCode)
            .ToListAsync();
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.DistrictId == id);
    }
}