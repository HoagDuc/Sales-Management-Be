using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Data.Dto.Product;

public class BrandResp
{
    public long BrandId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }
}