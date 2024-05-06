using ptdn_net.Data.Dto.Location;

namespace ptdn_net.Services.interfaces;

public interface IProvinceService 
{
    Task<List<ProvinceResp>> GetAll();

    Task ValidNotExistsByProvinceId(long? id);
}