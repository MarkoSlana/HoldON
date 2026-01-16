using Microsoft.Extensions.DependencyInjection;
using HoldON.Services;

namespace HoldON;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        try
        {
            InitializeComponent();

            // Set up global exception handler
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"Unhandled exception: {e.ExceptionObject}");
            };

            // Initialize database
            InitializeDatabaseAsync(serviceProvider).ConfigureAwait(false);

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }
        catch (Exception ex)
        {
            var innerMsg = ex.InnerException?.Message ?? "No inner exception";
            var innerStack = ex.InnerException?.StackTrace ?? "No stack trace";

            System.Diagnostics.Debug.WriteLine($"Error in App constructor: {ex}");
            System.Diagnostics.Debug.WriteLine($"Inner exception: {innerMsg}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Diagnostics.Debug.WriteLine($"Inner stack: {innerStack}");

            // Create a simple error page with scrollable details
            MainPage = new ContentPage
            {
                Content = new ScrollView
                {
                    Content = new VerticalStackLayout
                    {
                        Padding = 20,
                        Children =
                        {
                            new Label { Text = "Error starting app:", FontSize = 20, FontAttributes = FontAttributes.Bold },
                            new Label { Text = ex.Message, Margin = new Thickness(0, 10) },
                            new Label { Text = "Inner exception:", FontSize = 16, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 20, 0, 0) },
                            new Label { Text = innerMsg, Margin = new Thickness(0, 10) },
                            new Label { Text = "Stack trace:", FontSize = 16, FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 20, 0, 0) },
                            new Label { Text = innerStack, FontSize = 10, Margin = new Thickness(0, 10) }
                        }
                    }
                }
            };
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(MainPage!);
    }

    private async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        try
        {
            var dbService = serviceProvider.GetRequiredService<DatabaseService>();

            // Seed initial data if needed
            await dbService.SeedInitialDataAsync();

            System.Diagnostics.Debug.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Database initialization error: {ex.Message}");
        }
    }
}