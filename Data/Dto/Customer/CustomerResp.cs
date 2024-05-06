using ptdn_net.Const;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Data.Dto.Customer;

public class CustomerResp
{
    public Guid CustomerId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public long? ProvinceId { get; set; }

    public long? DistrictId { get; set; }

    public long? SubDistrictId { get; set; }

    public string? Address { get; set; }

    public DateTime? Dob { get; set; }

    public Gender? Gender { get; set; }

    public decimal? Fax { get; set; }

    public decimal? Tax { get; set; }

    public string? Website { get; set; }

    public decimal? Debt { get; set; }

    public decimal? TotalExpenditure { get; set; }
    
    public CustomerGroup CustomerGroup { get; set; }

    public long? CustomerGroupId { get; set; }
    
    public DateTime CreateAt { get; set; }
    
    public string? CreateBy { get; set; }
    
    public DateTime UpdateAt { get; set; }
    
    public string? UpdateBy { get; set; }
}