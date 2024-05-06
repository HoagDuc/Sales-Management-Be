using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Services.interfaces;

public interface ITransactionTypeService
{
    Task<List<TransactionTypeResp>> GetAll();

    Task<TransactionTypeResp> GetById(long id);
    
    Task<long> CreateBy(TransactionTypeReq req);
    
    Task<long> UpdateBy(TransactionTypeReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByTransactionTypeId(long? id);
}