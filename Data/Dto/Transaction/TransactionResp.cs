namespace ptdn_net.Data.Dto.Transaction;

public class TransactionResp
{
    public Guid TransactionId { get; set; }

    public string? Code { get; set; }

    public long? TransactionTypeId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? Date { get; set; }
}