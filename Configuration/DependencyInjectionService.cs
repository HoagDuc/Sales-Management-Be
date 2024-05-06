using ptdn_net.Services;
using ptdn_net.Services.BaoCao;
using ptdn_net.Services.BaoCao.Interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Services.Product;
using ptdn_net.Services.System;
using ptdn_net.Services.System.Interfaces;

namespace ptdn_net.Configuration;

public static class DependencyInjectionService
{
    public static IServiceCollection AddDependencyInjectionService(this IServiceCollection services)
    {
        return services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IImageService, ImageService>()
                .AddScoped<IBrandService, BrandService>()
                .AddScoped<IUnitService, UnitService>()
                .AddScoped<IOriginService, OriginService>()
                .AddScoped<IVendorService, VendorService>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IProvinceService, ProvinceService>()
                .AddScoped<IDistrictService, DistrictService>()
                .AddScoped<ISubDistrictService, SubDistrictService>()
                .AddScoped<IRefundOrderService, RefundOrderService>()
                .AddScoped<ICacheService, CacheService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IInventoryService, InventoryService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICustomerGroupService, CustomerGroupService>()
                .AddScoped<ITransactionTypeService, TransactionTypeService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<ICapitailPriceVersionService, CapitailPriceVersionService>()
                .AddScoped<ICustomerService, CustomerService>()
                .AddScoped<IPurchaseOrderService, PurchaseOrderService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<IPermissionService, PermissionService>()
                .AddScoped<IBaoCaoBanHangService, BaoCaoBanHangService>()
                .AddScoped<IBaoCaoNhapHangService, BaoCaoNhapHangService>()
                .AddScoped<IBaoCaoKhoService, BaoCaoKhoService>()
                .AddScoped<IDashboardService, DashboardService>()
            ;
    }
}