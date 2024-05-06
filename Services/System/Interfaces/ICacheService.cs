using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Services.System.Interfaces;

public interface ICacheService
{
    Task<User> GetUser(string username);
}