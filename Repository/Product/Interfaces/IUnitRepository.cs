using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface IUnitRepository : IGenericRepository<Unit, long>
{
    Task<bool> ExistsById(long? id);
}