using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Data.Dto.Location;

public class SubDistrictResp
{
    public int SubDistrictId { get; set; }

    public string Code { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string ProvinceCode { get; set; }

    public string DistrictCode { get; set; }

    public bool IsActive { get; set; }
}