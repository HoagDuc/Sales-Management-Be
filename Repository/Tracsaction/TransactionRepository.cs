using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class TransactionRepository : GenericRepository<Transaction, Guid>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByCode(string code, Guid? id)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.TransactionId != id)
            && EF.Functions.ILike(x.Code, code));
    }

    public async Task<List<Transaction>> GetAllFromDate(DateTime ngayBatDau, DateTime ngayKetThuc)
    {
        return await DbSet
            .Where(x => x.Date >= ngayBatDau && x.Date <= ngayKetThuc)
            .ToListAsync();
    }
}