using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymTimer.Models;
using GymTimer.Views;
using Plugin.Maui.Audio;

namespace GymTimer.ViewModels;

public partial class TimerViewModel : ObservableObject
{
	static Page MainPage => Application.Current.MainPage;
	IAudioPlayer bell;
	IAudioPlayer done;
	IAudioManager audioManager;
	IDispatcherTimer timer;

	public TimerViewModel()
	{
		audioManager = AudioManager.Current;
		FileSystem.OpenAppPackageFileAsync("bell.wav").ContinueWith((e) => bell = audioManager.CreatePlayer(e.Result));
		FileSystem.OpenAppPackageFileAsync("done.wav").ContinueWith((e) => done = audioManager.CreatePlayer(e.Result));

		timer = MainPage.Dispatcher.CreateTimer();
		timer.Interval = TimeSpan.FromSeconds(1);
		timer.Tick += (s, e) =>
		{
			if (TimerValue <= 0 && Resting) {
				if (AppSettings.AutoStartSet) {
					BeginSet();
				} else {
					timer.Stop();
					return;
				}
			}
			TimerValue--;
			PlaySound();
		};
	}

	async void PlaySound()
	{
		if (!AppSettings.PlaySounds) return;
		switch (TimerValue) {
			case > 0 and <= 3:
				bell.Play();
				await Task.Delay(500);
				bell.Stop();
				break;
			case 0:
				done.Play();
				break;
			default: break;
		}
	}

	[ObservableProperty]
	Settings appSettings = new();	
	
	[ObservableProperty]
	int restDuration = 60;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(TimerDescription))]
	bool resting = true;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(TimerDisplay))]
	[NotifyPropertyChangedFor(nameof(TimerSize))]
	int timerValue;

	[ObservableProperty]
	int setsCompleted;

	[ObservableProperty]
	bool refreshing;

	public int TimerSize => TimerValue >= 600 ? 92 : 78;

	public string TimerDisplay => TimeSpan.FromSeconds(TimerValue).ToString(@"m\:ss");

	public string TimerDescription =>
		Resting && timer.IsRunning ? "Rest" : "Set";

	[RelayCommand]
	public void ShowSettings()
	{
		MainPage.Navigation.PushModalAsync(new ContentPage { Content = new SettingsView(AppSettings) });
	}

	[RelayCommand]
	public void BeginSet()
	{
		Resting = false;
		TimerValue = 0;
		timer.Start();
	}

	[RelayCommand]
	public void BeginRest()
	{
		TimerValue = RestDuration;
		Resting = true;
		SetsCompleted++;
	}

	[RelayCommand]
	public async Task Reset()
	{
		Refreshing = true;
		bool answer = await MainPage.DisplayAlert(
			"Reset?",
			"Would you like to reset the counter?",
			"Yes",
			"No"
		);
		if (answer) {
			timer.Stop();
			TimerValue = 0;
			SetsCompleted = 0;
			Resting = true;
		}
		Refreshing = false;
	}

	[RelayCommand]
	public async Task ShowCounterPrompt()
	{
		string rest = await MainPage.DisplayPromptAsync(
			"Completed Sets",
			"Enter the number of completed sets:",
			initialValue: SetsCompleted.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		} else if (int.TryParse(rest, out int _GymTimer)) {
			SetsCompleted = _GymTimer;
		} else {
			await MainPage.DisplayAlert(
				"Invalid entry",
				"You must enter the number of seconds of your rest.",
				"OK"
			);
		}
	}

	[RelayCommand]
	public async Task ShowTimerPrompt()
	{
		string rest = await MainPage.DisplayPromptAsync(
			"Rest Duration",
			"Enter the desired rest duration in seconds:",
			initialValue: RestDuration.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		} else if (int.TryParse(rest, out int _restDuration)) {
			RestDuration = _restDuration;
		} else {
			await MainPage.DisplayAlert(
				"Invalid entry",
				"You must enter the number of seconds of your rest.",
				"OK"
			);
		}
	}
}
