using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IVendorService
{
    Task<List<VendorResp>> GetAll();

    Task<VendorResp> GetById(long id);
    
    Task<long> CreateBy(VendorReq req);
    
    Task<long> UpdateBy(VendorReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByVendorId(long? vendorId);
    
    Task<XLWorkbook> ExportExcel();

    XLWorkbook Export(List<VendorResp> vendors);
}