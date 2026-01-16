using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Microcharts.Maui;
using HoldON.Services;
using HoldON.ViewModels;
using HoldON.Views;

namespace HoldON;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMicrocharts()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<LanguageService>();
        builder.Services.AddSingleton<DataService>();
        builder.Services.AddSingleton<WorkoutPlanGenerator>();

        // Shell
        builder.Services.AddSingleton<AppShell>();

        // ViewModels
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<WorkoutViewModel>();
        builder.Services.AddSingleton<ProgressViewModel>();
        builder.Services.AddSingleton<CommunityViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddTransient<WorkoutPlansViewModel>();

        // Views
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<WorkoutPage>();
        builder.Services.AddSingleton<ProgressPage>();
        builder.Services.AddSingleton<CommunityPage>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddTransient<WorkoutPlansPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}