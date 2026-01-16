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
    private void AddExercise()
    {
        var library = _dataService.GetExerciseLibrary();
        var exercise = library[new Random().Next(library.Count)];

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

            // Save all sets to database
            foreach (var exerciseEntry in CurrentExercises)
            {
                // For now, use exercise ID 1 (you'll need to map exercises properly)
                int exerciseId = 1;

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

            await Application.Current.MainPage.DisplayAlert(
                "Uspešno",
                "Vadba je bila shranjena!",
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