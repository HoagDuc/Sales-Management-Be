using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.Dto.Location;

public class ProvinceResp
{
    public int ProvinceId { get; set; }

    public string Code { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }
}