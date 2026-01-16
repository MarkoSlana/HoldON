using HoldON.ViewModels;

namespace HoldON.Views;

public partial class WorkoutPage : ContentPage
{
	public WorkoutPage(WorkoutViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
	}
}