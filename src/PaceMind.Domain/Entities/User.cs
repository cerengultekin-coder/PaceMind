namespace PaceMind.Domain.Entities;

/// <summary>An authenticated athlete. Identity comes from Google OAuth.</summary>
public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string DisplayName { get; set; }

    /// <summary>Stable Google subject identifier (the OIDC "sub" claim).</summary>
    public required string GoogleSubjectId { get; set; }

    public ICollection<Goal> Goals { get; set; } = [];
}
