
using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Entity;

public partial class Inventory
{
    public long InventoryId { get; set; }

    public long ProductId { get; set; }

    public long? MinQuantity { get; set; }

    public long Quantity { get; set; }

    public DateTime ReceiptDate { get; set; }

    public DateTime? DispatchDate { get; set; }

    public decimal? CapitalPrice { get; set; }

    public virtual ICollection<CapitailPriceVersion> CapitailPriceVersions { get; set; } = new List<CapitailPriceVersion>();

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}
