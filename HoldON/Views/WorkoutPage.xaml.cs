using HoldON.ViewModels;
using HoldON.Services;

namespace HoldON.Views;

public partial class WorkoutPage : ContentPage
{
	public WorkoutPage(WorkoutViewModel viewModel, LanguageService languageService)
	{
		InitializeComponent();
        BindingContext = new
        {
            VM = viewModel,
            Lang = languageService
        };
	}
}