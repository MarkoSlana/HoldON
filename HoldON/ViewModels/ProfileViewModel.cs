using HoldON.Models;
using HoldON.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private readonly DatabaseService _databaseService;
    private readonly LanguageService _languageService;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    [ObservableProperty]
    private UserProfile profile = new();

    [ObservableProperty]
    private string currentLanguage = "Slovenščina";

    public ProfileViewModel(DataService dataService, DatabaseService databaseService, LanguageService languageService)
    {
        _dataService = dataService;
        _databaseService = databaseService;
        _languageService = languageService;
        Title = "Profil";
        LoadLanguagePreference();

        // Subscribe to language changes
        _languageService.LanguageChangedEvent += OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        CurrentLanguage = _languageService.CurrentLanguage == "en" ? "English" : "Slovenščina";
    }

    private async void LoadLanguagePreference()
    {
        try
        {
            var user = await _databaseService.GetUserAsync(_currentUserId);
            if (user != null)
            {
                CurrentLanguage = user.PreferredLanguage == "en" ? "English" : "Slovenščina";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading language preference: {ex.Message}");
        }
    }

    public void LoadProfile()
    {
        try
        {
            Profile = _dataService.GetProfile();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading profile: {ex.Message}");
            Profile = new UserProfile { Name = "Uporabnik" };
        }
    }

    [RelayCommand]
    private async Task AddNutritionAsync()
    {
        string caloriesInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Vnos kalorij",
            "Vnesite število kalorij:",
            keyboard: Keyboard.Numeric,
            placeholder: "0");

        if (string.IsNullOrWhiteSpace(caloriesInput))
            return;

        string proteinInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Vnos beljakovin",
            "Vnesite količino beljakovin (g):",
            keyboard: Keyboard.Numeric,
            placeholder: "0");

        if (string.IsNullOrWhiteSpace(proteinInput))
            return;

        string carbsInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Vnos ogljikovih hidratov",
            "Vnesite količino ogljikovih hidratov (g):",
            keyboard: Keyboard.Numeric,
            placeholder: "0");

        if (string.IsNullOrWhiteSpace(carbsInput))
            return;

        string fatsInput = await Application.Current.MainPage.DisplayPromptAsync(
            "Vnos maščob",
            "Vnesite količino maščob (g):",
            keyboard: Keyboard.Numeric,
            placeholder: "0");

        if (string.IsNullOrWhiteSpace(fatsInput))
            return;

        if (double.TryParse(caloriesInput, out double calories) &&
            double.TryParse(proteinInput, out double protein) &&
            double.TryParse(carbsInput, out double carbs) &&
            double.TryParse(fatsInput, out double fats))
        {
            // Save to database
            var nutritionLog = new NutritionLog
            {
                UserId = _currentUserId,
                LogDate = DateTime.Today,
                MealType = "snack",
                FoodId = 1, // Default food ID
                Servings = 1,
                TotalCalories = calories,
                TotalProteinG = protein,
                TotalCarbsG = carbs,
                TotalFatG = fats
            };
            await _databaseService.SaveNutritionLogAsync(nutritionLog);

            // Update UI
            Profile.CurrentCalories += calories;
            Profile.CurrentProtein += protein;
            Profile.CurrentCarbs += carbs;
            Profile.CurrentFats += fats;

            _dataService.SaveProfile(Profile);
            OnPropertyChanged(nameof(Profile));

            await Application.Current.MainPage.DisplayAlert(
                "Uspešno",
                "Prehrana je bila dodana.",
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
    private async Task SetFitnessGoal()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet(
            "Izberi svoj cilj",
            "Prekliči",
            null,
            "Mišična masa",
            "Hujšanje",
            "Moč",
            "Kondicija");

        if (!string.IsNullOrEmpty(result) && result != "Prekliči")
        {
            Profile.FitnessGoal = result;
            _dataService.SaveProfile(Profile);
            OnPropertyChanged(nameof(Profile));
        }
    }

    [RelayCommand]
    private async Task SetEquipment()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet(
            "Izberi razpoložljivo opremo",
            "Prekliči",
            null,
            "Gym",
            "Doma",
            "Minimalno");

        if (!string.IsNullOrEmpty(result) && result != "Prekliči")
        {
            Profile.EquipmentAvailable = result;
            _dataService.SaveProfile(Profile);
            OnPropertyChanged(nameof(Profile));
        }
    }

    [RelayCommand]
    private async Task SetExperienceLevel()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet(
            "Izberi svojo raven izkušenj",
            "Prekliči",
            null,
            "Začetnik",
            "Napredni");

        if (!string.IsNullOrEmpty(result) && result != "Prekliči")
        {
            Profile.ExperienceLevel = result;
            _dataService.SaveProfile(Profile);
            OnPropertyChanged(nameof(Profile));
        }
    }

    [RelayCommand]
    private async Task SetWorkoutDays()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet(
            "Koliko dni na teden boš treniral?",
            "Prekliči",
            null,
            "3 dni",
            "4 dni",
            "5 dni",
            "6 dni");

        if (!string.IsNullOrEmpty(result) && result != "Prekliči")
        {
            Profile.WorkoutDaysPerWeek = int.Parse(result.Split(' ')[0]);
            _dataService.SaveProfile(Profile);
            OnPropertyChanged(nameof(Profile));
        }
    }

    [RelayCommand]
    private async Task ChangeLanguage()
    {
        string result = await Application.Current.MainPage.DisplayActionSheet(
            "Izberi jezik / Select Language",
            "Prekliči / Cancel",
            null,
            "Slovenščina",
            "English");

        if (!string.IsNullOrEmpty(result) && result != "Prekliči / Cancel")
        {
            try
            {
                string languageCode = result == "English" ? "en" : "sl";

                // Get or create user
                var user = await _databaseService.GetUserAsync(_currentUserId);
                if (user == null)
                {
                    // Create default user if doesn't exist
                    user = new User
                    {
                        UserId = _currentUserId,
                        Username = "User",
                        Email = "user@holdon.app",
                        PasswordHash = "temp",
                        PreferredLanguage = languageCode
                    };
                    await _databaseService.SaveUserAsync(user);
                }

                // Use LanguageService to change language (this will notify all subscribers)
                await _languageService.SetLanguageAsync(languageCode);

                await Application.Current.MainPage.DisplayAlert(
                    result == "English" ? "Success" : "Uspešno",
                    result == "English"
                        ? "Language changed to English!"
                        : "Jezik spremenjen v slovenščino!",
                    "OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error changing language: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert(
                    "Napaka / Error",
                    "Napaka pri spreminjanju jezika. / Error changing language.",
                    "OK");
            }
        }
    }
}