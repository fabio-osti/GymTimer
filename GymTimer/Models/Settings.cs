using CommunityToolkit.Mvvm.ComponentModel;

namespace GymTimer.Models
{
	public partial class Settings : ObservableObject
	{
		[ObservableProperty]
		public bool autoStartSet = true;
		[ObservableProperty]
		public bool playSounds = true;
		[ObservableProperty]
		public bool showNotification = true;
	}
}
