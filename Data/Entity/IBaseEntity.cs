namespace ptdn_net.Data.Entity;

public interface IBaseEntity
{
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}