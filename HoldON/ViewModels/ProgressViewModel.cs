using HoldON.Services;
using Microcharts;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public partial class ProgressViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private readonly DatabaseService _databaseService;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    [ObservableProperty]
    private Chart? volumeChart;

    [ObservableProperty]
    private Chart? exerciseChart;

    [ObservableProperty]
    private int selectedViewIndex = 0;

    [ObservableProperty]
    private string selectedExercise = "Bench press";

    public ProgressViewModel(DataService dataService, DatabaseService databaseService)
    {
        _dataService = dataService;
        _databaseService = databaseService;
        Title = "Napredek";
    }

    [RelayCommand]
    private void SelectView(string viewName)
    {
        SelectedViewIndex = viewName switch
        {
            "Pregled" => 0,
            "Vaje" => 1,
            "Moč" => 2,
            _ => 0
        };
    }

    [RelayCommand]
    private void SelectExercise(string exercise)
    {
        SelectedExercise = exercise;
        LoadExerciseChart();
    }

    private void LoadChartData()
    {
        try
        {
            var entries = new[]
            {
                new ChartEntry(15000) { Label = "Pon", ValueLabel = "15k", Color = SKColor.Parse("#2F81F7") },
                new ChartEntry(18000) { Label = "Tor", ValueLabel = "18k", Color = SKColor.Parse("#2F81F7") },
                new ChartEntry(12000) { Label = "Sre", ValueLabel = "12k", Color = SKColor.Parse("#2F81F7") },
                new ChartEntry(20000) { Label = "Čet", ValueLabel = "20k", Color = SKColor.Parse("#2F81F7") },
                new ChartEntry(17000) { Label = "Pet", ValueLabel = "17k", Color = SKColor.Parse("#2F81F7") },
            };

            VolumeChart = new BarChart
            {
                Entries = entries,
                LabelTextSize = 30,
                BackgroundColor = SKColors.Transparent,
                Margin = 20
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading chart: {ex.Message}");
        }
    }

    private void LoadExerciseChart()
    {
        try
        {
            var entries = new[]
            {
                new ChartEntry(70) { Label = "Jan", ValueLabel = "70", Color = SKColor.Parse("#3B82F6") },
                new ChartEntry(75) { Label = "Feb", ValueLabel = "75", Color = SKColor.Parse("#3B82F6") },
                new ChartEntry(82) { Label = "Mar", ValueLabel = "82", Color = SKColor.Parse("#3B82F6") },
                new ChartEntry(88) { Label = "Apr", ValueLabel = "88", Color = SKColor.Parse("#3B82F6") },
                new ChartEntry(92) { Label = "Maj", ValueLabel = "92", Color = SKColor.Parse("#3B82F6") },
                new ChartEntry(95) { Label = "Jun", ValueLabel = "95", Color = SKColor.Parse("#3B82F6") },
            };

            ExerciseChart = new LineChart
            {
                Entries = entries,
                LabelTextSize = 30,
                BackgroundColor = SKColors.Transparent,
                Margin = 20,
                LineMode = LineMode.Straight,
                PointMode = PointMode.Circle,
                PointSize = 15
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading exercise chart: {ex.Message}");
        }
    }

    public async void InitializeChart()
    {
        if (VolumeChart == null)
        {
            await LoadChartDataFromDatabaseAsync();
        }
        if (ExerciseChart == null)
        {
            LoadExerciseChart();
        }
    }

    private async Task LoadChartDataFromDatabaseAsync()
    {
        try
        {
            // Load workout sessions from database
            var sessions = await _databaseService.GetUserWorkoutSessionsAsync(_currentUserId);

            if (sessions.Count == 0)
            {
                LoadChartData(); // Use default data if no sessions
                return;
            }

            // Calculate volume per session (last 7 days)
            var lastWeekSessions = sessions
                .Where(s => s.SessionDate >= DateTime.Now.AddDays(-7))
                .OrderBy(s => s.SessionDate)
                .ToList();

            var entries = new List<ChartEntry>();
            foreach (var session in lastWeekSessions)
            {
                var sets = await _databaseService.GetSessionSetsAsync(session.SessionId);
                var totalVolume = sets.Sum(s => (s.WeightKg ?? 0) * (s.Reps ?? 0));

                entries.Add(new ChartEntry((float)totalVolume)
                {
                    Label = session.SessionDate.ToString("ddd"),
                    ValueLabel = $"{totalVolume:N0}",
                    Color = SKColor.Parse("#2F81F7")
                });
            }

            if (entries.Any())
            {
                VolumeChart = new BarChart
                {
                    Entries = entries,
                    LabelTextSize = 30,
                    BackgroundColor = SKColors.Transparent,
                    Margin = 20
                };
            }
            else
            {
                LoadChartData(); // Fallback to default
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading chart from DB: {ex.Message}");
            LoadChartData(); // Fallback to default
        }
    }
}