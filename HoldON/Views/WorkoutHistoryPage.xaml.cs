using HoldON.ViewModels;

namespace HoldON.Views;

public partial class WorkoutHistoryPage : ContentPage
{
	private readonly WorkoutHistoryViewModel _viewModel;

	public WorkoutHistoryPage(WorkoutHistoryViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadWorkoutHistory();
	}
}
