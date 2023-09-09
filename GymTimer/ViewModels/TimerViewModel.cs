using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymTimer.Helpers;
using GymTimer.Models;

namespace GymTimer.ViewModels;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed partial class TimerViewModel : ObservableObject
{
	[ObservableProperty] private Chronometer chrono;

	[ObservableProperty] private bool refreshing;

	public TimerViewModel(Chronometer chrono, Ringer ringer)
	{
		Chrono = chrono;

		Chrono.PropertyChanged += (_, _) =>
		{
			OnPropertyChanged(nameof(TimerDisplay));
			OnPropertyChanged(nameof(TimerSize));
			OnPropertyChanged(nameof(TimerDescription));
		};

		Chrono.OnRunningOut += ringer.RingFinishingBell;
		Chrono.OnOver += ringer.RingFinishedBell;
	}

	private static Page MainPage => Application.Current?.MainPage;


	public int TimerSize => Chrono.TimerValue >= 600 ? 92 : 78;

	public string TimerDisplay => TimeSpan.FromSeconds(Chrono.TimerValue).ToString(@"m\:ss");

	public string TimerDescription =>
		Chrono.IsResting ? "Rest" : "Set";

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
		var answer = await MainPage.DisplayAlert(
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
		var rest = await MainPage.DisplayPromptAsync(
			"Completed Sets",
			"Enter the number of completed sets:",
			initialValue: Chrono.SetsCompleted.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		}
		if (int.TryParse(rest, out var _setsCompleted)) {
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
		var rest = await MainPage.DisplayPromptAsync(
			"Rest Duration",
			"Enter the desired rest duration in seconds:",
			initialValue: Chrono.RestDuration.ToString()
		);
		if (string.IsNullOrEmpty(rest)) {
			return;
		}
		if (int.TryParse(rest, out var _restDuration)) {
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
