using PaceMind.Contracts;

namespace PaceMind.Web.Services;

public sealed class PlanState
{
    public PlanPreviewRequest? Goal { get; private set; }
    public DateOnly StartDate { get; private set; }
    public int TotalWeeks { get; private set; }
    public PlanWeekDto? CurrentWeek { get; private set; }

    public bool HasProgram => Goal is not null && CurrentWeek is not null;

    public event Action? Changed;

    public void SetWeek(PlanPreviewRequest goal, DateOnly startDate, int totalWeeks, PlanWeekDto week)
    {
        Goal = goal;
        StartDate = startDate;
        TotalWeeks = totalWeeks;
        CurrentWeek = week;
        Changed?.Invoke();
    }
}
