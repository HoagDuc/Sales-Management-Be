using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Repository.interfaces;

public interface ICustomerGroupRepository : IGenericRepository<CustomerGroup, long>
{
    Task<bool> ExistsById(long? id);
    
    Task<bool> ExistsByCode(string code, long? id);
}