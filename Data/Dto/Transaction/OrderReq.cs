namespace ptdn_net.Data.Dto.Transaction;

public class OrderReq
{
    public Guid CustomerId { get; set; }

    public string? Code { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public short PaymentMethod { get; set; }

    public short? Discount { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Vat { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal? AmountRemaining { get; set; }

    public decimal TotalAmount { get; set; }
    
    public decimal? TotalNhap { get; set; }

    public string? Note { get; set; }

    public short? DeliveryMethod { get; set; }

    public short? Status { get; set; }

    public virtual ICollection<OrderDetailReq> OrderDetails { get; set; } = new List<OrderDetailReq>();
    
    public decimal? Pay { get; set; }
}