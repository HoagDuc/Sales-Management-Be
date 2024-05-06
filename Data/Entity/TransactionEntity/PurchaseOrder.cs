using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class PurchaseOrder : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }

    public long? VendorId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string Address { get; set; } = null!;

    public short? PaymentMethod { get; set; }

    public short? Status { get; set; }

    public short? Discount { get; set; }

    public decimal? Vat { get; set; }

    public decimal? Tax { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountRemaining { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? AmountOther { get; set; }

    public string? Note { get; set; }
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    public virtual Vendor? Vendor { get; set; }
}
