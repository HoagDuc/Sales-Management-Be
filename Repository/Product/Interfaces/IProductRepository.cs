using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface IProductRepository : IGenericRepository<Product, long>
{
    Task<bool> ExistsByCode(string code, long? id);
    
    new Task<List<Product>> GetAll();
    
    new Task<Product?> GetByIdAsync(long id);
}