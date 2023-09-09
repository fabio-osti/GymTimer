using GymTimer.Models;
using Plugin.Maui.Audio;

namespace GymTimer.Helpers;

public class Ringer
{
	private readonly Settings _appSettings;
	private readonly IAudioManager audioManager;
	private readonly Task<IAudioPlayer> overPlayer;
	private readonly Task<IAudioPlayer> runningOutPlayer;

	public Ringer(Settings settings)
	{
		audioManager = AudioManager.Current;
		runningOutPlayer = LoadFile("runningout.mp3");
		overPlayer = LoadFile("over.mp3");
		_appSettings = settings;
	}

	private async Task<IAudioPlayer> LoadFile(string fileName) =>
		audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));

	public void RingFinishingBell()
	{
		if (!runningOutPlayer.IsCompleted || !_appSettings.PlaySounds) return;
		var player = runningOutPlayer.Result;
		player.Play();
		Task.Delay(500).ContinueWith(_ => player.Stop());
	}

	public void RingFinishedBell()
	{
		if (!overPlayer.IsCompleted || !_appSettings.PlaySounds) return;
		overPlayer.Result.Play();
	}
}
