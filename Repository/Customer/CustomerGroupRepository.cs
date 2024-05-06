using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.CustomerEntity;
using ptdn_net.Repository.interfaces;

namespace ptdn_net.Repository;

public class CustomerGroupRepository :GenericRepository<CustomerGroup, long>, ICustomerGroupRepository
{
    public CustomerGroupRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsById(long? id)
    {
        return await DbSet.AnyAsync(x => x.CustomerGroupId == id);
    }

    public async Task<bool> ExistsByCode(string code, long? id)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.CustomerGroupId != id)
            && EF.Functions.ILike(x.Code, code));
    }
}