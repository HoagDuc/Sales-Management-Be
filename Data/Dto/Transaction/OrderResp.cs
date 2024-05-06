using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Dto.Transaction;

public class OrderResp
{
    public Guid OrderId { get; set; }

    public Guid CustomerId { get; set; }

    public string? Code { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public short? PaymentMethod { get; set; }

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

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public Entity.CustomerEntity.Customer? Customer { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
    
    public OrderResp(Order entity)
    {
        OrderId = entity.OrderId;
        CustomerId = entity.CustomerId;
        Code = entity.Code;
        Phone = entity.Phone;
        Address = entity.Address;
        PaymentMethod = entity.PaymentMethod;
        Discount = entity.Discount;
        Tax = entity.Tax;
        Vat = entity.Vat;
        AmountDue = entity.AmountDue;
        AmountPaid = entity.AmountPaid;
        AmountRemaining = entity.AmountRemaining;
        TotalAmount = entity.TotalAmount;
        Note = entity.Note;
        DeliveryMethod = entity.DeliveryMethod;
        Status = entity.Status;
        CreateBy = entity.CreatedBy;
        CreateAt = entity.CreatedAt;
        Customer = entity.Customer;
        OrderDetails = entity.OrderDetails;
        TotalNhap = entity.TotalNhap;
    }
}