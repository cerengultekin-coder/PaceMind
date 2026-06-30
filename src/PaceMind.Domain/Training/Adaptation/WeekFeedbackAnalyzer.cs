using PaceMind.Domain.Entities;
using PaceMind.Domain.Enums;

namespace PaceMind.Domain.Training.Adaptation;

/// <summary>Reduces a completed week's workouts and their feedback to a <see cref="WeekFeedbackSummary"/>.</summary>
internal static class WeekFeedbackAnalyzer
{
    public static WeekFeedbackSummary Analyze(TrainingWeek week)
    {
        int planned = 0, completed = 0, skipped = 0, tooHard = 0, justRight = 0, easy = 0;
        var hadPain = false;

        foreach (var workout in week.Workouts)
        {
            if (workout.Type is WorkoutType.Rest)
                continue;

            planned++;

            var feedback = workout.Feedback.LastOrDefault();
            if (feedback is null)
                continue;

            if (feedback.Outcome is FeedbackOutcome.Skipped)
            {
                skipped++;
                continue;
            }

            completed++;
            switch (feedback.Difficulty)
            {
                case Difficulty.Pain:
                    tooHard++;
                    hadPain = true;
                    break;
                case Difficulty.TooHard:
                    tooHard++;
                    break;
                case Difficulty.JustRight:
                    justRight++;
                    break;
                case Difficulty.Easy:
                    easy++;
                    break;
            }
        }

        return new WeekFeedbackSummary(planned, completed, skipped, tooHard, justRight, easy, hadPain);
    }
}
