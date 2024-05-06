using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IImageService
{
    Task<List<ImageResp>> GetByProductId(long id);
    
    Task<long> CreateBy(long productId,IFormFile req);
    
    Task DeleteBy(long id);
}