
using System.Text.Json.Serialization;

namespace ptdn_net.Data.Entity.CustomerEntity;

public partial class CustomerGroup
{
    public long CustomerGroupId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public short? Discount { get; set; }

    [JsonIgnore]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
