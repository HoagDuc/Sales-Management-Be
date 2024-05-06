using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Repository.interfaces;

public interface IRoleRepository : IGenericRepository<Role, long>
{
    Task<Role> GetById(long id);
    
    Task<bool> ExistsByUserName(string name, long? id = null);
}