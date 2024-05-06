
namespace ptdn_net.Data.Entity.LocationEntity;

public partial class District
{
    public int DistrictId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string ProvinceCode { get; set; } = null!;

    public bool IsActive { get; set; }
}
