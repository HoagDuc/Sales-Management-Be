using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Repository.interfaces;

public interface IOrderRepository : IGenericRepository<Order, Guid>
{
    new Task<List<Order>> GetAll();
    
    Task<List<Order>> GetAllFromDate(DateTime? fromDate, DateTime? toDate);
    
    Task<List<Order>> GetAllFromDatev2(DateTime? fromDate, DateTime? toDate);
    
    new Task<Order?> GetByIdAsync(Guid id);
    
    Task<bool> ExistsByCode(string code, Guid? id = null);
    
    Task<bool> ExistsById(Guid? id);
    
    Task<List<Order>> GetAllDangVanChuyen();
    
    Task<List<Order>> GetAllOrderDone();
    
    Task<List<Order>> GetByCustomerId(Guid customerId);
}