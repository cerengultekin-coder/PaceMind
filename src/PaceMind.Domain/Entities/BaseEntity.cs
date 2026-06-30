namespace PaceMind.Domain.Entities;

/// <summary>Common identity and audit fields shared by all aggregate roots and entities.</summary>
public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
