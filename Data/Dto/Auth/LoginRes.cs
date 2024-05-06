namespace ptdn_net.Data.Dto.Auth;

public class LoginRes
{
    public LoginRes(Entity.SystemEntity.User user)
    {
        UserId = user.UserId;
        Email = user.Email;
        Username = user.Username;
        Username = user.Username;
        Role = user.Role!.Name;
        Avatar = user.Avatar;
    }

    public long UserId { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }
    
    public string Role { get; set; }
    
    public string? Avatar { get; set; }
}