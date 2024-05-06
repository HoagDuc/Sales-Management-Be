using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IOriginService
{
    Task<List<OriginResp>> GetAll();

    Task<OriginResp> GetById(long id);
    
    Task<long> CreateBy(OriginReq req);
    
    Task<long> UpdateBy(OriginReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByOriginId(long? id);
}