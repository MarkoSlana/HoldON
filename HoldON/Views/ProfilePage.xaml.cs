using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class ProfilePage : ContentPage
{
	private readonly ProfileViewModel _viewModel;
	private readonly LanguageService _languageService;

	public ProfilePage(ProfileViewModel viewModel, LanguageService languageService)
	{
		InitializeComponent();
		_viewModel = viewModel;
		_languageService = languageService;
        BindingContext = new
        {
            VM = viewModel,
            Lang = languageService
        };
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_viewModel.LoadProfile();
	}
}