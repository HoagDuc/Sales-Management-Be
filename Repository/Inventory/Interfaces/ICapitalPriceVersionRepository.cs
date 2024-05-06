using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface ICapitalPriceVersionRepository : IGenericRepository<CapitailPriceVersion, int>
{
    Task<List<CapitailPriceVersion>> GetByInventoryId(long inventoryId);
}