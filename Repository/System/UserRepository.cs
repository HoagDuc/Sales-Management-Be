using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.Interfaces;

namespace ptdn_net.Repository;

public class UserRepository : GenericRepository<User, long>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<User>> GetAllUser()
    {
        return await DbSet.Include(x => x.Role).ToListAsync();
    }

    public async Task<bool> ExistsByUserName(string name, long? id = null)
    {
        return await DbSet.AnyAsync(x =>
            (id == null || x.UserId != id)
            && EF.Functions.ILike(x.Username, name));
    }
    
    public async Task<User> GetByUsernameAsync(string username)
    {
        return await DbSet.Where(x => x.Username == username)
            .Include(x => x.Role)
            .ThenInclude(y => y.Permissions)
            .FirstOrDefaultAsync();
    }
}