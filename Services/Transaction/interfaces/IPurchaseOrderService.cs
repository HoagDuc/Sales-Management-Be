using ClosedXML.Excel;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Services.interfaces;

public interface IPurchaseOrderService
{
    Task<List<PurchaseOrderResp>> GetAll();

    Task<PurchaseOrderResp> GetById(Guid id);
    
    Task<Guid> CreateBy(PurchaseOrderReq req);
    
    Task<Guid> UpdateBy(PurchaseOrderReq req, Guid id);
    
    Task DeleteBy(Guid id);
    
    Task<Guid> GoInventory(PurchaseOrderReq req, Guid id);
    
    XLWorkbook Export(List<PurchaseOrderResp> purchaseOrders, DateTime fromDate, DateTime toDate);
    
    Task<List<PurchaseOrderResp>> GetAllDangVanChuyen();
    
    Task<List<PurchaseOrderResp>> GetAllFromDate(DateTime? ngayBatDau, DateTime? ngayKetThuc);
}