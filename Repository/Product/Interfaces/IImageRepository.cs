using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Repository.interfaces;

public interface IImageRepository : IGenericRepository<Image, long>
{
    Task<List<Image>> GetByProductId(long? id);
}