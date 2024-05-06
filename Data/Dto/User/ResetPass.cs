namespace ptdn_net.Data.Dto.User;

public class ResetPass
{
    public string? PasswordOld { get; set; }
    
    public string? PasswordNew { get; set; }
    
    public string? PasswordConfirm { get; set; }
}