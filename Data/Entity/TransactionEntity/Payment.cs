
namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class Payment
{
    public Guid PaymentId { get; set; }

    public short Type { get; set; }

    public short DeliveryMethod { get; set; }

    public DateTime PaymentAt { get; set; }

    public decimal PaymentTotal { get; set; }

    public string CreateBy { get; set; } = null!;

    public DateTime CreateAt { get; set; }
}
