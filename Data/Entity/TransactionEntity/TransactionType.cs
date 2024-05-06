
namespace ptdn_net.Data.Entity.TransactionEntity;

public partial class TransactionType
{
    public long TransactionTypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
