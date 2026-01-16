using HoldON.Models;
using HoldON.Services;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HoldON.ViewModels;

public partial class ExerciseLibraryViewModel : BaseViewModel
{
    private readonly DataService _dataService;

    [ObservableProperty]
    private ObservableCollection<Exercise> exercises = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    public ExerciseLibraryViewModel(DataService dataService)
    {
        _dataService = dataService;
        Title = "Zbirka vaj";
        LoadExercises();
    }

    private void LoadExercises()
    {
        var library = _dataService.GetExerciseLibrary();
        Exercises = new ObservableCollection<Exercise>(library);
    }

    [RelayCommand]
    private void Search()
    {
        var library = _dataService.GetExerciseLibrary();
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            Exercises = new ObservableCollection<Exercise>(library);
        }
        else
        {
            var filtered = library.Where(e => e.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            Exercises = new ObservableCollection<Exercise>(filtered);
        }
    }
}