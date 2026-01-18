using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class ProgressPage : ContentPage
{
	private readonly ProgressViewModel _viewModel;

	public ProgressPage(ProgressViewModel viewModel, LanguageService languageService)
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
		_viewModel.InitializeChart();
	}
}