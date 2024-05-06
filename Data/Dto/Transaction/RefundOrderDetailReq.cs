namespace ptdn_net.Data.Dto.Transaction;

public class RefundOrderDetailReq
{
    public long ProductId { get; set; }

    public decimal? Price { get; set; }

    public short Quantity { get; set; }

    public string? Note { get; set; }
}