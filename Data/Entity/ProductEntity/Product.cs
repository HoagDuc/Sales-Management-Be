using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Product
{
    public long ProductId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ShortDescription { get; set; }

    public long? OriginId { get; set; }

    public long CategoryId { get; set; }

    public long? BrandId { get; set; }

    public long? VendorId { get; set; }

    public string? Description { get; set; }

    public float? Volume { get; set; }

    public long UnitId { get; set; }

    public short? Discount { get; set; }

    public decimal? GiaNiemYet { get; set; }

    public decimal? ChietKhauNhap { get; set; }

    public decimal? CostPrice { get; set; }

    public decimal? Vat { get; set; }

    public string Barcode { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [JsonIgnore] 
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Origin? Origin { get; set; }

    [JsonIgnore]
    public virtual ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = new List<PurchaseOrderDetail>();

    [JsonIgnore]
    public virtual ICollection<RefundOrderDetail> RefundOrderDetails { get; set; } = new List<RefundOrderDetail>();

    public virtual Unit Unit { get; set; } = null!;

    public virtual Vendor? Vendor { get; set; }
}