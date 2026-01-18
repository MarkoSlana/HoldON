using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class WorkoutHistoryPage : ContentPage
{
	private readonly WorkoutHistoryViewModel _viewModel;

	public WorkoutHistoryPage(WorkoutHistoryViewModel viewModel, LanguageService languageService)
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
		_viewModel.LoadWorkoutHistory();
	}
}
