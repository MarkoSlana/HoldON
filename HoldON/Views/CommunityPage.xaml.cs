using HoldON.ViewModels;

namespace HoldON.Views;

public partial class CommunityPage : ContentPage
{
	private readonly CommunityViewModel _viewModel;

	public CommunityPage(CommunityViewModel viewModel)
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