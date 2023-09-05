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
		readonly Task<IAudioPlayer> finishing;
		readonly Task<IAudioPlayer> finished;
		readonly IAudioManager audioManager;

		public Ringer()
		{
			audioManager = AudioManager.Current;
			finishing = LoadFile("bell.wav");
			finished = LoadFile("done.wav");
		}

		private async Task<IAudioPlayer> LoadFile(string fileName) => 
			audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));

		public async void RingFinishingBellAsync()
		{
			var player = await finishing;
			player.Play();
			await Task.Delay(500);
			player.Stop();
		}

		public async void RingFinishedBellAsync()
		{
			var player = await finished;
			player.Play();
		}

		public void RingFinishingBell()
		{
			if (finishing.IsCompleted) {
				var player = finishing.Result;
				player.Play();
				Task.Delay(500).ContinueWith((_) => player.Stop());
			}
		}

		public void RingFinishedBell()
		{
			if (finished.IsCompleted) {
				var player = finished.Result;
				player.Play();
			}
		}
	}
}
