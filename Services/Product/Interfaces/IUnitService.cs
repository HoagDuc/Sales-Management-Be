using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IUnitService 
{
    Task<List<UnitResp>> GetAll();

    Task<UnitResp> GetById(long id);
    
    Task<long> CreateBy(UnitReq req);
    
    Task<long> UpdateBy(UnitReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByUnitId(long? id);
}