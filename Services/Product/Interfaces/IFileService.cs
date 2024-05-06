namespace ptdn_net.Services.interfaces;

public interface IFileService
{
    Task<Guid> CreateBy(IFormFile req);
    Task DeleteBy(Guid id);
}