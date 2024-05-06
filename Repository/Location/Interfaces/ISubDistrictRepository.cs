using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Repository.interfaces;

public interface ISubDistrictRepository : IGenericRepository<Subdistrict, int>
{
    Task<List<Subdistrict>> GetByDistrictCode(string districtCode);
    
    Task<bool> ExistsById(long? id);
}