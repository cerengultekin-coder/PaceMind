using PaceMind.Contracts;

namespace PaceMind.Web.Services;

/// <summary>Holds the most recently generated plan so screens can share it without a
/// round-trip. A stand-in for persistence until the API stores plans per athlete.</summary>
public sealed class PlanState
{
    public PlanPreviewRequest? LastRequest { get; private set; }
    public PlanPreviewResponse? Plan { get; private set; }

    public bool HasPlan => Plan is not null;

    public event Action? Changed;

    public void Set(PlanPreviewRequest request, PlanPreviewResponse plan)
    {
        LastRequest = request;
        Plan = plan;
        Changed?.Invoke();
    }
}
