using CommunityToolkit.Mvvm.ComponentModel;

namespace GymTimer.Services;

public sealed partial class Settings : ObservableObject
{
    [ObservableProperty]
    private bool _autoStartSet = Preferences.Default.Get(nameof(AutoStartSet), true);
    partial void OnAutoStartSetChanged(bool value)
    {
        Preferences.Default.Set(nameof(AutoStartSet), value);
    }

    [ObservableProperty]
    private bool _playSounds = Preferences.Default.Get(nameof(PlaySounds), true);
    partial void OnPlaySoundsChanged(bool value)
    {
        Preferences.Default.Set(nameof(PlaySounds), value);
    }

    [ObservableProperty]
    private bool _showNotification = Preferences.Default.Get(nameof(ShowNotification), true);
    partial void OnShowNotificationChanged(bool value)
    {
        Preferences.Default.Set(nameof(ShowNotification), value);
    }

    [ObservableProperty]
    private int _runningOutThreshold = Preferences.Default.Get(nameof(RunningOutThreshold), 3);

    partial void OnRunningOutThresholdChanged(int value)
    {
        Preferences.Default.Set(nameof(RunningOutThreshold), value);
    }
}
