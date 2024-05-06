using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Services.interfaces;

public interface ICapitailPriceVersionService
{
    Task CreateBy(short? quantity, decimal? price, long inventoryId, DateTime? dateTime = null);

    Task<List<CapitailPriceVersion>> GetByInventoryId(long inventoryId);

    Task<CapitailPriceResp> GetTotalPrice(long inventoryId);
    
    Task OutInventory(long entityInventoryId, short? quantity);
}