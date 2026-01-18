using HoldON.Models;
using HoldON.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public partial class WorkoutSessionDisplay : ObservableObject
{
    public int SessionId { get; set; }
    public DateTime SessionDate { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int DurationMinutes { get; set; }
    public string Notes { get; set; } = string.Empty;
    public int TotalSets { get; set; }
    public int TotalExercises { get; set; }
    public double TotalVolumeKg { get; set; }
    public string ExercisesSummary { get; set; } = string.Empty;

    public string DisplayDate => SessionDate.ToString("dddd, d. MMMM yyyy");
    public string DisplayTime => $"{StartTime:HH:mm} - {EndTime:HH:mm}";
    public string TotalVolume => $"{TotalVolumeKg:N0} kg";
    public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);
}

public partial class WorkoutHistoryViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    [ObservableProperty]
    private ObservableCollection<WorkoutSessionDisplay> workoutSessions = new();

    [ObservableProperty]
    private int totalWorkouts;

    [ObservableProperty]
    private int workoutsThisMonth;

    [ObservableProperty]
    private bool hasNoWorkouts;

    public WorkoutHistoryViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        Title = "Zgodovina";
    }

    public async void LoadWorkoutHistory()
    {
        try
        {
            IsBusy = true;

            // Load all workout sessions for the current user
            var sessions = await _databaseService.GetUserWorkoutSessionsAsync(_currentUserId);

            // Calculate stats
            TotalWorkouts = sessions.Count;
            WorkoutsThisMonth = sessions.Count(s => s.SessionDate.Month == DateTime.Now.Month &&
                                                     s.SessionDate.Year == DateTime.Now.Year);
            HasNoWorkouts = sessions.Count == 0;

            var displaySessions = new List<WorkoutSessionDisplay>();

            foreach (var session in sessions.OrderByDescending(s => s.SessionDate).ThenByDescending(s => s.StartTime))
            {
                // Get all sets for this session
                var sets = await _databaseService.GetSessionSetsAsync(session.SessionId);

                // Group by exercise to get unique exercises
                var exerciseGroups = sets.GroupBy(s => s.ExerciseId).ToList();

                // Calculate total volume
                double totalVolume = sets.Sum(s => (s.WeightKg ?? 0) * (s.Reps ?? 0));

                // Create exercises summary (show first 2-3 exercises)
                var exerciseNames = new List<string>();
                foreach (var group in exerciseGroups.Take(3))
                {
                    // For now, use generic names based on exercise ID
                    // You could enhance this by loading actual exercise names from the database
                    string exerciseName = GetExerciseName(group.Key);
                    exerciseNames.Add(exerciseName);
                }

                string summary = string.Join(", ", exerciseNames);
                if (exerciseGroups.Count > 3)
                {
                    summary += $" +{exerciseGroups.Count - 3} več";
                }

                var display = new WorkoutSessionDisplay
                {
                    SessionId = session.SessionId,
                    SessionDate = session.SessionDate,
                    StartTime = session.StartTime ?? DateTime.Now,
                    EndTime = session.EndTime ?? DateTime.Now,
                    DurationMinutes = session.DurationMinutes ?? 0,
                    Notes = session.Notes ?? string.Empty,
                    TotalSets = sets.Count,
                    TotalExercises = exerciseGroups.Count,
                    TotalVolumeKg = totalVolume,
                    ExercisesSummary = summary
                };

                displaySessions.Add(display);
            }

            WorkoutSessions = new ObservableCollection<WorkoutSessionDisplay>(displaySessions);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading workout history: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert(
                "Napaka",
                "Napaka pri nalaganju zgodovine treningov.",
                "V redu");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private string GetExerciseName(int exerciseId)
    {
        // Map exercise IDs to names
        // This should ideally be loaded from the database
        return exerciseId switch
        {
            1 => "Bench Press",
            2 => "Squat",
            3 => "Deadlift",
            4 => "Shoulder Press",
            5 => "Pull-up",
            _ => $"Vaja {exerciseId}"
        };
    }

    [RelayCommand]
    private async Task ShowWorkoutDetails(WorkoutSessionDisplay session)
    {
        try
        {
            // Load detailed information about this workout session
            var sets = await _databaseService.GetSessionSetsAsync(session.SessionId);
            var exerciseGroups = sets.GroupBy(s => s.ExerciseId);

            var details = new System.Text.StringBuilder();
            details.AppendLine($"📅 {session.DisplayDate}");
            details.AppendLine($"⏱️ {session.DisplayTime}");
            details.AppendLine($"⏳ {session.DurationMinutes} minut\n");

            foreach (var group in exerciseGroups)
            {
                string exerciseName = GetExerciseName(group.Key);
                details.AppendLine($"💪 {exerciseName}");

                foreach (var set in group.OrderBy(s => s.SetNumber))
                {
                    string setInfo = $"   Serija {set.SetNumber}: {set.WeightKg:F1} kg × {set.Reps} ponovitev";
                    details.AppendLine(setInfo);
                }
                details.AppendLine();
            }

            if (!string.IsNullOrWhiteSpace(session.Notes))
            {
                details.AppendLine($"📝 Opombe: {session.Notes}");
            }

            await Application.Current.MainPage.DisplayAlert(
                "Podrobnosti treninga",
                details.ToString(),
                "Zapri");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error showing workout details: {ex.Message}");
        }
    }
}
