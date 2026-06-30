namespace PaceMind.Domain.Enums;

/// <summary>Sport the goal targets. MVP ships running only; the enum leaves room for more.</summary>
public enum Sport
{
    Running = 1,
}

/// <summary>What kind of target the athlete set. MVP supports distance + time.</summary>
public enum TargetType
{
    DistanceTime = 1,
}

public enum GoalStatus
{
    Active = 1,
    Completed = 2,
    Abandoned = 3,
}

public enum WeekStatus
{
    Upcoming = 1,
    Active = 2,
    Done = 3,
}

/// <summary>Type of a single session. The coaching logic balances these across a week.</summary>
public enum WorkoutType
{
    Rest = 0,
    Easy = 1,
    Tempo = 2,
    Interval = 3,
    Long = 4,
}

public enum WorkoutStatus
{
    Planned = 1,
    Completed = 2,
    Skipped = 3,
}

public enum FeedbackOutcome
{
    Completed = 1,
    Skipped = 2,
}

/// <summary>
/// Sport-agnostic effort band for a session. Each sport profile maps its own workout
/// types onto these zones, so training load can be reasoned about uniformly across sports.
/// Ordered from lightest to hardest.
/// </summary>
public enum IntensityZone
{
    Recovery = 1,
    Easy = 2,
    Moderate = 3,
    Threshold = 4,
    Anaerobic = 5,
}

/// <summary>How the session felt — the core signal the adapter reacts to.</summary>
public enum Difficulty
{
    Easy = 1,
    JustRight = 2,
    TooHard = 3,
    Pain = 4,
}

public enum SkipReason
{
    NoTime = 1,
    Sick = 2,
    Unmotivated = 3,
    Other = 4,
}

public enum ChatRole
{
    User = 1,
    Coach = 2,
}

/// <summary>Where activity metrics came from. MVP only ever uses Manual; Gpx/Fit are v2.</summary>
public enum ActivitySource
{
    Manual = 1,
    Gpx = 2,
    Fit = 3,
}
