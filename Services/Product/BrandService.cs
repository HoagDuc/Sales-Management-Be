using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class BrandService : BaseService, IBrandService
{
    private readonly IMapper _mapper;

    public BrandService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<BrandResp>> GetAll()
    {
        var entities = await UnitOfWork.BrandRepo.GetAll();
        return _mapper.Map<List<BrandResp>>(entities);
    }

    public async Task<BrandResp> GetById(long id)
    {
        var entity = await UnitOfWork.BrandRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Brand " + id + " not found");
        return _mapper.Map<BrandResp>(entity);
    }

    public async Task<long> CreateBy(BrandReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<Brand>(req);

        await UnitOfWork.BrandRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.BrandId;
    }

    public async Task<long> UpdateBy(BrandReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.BrandRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.BrandRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.BrandId;
    }

    private async Task ValidCreateBy(BrandReq req)
    {
        await ValidExistsByCode(req);
    }

    private async Task ValidUpdateBy(long id, BrandReq req)
    {
        await ValidExistsByCode(req, id);
    }

    private async Task ValidExistsByCode(BrandReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.BrandRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task ValidNotExistsByBrandId(long? id)
    {
        var existsBy = await UnitOfWork.BrandRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by brand id: " + id);
        }
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.BrandRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Brand " + id + " not found");
        UnitOfWork.BrandRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }
}