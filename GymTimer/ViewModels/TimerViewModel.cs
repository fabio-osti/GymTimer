using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JetBrains.Annotations;

namespace GymTimer.ViewModels;

[UsedImplicitly]
public sealed partial class TimerViewModel : ObservableObject
{
    [ObservableProperty]
    private Chronometer _chrono;

    [ObservableProperty]
    private bool _refreshing;

    public TimerViewModel(Chronometer chrono, Ringer ringer)
    {
        Chrono = chrono;

        Chrono.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(Chronometer.TimerValue):
                    OnPropertyChanged(nameof(TimerDisplay));
                    OnPropertyChanged(nameof(TimerSize));
                    break;
                case nameof(Chronometer.IsResting):
                    OnPropertyChanged(nameof(TimerDescription));
                    break;
            }
        };

        Chrono.OnRunningOut += ringer.RingFinishingBell;
        Chrono.OnOver += ringer.RingFinishedBell;
    }

    private static Page MainPage => Application.Current?.MainPage;

    public int TimerSize => Chrono.TimerValue >= 600 ? 92 : 78;

    public string TimerDisplay => TimeSpan.FromSeconds(Chrono.TimerValue).ToString(@"m\:ss");

    public string TimerDescription => Chrono.IsResting ? "Rest" : "Set";

    [RelayCommand]
    private static void ShowSettings()
    {
        Shell.Current.GoToAsync("Settings");
    }

    [RelayCommand]
    private void BeginSet()
    {
        Chrono.BeginSet();
    }

    [RelayCommand]
    private void BeginRest()
    {
        Chrono.BeginRest();
    }

    [RelayCommand]
    private async Task Reset()
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
    private async Task ShowCounterPrompt()
    {
        var rest = await MainPage.DisplayPromptAsync(
            "Completed Sets",
            "Enter the number of completed sets:",
            initialValue: Chrono.SetsCompleted.ToString()
        );
        if (string.IsNullOrEmpty(rest)) {
            return;
        }
        if (int.TryParse(rest, out var setsCompleted)) {
            Chrono.SetsCompleted = setsCompleted;
        } else {
            await MainPage.DisplayAlert(
                "Invalid entry",
                "You must enter the number of seconds of your rest.",
                "OK"
            );
        }
    }

    [RelayCommand]
    private async Task ShowTimerPrompt()
    {
        var rest = await MainPage.DisplayPromptAsync(
            "Rest Duration",
            "Enter the desired rest duration in seconds:",
            initialValue: Chrono.RestDuration.ToString()
        );
        if (string.IsNullOrEmpty(rest)) {
            return;
        }
        if (int.TryParse(rest, out var restDuration)) {
            Chrono.RestDuration = restDuration;
        } else {
            await MainPage.DisplayAlert(
                "Invalid entry",
                "You must enter the number of seconds of your rest.",
                "OK"
            );
        }
    }
}
