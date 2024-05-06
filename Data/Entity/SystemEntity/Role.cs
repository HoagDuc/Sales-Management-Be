
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.SystemEntity;

public partial class Role
{
    public long RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [JsonIgnore]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    
    [JsonIgnore]
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    
    [JsonIgnore]
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
