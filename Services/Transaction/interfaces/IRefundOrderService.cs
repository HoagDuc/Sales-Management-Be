using ClosedXML.Excel;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Services.interfaces;

public interface IRefundOrderService
{
    Task<List<RefundOrderResp>> GetAll();
    
    Task<List<RefundOrderResp>> GetAllFromDate(DateTime? fromDate, DateTime? toDate);

    Task<RefundOrderResp> GetById(Guid id);
    
    Task<Guid> CreateBy(RefundOrderReq req);
    
    Task<Guid> UpdateBy(RefundOrderReq req, Guid id);
    
    Task DeleteBy(Guid id);
    
    Task<Guid> GoInventory(RefundOrderReq req, Guid id);
    
    XLWorkbook Export(List<RefundOrderResp> refundOrders, DateTime fromDate, DateTime toDate);
    
    Task<List<RefundOrderResp>> GetAllDangVanChuyen();
}