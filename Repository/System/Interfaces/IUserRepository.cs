using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository.Interfaces;

public interface IUserRepository : IGenericRepository<User, long>
{
    Task<User> GetByUsernameAsync(string username);
    
    Task<List<User>> GetAllUser();

    Task<bool> ExistsByUserName(string name, long? id = null);
}