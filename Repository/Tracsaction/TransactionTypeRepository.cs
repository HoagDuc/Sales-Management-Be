using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class TransactionTypeRepository : GenericRepository<TransactionType, long>, ITransactionTypeRepository
{
    public TransactionTypeRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.TransactionTypeId == id);
    }
}