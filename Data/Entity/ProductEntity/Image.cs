
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public partial class Image
{
    public long ImageId { get; set; }

    public long ProductId { get; set; }

    public Guid FileId { get; set; }

    public virtual File File { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}
