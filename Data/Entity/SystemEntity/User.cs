
using Newtonsoft.Json;
using ptdn_net.Const;

namespace ptdn_net.Data.Entity.SystemEntity;

public partial class User : BaseEntity
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    /// <summary>
    /// Password mã hoá theo Bcrypt
    /// </summary>
    public string Password { get; set; } = null!;

    public string? Fullname { get; set; }

    public DateTime? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    /// <summary>
    /// 0 = Nam. 1 = Nữ. 2 = Etc.
    /// </summary>
    public Gender? Gender { get; set; }

    public string? Avatar { get; set; }

    /// <summary>
    /// t = Active. f = InActive.
    /// </summary>
    public bool IsActive { get; set; }

    public long RoleId { get; set; }

    public virtual Role? Role { get; set; }
}
