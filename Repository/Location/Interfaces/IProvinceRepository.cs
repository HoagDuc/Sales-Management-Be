using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Repository.interfaces;

public interface IProvinceRepository : IGenericRepository<Province, int>
{
    Task<bool> ExistsById(long? id);
}