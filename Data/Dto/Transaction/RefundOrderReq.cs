namespace ptdn_net.Data.Dto.Transaction;

public class RefundOrderReq
{
    public Guid OrderId { get; set; }

    public string? Code { get; set; }

    public string? Note { get; set; }

    public string? Adress { get; set; }

    public short? Quantity { get; set; }

    public decimal? AmountOther { get; set; }

    public decimal? TotalAmount { get; set; }

    public short? Status { get; set; }

    public ICollection<RefundOrderDetailReq>? RefundOrderDetails { get; set; }
}