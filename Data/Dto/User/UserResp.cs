using ptdn_net.Const;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Data.Dto.User;

public class UserResp
{
    public long UserId { get; set; }
    
    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Fullname { get; set; }

    public DateTime? Dob { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public Gender? Gender { get; set; }

    public string? Avatar { get; set; }

    public bool IsActive { get; set; }

    public long? RoleId { get; set; }
    
    public Role? Role { get; set; }

}