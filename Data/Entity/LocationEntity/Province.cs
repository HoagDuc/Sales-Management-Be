
namespace ptdn_net.Data.Entity.LocationEntity;

public partial class Province
{
    public int ProvinceId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    /// <summary>
    /// True: Active. False: Inactive
    /// </summary>
    public bool IsActive { get; set; }
}
