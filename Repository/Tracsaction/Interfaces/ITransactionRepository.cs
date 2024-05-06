using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Repository.interfaces;

public interface ITransactionRepository : IGenericRepository<Transaction, Guid>
{
    Task<bool> ExistsByCode(string code, Guid? id);
    
    Task<List<Transaction>> GetAllFromDate(DateTime ngayBatDau, DateTime ngayKetThuc);
}