using HoldON.ViewModels;

namespace HoldON.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel _viewModel;

	public HomePage(HomeViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
        BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadData();
	}
}