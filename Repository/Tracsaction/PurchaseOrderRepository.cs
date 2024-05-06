using Microsoft.EntityFrameworkCore;
using ptdn_net.Const.Emun;
using ptdn_net.Data;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class PurchaseOrderRepository : GenericRepository<PurchaseOrder, Guid>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public new async Task<List<PurchaseOrder>> GetAll()
    {
        return await DbSet
            .Include(x => x.Vendor)
            .Include(x => x.PurchaseOrderDetails)
            .ThenInclude(x=>x.Product)
            .ToListAsync();
    }

    public new async Task<PurchaseOrder?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .Include(x => x.Vendor)
            .Include(x => x.PurchaseOrderDetails)
            .ThenInclude(x=>x.Product)
            .ThenInclude(x => x.Unit)
            .FirstOrDefaultAsync(x => x.PurchaseOrderId == id);
    }
    
    public async Task<bool> ExistsByCode(string code, Guid? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.PurchaseOrderId != id)
            && EF.Functions.ILike(x.Code!, code));
    }

    public async Task<List<PurchaseOrder>> GetAllDangVanChuyen()
    {
        return await DbSet
            .Where(x => x.Status == (short)Status.DangVanChuyen)
            .Include(x => x.Vendor)
            .Include(x => x.PurchaseOrderDetails)
            .ThenInclude(x=>x.Product)
            .ToListAsync();
    }

    public async Task<List<PurchaseOrder>> GetAllFromDate(DateTime? ngayBatDau, DateTime? ngayKetThuc)
    {
        var query =  DbSet
            .Include(x => x.Vendor)
            .Include(x => x.PurchaseOrderDetails)
            .ThenInclude(x=>x.Product);
        return ngayBatDau is null && ngayKetThuc is null
            ? await query.ToListAsync()
            : await query.Where(x => x.CreatedAt >= ngayBatDau && x.CreatedAt <= ngayKetThuc).ToListAsync();
    }
}