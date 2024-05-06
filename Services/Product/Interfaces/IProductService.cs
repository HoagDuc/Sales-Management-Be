using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface IProductService
{
    Task<List<ProductResp>> GetAll();

    Task<ProductResp> GetById(long id);
    
    Task<long> CreateBy(ProductReq req,  List<IFormFile>  a);
    
    Task<long> UpdateBy(ProductReq req, long id);
    
    Task DeleteBy(long id);
    
    XLWorkbook Export(List<ProductResp> products);
    
    Task ImportExcel(IFormFile file);
}