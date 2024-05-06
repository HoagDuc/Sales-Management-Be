using ClosedXML.Excel;
using ptdn_net.Data.Dto.Product;

namespace ptdn_net.Services.interfaces;

public interface ICategoryService
{
    Task<List<CategoryResp>> GetAll();

    Task<CategoryResp> GetById(long id);
    
    Task<long> CreateBy(CategoryReq req);
    
    Task<long> UpdateBy(CategoryReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByCategoryId(long? id);
    
    XLWorkbook Export(List<CategoryResp> categories);
}