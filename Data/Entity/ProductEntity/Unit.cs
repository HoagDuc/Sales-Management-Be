
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Unit
{
    public long UnitId { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}