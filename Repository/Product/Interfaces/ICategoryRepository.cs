using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface ICategoryRepository : IGenericRepository<Category, long>
{
    Task<bool> ExistsByCode(string code, long? id = null);
    
    Task<bool> ExistsById(long? id);
}