using ptdn_net.Data.Dto.Location;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Services.interfaces;

public interface ISubDistrictService
{
    Task<List<SubDistrictResp>> GetByDistrictCode(string districtCode);

    Task ValidNotExistsBySubDistrictId(long? id);
}