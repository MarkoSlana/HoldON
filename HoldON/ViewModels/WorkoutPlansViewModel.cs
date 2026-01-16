using HoldON.Models;
using HoldON.Services;
using CommunityToolkit.Mvvm.ComponentModel;
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
}
