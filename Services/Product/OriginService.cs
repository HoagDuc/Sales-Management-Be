using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class OriginService : BaseService, IOriginService
{
    private readonly IMapper _mapper;

    public OriginService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<OriginResp>> GetAll()
    {
        var data = await UnitOfWork.OriginRepo.GetAll();
        return _mapper.Map<List<OriginResp>>(data);
    }

    public async Task<OriginResp> GetById(long id)
    {
        var entity = await UnitOfWork.OriginRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Origin " + id + " not found");

        return _mapper.Map<OriginResp>(entity);
    }

    public async Task<long> CreateBy(OriginReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<Origin>(req);

        await UnitOfWork.OriginRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.OriginId;
    }

    public async Task<long> UpdateBy(OriginReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.OriginRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.OriginRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.OriginId;
    }

    private async Task ValidCreateBy(OriginReq req)
    {
        await ValidExistsByCode(req);
    }

    private async Task ValidUpdateBy(long id, OriginReq req)
    {
        await ValidExistsByCode(req, id);
    }

    private async Task ValidExistsByCode(OriginReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.OriginRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.OriginRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Origin " + id + " not found");
        UnitOfWork.OriginRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByOriginId(long? id)
    {
        var existsBy = await UnitOfWork.OriginRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by origin id: " + id);
        }
    }
}