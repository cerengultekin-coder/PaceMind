using System.Collections.Frozen;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training;

public sealed class SportProfileResolver : ISportProfileResolver
{
    private readonly FrozenDictionary<Sport, ISportProfile> _profiles;

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
