namespace ptdn_net.Data.Dto.Product;

public class VendorReq
{
    public string? Code { get; set; } 

    public string? Name { get; set; } 

    public string? Email { get; set; }

    public decimal? Debt { get; set; }

    public string? Website { get; set; }

    public string? Phone { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Fax { get; set; }

    public long? ProvinceId { get; set; }

    public long? SubDistrictId { get; set; }

    public long? DistrictId { get; set; }

    public string? Address { get; set; }
}