using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class WorkoutPlansPage : ContentPage
{
    private readonly WorkoutPlansViewModel _viewModel;

    public WorkoutPlansPage(WorkoutPlansViewModel viewModel, LanguageService languageService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = new
        {
            VM = viewModel,
            Lang = languageService
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadPlan();
    }
}
