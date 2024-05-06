using ptdn_net.Const;

namespace ptdn_net.Data.Dto.Customer;

public class CustomerReq
{
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

    public long? CustomerGroupId { get; set; }
}