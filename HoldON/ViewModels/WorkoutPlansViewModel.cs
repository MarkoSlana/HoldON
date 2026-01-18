using HoldON.Models;
using HoldON.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace HoldON.ViewModels;

public partial class WorkoutPlansViewModel : BaseViewModel
{
    private readonly WorkoutPlanGenerator _planGenerator;
    private readonly DataService _dataService;

    [ObservableProperty]
    private WorkoutPlan? currentPlan;

    [ObservableProperty]
    private ObservableCollection<WorkoutDay> workoutDays = new();

    public WorkoutPlansViewModel(WorkoutPlanGenerator planGenerator, DataService dataService)
    {
        _planGenerator = planGenerator;
        _dataService = dataService;
        Title = "Moj program";
    }

    public void LoadPlan()
    {
        var profile = _dataService.GetProfile();
        CurrentPlan = _planGenerator.GeneratePlan(
            profile.FitnessGoal,
            profile.EquipmentAvailable,
            profile.ExperienceLevel,
            profile.WorkoutDaysPerWeek
        );

        if (CurrentPlan != null)
        {
            WorkoutDays = new ObservableCollection<WorkoutDay>(CurrentPlan.WorkoutDays);
        }
    }

    [RelayCommand]
    async Task OpenExerciseVideo(PlannedExercise exercise)
    {
        if (string.IsNullOrEmpty(exercise.YouTubeUrl))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Video ni na voljo",
                $"Za vajo '{exercise.Name}' video trenutno ni na voljo.",
                "V redu");
            return;
        }

        try
        {
            await Browser.Default.OpenAsync(exercise.YouTubeUrl, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Napaka",
                $"Napaka pri odpiranju videa: {ex.Message}",
                "V redu");
        }
    }
}
