
namespace ptdn_net.Data.Entity.LocationEntity;

public partial class Subdistrict
{
    public int SubDistrictId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    /// <summary>
    /// Map bảng province trường code
    /// </summary>
    public string ProvinceCode { get; set; } = null!;

    /// <summary>
    /// Map bảng district trường code
    /// </summary>
    public string DistrictCode { get; set; } = null!;

    /// <summary>
    /// True: Active. False: Inactive
    /// </summary>
    public bool IsActive { get; set; }
}
