namespace ptdn_net.Data.Dto.Auth;

public class RoleReq
{
    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<long>? PermissionIds { get; set; } = new List<long>();
}