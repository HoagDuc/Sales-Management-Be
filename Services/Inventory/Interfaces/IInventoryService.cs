using ClosedXML.Excel;
using ptdn_net.Data.Dto;
using ptdn_net.Data.Dto.BaoCao;
using ptdn_net.Data.Entity;

namespace ptdn_net.Services.interfaces;

public interface IInventoryService
{
    Task<long> CreateBy(long productId, long? minQuantity, long quantity, decimal price);
    
    Task<long> UpdateBy(long productId, long? minQuantity, long quantity, decimal price);

    Task GoInventory(long productId, short? quantity, decimal? price);
    Task<IEnumerable<Inventory>?> GetAllSpHetHang();
    
    Task GoInventory2(long productId, short? quantity);

    Task OutInventory(long productId, short? quantity);

    Task<InventoryResp> GetThongKeTonKho(long productId);
    
    Task<List<InventoryResp>> GetAll();
    
    Task<XLWorkbook> Export(List<InventoryResp> inventories);

    Task<List<BaoCaoKhoResp>> ThongKeKho();
    
    Task<InventoryResp?> GetByProductId(long orderDetailProductId);
}