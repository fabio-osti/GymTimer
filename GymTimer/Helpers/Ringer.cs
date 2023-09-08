using GymTimer.Models;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymTimer.Helpers
{
	public class Ringer
	{
		readonly Task<IAudioPlayer> runningOutPlayer;
		readonly Task<IAudioPlayer> overPlayer;
		readonly IAudioManager audioManager;
		readonly Settings _appSettings;

		public Ringer(Settings settings)
		{
			audioManager = AudioManager.Current;
			runningOutPlayer = LoadFile("runningout.mp3");
			overPlayer = LoadFile("over.mp3");
			_appSettings = settings;
		}

		private async Task<IAudioPlayer> LoadFile(string fileName) =>
			audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));

		public async void RingFinishingBellAsync()
		{
			if (_appSettings.PlaySounds) {
				var player = await runningOutPlayer;
				player.Play();
				await Task.Delay(500);
				player.Stop();
			}
		}

		public async void RingFinishedBellAsync()
		{
			if (_appSettings.PlaySounds) {
				(await overPlayer).Play();
			}
		}

		public void RingFinishingBell()
		{
			if (runningOutPlayer.IsCompleted && _appSettings.PlaySounds) {
				var player = runningOutPlayer.Result;
				player.Play();
				Task.Delay(500).ContinueWith((_) => player.Stop());
			}
		}

		public void RingFinishedBell()
		{
			if (overPlayer.IsCompleted && _appSettings.PlaySounds) {
				overPlayer.Result.Play();
			}
		}
	}
}
