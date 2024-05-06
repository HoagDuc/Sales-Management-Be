namespace ptdn_net.Data.Dto.Customer;

public class CustomerGroupReq
{
    public string? Code { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    public short? Discount { get; set; }
}