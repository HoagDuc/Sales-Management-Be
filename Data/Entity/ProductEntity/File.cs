using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.ProductEntity;

public class File
{
    public Guid FileId { get; set; }

    public string? Name { get; set; }

    public string? ContentType { get; set; }

    public int? ContentSize { get; set; }

    public string? Extension { get; set; }

    public string? FilePath { get; set; }

    [JsonIgnore]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}