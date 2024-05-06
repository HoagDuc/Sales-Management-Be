using ptdn_net.Data;
using ptdn_net.Repository.interfaces;
using ptdn_net.Repository.Interfaces;

namespace ptdn_net.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly Dictionary<Type, object> _repositories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        CategoryRepo = new CategoryRepository(_context);
        UserRepo = new UserRepository(_context);
        ImageRepo = new ImageRepository(_context);
        ProvinceRepo = new ProvinceRepository(_context);
        DistrictRepo = new DistrictRepository(_context);
        SubDistrictRepo = new SubDistrictRepository(_context);
        OrderRepo = new OrderRepository(_context);
        BrandRepo = new BrandRepository(_context);
        VendorRepo = new VendorRepository(_context);
        RefundOrderRepo = new RefundOrderRepository(_context);
        CapitalPriceVersionRepo = new CapitalPriceVersionRepository(_context);
        InventoryRepo = new InventoryRepository(_context);
        TransactionTypeRepo = new TransactionTypeRepository(_context);
        CustomerGroupRepo = new CustomerGroupRepository(_context);
        TransactionRepo = new TransactionRepository(_context);
        PurchaseOrderRepo = new PurchaseOrderRepository(_context);
        CustomerRepo = new CustomerRepository(_context);
        FileRepo = new FileRepository(_context);
        UnitRepo = new UnitRepository(_context);
        OriginRepo = new OriginRepository(_context);
        ProductRepo = new ProductRepository(_context);
        RoleRepo = new RoleRepository(_context);
        PermissionRepo = new PermissionRepository(_context);
        _repositories = new Dictionary<Type, object>();
    }


    public IGenericRepository<T, TK> GetRepository<T, TK>() where T : class where TK : IComparable<TK>
    {
        if (_repositories.ContainsKey(typeof(T))) return (IGenericRepository<T, TK>)_repositories[typeof(T)];

        var repository = new GenericRepository<T, TK>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public ICategoryRepository CategoryRepo { get; set; }
    public IUserRepository UserRepo { get; set; }
    public IImageRepository ImageRepo { get; set; }
    public IBrandRepository BrandRepo { get; set; }
    public IUnitRepository UnitRepo { get; set; }
    public IOriginRepository OriginRepo { get; set; }
    public IOrderRepository OrderRepo { get; set; }
    public IVendorRepository VendorRepo { get; set; }
    public ICustomerGroupRepository CustomerGroupRepo { get; set; }
    public ICustomerRepository CustomerRepo { get; set; }
    public IProductRepository ProductRepo { get; set; }
    public ICapitalPriceVersionRepository CapitalPriceVersionRepo { get; set; }
    public IRefundOrderRepository RefundOrderRepo { get; set; }
    public IFileRepository FileRepo { get; set; }
    public IInventoryRepository InventoryRepo { get; set; }
    public ITransactionTypeRepository TransactionTypeRepo { get; set; }
    public ITransactionRepository TransactionRepo { get; set; }
    public IPurchaseOrderRepository PurchaseOrderRepo { get; set; }
    public IProvinceRepository ProvinceRepo { get; set; }
    public IDistrictRepository DistrictRepo { get; set; }
    public ISubDistrictRepository SubDistrictRepo { get; set; }
    public IRoleRepository RoleRepo { get; set; }
    public IPermissionRepository PermissionRepo { get; set; }


    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }


    public async Task Revert()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}