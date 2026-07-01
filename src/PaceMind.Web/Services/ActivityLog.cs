namespace PaceMind.Web.Services;

public sealed record ActivityEntry(
    DateTimeOffset LoggedAt,
    string Sport,
    DayOfWeek Day,
    string Type,
    int? DurationMinutes,
    string Feeling);

public sealed class ActivityLog
{
    private readonly List<ActivityEntry> _entries = [];

    public IReadOnlyList<ActivityEntry> Entries => _entries;

    public event Action? Changed;

    public void Add(ActivityEntry entry)
    {
        _entries.Insert(0, entry);
        Changed?.Invoke();
    }
}
