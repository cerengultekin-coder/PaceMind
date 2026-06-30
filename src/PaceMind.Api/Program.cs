using System.Text.Json.Serialization;
using PaceMind.Api.Contracts;
using PaceMind.Application;
using PaceMind.Contracts;
using PaceMind.Domain.Entities;
using PaceMind.Domain.Enums;
using PaceMind.Domain.Training;
using PaceMind.Domain.Training.Adaptation;
using PaceMind.Domain.Training.Planning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTrainingEngine();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapPost("/api/plan/preview", (
    PlanPreviewRequest request,
    IPlanGenerator planGenerator,
    ISportProfileResolver profileResolver) =>
{
    if (!profileResolver.Supports(request.Sport))
        return Results.BadRequest($"Sport '{request.Sport}' is not supported yet.");

    var startDate = request.StartDate ?? NextMonday(DateOnly.FromDateTime(DateTime.UtcNow));
    var goal = BuildGoal(request);

    try
    {
        var plan = planGenerator.Generate(goal, startDate);
        var profile = profileResolver.Resolve(goal.Sport);
        return Results.Ok(PlanPreviewMapper.ToResponse(plan, goal, startDate, profile));
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("PreviewPlan");

app.MapPost("/api/plan/adapt", (
    PlanAdaptRequest request,
    IPlanGenerator planGenerator,
    IWeekAdapter weekAdapter,
    ISportProfileResolver profileResolver) =>
{
    if (!profileResolver.Supports(request.Goal.Sport))
        return Results.BadRequest($"Sport '{request.Goal.Sport}' is not supported yet.");

    var goal = BuildGoal(request.Goal);

    try
    {
        var plan = planGenerator.Generate(goal, request.StartDate);
        var weeks = plan.Weeks.OrderBy(week => week.WeekNumber).ToList();
        if (weeks.Count < 2)
            return Results.BadRequest("The plan is too short to adapt.");

        ApplyFeedback(weeks[0], request.Week1Feedback);

        var profile = profileResolver.Resolve(goal.Sport);
        var result = weekAdapter.Adapt(weeks[0], weeks[1], profile);

        var response = new PlanAdaptResponse(
            PlanPreviewMapper.ToResponse(plan, goal, request.StartDate, profile),
            new AdaptationSummaryDto(
                Math.Round(result.AppliedFactor, 2),
                result.Log.TriggerReason,
                result.Log.Summary,
                result.GoalAtRisk,
                result.Summary.PlannedSessions,
                result.Summary.Completed,
                result.Summary.Skipped));

        return Results.Ok(response);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
})
.WithName("AdaptPlan");

app.MapFallbackToFile("index.html");

app.Run();

static Goal BuildGoal(PlanPreviewRequest request) => new()
{
    Sport = request.Sport,
    TargetDistanceKm = request.TargetDistanceKm,
    TargetTimeMinutes = request.TargetTimeMinutes,
    TargetDate = request.TargetDate,
    DaysPerWeekMax = request.DaysPerWeekMax,
    BlackoutDays = request.BlackoutDays?.ToList() ?? [],
    BaselineNote = request.BaselineNote ?? string.Empty,
};

static void ApplyFeedback(TrainingWeek week, IReadOnlyList<SessionFeedbackInput> feedback)
{
    foreach (var input in feedback)
    {
        var workout = week.Workouts.FirstOrDefault(w => w.DayOfWeek == input.DayOfWeek && w.Type is not WorkoutType.Rest);
        if (workout is null)
            continue;

        workout.Feedback.Add(new WorkoutFeedback
        {
            WorkoutId = workout.Id,
            Workout = workout,
            Outcome = input.Outcome,
            Difficulty = input.Difficulty,
            SkipReason = input.SkipReason,
        });
        workout.Status = input.Outcome is FeedbackOutcome.Completed ? WorkoutStatus.Completed : WorkoutStatus.Skipped;
    }
}

static DateOnly NextMonday(DateOnly from)
{
    var daysUntilMonday = ((int)DayOfWeek.Monday - (int)from.DayOfWeek + 7) % 7;
    return from.AddDays(daysUntilMonday == 0 ? 7 : daysUntilMonday);
}
