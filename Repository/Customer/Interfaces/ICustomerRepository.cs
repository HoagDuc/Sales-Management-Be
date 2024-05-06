using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Repository.interfaces;

public interface ICustomerRepository : IGenericRepository<Customer, Guid>
{
    new Task<List<Customer>> GetAll();
    
    new Task<Customer?> GetByIdAsync(Guid? id);
    
    Task<bool> ExistsById(Guid? id);

    Task<bool> ExistsByCode(string code, Guid? id = null);
}