
using System.Text.Json.Serialization;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Vendor
{
    public long VendorId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal? Debt { get; set; }

    public string? Website { get; set; }

    public string Phone { get; set; } = null!;

    public decimal? Tax { get; set; }

    public decimal? Fax { get; set; }

    public long? ProvinceId { get; set; }

    public long? SubDistrictId { get; set; }

    public long? DistrictId { get; set; }

    public string? Address { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [JsonIgnore]
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}
