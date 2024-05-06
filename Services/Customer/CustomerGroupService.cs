using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Data.Entity.CustomerEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class CustomerGroupService : BaseService, ICustomerGroupService
{
    private readonly IMapper _mapper;

    public CustomerGroupService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<CustomerGroupResp>> GetAll()
    {
        var entities = await UnitOfWork.CustomerGroupRepo.GetAll();
        return _mapper.Map<List<CustomerGroupResp>>(entities);
    }

    public async Task<CustomerGroupResp> GetById(long id)
    {
        var entity = await UnitOfWork.CustomerGroupRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Customer group " + id + " not found");

        return _mapper.Map<CustomerGroupResp>(entity);
    }

    public async Task<long> CreateBy(CustomerGroupReq req)
    {
        await ValidCreateBy(req);

        var entity = _mapper.Map<CustomerGroup>(req);

        await UnitOfWork.CustomerGroupRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.CustomerGroupId;
    }

    public async Task<long> UpdateBy(CustomerGroupReq req, long id)
    {
        await ValidUpdateBy(id, req);

        var entity = await UnitOfWork.CustomerGroupRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.CustomerGroupRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.CustomerGroupId;
    }

    private async Task ValidCreateBy(CustomerGroupReq req)
    {
        await ValidExistsByCode(req);
    }

    private async Task ValidUpdateBy(long id, CustomerGroupReq req)
    {
        await ValidExistsByCode(req, id);
    }

    private async Task ValidExistsByCode(CustomerGroupReq req, long? id = null)
    {
        var existsBy = await UnitOfWork.CustomerGroupRepo.ExistsByCode(req.Code!, id);
        if (existsBy)
        {
            throw new NotFoundException("Exit by code: " + req.Code);
        }
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.CustomerGroupRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("Customer group " + id + " not found");
        UnitOfWork.CustomerGroupRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByCustomerGroupId(long? id)
    {
        var existsBy = await UnitOfWork.CustomerGroupRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Customer group not exit by id: " + id);
        }
    }
}