using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.LocationEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class SubDistrictRepository : GenericRepository<Subdistrict, int>, ISubDistrictRepository
{
    public SubDistrictRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Subdistrict>> GetByDistrictCode(string districtCode)
    {
        return await DbSet
            .Where(s => s.DistrictCode == districtCode)
            .ToListAsync();
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.SubDistrictId == id);
    }
}