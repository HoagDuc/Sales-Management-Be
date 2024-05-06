using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.Dto.Auth;

public class RoleResp
{
    public long RoleId { get; set; }
    
    public string? Name { get; set; } 

    public string? Description { get; set; }

    public bool IsActive { get; set; }
    
    public List<long> Permissions { get; set; } = new List<long>();

}