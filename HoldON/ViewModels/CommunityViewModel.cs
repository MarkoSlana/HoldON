using HoldON.Models;
using HoldON.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public class Friend
{
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string WorkoutsThisWeek { get; set; } = string.Empty;
    public string BestLift { get; set; } = string.Empty;
    public bool IsTopPerformer { get; set; }
}

public class LeaderboardEntry
{
    public int Rank { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;
    public bool IsCurrentUser { get; set; }
    public bool ShowRankBadge { get; set; }
    public string RankBadge { get; set; } = string.Empty;
    public bool HasTrend { get; set; }
}

public class FriendActivity
{
    public string Name { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string Activity { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}

public partial class CommunityViewModel : BaseViewModel
{
    private readonly DataService _dataService;
    private readonly DatabaseService _databaseService;
    private int _currentUserId = 1; // TODO: Replace with actual user authentication

    [ObservableProperty]
    private ObservableCollection<Friend> friends = new();

    [ObservableProperty]
    private ObservableCollection<LeaderboardEntry> leaderboard = new();

    [ObservableProperty]
    private ObservableCollection<FriendActivity> activities = new();

    [ObservableProperty]
    private bool showFriends = true;

    [ObservableProperty]
    private bool showLeaderboard = false;

    [ObservableProperty]
    private string searchText = string.Empty;

    [ObservableProperty]
    private string selectedExercise = "Bench press";

    public CommunityViewModel(DataService dataService, DatabaseService databaseService)
    {
        _dataService = dataService;
        _databaseService = databaseService;
        Title = "Skupnost";
    }

    public async void LoadData()
    {
        await LoadFriendsFromDatabaseAsync();
        await LoadLeaderboardFromDatabaseAsync();
        await LoadActivitiesFromDatabaseAsync();
    }

    private async Task LoadFriendsFromDatabaseAsync()
    {
        try
        {
            var friendships = await _databaseService.GetUserFriendsAsync(_currentUserId);

            if (friendships.Count == 0)
            {
                LoadFriends(); // Fallback to default data
                return;
            }

            // Load friends from database (you would need to query user details)
            System.Diagnostics.Debug.WriteLine($"Loaded {friendships.Count} friends from database");
            LoadFriends(); // Fallback for now
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading friends from DB: {ex.Message}");
            LoadFriends();
        }
    }

    private async Task LoadLeaderboardFromDatabaseAsync()
    {
        try
        {
            // You would query leaderboard_entries table here
            System.Diagnostics.Debug.WriteLine("Loading leaderboard from database");
            LoadLeaderboard(); // Fallback for now
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading leaderboard from DB: {ex.Message}");
            LoadLeaderboard();
        }
    }

    private async Task LoadActivitiesFromDatabaseAsync()
    {
        try
        {
            var activities = await _databaseService.GetActivityFeedAsync(_currentUserId, 50);

            if (activities.Count == 0)
            {
                LoadActivities(); // Fallback to default data
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Loaded {activities.Count} activities from database");
            LoadActivities(); // Fallback for now
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading activities from DB: {ex.Message}");
            LoadActivities();
        }
    }

    private void LoadFriends()
    {
        try
        {
            Friends = new ObservableCollection<Friend>
            {
                new Friend { Name = "Ana Novak", Initials = "AN", WorkoutsThisWeek = "5 treningov ta teden", BestLift = "85 kg", IsTopPerformer = true },
                new Friend { Name = "Luka Horvat", Initials = "LH", WorkoutsThisWeek = "4 treningov ta teden", BestLift = "100 kg", IsTopPerformer = true },
                new Friend { Name = "Nina Kos", Initials = "NK", WorkoutsThisWeek = "6 treningov ta teden", BestLift = "60 kg", IsTopPerformer = true },
                new Friend { Name = "David Krajnc", Initials = "DK", WorkoutsThisWeek = "3 treningov ta teden", BestLift = "95 kg", IsTopPerformer = true }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading friends: {ex.Message}");
        }
    }

    private void LoadLeaderboard()
    {
        try
        {
            Leaderboard = new ObservableCollection<LeaderboardEntry>
            {
                new LeaderboardEntry { Rank = 1, Name = "Miha Strong", Initials = "MS", Weight = "150 kg", ShowRankBadge = true, RankBadge = "🥇", HasTrend = true },
                new LeaderboardEntry { Rank = 2, Name = "Peter Power", Initials = "PP", Weight = "145 kg", ShowRankBadge = true, RankBadge = "🥈", HasTrend = true },
                new LeaderboardEntry { Rank = 3, Name = "Marko", Initials = "M", Weight = "95 kg", IsCurrentUser = true, ShowRankBadge = true, RankBadge = "🥉", HasTrend = true },
                new LeaderboardEntry { Rank = 4, Name = "Jan Lift", Initials = "JL", Weight = "90 kg", ShowRankBadge = false, HasTrend = false }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading leaderboard: {ex.Message}");
        }
    }

    private void LoadActivities()
    {
        try
        {
            Activities = new ObservableCollection<FriendActivity>
            {
                new FriendActivity { Name = "Ana Novak", Initials = "AN", Activity = "dosegel novi PR", Detail = "Bench press - 85 kg", Time = "2h" },
                new FriendActivity { Name = "Luka Horvat", Initials = "LH", Activity = "dokončal", Detail = "Potisni dan", Time = "4h" },
                new FriendActivity { Name = "Nina Kos", Initials = "NK", Activity = "dosegel novi PR", Detail = "Počep - 80 kg", Time = "6h" },
                new FriendActivity { Name = "David Krajnc", Initials = "DK", Activity = "dokončal", Detail = "Vlečni dan", Time = "8h" }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading activities: {ex.Message}");
        }
    }

    [RelayCommand]
    private void ShowFriendsTab()
    {
        ShowFriends = true;
        ShowLeaderboard = false;
    }

    [RelayCommand]
    private void ShowLeaderboardTab()
    {
        ShowFriends = false;
        ShowLeaderboard = true;
    }

    [RelayCommand]
    private void SelectExercise(string exercise)
    {
        SelectedExercise = exercise;
        // Reload leaderboard based on selected exercise
        LoadLeaderboard();
    }

    [RelayCommand]
    private async Task AddFriend()
    {
        string email = await Application.Current.MainPage.DisplayPromptAsync(
            "Dodaj prijatelja",
            "Vnesite email naslov prijatelja:",
            keyboard: Keyboard.Email,
            placeholder: "prijatelj@email.com");

        if (string.IsNullOrWhiteSpace(email))
            return;

        // Validate email format (basic check)
        if (!email.Contains("@") || !email.Contains("."))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Napaka",
                "Prosim vnesite veljaven email naslov.",
                "V redu");
            return;
        }

        // Simulate adding friend
        string friendName = email.Split('@')[0];
        string initials = friendName.Length >= 2
            ? (friendName[0].ToString() + friendName[1].ToString()).ToUpper()
            : friendName.Substring(0, 1).ToUpper();

        var newFriend = new Friend
        {
            Name = char.ToUpper(friendName[0]) + friendName.Substring(1),
            Initials = initials,
            WorkoutsThisWeek = "0 treningov ta teden",
            BestLift = "0 kg",
            IsTopPerformer = false
        };

        Friends.Add(newFriend);

        await Application.Current.MainPage.DisplayAlert(
            "Uspešno",
            $"Prijatelj {newFriend.Name} je bil dodan!",
            "V redu");
    }
}