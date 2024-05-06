using ptdn_net.Data.Entity;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Dto;

public class InventoryResp
{
    public InventoryResp(Inventory entity)
    {
        InventoryId = entity.InventoryId;
        ProductId = entity.ProductId;
        MinQuantity = entity.MinQuantity;
        Quantity = entity.Quantity;
        ReceiptDate = entity.ReceiptDate;
        DispatchDate = entity.DispatchDate;
        CapitalPrice = entity.CapitalPrice;
        CapitailPriceVersions = entity.CapitailPriceVersions;
        Product = entity.Product;
    }

    public InventoryResp()
    {
        
    }


    public long InventoryId { get; set; }

    public long ProductId { get; set; }

    public long? MinQuantity { get; set; }

    public long? Quantity { get; set; }

    public DateTime ReceiptDate { get; set; }

    public DateTime? DispatchDate { get; set; }

    public decimal? CapitalPrice { get; set; }

    public ICollection<CapitailPriceVersion> CapitailPriceVersions { get; set; }
    
    public Entity.ProductEntity.Product Product { get; set; }
    
    public decimal? TotalPrice { get; set; }
}