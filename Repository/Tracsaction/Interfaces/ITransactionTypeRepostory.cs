using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Repository.interfaces;

public interface ITransactionTypeRepository : IGenericRepository<TransactionType, long>
{
    Task<bool> ExistsById(long? id);
}