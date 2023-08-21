using CommunityToolkit.Mvvm.ComponentModel;
using GymTimer.Models;

namespace GymTimer.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
	public SettingsViewModel(Settings _settings)
	{
		AppSettings = _settings;
	}

	[ObservableProperty]
	Settings appSettings;

}
