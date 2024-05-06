using System.Text.Json.Serialization;
using ptdn_net.Const;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Data.Entity.CustomerEntity;

public partial class Customer: BaseEntity
{
    public Guid CustomerId { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public long? ProvinceId { get; set; }

    public long? DistrictId { get; set; }

    public long? SubDistrictId { get; set; }

    public string? Address { get; set; }

    public DateTime? Dob { get; set; }

    public Gender? Gender { get; set; }

    public decimal? Fax { get; set; }

    public decimal? Tax { get; set; }

    public string? Website { get; set; }

    public decimal? Debt { get; set; }

    public decimal? TotalExpenditure { get; set; }

    public long? CustomerGroupId { get; set; }

    public virtual CustomerGroup? CustomerGroup { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
