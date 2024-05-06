using Microsoft.EntityFrameworkCore;
using ptdn_net.Const.Emun;
using ptdn_net.Data;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class RefundOrderRepository : GenericRepository<RefundOrder, Guid>, IRefundOrderRepository
{
    public RefundOrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async new Task<List<RefundOrder>> GetAll()
    {
        return await DbSet
            .Include(x => x.Order)
            .ThenInclude(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Include(x => x.RefundOrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }
    
    public async new Task<RefundOrder?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(x => x.Order)
            .ThenInclude(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Include(x => x.RefundOrderDetails)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.RefundOrderId == id)!;
    }

    public async Task<bool> ExistsByCode(string code, Guid? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.RefundOrderId != id)
            && EF.Functions.ILike(x.Code!, code));
    }

    public async Task<List<RefundOrder>> GetAllDangVanChuyen()
    {
        return await DbSet
            .Where(x => x.Status == (short)Status.DangVanChuyen)
            .Include(x => x.Order)
            .ThenInclude(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Include(x => x.RefundOrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<List<RefundOrder>> GetAllFromDate(DateTime? fromDate, DateTime? toDate)
    {
        var query =  DbSet
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Include(x => x.Order)
            .ThenInclude(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .Include(x => x.Order)
            .ThenInclude(x => x.Customer)
            .Include(x => x.RefundOrderDetails)
            .ThenInclude(x => x.Product);
        return fromDate is null && toDate is null
            ? await query.ToListAsync()
            : await query.Where(x => x.CreatedAt >= fromDate && x.CreatedAt <= toDate).ToListAsync();
    }
}