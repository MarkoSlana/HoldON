using HoldON.Views;
using HoldON.Services;

namespace HoldON;

public partial class AppShell : Shell
{
    private readonly LanguageService _languageService;

    public AppShell(LanguageService languageService)
    {
        _languageService = languageService;
        BindingContext = _languageService;
        InitializeComponent();

        // Register routes
        Routing.RegisterRoute(nameof(WorkoutPlansPage), typeof(WorkoutPlansPage));
        Routing.RegisterRoute(nameof(WorkoutHistoryPage), typeof(WorkoutHistoryPage));
    }
}