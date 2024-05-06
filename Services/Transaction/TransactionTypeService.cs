using AutoMapper;
using BT_MVC_Web.Exceptions;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;

namespace ptdn_net.Services;

public class TransactionTypeService : BaseService, ITransactionTypeService
{
    private readonly IMapper _mapper;

    public TransactionTypeService(IUnitOfWork unitOfWork,
        IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public async Task<List<TransactionTypeResp>> GetAll()
    {
        var entities = await UnitOfWork.TransactionTypeRepo.GetAll();
        return _mapper.Map<List<TransactionTypeResp>>(entities);
    }

    public async Task<TransactionTypeResp> GetById(long id)
    {
        var entity = await UnitOfWork.TransactionTypeRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("TransactionType " + id + " not found");

        return _mapper.Map<TransactionTypeResp>(entity);
    }

    public async Task<long> CreateBy(TransactionTypeReq req)
    {
        var entity = _mapper.Map<TransactionType>(req);

        await UnitOfWork.TransactionTypeRepo.InsertAsync(entity);
        await UnitOfWork.CompleteAsync();
        return entity.TransactionTypeId;
    }

    public async Task<long> UpdateBy(TransactionTypeReq req, long id)
    {
        var entity = await UnitOfWork.TransactionTypeRepo.GetByIdAsync(id);
        _mapper.Map(req, entity);

        UnitOfWork.TransactionTypeRepo.Update(entity!);
        await UnitOfWork.CompleteAsync();
        return entity!.TransactionTypeId;
    }

    public async Task DeleteBy(long id)
    {
        var entity = await UnitOfWork.TransactionTypeRepo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException("TransactionType " + id + " not found");
        UnitOfWork.TransactionTypeRepo.Remove(entity);
        await UnitOfWork.CompleteAsync();
    }

    public async Task ValidNotExistsByTransactionTypeId(long? id)
    {
        var existsBy = await UnitOfWork.TransactionTypeRepo.ExistsById(id);
        if (!existsBy)
        {
            throw new NotFoundException("Not exit by TransactionType id: " + id);
        }
    }
}