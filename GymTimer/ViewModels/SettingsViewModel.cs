using CommunityToolkit.Mvvm.ComponentModel;
using JetBrains.Annotations;

namespace GymTimer.ViewModels;

[UsedImplicitly]
public sealed partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private Settings _appSettings;

    public SettingsViewModel(Settings settings)
    {
        AppSettings = settings;
    }
}
