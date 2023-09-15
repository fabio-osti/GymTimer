using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;

namespace GymTimer.ViewModels;

[UsedImplicitly]
public sealed partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private Settings _appSettings;

    [ObservableProperty]
    private string _runningOutThreshold;

    [RelayCommand]
    private void ReturnThresholdEntry()
    {
        if (int.TryParse(RunningOutThreshold, out var threshold)) {
            AppSettings.RunningOutThreshold = threshold;
        } else {
            RunningOutThreshold = AppSettings.RunningOutThreshold.ToString();
        }
    }

    public SettingsViewModel(Settings settings)
    {
        AppSettings = settings;
        _runningOutThreshold = AppSettings.RunningOutThreshold.ToString();
    }
}
