using CommunityToolkit.Mvvm.ComponentModel;

namespace GymTimer.Services;

public sealed partial class Settings : ObservableObject
{
    [ObservableProperty]
    private bool _autoStartSet = true;

    [ObservableProperty]
    private bool _playSounds = true;

    [ObservableProperty]
    private bool _showNotification = true;

    [ObservableProperty]
    private int _runningOutThreshold = 3;
}
