using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface IOriginRepository : IGenericRepository<Origin, long>
{
    Task<bool> ExistsByCode(string code, long? id = null);
    
    Task<bool> ExistsById(long? id);
}