using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public class CapitailPriceVersion
{
    public int CapitalPriceId { get; set; }

    public long? Quantity { get; set; }

    public decimal? CapitalPrice { get; set; }

    public long? InventoryId { get; set; }

    public DateTime? ReceiptDate { get; set; }

    [JsonIgnore]
    public virtual Inventory? Inventory { get; set; }
}