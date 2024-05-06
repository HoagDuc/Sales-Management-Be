using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Dto.Product;

public class ImageResp
{
    public long ImageId { get; set; }

    public long ProductId { get; set; }

    public string Url { get; set; }

    public ImageResp(Image entity)
    {
        ImageId = entity.ImageId;
        ProductId = entity.ProductId;
    }
}