using ClosedXML.Excel;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Data.Dto.Transaction;

namespace ptdn_net.Services.interfaces;

public interface IOrderService
{
    Task<List<OrderResp>> GetAll();

    Task<List<OrderResp>> GetByCustomerId(Guid customerId);

    Task<List<OrderResp>> GetAllFromDate(DateTime? fromDate, DateTime? toDate);

    Task<List<OrderResp>> GetAllFromDateV2(DateTime? fromDate, DateTime? toDate);

    Task<List<OrderResp>> GetAllDangVanChuyen();

    Task<OrderResp> GetById(Guid id);

    Task<Guid> CreateBy(OrderReq req);

    Task<Guid> CreateDonBanTaiQuayBy(OrderReq req);

    Task<Guid> UpdateBy(OrderReq req, Guid id);

    Task DeleteBy(Guid id);

    Task<Guid> OutInventory(OrderReq req, Guid id);
    
    XLWorkbook Export(List<OrderResp> orders, DateTime fromDate, DateTime toDate);
    
    Task<List<OrderResp>> GetAllOrderDone();

    Task<Guid> UpdateStatusRefundBy(Guid id);
    
    Task ValidNotExistsByOrderId(Guid? id);
    
    Task<Guid> UpdateStatusGiaoThatBai(Guid id);
}