using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Repository.interfaces;

public interface IDistrictRepository : IGenericRepository<District, int>
{
    Task<List<District>> GetByProvinceCode(string provinceCode);
    
    Task<bool> ExistsById(long? id);
}