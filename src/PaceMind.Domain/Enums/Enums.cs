namespace PaceMind.Domain.Enums;

public enum Sport
{
    Running = 1,
}

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

public enum IntensityZone
{
    Recovery = 1,
    Easy = 2,
    Moderate = 3,
    Threshold = 4,
    Anaerobic = 5,
}

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

public enum ActivitySource
{
    Manual = 1,
    Gpx = 2,
    Fit = 3,
}
