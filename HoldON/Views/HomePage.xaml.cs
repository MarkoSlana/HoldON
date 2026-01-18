using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel _viewModel;
	private readonly LanguageService _languageService;

	public HomePage(HomeViewModel viewModel, LanguageService languageService)
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
		_viewModel.LoadData();
	}
}