using ClosedXML.Excel;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Services.interfaces;

public interface ITransactionService
{
    Task<List<TransactionResp>> GetAll();

    Task<TransactionResp> GetById(Guid id);
    
    Task<Guid> CreateBy(TransactionReq req);
    
    Task<Guid> Create(string code, long transactionTypeId, decimal? price, DateTime date);
    
    Task<Guid> UpdateBy(TransactionReq req, Guid id);
    
    Task DeleteBy(Guid id);
    
    Task<List<TransactionResp>> GetAllFromDate(DateTime ngayBatDau, DateTime ngayKetThuc);
    
    XLWorkbook Export(List<TransactionResp> transactionList, DateTime fromDate, DateTime toDate);
}