using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IBrandService
{
    Task<List<BrandResp>> GetAll();

    Task<BrandResp> GetById(long id);
    
    Task<long> CreateBy(BrandReq req);
    
    Task<long> UpdateBy(BrandReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByBrandId(long? id);
}