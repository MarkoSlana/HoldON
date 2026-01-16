namespace HoldON.Models;

public class UserProfile
{
    public string Name { get; set; } = "Uporabnik";
    public double Weight { get; set; }
    public double Height { get; set; }
    public string Goal { get; set; } = string.Empty; // e.g., Muscle gain, Weight loss
    public List<string> Achievements { get; set; } = new();
    public int Level { get; set; } = 1;
    public int ExperiencePoints { get; set; } = 0;

    // Workout Plan Settings
    public string FitnessGoal { get; set; } = "Mišična masa"; // "Mišična masa", "Hujšanje", "Moč", "Kondicija"
    public string EquipmentAvailable { get; set; } = "Gym"; // "Gym", "Doma", "Minimalno"
    public string ExperienceLevel { get; set; } = "Začetnik"; // "Začetnik", "Napredni"
    public int WorkoutDaysPerWeek { get; set; } = 3;
    public string? CurrentWorkoutPlanId { get; set; }

    // Nutrition
    public double CurrentCalories { get; set; } = 0;
    public double TargetCalories { get; set; } = 2800;
    public double CurrentProtein { get; set; } = 0;
    public double TargetProtein { get; set; } = 180;
    public double CurrentCarbs { get; set; } = 0;
    public double TargetCarbs { get; set; } = 320;
    public double CurrentFats { get; set; } = 0;
    public double TargetFats { get; set; } = 80;

    // Recovery & Wellness
    public List<WellnessEntry> WellnessHistory { get; set; } = new();
}

public class WellnessEntry
{
    public DateTime Date { get; set; }
    public int MoodRating { get; set; } // 1-5
    public double SleepHours { get; set; }
    public int EnergyLevel { get; set; } // 1-5
    public string Notes { get; set; } = string.Empty;
}