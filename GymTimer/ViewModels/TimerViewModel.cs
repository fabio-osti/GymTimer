using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymTimer.Helpers;
using GymTimer.Models;
using GymTimer.Views;
using Plugin.Maui.Audio;
using System.Timers;

namespace GymTimer.ViewModels;

public partial class TimerViewModel : ObservableObject
{
	static Page MainPage => Application.Current.MainPage;

	public TimerViewModel(Ringer ringer, Settings appSettings)
	{
		_ringer = ringer;
		_appSettings = appSettings;

		timer = new System.Timers.Timer(1000);
		timer.Elapsed += (s, e) =>
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

	void PlaySound()
	{
		if (!AppSettings.PlaySounds) return;
		switch (TimerValue) {
			case > 0 and <= 3:
				_ringer.RingFinishingBell();
				break;
			case 0:
				_ringer.RingFinishedBell();
				break;
			default: break;
		}
	}

	readonly System.Timers.Timer timer;
	readonly Ringer _ringer;

	[ObservableProperty]
	Settings _appSettings;

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
		Resting && timer.Enabled ? "Rest" : "Set";

	[RelayCommand]
	public void ShowSettings()
	{
		Shell.Current.GoToAsync("Settings");
		//MainPage.Navigation.PushModalAsync(_settingsView);
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
