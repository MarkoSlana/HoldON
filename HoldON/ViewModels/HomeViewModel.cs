using HoldON.Models;
using HoldON.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace HoldON.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private readonly DatabaseService _databaseService;
    private readonly WorkoutPlanGenerator _planGenerator;
    private System.Timers.Timer? _restTimer;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    [ObservableProperty]
    private UserProfile profile = new();

    [ObservableProperty]
    private string motivationalQuote = "Pusti vse na igrišču";

    [ObservableProperty]
    private ObservableCollection<Workout> recentWorkouts = new();

    [ObservableProperty]
    private int totalWorkoutsThisMonth;

    [ObservableProperty]
    private double totalWeightThisMonth;

    [ObservableProperty]
    private string weightProgressPercent = "+12% ta mesec";

    [ObservableProperty]
    private int workoutsThisWeek = 0;

    [ObservableProperty]
    private string totalVolumeFormatted = "0 kg";

    [ObservableProperty]
    private int monthlyGoal = 20;

    [ObservableProperty]
    private Workout? lastWorkout;

    [ObservableProperty]
    private string restTimerDisplay = "00:00";

    [ObservableProperty]
    private bool isRestTimerRunning = false;

    [ObservableProperty]
    private int restTimeSeconds = 0;

    [ObservableProperty]
    private WellnessEntry? todaysWellness;

    [ObservableProperty]
    private WorkoutPlan? currentWorkoutPlan;

    [ObservableProperty]
    private WorkoutDay? todaysWorkout;

    [ObservableProperty]
    private string todaysWorkoutSummary = "Ni programa";

    public HomeViewModel(DataService dataService, DatabaseService databaseService, WorkoutPlanGenerator planGenerator)
    {
        _dataService = dataService;
        _databaseService = databaseService;
        _planGenerator = planGenerator;
        Title = "Stronger";
    }

    public async void LoadData()
    {
        try
        {
            // Load from database
            await LoadFromDatabaseAsync();

            // Fallback to DataService if needed
            Profile = _dataService.GetProfile();
            var allWorkouts = _dataService.GetWorkouts();

            RecentWorkouts = new ObservableCollection<Workout>(allWorkouts.OrderByDescending(w => w.Date).Take(5));
            LastWorkout = RecentWorkouts.FirstOrDefault();

            var thisMonthWorkouts = allWorkouts.Where(w => w.Date.Month == DateTime.Now.Month && w.Date.Year == DateTime.Now.Year).ToList();
            TotalWorkoutsThisMonth = thisMonthWorkouts.Count;
            TotalWeightThisMonth = thisMonthWorkouts.Sum(w => w.TotalVolume);
            TotalVolumeFormatted = $"{TotalWeightThisMonth:N0} kg";

            var calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            var currentWeek = calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            WorkoutsThisWeek = allWorkouts.Count(w => calendar.GetWeekOfYear(w.Date, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == currentWeek && w.Date.Year == DateTime.Now.Year);

            // Load workout plan
            LoadWorkoutPlan();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading data: {ex.Message}");
            Profile = new UserProfile { Name = "Uporabnik" };
        }
    }

    private async Task LoadFromDatabaseAsync()
    {
        try
        {
            // Load user sessions from database
            var sessions = await _databaseService.GetUserWorkoutSessionsAsync(_currentUserId);

            // Load sleep logs
            var sleepLogs = await _databaseService.GetSleepLogsAsync(_currentUserId, 1);
            if (sleepLogs.Any())
            {
                var todaySleep = sleepLogs.FirstOrDefault();
                TodaysWellness = new WellnessEntry
                {
                    Date = todaySleep?.SleepDate ?? DateTime.Today,
                    SleepHours = todaySleep?.SleepDurationHours ?? 0,
                    MoodRating = 3,
                    EnergyLevel = 3
                };
            }

            // Load personal records
            var records = await _databaseService.GetUserPersonalRecordsAsync(_currentUserId);

            System.Diagnostics.Debug.WriteLine($"Loaded {sessions.Count} sessions from database");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database load error: {ex.Message}");
        }
    }

    private void LoadWorkoutPlan()
    {
        // Generate or load current plan
        if (string.IsNullOrEmpty(Profile.CurrentWorkoutPlanId))
        {
            // Generate default plan based on profile settings
            CurrentWorkoutPlan = _planGenerator.GeneratePlan(
                Profile.FitnessGoal,
                Profile.EquipmentAvailable,
                Profile.ExperienceLevel,
                Profile.WorkoutDaysPerWeek
            );
            Profile.CurrentWorkoutPlanId = CurrentWorkoutPlan.Id;
            _dataService.SaveProfile(Profile);
        }
        else
        {
            // For now, regenerate (in production, load from storage)
            CurrentWorkoutPlan = _planGenerator.GeneratePlan(
                Profile.FitnessGoal,
                Profile.EquipmentAvailable,
                Profile.ExperienceLevel,
                Profile.WorkoutDaysPerWeek
            );
        }

        // Get today's workout
        int todayDayNumber = (int)DateTime.Today.DayOfWeek;
        if (todayDayNumber == 0) todayDayNumber = 7; // Sunday = 7

        TodaysWorkout = CurrentWorkoutPlan?.WorkoutDays.FirstOrDefault(d => d.DayNumber == todayDayNumber);

        if (TodaysWorkout != null)
        {
            if (TodaysWorkout.IsRestDay)
            {
                TodaysWorkoutSummary = "Počitek";
            }
            else
            {
                TodaysWorkoutSummary = $"{TodaysWorkout.Focus} • {TodaysWorkout.Exercises.Count} vaj";
            }
        }
        else
        {
            TodaysWorkoutSummary = "Ni programa";
        }
    }

    [RelayCommand]
    private async Task StartWorkout()
    {
        await Shell.Current.GoToAsync("//WorkoutPage");
    }

    [RelayCommand]
    private async Task StartRestTimer()
    {
        string input = await Application.Current.MainPage.DisplayPromptAsync(
            "Časovnik počitka",
            "Vnesite čas počitka (sekunde):",
            keyboard: Keyboard.Numeric,
            placeholder: "60");

        if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int seconds))
            return;

        RestTimeSeconds = seconds;
        IsRestTimerRunning = true;

        _restTimer?.Stop();
        _restTimer = new System.Timers.Timer(1000);
        _restTimer.Elapsed += (s, e) =>
        {
            RestTimeSeconds--;
            int minutes = RestTimeSeconds / 60;
            int secs = RestTimeSeconds % 60;
            RestTimerDisplay = $"{minutes:D2}:{secs:D2}";

            if (RestTimeSeconds <= 0)
            {
                _restTimer?.Stop();
                IsRestTimerRunning = false;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Počitek končan",
                        "Čas za naslednji set!",
                        "V redu");
                });
            }
        };
        _restTimer.Start();
    }

    [RelayCommand]
    private void StopRestTimer()
    {
        _restTimer?.Stop();
        IsRestTimerRunning = false;
        RestTimeSeconds = 0;
        RestTimerDisplay = "00:00";
    }

    [RelayCommand]
    private async Task LogWellness()
    {
        string sleepInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Beleženje spanca",
            "Koliko ur ste spali?",
            keyboard: Keyboard.Numeric,
            placeholder: "8");

        if (string.IsNullOrWhiteSpace(sleepInput))
            return;

        string moodInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Ocena počutja",
            "Kako se počutite? (1-5, kjer je 5 odlično)",
            keyboard: Keyboard.Numeric,
            placeholder: "5");

        if (string.IsNullOrWhiteSpace(moodInput))
            return;

        string energyInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Ocena energije",
            "Kakšna je vaša raven energije? (1-5)",
            keyboard: Keyboard.Numeric,
            placeholder: "5");

        if (string.IsNullOrWhiteSpace(energyInput))
            return;

        if (double.TryParse(sleepInput, out double sleepHours) &&
            int.TryParse(moodInput, out int mood) &&
            int.TryParse(energyInput, out int energy))
        {
            // Save to database
            var sleepLog = new SleepLog
            {
                UserId = _currentUserId,
                SleepDate = DateTime.Today,
                SleepDurationHours = sleepHours,
                SleepQuality = Math.Clamp(mood, 1, 5)
            };
            await _databaseService.SaveSleepLogAsync(sleepLog);

            var recoveryLog = new RecoveryLog
            {
                UserId = _currentUserId,
                LogDate = DateTime.Today,
                EnergyLevel = Math.Clamp(energy, 1, 5),
                MuscleSorenessLevel = 3,
                StressLevel = 3
            };
            await _databaseService.SaveRecoveryLogAsync(recoveryLog);

            // Update UI
            var entry = new WellnessEntry
            {
                Date = DateTime.Now,
                SleepHours = sleepHours,
                MoodRating = Math.Clamp(mood, 1, 5),
                EnergyLevel = Math.Clamp(energy, 1, 5)
            };

            Profile.WellnessHistory.RemoveAll(w => w.Date.Date == DateTime.Today);
            Profile.WellnessHistory.Add(entry);
            _dataService.SaveProfile(Profile);
            TodaysWellness = entry;

            await Application.Current.MainPage.DisplayAlert(
                "Uspešno",
                "Počutje je bilo zabeleženo.",
                "V redu");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                "Napaka",
                "Prosim vnesite veljavne številke.",
                "V redu");
        }
    }

    [RelayCommand]
    private async Task ViewWorkoutPlan()
    {
        if (CurrentWorkoutPlan == null)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Ni programa",
                "Nastavi svoj cilj in opremo v profilu za personaliziran program.",
                "V redu");
            return;
        }

        await Shell.Current.GoToAsync(nameof(Views.WorkoutPlansPage));
    }

    [RelayCommand]
    private async Task ViewWorkoutHistory()
    {
        await Shell.Current.GoToAsync(nameof(Views.WorkoutHistoryPage));
    }
}