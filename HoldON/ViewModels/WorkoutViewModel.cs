using HoldON.Models;
using HoldON.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public partial class WorkoutViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private readonly DatabaseService _databaseService;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    // Mapping exercise names to database IDs
    private readonly Dictionary<string, int> _exerciseNameToId = new()
    {
        { "Bench Press", 1 },
        { "Squat", 2 },
        { "Deadlift", 3 },
        { "Shoulder Press", 4 },
        { "Pull-up", 5 }
    };

    [ObservableProperty]
    private ObservableCollection<ExerciseEntry> currentExercises = new();

    [ObservableProperty]
    private string workoutName = "Novi trening";

    [ObservableProperty]
    private DateTime workoutStartTime = DateTime.Now;

    public WorkoutViewModel(DataService dataService, DatabaseService databaseService)
    {
        _dataService = dataService;
        _databaseService = databaseService;
        Title = "Vadba";
        workoutStartTime = DateTime.Now;
    }

    [RelayCommand]
    private async Task AddExercise()
    {
        var library = _dataService.GetExerciseLibrary();

        // Show exercise picker
        var exerciseNames = library.Select(e => e.Name).ToArray();
        var selectedExercise = await Application.Current.MainPage.DisplayActionSheet(
            "Izberi vajo",
            "Prekliči",
            null,
            exerciseNames);

        if (selectedExercise == "Prekliči" || string.IsNullOrEmpty(selectedExercise))
            return;

        var exercise = library.FirstOrDefault(e => e.Name == selectedExercise);
        if (exercise == null)
            return;

        var entry = new ExerciseEntry
        {
            Exercise = exercise
        };
        entry.Sets.Add(new WorkoutSet { SetNumber = 1, Reps = 10, Weight = 50 });

        CurrentExercises.Add(entry);
    }

    [RelayCommand]
    private void AddSet(ExerciseEntry entry)
    {
        if (entry == null) return;

        var lastSet = entry.Sets.LastOrDefault();
        var newSetNumber = entry.Sets.Count + 1;
        entry.Sets.Add(new WorkoutSet
        {
            SetNumber = newSetNumber,
            Reps = lastSet?.Reps ?? 10,
            Weight = lastSet?.Weight ?? 0
        });
    }

    [RelayCommand]
    private async Task SaveWorkout()
    {
        if (CurrentExercises.Count == 0)
        {
            await Shell.Current.GoToAsync("//HomePage");
            return;
        }

        try
        {
            // Create workout session in database
            var endTime = DateTime.Now;
            var duration = (int)(endTime - WorkoutStartTime).TotalMinutes;

            var session = new WorkoutSession
            {
                UserId = _currentUserId,
                SessionDate = DateTime.Today,
                StartTime = WorkoutStartTime,
                EndTime = endTime,
                DurationMinutes = duration,
                Notes = WorkoutName
            };

            await _databaseService.SaveWorkoutSessionAsync(session);

            // Track new personal records
            var newRecordsAchieved = new List<string>();

            // Save all sets to database and check for personal records
            foreach (var exerciseEntry in CurrentExercises)
            {
                // Map exercise name to ID
                if (!_exerciseNameToId.TryGetValue(exerciseEntry.Exercise.Name, out int exerciseId))
                {
                    exerciseId = 1; // Default fallback
                }

                // Find the maximum weight for this exercise in this session
                double maxWeight = exerciseEntry.Sets.Max(s => s.Weight);

                for (int i = 0; i < exerciseEntry.Sets.Count; i++)
                {
                    var set = exerciseEntry.Sets[i];
                    var workoutSet = new WorkoutSetDb
                    {
                        SessionId = session.SessionId,
                        ExerciseId = exerciseId,
                        SetNumber = i + 1,
                        Reps = set.Reps,
                        WeightKg = set.Weight,
                        IsWarmup = false
                    };

                    await _databaseService.SaveWorkoutSetAsync(workoutSet);
                }

                // Check and update personal record for this exercise
                bool isNewRecord = await _databaseService.UpdatePersonalRecordIfBetterAsync(
                    _currentUserId,
                    exerciseId,
                    maxWeight,
                    session.SessionId);

                if (isNewRecord)
                {
                    newRecordsAchieved.Add($"{exerciseEntry.Exercise.Name}: {maxWeight} kg");
                }
            }

            // Also save to DataService for backward compatibility
            var workout = new Workout
            {
                Name = string.IsNullOrWhiteSpace(WorkoutName) ? "Novi trening" : WorkoutName,
                Date = DateTime.Now,
                Exercises = new ObservableCollection<ExerciseEntry>(CurrentExercises)
            };
            _dataService.SaveWorkout(workout);

            // Reset state
            CurrentExercises.Clear();
            WorkoutName = "Novi trening";
            WorkoutStartTime = DateTime.Now;

            // Show success message with personal records info
            string successMessage = "Vadba je bila shranjena!";
            if (newRecordsAchieved.Count > 0)
            {
                successMessage += "\n\n🎉 Novi osebni rekordi:\n" + string.Join("\n", newRecordsAchieved);
            }

            await Application.Current.MainPage.DisplayAlert(
                "Uspešno",
                successMessage,
                "V redu");

            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving workout: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert(
                "Napaka",
                "Napaka pri shranjevanju vadbe.",
                "V redu");
        }
    }

    [RelayCommand]
    private void RemoveExercise(ExerciseEntry entry)
    {
        if (entry != null)
            CurrentExercises.Remove(entry);
    }

    [RelayCommand]
    private void RemoveSet(WorkoutSet set)
    {
        foreach (var exercise in CurrentExercises)
        {
            if (exercise.Sets.Contains(set))
            {
                exercise.Sets.Remove(set);
                break;
            }
        }
    }
}