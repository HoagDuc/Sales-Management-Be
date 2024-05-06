
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Brand
{
    public long BrandId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
