using Microsoft.EntityFrameworkCore;
using ptdn_net.Const.Emun;
using ptdn_net.Data;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class OrderRepository : GenericRepository<Order, Guid>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Order>> GetAll()
    {
        return await DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }
    
    public async Task<List<Order>> GetAllFromDate(DateTime? fromDate, DateTime? toDate)
    {
        var query = DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product);
        if (fromDate is null && toDate is null)
            return await query.ToListAsync();
        return await query
            .Where(x => toDate != null && fromDate != null && x.CreatedAt.Date >= fromDate.Value.Date && x.CreatedAt.Date <= toDate.Value.Date)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllFromDatev2(DateTime? fromDate, DateTime? toDate)
    {
        var query = DbSet
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product);
        if (fromDate is null && toDate is null)
            return await query.ToListAsync();
        return await query
            .Where(x => toDate != null && fromDate != null && x.CreatedAt.Date >= fromDate.Value.Date && x.CreatedAt.Date <= toDate.Value.Date)
            .ToListAsync();
    }

    public new async Task<Order?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)    
            .ThenInclude(x=>x.Unit)
            .FirstOrDefaultAsync(x => x.OrderId == id)!;
    }

    public async Task<bool> ExistsByCode(string code, Guid? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.OrderId != id)
            && EF.Functions.ILike(x.Code!, code));
    }

    public async Task<bool> ExistsById(Guid? id)
    {
        return await DbSet.AnyAsync(x => x.OrderId == id);
    }

    public async Task<List<Order>> GetAllDangVanChuyen()
    {
        return await DbSet
            .Where(x => x.Status == (short)Status.DangVanChuyen)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<List<Order>> GetAllOrderDone()
    {
        return await DbSet
            .Where(x => x.Status == (short)Status.HoanThanh)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }

    public async Task<List<Order>> GetByCustomerId(Guid customerId)
    {
        return await DbSet
            .Where(x => x.CustomerId == customerId)
            .Include(x => x.Customer)
            .Include(x => x.OrderDetails)
            .ThenInclude(x => x.Product)
            .ToListAsync();
    }
}