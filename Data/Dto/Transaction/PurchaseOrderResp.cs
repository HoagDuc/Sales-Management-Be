using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Dto.Transaction;

public class PurchaseOrderResp
{
    public PurchaseOrderResp(PurchaseOrder entity)
    {
        PurchaseOrderId = entity.PurchaseOrderId;
        VendorId = entity.VendorId;
        Code = entity.Code;
        OrderDate = entity.OrderDate;
        DeliveryDate = entity.DeliveryDate;
        Address = entity.Address;
        PaymentMethod = entity.PaymentMethod;
        Status = entity.Status;
        Discount = entity.Discount;
        Vat = entity.Vat;
        Tax = entity.Tax;
        AmountDue = entity.AmountDue;
        AmountRemaining = entity.AmountRemaining;
        AmountPaid = entity.AmountPaid;
        TotalAmount = entity.TotalAmount;
        AmountOther = entity.AmountOther;
        Note = entity.Note;
        CreateBy = entity.CreatedBy;
        CreateAt = entity.CreatedAt;
        ModifyBy = entity.ModifiedBy;
        ModifyAt = entity.ModifiedAt;
        PurchaseOrderDetails = entity.PurchaseOrderDetails;
        Vendor = entity.Vendor;
    }

    public Guid PurchaseOrderId { get; set; }

    public long? VendorId { get; set; }

    public string Code { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string Address { get; set; }

    public short? PaymentMethod { get; set; }

    public short? Status { get; set; }

    public short? Discount { get; set; }

    public decimal? Vat { get; set; }

    public decimal? Tax { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountRemaining { get; set; }

    public decimal? AmountPaid { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? AmountOther { get; set; }

    public string? Note { get; set; }

    public string? CreateBy { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? ModifyBy { get; set; }

    public DateTime? ModifyAt { get; set; }

    public ICollection<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

    public Vendor? Vendor { get; set; }
}