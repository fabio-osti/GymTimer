using CommunityToolkit.Mvvm.ComponentModel;

namespace GymTimer.Models;

public sealed partial class Settings : ObservableObject
{
	[ObservableProperty] private bool autoStartSet = true;
	[ObservableProperty] private bool playSounds = true;
	[ObservableProperty] private bool showNotification = true;
}
