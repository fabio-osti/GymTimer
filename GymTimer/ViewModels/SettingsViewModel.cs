using CommunityToolkit.Mvvm.ComponentModel;
using GymTimer.Models;

namespace GymTimer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private Settings appSettings;

    public SettingsViewModel(Settings settings)
    {
        AppSettings = settings;
    }
}
