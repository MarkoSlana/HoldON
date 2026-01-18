using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class CommunityPage : ContentPage
{
	private readonly CommunityViewModel _viewModel;

	public CommunityPage(CommunityViewModel viewModel, LanguageService languageService)
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
		_viewModel.LoadData();
	}
}