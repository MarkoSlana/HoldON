namespace HoldON.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using HoldON.Models;
using System.Collections.ObjectModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private bool isBusy;
}