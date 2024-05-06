using ptdn_net.Data;
using ptdn_net.Repository.interfaces;
using File = ptdn_net.Data.Entity.ProductEntity.File;

namespace ptdn_net.Repository;

public class FileRepository : GenericRepository<File, Guid>, IFileRepository
{
    public FileRepository(ApplicationDbContext context) : base(context)
    {
    }
}