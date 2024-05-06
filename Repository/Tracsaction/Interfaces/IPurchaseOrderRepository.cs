using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Repository.interfaces;

public interface IPurchaseOrderRepository : IGenericRepository<PurchaseOrder, Guid>
{
    new Task<List<PurchaseOrder>> GetAll();

    new Task<PurchaseOrder?> GetByIdAsync(Guid id);
    
    Task<bool> ExistsByCode(string code, Guid? id = null);
    
    Task<List<PurchaseOrder>> GetAllDangVanChuyen();
    
    Task<List<PurchaseOrder>> GetAllFromDate(DateTime? ngayBatDau, DateTime? ngayKetThuc);
}