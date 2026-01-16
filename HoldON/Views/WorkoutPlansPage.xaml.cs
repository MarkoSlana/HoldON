using HoldON.ViewModels;

namespace HoldON.Views;

public partial class WorkoutPlansPage : ContentPage
{
    private readonly WorkoutPlansViewModel _viewModel;

    public WorkoutPlansPage(WorkoutPlansViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadPlan();
    }
}
