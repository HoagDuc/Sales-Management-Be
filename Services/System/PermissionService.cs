using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class PermissionService : BaseService, IPermissionService
{
    private readonly IMapper _mapper;

    public PermissionService(IUnitOfWork unitOfWork,
        IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<Permission> GetById(long id)
    {
        var entity = await UnitOfWork.PermissionRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Permission " + id + " not found");
        
        return entity;
    }
    
    public async Task<long> CreateBy(PermissionReq req)
    {
        //await ValidCreateBy(req);
        var entity = new Permission
        {
            Code = req.Code!,
            Name = req.Name!,
            Description = req.Description
        };

        await UnitOfWork.PermissionRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.PermissionId;
    }

    private async Task ValidCreateBy(PermissionReq req)
    {
        throw new NotImplementedException();
    }
}