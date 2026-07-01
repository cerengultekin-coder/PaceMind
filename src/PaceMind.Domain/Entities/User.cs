namespace PaceMind.Domain.Entities;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string DisplayName { get; set; }

    public required string GoogleSubjectId { get; set; }

    public ICollection<Goal> Goals { get; set; } = [];
}
