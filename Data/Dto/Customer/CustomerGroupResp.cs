using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Data.Dto.Customer;

public class CustomerGroupResp
{
    public long CustomerGroupId { get; set; }

    public string? Code { get; set; } 

    public string? Name { get; set; } 

    public string? Description { get; set; }

    public short? Discount { get; set; }
}