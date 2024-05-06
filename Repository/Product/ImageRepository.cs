using Microsoft.EntityFrameworkCore;
using ptdn_net.Data;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Repository;

public class ImageRepository : GenericRepository<Image, long>, IImageRepository
{
    public ImageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Image>> GetByProductId(long? id)
    {
        return await DbSet.Where(x => x.ProductId == id).ToListAsync();
    }
}