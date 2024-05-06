using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Dto.Transaction;

public class RefundOrderResp
{
    public RefundOrderResp(RefundOrder entity)
    {
        RefundOrderId = entity.RefundOrderId;
        OrderId = entity.OrderId;
        Code = entity.Code;
        Note = entity.Note;
        Adress = entity.Adress;
        Quantity = entity.Quantity;
        AmountOther = entity.AmountOther;
        TotalAmount = entity.TotalAmount;
        Status = entity.Status;
        CreateBy = entity.CreatedBy;
        CreateAt = entity.CreatedAt;
        ModifyBy = entity.ModifiedBy;
        ModifyAt = entity.ModifiedAt;
        Order = entity.Order;
        RefundOrderDetails = entity.RefundOrderDetails;
    }

    public RefundOrderResp()
    {
    }

    public Guid RefundOrderId { get; set; }

    public Guid OrderId { get; set; }

    public string? Code { get; set; }

    public string? Note { get; set; }

    public string? Adress { get; set; }

    public short? Quantity { get; set; }

    public decimal? AmountOther { get; set; }

    public decimal? TotalAmount { get; set; }

    public short? Status { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? ModifyBy { get; set; }

    public DateTime? ModifyAt { get; set; }

    public Order? Order { get; set; }

    public ICollection<RefundOrderDetail>? RefundOrderDetails { get; set; }
}