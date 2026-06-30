using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

/// <summary>Resolves the <see cref="ISportProfile"/> for a given sport.</summary>
public interface ISportProfileResolver
{
    /// <summary>True if a profile is registered for the sport.</summary>
    bool Supports(Sport sport);

    /// <summary>
    /// Returns the profile for the sport, or throws <see cref="NotSupportedException"/>
    /// when no profile is registered.
    /// </summary>
    ISportProfile Resolve(Sport sport);
}
