using ClosedXML.Excel;
using ptdn_net.Data.Dto.Customer;

namespace ptdn_net.Services.interfaces;

public interface ICustomerService
{
    Task<List<CustomerResp>> GetAll();

    Task<CustomerResp> GetById(Guid id);
    
    Task<Guid> CreateBy(CustomerReq req);
    
    Task<Guid> UpdateBy(CustomerReq req, Guid id);
    
    Task DeleteBy(Guid id);
    
    Task ValidNotExistsByCustomerId(Guid? id);
    
    Task UpdateDebtBy(Guid id, decimal? debt);
    
    XLWorkbook Export(List<CustomerResp> customers);
    
    Task<Guid> UpdateNoBy(Guid id);
}