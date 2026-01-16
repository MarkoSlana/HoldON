using HoldON.Models;
using Newtonsoft.Json;

namespace HoldON.Services;

public class DataService
{
    private const string WorkoutsKey = "workouts_data";
    private const string ProfileKey = "profile_data";

    public List<Workout> GetWorkouts()
    {
        try
        {
            var json = Preferences.Default.Get(WorkoutsKey, string.Empty);
            if (string.IsNullOrEmpty(json)) return new List<Workout>();
            return JsonConvert.DeserializeObject<List<Workout>>(json) ?? new List<Workout>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading workouts: {ex.Message}");
            return new List<Workout>();
        }
    }

    public void SaveWorkout(Workout workout)
    {
        try
        {
            var workouts = GetWorkouts();
            workouts.Add(workout);
            var json = JsonConvert.SerializeObject(workouts);
            Preferences.Default.Set(WorkoutsKey, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving workout: {ex.Message}");
        }
    }

    public UserProfile GetProfile()
    {
        try
        {
            var json = Preferences.Default.Get(ProfileKey, string.Empty);
            if (string.IsNullOrEmpty(json)) return new UserProfile { Name = "Janez Novak" };
            return JsonConvert.DeserializeObject<UserProfile>(json) ?? new UserProfile { Name = "Janez Novak" };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading profile: {ex.Message}");
            return new UserProfile { Name = "Janez Novak" };
        }
    }

    public void SaveProfile(UserProfile profile)
    {
        try
        {
            var json = JsonConvert.SerializeObject(profile);
            Preferences.Default.Set(ProfileKey, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving profile: {ex.Message}");
        }
    }

    public List<Exercise> GetExerciseLibrary()
    {
        return new List<Exercise>
        {
            new Exercise { Name = "Bench Press", Category = "Chest", Description = "Ležeči potisk s palico." },
            new Exercise { Name = "Squat", Category = "Legs", Description = "Počep z utežjo." },
            new Exercise { Name = "Deadlift", Category = "Back", Description = "Mrtvi dvig." },
            new Exercise { Name = "Shoulder Press", Category = "Shoulders", Description = "Potisk nad glavo." },
            new Exercise { Name = "Pull-up", Category = "Back", Description = "Dvig na drogu." }
        };
    }
}