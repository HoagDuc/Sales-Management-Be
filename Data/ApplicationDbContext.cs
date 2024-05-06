using Microsoft.EntityFrameworkCore;
using ptdn_net.Data.Entity;
using ptdn_net.Data.Entity.CustomerEntity;
using ptdn_net.Data.Entity.LocationEntity;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Utils;
using File = ptdn_net.Data.Entity.ProductEntity.File;

namespace ptdn_net.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerGroup> CustomerGroups { get; set; }
    
    public virtual DbSet<CapitailPriceVersion> CapitailPriceVersions { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Origin> Origins { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public virtual DbSet<RefundOrder> RefundOrders { get; set; }

    public virtual DbSet<RefundOrderDetail> RefundOrderDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subdistrict> Subdistricts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionType> TransactionTypes { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    

    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesAdded();
        SaveChangesModified();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void SaveChangesAdded()
    {
        var now = DateTime.Now;
        var currUsername = SessionUtil.GetCurrUsername();
        ChangeTracker.Entries()
            .Where(entity => entity is { State: EntityState.Added, Entity: IBaseEntity })
            .Select(entry => (IBaseEntity)entry.Entity)
            .ToList()
            .ForEach(entity =>
            {
                entity.CreatedBy = currUsername;
                entity.CreatedAt = now;
            });
    }

    private void SaveChangesModified()
    {
        var now = DateTime.Now;
        var currUsername = SessionUtil.GetCurrUsername();
        ChangeTracker.Entries()
            .Where(entity => entity is { State: EntityState.Modified, Entity: IBaseEntity })
            .Select(entry => (IBaseEntity)entry.Entity)
            .ToList()
            .ForEach(entity =>
            {
                Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                entity.ModifiedBy = currUsername;
                entity.ModifiedAt = now;
            });
    }
}