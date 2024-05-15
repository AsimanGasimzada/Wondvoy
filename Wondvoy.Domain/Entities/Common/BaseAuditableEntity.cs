namespace Wondvoy.Domain.Entities.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public string CreatedAt { get; set; } = null!;
    public string UpdatedAt { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
}