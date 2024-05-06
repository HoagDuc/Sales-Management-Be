using File = ptdn_net.Data.Entity.ProductEntity.File;

namespace ptdn_net.Repository.interfaces;

public interface IFileRepository : IGenericRepository<File, Guid>
{
}