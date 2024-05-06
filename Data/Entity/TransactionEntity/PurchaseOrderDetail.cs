
using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class PurchaseOrderDetail
{
    public Guid PurchaseOrderId { get; set; }

    public long ProductId { get; set; }

    public short? Quantity { get; set; }

    public short? Discount { get; set; }

    public decimal? Price { get; set; }

    public virtual Product Product { get; set; } = null!;

    [JsonIgnore]
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
}
