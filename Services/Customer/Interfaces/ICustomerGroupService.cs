using ptdn_net.Data.Dto.Customer;

namespace ptdn_net.Services.interfaces;

public interface ICustomerGroupService
{
    Task<List<CustomerGroupResp>> GetAll();

    Task<CustomerGroupResp> GetById(long id);
    
    Task<long> CreateBy(CustomerGroupReq req);
    
    Task<long> UpdateBy(CustomerGroupReq req, long id);
    
    Task DeleteBy(long id);
    
    Task ValidNotExistsByCustomerGroupId(long? id);
}