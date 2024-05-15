using Wondvoy.Domain.Entities.Common;

namespace Wondvoy.Domain.Entities;

public class Setting : BaseEntity
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}
