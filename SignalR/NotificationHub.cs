using Microsoft.AspNetCore.SignalR;

namespace ptdn_net.SignalR;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("Connection", Context.ConnectionId);
    }
    
    public async Task NotifyAdminAboutLogin(string username)
    {
        await Clients.All.SendAsync("UserLoggedIn", username);
    }
    
    public async Task NotifyProductHetHang(int count)
    {
        await Clients.All.SendAsync("ProductHetHang", count);
    }
}