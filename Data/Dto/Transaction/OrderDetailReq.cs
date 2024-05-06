namespace ptdn_net.Data.Dto.Transaction;

public class OrderDetailReq
{
    public long ProductId { get; set; }

    public short? Quantity { get; set; }

    public decimal? Price { get; set; }

    public short? Discount { get; set; }
}