using System;
using System.Collections.Generic;

namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class RefundOrder : BaseEntity
{
    public Guid RefundOrderId { get; set; }

    public Guid OrderId { get; set; }

    public string Code { get; set; } = null!;

    public string? Note { get; set; }

    public string Adress { get; set; } = null!;

    public short? Quantity { get; set; }

    public decimal? AmountOther { get; set; }

    public decimal? TotalAmount { get; set; }

    public short? Status { get; set; }
    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<RefundOrderDetail> RefundOrderDetails { get; set; } = new List<RefundOrderDetail>();
}
