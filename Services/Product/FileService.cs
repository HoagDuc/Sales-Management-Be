using BT_MVC_Web.Exceptions;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils;
using File = ptdn_net.Data.Entity.ProductEntity.File;

namespace ptdn_net.Services;

public class FileService : BaseService, IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public FileService(IUnitOfWork unitOfWork,IWebHostEnvironment hostingEnvironment) : base(unitOfWork)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<Guid> CreateBy(IFormFile req)
    {
        var fileInfo = FileUtil.GetFileUploadInfo(req);
        var rootPath = _hostingEnvironment.ContentRootPath;
        var relativeFilePath = Path.Combine("images", fileInfo.UniqueFileName);
        var absoluteFilePath = Path.Combine(rootPath, "wwwroot", relativeFilePath);
        await FileUtil.WriteFile(req, absoluteFilePath);

        var entity = new File
        {
            FileId = Guid.NewGuid(),
            Name = fileInfo.FileName,
            FilePath = relativeFilePath,  // ~/abc + "/" + name
            ContentType = fileInfo.ContentType, 
            ContentSize = fileInfo.ContentSize,
            Extension = fileInfo.Extension

        };
        
        await UnitOfWork.FileRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();

        return entity.FileId;
    }

    public async Task DeleteBy(Guid id)
    {
        var entity = await UnitOfWork.FileRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Image " + id + " not found");

        UnitOfWork.FileRepo.Remove(entity);
    }
}