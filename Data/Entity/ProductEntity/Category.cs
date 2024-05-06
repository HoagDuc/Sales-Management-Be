
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Category
{
    public long CategoryId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
