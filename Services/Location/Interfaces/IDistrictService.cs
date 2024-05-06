using ptdn_net.Data.Dto.Location;

namespace ptdn_net.Services.interfaces;

public interface IDistrictService
{
    Task<List<DistrictResp>> GetByProvinceCode(string provinceCode);

    Task ValidNotExistsByDistrictId(long? id);
}