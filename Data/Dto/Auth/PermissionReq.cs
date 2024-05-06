using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.Dto.Auth;

public class PermissionReq
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
}