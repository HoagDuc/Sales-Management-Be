using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Repository.interfaces;

public interface IRefundOrderRepository : IGenericRepository<RefundOrder, Guid>
{
    new Task<List<RefundOrder>> GetAll();
    
    new Task<RefundOrder?> GetByIdAsync(Guid id);
    
    Task<bool> ExistsByCode(string code, Guid? id = null);
    
    Task<List<RefundOrder>> GetAllDangVanChuyen();
    
    Task<List<RefundOrder>> GetAllFromDate(DateTime? fromDate, DateTime? toDate);
}