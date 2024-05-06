using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.Dto.Location;

public class DistrictResp
{
    public int DistrictId { get; set; }

    public string Code { get; set; }

    public string? Name { get; set; }

    public string? ProvinceCode { get; set; }

    public bool IsActive { get; set; }
}