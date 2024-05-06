using ptdn_net.Data;
using ptdn_net.Data.Entity;

namespace ptdn_net.Repository.interfaces;

public interface IInventoryRepository : IGenericRepository<Inventory, long>
{
    Task<Inventory?> GetByProductId(long productId);
    
    new Task<List<Inventory>> GetAll();
}