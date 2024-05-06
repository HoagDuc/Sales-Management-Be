using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Dto.Product;

public class FileResp
{
    public Guid FileId { get; set; }

    public string? Name { get; set; }

    public string? ContentType { get; set; }

    public int? ContentSize { get; set; }

    public string? Extension { get; set; }

    public string? FilePath { get; set; }

    public virtual ICollection<Image> Images { get; set; }
}