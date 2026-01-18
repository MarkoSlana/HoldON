using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HoldON.Models;

public partial class Exercise : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private string description = string.Empty;
    [ObservableProperty]
    private string imageUrl = string.Empty;
    [ObservableProperty]
    private string category = string.Empty;
}

public partial class WorkoutSet : ObservableObject
{
    [ObservableProperty]
    private int setNumber = 1;
    [ObservableProperty]
    private int reps;
    [ObservableProperty]
    private double weight;
}

public partial class ExerciseEntry : ObservableObject
{
    [ObservableProperty]
    private Exercise exercise = new();
    [ObservableProperty]
    private ObservableCollection<WorkoutSet> sets = new();
    
    public double TotalVolume => Sets.Sum(s => s.Weight * s.Reps);
}

public partial class Workout : ObservableObject
{
    [ObservableProperty]
    private string id = Guid.NewGuid().ToString();
    [ObservableProperty]
    private string name = string.Empty;
    [ObservableProperty]
    private DateTime date = DateTime.Now;
    [ObservableProperty]
    private ObservableCollection<ExerciseEntry> exercises = new();
    [ObservableProperty]
    private TimeSpan duration;

    public double TotalVolume => Exercises.Sum(e => e.TotalVolume);
    public string FormattedTotalVolume => $"{TotalVolume:N0} kg";
    public string FormattedDate => Date.ToString("d. MMM");
}

// Workout Plan Models
public class WorkoutPlan
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Goal { get; set; } = string.Empty; // "Mišična masa", "Hujšanje", "Moč", "Kondicija"
    public string EquipmentType { get; set; } = string.Empty; // "Gym", "Doma", "Minimalno"
    public string Level { get; set; } = string.Empty; // "Začetnik", "Napredni"
    public int DaysPerWeek { get; set; }
    public int DurationWeeks { get; set; }
    public List<WorkoutDay> WorkoutDays { get; set; } = new();
}

public class WorkoutDay
{
    public int DayNumber { get; set; } // 1-7 (pon-ned)
    public string DayName { get; set; } = string.Empty; // "Ponedeljek", "Torek"...
    public string Focus { get; set; } = string.Empty; // "Potisni dan", "Vlečni dan", "Noge"
    public List<PlannedExercise> Exercises { get; set; } = new();
    public bool IsRestDay { get; set; }
}

public class PlannedExercise
{
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public string Reps { get; set; } = string.Empty; // "8-12", "12-15", "AMRAP"
    public string RestTime { get; set; } = string.Empty; // "60s", "90s"
    public string Notes { get; set; } = string.Empty;
    public string YouTubeUrl { get; set; } = string.Empty;
}