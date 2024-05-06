using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class UnitService : BaseService, IUnitService
{
    private readonly IMapper _mapper;
    
    public UnitService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<UnitResp>> GetAll()
    {
        var data = await UnitOfWork.UnitRepo.GetAll();
        return _mapper.Map<List<UnitResp>>(data).ToList();
    }

    public async Task<UnitResp> GetById(long id)
    {
        var entity = await UnitOfWork.UnitRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Unit " + id + " not found");
        
        return _mapper.Map<UnitResp>(entity);
    }

    public async Task<long> CreateBy(UnitReq req)
    {
        var entity = _mapper.Map<Unit>(req);

        await UnitOfWork.UnitRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.UnitId;
    }

    public async Task<long> UpdateBy(UnitReq req, long id)
    {
        var entity = await UnitOfWork.UnitRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.UnitRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.UnitId;
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.UnitRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Unit " + id + " not found");
        UnitOfWork.UnitRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByUnitId(long? id)
    {
        var existsBy = await UnitOfWork.UnitRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by Unit id: " + id);
        }
    }
}