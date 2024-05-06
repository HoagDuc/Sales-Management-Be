
namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class Transaction
{
    public Guid TransactionId { get; set; }

    public string Code { get; set; } = null!;

    public long TransactionTypeId { get; set; }

    public decimal Price { get; set; }

    public DateTime Date { get; set; }

    public virtual TransactionType TransactionType { get; set; } = null!;
}
