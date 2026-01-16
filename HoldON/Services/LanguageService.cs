using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HoldON.Services;

public class LanguageService : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;
    private string _currentLanguage = "sl"; // Default to Slovenian

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? LanguageChangedEvent;

    public LanguageService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadLanguagePreference();
    }

    public string CurrentLanguage => _currentLanguage;

    public bool IsSlovenian => _currentLanguage == "sl";
    public bool IsEnglish => _currentLanguage == "en";

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void NotifyAllPropertiesChanged()
    {
        OnPropertyChanged(nameof(CurrentLanguage));
        OnPropertyChanged(nameof(IsSlovenian));
        OnPropertyChanged(nameof(IsEnglish));

        // Notify all translation properties
        OnPropertyChanged(nameof(HomeTab));
        OnPropertyChanged(nameof(Workout));
        OnPropertyChanged(nameof(Progress));
        OnPropertyChanged(nameof(Community));
        OnPropertyChanged(nameof(Profile));
        OnPropertyChanged(nameof(Save));
        OnPropertyChanged(nameof(Cancel));
        OnPropertyChanged(nameof(Success));
        OnPropertyChanged(nameof(Error));
        OnPropertyChanged(nameof(Settings));
        OnPropertyChanged(nameof(Language));
        OnPropertyChanged(nameof(Slovenian));
        OnPropertyChanged(nameof(English));
        OnPropertyChanged(nameof(Exercise));
        OnPropertyChanged(nameof(Sets));
        OnPropertyChanged(nameof(Reps));
        OnPropertyChanged(nameof(Weight));
        OnPropertyChanged(nameof(RestTime));
        OnPropertyChanged(nameof(AddExercise));
        OnPropertyChanged(nameof(AddSet));
        OnPropertyChanged(nameof(StartWorkout));
        OnPropertyChanged(nameof(FinishWorkout));
        OnPropertyChanged(nameof(Nutrition));
        OnPropertyChanged(nameof(Calories));
        OnPropertyChanged(nameof(Protein));
        OnPropertyChanged(nameof(Carbs));
        OnPropertyChanged(nameof(Fats));
        OnPropertyChanged(nameof(Goals));
        OnPropertyChanged(nameof(MuscleGain));
        OnPropertyChanged(nameof(WeightLoss));
        OnPropertyChanged(nameof(Strength));
        OnPropertyChanged(nameof(Endurance));
        OnPropertyChanged(nameof(Equipment));
        OnPropertyChanged(nameof(GymEquipment));
        OnPropertyChanged(nameof(HomeEquipment));
        OnPropertyChanged(nameof(Minimal));
        OnPropertyChanged(nameof(Beginner));
        OnPropertyChanged(nameof(Intermediate));
        OnPropertyChanged(nameof(Advanced));
        OnPropertyChanged(nameof(Friends));
        OnPropertyChanged(nameof(Leaderboard));
        OnPropertyChanged(nameof(AddFriend));
        OnPropertyChanged(nameof(Share));
        OnPropertyChanged(nameof(Sleep));
        OnPropertyChanged(nameof(Recovery));
        OnPropertyChanged(nameof(HoursSlept));
        OnPropertyChanged(nameof(Mood));
        OnPropertyChanged(nameof(EnergyLevel));
        OnPropertyChanged(nameof(Achievements));
        OnPropertyChanged(nameof(Badges));
        OnPropertyChanged(nameof(PersonalRecord));
        OnPropertyChanged(nameof(NewPR));
        OnPropertyChanged(nameof(Today));
        OnPropertyChanged(nameof(ThisWeek));
        OnPropertyChanged(nameof(ThisMonth));
        OnPropertyChanged(nameof(AllTime));
        OnPropertyChanged(nameof(Monday));
        OnPropertyChanged(nameof(Tuesday));
        OnPropertyChanged(nameof(Wednesday));
        OnPropertyChanged(nameof(Thursday));
        OnPropertyChanged(nameof(Friday));
        OnPropertyChanged(nameof(Saturday));
        OnPropertyChanged(nameof(Sunday));
        OnPropertyChanged(nameof(WorkoutSaved));
        OnPropertyChanged(nameof(ErrorSavingWorkout));
        OnPropertyChanged(nameof(PleaseEnterValidNumber));
        OnPropertyChanged(nameof(RestartAppForChanges));

        // Trigger event for other components
        LanguageChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    private async void LoadLanguagePreference()
    {
        try
        {
            var user = await _databaseService.GetUserAsync(1); // TODO: Use actual user ID
            if (user != null)
            {
                _currentLanguage = user.PreferredLanguage;
                SetCulture(_currentLanguage);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading language: {ex.Message}");
        }
    }

    public async Task SetLanguageAsync(string languageCode)
    {
        if (languageCode != "sl" && languageCode != "en")
            return;

        _currentLanguage = languageCode;
        SetCulture(languageCode);

        // Notify all UI elements that language has changed
        NotifyAllPropertiesChanged();

        // Save to database
        try
        {
            var user = await _databaseService.GetUserAsync(1); // TODO: Use actual user ID
            if (user != null)
            {
                user.PreferredLanguage = languageCode;
                await _databaseService.SaveUserAsync(user);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving language: {ex.Message}");
        }
    }

    private void SetCulture(string languageCode)
    {
        var culture = new CultureInfo(languageCode == "en" ? "en-US" : "sl-SI");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    // Localization helpers
    public string GetString(string slovenianText, string englishText)
    {
        return _currentLanguage == "en" ? englishText : slovenianText;
    }

    // Common translations
    public string HomeTab => GetString("Domov", "Home");
    public string Workout => GetString("Vadba", "Workout");
    public string Progress => GetString("Napredek", "Progress");
    public string Community => GetString("Skupnost", "Community");
    public string Profile => GetString("Profil", "Profile");
    public string Save => GetString("Shrani", "Save");
    public string Cancel => GetString("Prekliči", "Cancel");
    public string Success => GetString("Uspešno", "Success");
    public string Error => GetString("Napaka", "Error");
    public string Settings => GetString("Nastavitve", "Settings");
    public string Language => GetString("Jezik", "Language");
    public string Slovenian => GetString("Slovenščina", "Slovenian");
    public string English => GetString("Angleščina", "English");

    // Workout related
    public string Exercise => GetString("Vaja", "Exercise");
    public string Sets => GetString("Serije", "Sets");
    public string Reps => GetString("Ponovitve", "Reps");
    public string Weight => GetString("Teža", "Weight");
    public string RestTime => GetString("Počitek", "Rest Time");
    public string AddExercise => GetString("Dodaj vajo", "Add Exercise");
    public string AddSet => GetString("Dodaj serijo", "Add Set");
    public string StartWorkout => GetString("Začni vadbo", "Start Workout");
    public string FinishWorkout => GetString("Končaj vadbo", "Finish Workout");

    // Nutrition
    public string Nutrition => GetString("Prehrana", "Nutrition");
    public string Calories => GetString("Kalorije", "Calories");
    public string Protein => GetString("Beljakovine", "Protein");
    public string Carbs => GetString("Ogljikovi hidrati", "Carbs");
    public string Fats => GetString("Maščobe", "Fats");

    // Goals
    public string Goals => GetString("Cilji", "Goals");
    public string MuscleGain => GetString("Mišična masa", "Muscle Gain");
    public string WeightLoss => GetString("Hujšanje", "Weight Loss");
    public string Strength => GetString("Moč", "Strength");
    public string Endurance => GetString("Kondicija", "Endurance");

    // Equipment
    public string Equipment => GetString("Oprema", "Equipment");
    public string GymEquipment => GetString("Telovadnica", "Gym");
    public string HomeEquipment => GetString("Doma", "Home");
    public string Minimal => GetString("Minimalno", "Minimal");

    // Experience levels
    public string Beginner => GetString("Začetnik", "Beginner");
    public string Intermediate => GetString("Napredni", "Intermediate");
    public string Advanced => GetString("Strokovnjak", "Advanced");

    // Social
    public string Friends => GetString("Prijatelji", "Friends");
    public string Leaderboard => GetString("Lestvica", "Leaderboard");
    public string AddFriend => GetString("Dodaj prijatelja", "Add Friend");
    public string Share => GetString("Deli", "Share");

    // Recovery
    public string Sleep => GetString("Spanec", "Sleep");
    public string Recovery => GetString("Regeneracija", "Recovery");
    public string HoursSlept => GetString("Ur spanca", "Hours Slept");
    public string Mood => GetString("Počutje", "Mood");
    public string EnergyLevel => GetString("Raven energije", "Energy Level");

    // Achievements
    public string Achievements => GetString("Dosežki", "Achievements");
    public string Badges => GetString("Značke", "Badges");
    public string PersonalRecord => GetString("Osebni rekord", "Personal Record");
    public string NewPR => GetString("Nov PR!", "New PR!");

    // Time periods
    public string Today => GetString("Danes", "Today");
    public string ThisWeek => GetString("Ta teden", "This Week");
    public string ThisMonth => GetString("Ta mesec", "This Month");
    public string AllTime => GetString("Vedno", "All Time");

    // Days of week
    public string Monday => GetString("Ponedeljek", "Monday");
    public string Tuesday => GetString("Torek", "Tuesday");
    public string Wednesday => GetString("Sreda", "Wednesday");
    public string Thursday => GetString("Četrtek", "Thursday");
    public string Friday => GetString("Petek", "Friday");
    public string Saturday => GetString("Sobota", "Saturday");
    public string Sunday => GetString("Nedelja", "Sunday");

    // Messages
    public string WorkoutSaved => GetString("Vadba shranjena!", "Workout saved!");
    public string ErrorSavingWorkout => GetString("Napaka pri shranjevanju vadbe", "Error saving workout");
    public string PleaseEnterValidNumber => GetString("Prosim vnesite veljavno številko", "Please enter a valid number");
    public string LanguageChangedMessage => GetString("Jezik spremenjen", "Language changed");
    public string RestartAppForChanges => GetString("Ponovno zaženite aplikacijo za popoln učinek", "Restart app for full effect");
}
