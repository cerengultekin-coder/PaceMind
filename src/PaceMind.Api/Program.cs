using System.Text.Json.Serialization;
using PaceMind.Api.Contracts;
using PaceMind.Application;
using PaceMind.Contracts;
using PaceMind.Domain.Entities;
using PaceMind.Domain.Training;
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
    var goal = new Goal
    {
        Sport = request.Sport,
        TargetDistanceKm = request.TargetDistanceKm,
        TargetTimeMinutes = request.TargetTimeMinutes,
        TargetDate = request.TargetDate,
        DaysPerWeekMax = request.DaysPerWeekMax,
        BlackoutDays = request.BlackoutDays?.ToList() ?? [],
        BaselineNote = request.BaselineNote ?? string.Empty,
    };

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

app.MapFallbackToFile("index.html");

app.Run();

static DateOnly NextMonday(DateOnly from)
{
    var daysUntilMonday = ((int)DayOfWeek.Monday - (int)from.DayOfWeek + 7) % 7;
    return from.AddDays(daysUntilMonday == 0 ? 7 : daysUntilMonday);
}
