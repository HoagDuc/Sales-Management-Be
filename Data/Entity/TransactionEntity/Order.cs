
using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class Order : BaseEntity
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public string Code { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public short PaymentMethod { get; set; }

    public short? Discount { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Vat { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? AmountRemaining { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public short? DeliveryMethod { get; set; }

    public short? Status { get; set; }
    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [JsonIgnore]
    public virtual ICollection<RefundOrder> RefundOrders { get; set; } = new List<RefundOrder>();
    
    public decimal? TotalNhap { get; set; }
}
