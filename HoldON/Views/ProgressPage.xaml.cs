using HoldON.ViewModels;

namespace HoldON.Views;

public partial class ProgressPage : ContentPage
{
	private readonly ProgressViewModel _viewModel;

	public ProgressPage(ProgressViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
        BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.InitializeChart();
	}
}