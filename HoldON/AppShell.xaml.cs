using HoldON.Views;

namespace HoldON;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes
        Routing.RegisterRoute(nameof(WorkoutPlansPage), typeof(WorkoutPlansPage));
    }
}