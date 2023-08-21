using Plugin.Maui.Audio;

namespace GymTimer.Helpers
{
	public static class Resources
	{
		public static IAudioPlayer BellSound { get; set; }
		public static IAudioPlayer DoneSound { get; set; }

		public static async Task Init()
		{
			BellSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("bell.wav"));
			DoneSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("dell.wav"));
		}
	}
}
