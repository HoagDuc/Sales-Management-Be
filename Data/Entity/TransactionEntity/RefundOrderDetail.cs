using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class RefundOrderDetail
{
    public long ProductId { get; set; }

    /// <summary>
    /// map với bảng refund_order
    /// </summary>
    public Guid RefundOrderId { get; set; }

    public decimal? Price { get; set; }

    public short Quantity { get; set; }

    public string? Note { get; set; }

    public virtual Product Product { get; set; } = null!;

    [JsonIgnore]
    public virtual RefundOrder RefundOrder { get; set; } = null!;
}