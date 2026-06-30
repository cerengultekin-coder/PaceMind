using System.Collections.Frozen;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

/// <summary>
/// Registry-backed resolver. The profile set is fixed at construction and stored in a
/// <see cref="FrozenDictionary{TKey,TValue}"/> for fast, allocation-free lookups on the
/// read-heavy planning/adaptation path.
/// </summary>
public sealed class SportProfileResolver : ISportProfileResolver
{
    private readonly FrozenDictionary<Sport, ISportProfile> _profiles;

    /// <summary>Builds the registry, rejecting duplicate profiles for the same sport.</summary>
    public SportProfileResolver(IEnumerable<ISportProfile> profiles)
    {
        ArgumentNullException.ThrowIfNull(profiles);
        _profiles = profiles.ToFrozenDictionary(profile => profile.Sport);
    }

    public bool Supports(Sport sport) => _profiles.ContainsKey(sport);

    public ISportProfile Resolve(Sport sport) =>
        _profiles.TryGetValue(sport, out var profile)
            ? profile
            : throw new NotSupportedException($"No training profile is registered for sport '{sport}'.");
}
