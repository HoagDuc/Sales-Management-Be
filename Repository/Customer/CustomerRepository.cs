using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.CustomerEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class CustomerRepository : GenericRepository<Customer, Guid>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<bool> ExistsByCode(string code, Guid? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.CustomerId != id)
            && EF.Functions.ILike(x.Code, code));
    }

    public new async Task<List<Customer>> GetAll()
    {
        return await DbSet
            .Include(x => x.CustomerGroup)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(Guid? id)
    {
        return await DbSet
            .Include(x => x.CustomerGroup)
            .FirstOrDefaultAsync(x => x.CustomerId == id);
    }

    public async Task<bool> ExistsById(Guid? id)
    {
        return await DbSet.AnyAsync(x => x.CustomerId == id);
    }
}