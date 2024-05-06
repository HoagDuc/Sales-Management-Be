using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class OrderDetail
{
    public long ProductId { get; set; }

    public Guid OrderId { get; set; }

    public short? Quantity { get; set; }

    public decimal? Price { get; set; }

    public short? Discount { get; set; }

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}