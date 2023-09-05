using CommunityToolkit.Mvvm.ComponentModel;

namespace GymTimer.Models
{
	public partial class Settings : ObservableObject
	{
		[ObservableProperty]
		bool autoStartSet = true;
		[ObservableProperty]
		bool playSounds = true;
		[ObservableProperty]
		bool showNotification = true;
	}
}
