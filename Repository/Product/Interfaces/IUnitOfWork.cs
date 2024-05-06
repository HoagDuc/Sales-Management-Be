using ptdn_net.Repository.Interfaces;

namespace ptdn_net.Repository.interfaces;

public interface IUnitOfWork
{
    ICategoryRepository CategoryRepo { get; set; }

    IImageRepository ImageRepo { get; set; }

    IUserRepository UserRepo { get; set; }

    IBrandRepository BrandRepo { get; set; }

    IUnitRepository UnitRepo { get; set; }

    IOriginRepository OriginRepo { get; set; }

    IOrderRepository OrderRepo { get; set; }

    IVendorRepository VendorRepo { get; set; }

    ICustomerGroupRepository CustomerGroupRepo { get; set; }

    ICustomerRepository CustomerRepo { get; set; }

    IProductRepository ProductRepo { get; set; }

    ICapitalPriceVersionRepository CapitalPriceVersionRepo { get; set; }

    IRefundOrderRepository RefundOrderRepo { get; set; }

    IFileRepository FileRepo { get; set; }

    IInventoryRepository InventoryRepo { get; set; }

    ITransactionTypeRepository TransactionTypeRepo { get; set; }

    ITransactionRepository TransactionRepo { get; set; }

    IPurchaseOrderRepository PurchaseOrderRepo { get; set; }

    IProvinceRepository ProvinceRepo { get; set; }

    IDistrictRepository DistrictRepo { get; set; }

    ISubDistrictRepository SubDistrictRepo { get; set; }

    IPermissionRepository PermissionRepo { get; set; }

    IRoleRepository RoleRepo { get; set; }

    IGenericRepository<T, TK> GetRepository<T, TK>() where T : class where TK : IComparable<TK>;

    Task CompleteAsync();

    Task Revert();
}