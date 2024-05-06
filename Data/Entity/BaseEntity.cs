namespace ptdn_net.Data.Entity;

public abstract class BaseEntity : IBaseEntity
{
    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}