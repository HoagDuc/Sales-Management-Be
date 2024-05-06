using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class ImageService : BaseService, IImageService
{
    private readonly IFileService _fileService;

    public ImageService(IUnitOfWork unitOfWork,
        IFileService fileService) : base(unitOfWork)
    {
        _fileService = fileService;
    }

    public async Task<List<ImageResp>> GetByProductId(long id)
    {
        var data = await UnitOfWork.ImageRepo.GetByProductId(id);

        return data.Select(x => new ImageResp(x)).ToList();
    }

    public async Task<long> CreateBy(long productId, IFormFile req)
    {
        var fileId = await _fileService.CreateBy(req);

        var entity = new Image();
        SetValue(entity, productId, fileId);

        await UnitOfWork.ImageRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.ImageId;
    }

    private static void SetValue(Image entity, long productId, Guid fileId)
    {
        entity.ProductId = productId;
        entity.FileId = fileId;
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.ImageRepo.GetByProductId(id);
        if (entity is null) throw new NotFoundException("Image " + id + " not found");
        UnitOfWork.ImageRepo.RemoveRange(entity);

        foreach (var item in entity)
        {
            await _fileService.DeleteBy(item.FileId);
        }

        await UnitOfWork.CompleteAsync();
    }
}