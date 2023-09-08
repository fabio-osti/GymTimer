using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymTimer.Helpers;
using GymTimer.Models;
using GymTimer.Views;
using Plugin.Maui.Audio;

namespace GymTimer.ViewModels;

public sealed partial class TimerViewModel : ObservableObject
{
	static Page MainPage => Application.Current.MainPage;

	public TimerViewModel(Chronometer chrono, Ringer ringer)
	{
		Chrono = chrono;

		Chrono.PropertyChanged += (e, h) =>
		{
			OnPropertyChanged(nameof(TimerDisplay));
			OnPropertyChanged(nameof(TimerSize));
			OnPropertyChanged(nameof(TimerDescription));
		};

		Chrono.OnRunningOut += ringer.RingFinishingBell;
		Chrono.OnOver += ringer.RingFinishedBell;
	}


	public int TimerSize => Chrono.TimerValue >= 600 ? 92 : 78;

	public string TimerDisplay => TimeSpan.FromSeconds(Chrono.TimerValue).ToString(@"m\:ss");

	public string TimerDescription =>
		Chrono.IsResting ? "Rest" : "Set";
	
	[ObservableProperty]
	Chronometer _chrono;

	[ObservableProperty]
	bool refreshing;

	[RelayCommand]
	public static void ShowSettings()
	{
		Shell.Current.GoToAsync("Settings");
	}

	[RelayCommand]
	public void BeginSet()
	{
		Chrono.BeginSet();
	}

	[RelayCommand]
	public void BeginRest()
	{
		Chrono.BeginRest();
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
			Chrono.Reset();
		}
		Refreshing = false;
	}

	[RelayCommand]
	public async Task ShowCounterPrompt()
	{
		string rest = await MainPage.DisplayPromptAsync(
			"Completed Sets",
			"Enter the number of completed sets:",
			initialValue: Chrono.SetsCompleted.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		} else if (int.TryParse(rest, out int _setsCompleted)) {
			Chrono.SetsCompleted = _setsCompleted;
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
			initialValue: Chrono.RestDuration.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		} else if (int.TryParse(rest, out int _restDuration)) {
			Chrono.RestDuration = _restDuration;
		} else {
			await MainPage.DisplayAlert(
				"Invalid entry",
				"You must enter the number of seconds of your rest.",
				"OK"
			);
		}
	}
}
