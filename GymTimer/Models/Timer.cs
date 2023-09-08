using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymTimer.Helpers;
using GymTimer.Models;
using GymTimer.Views;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System.Timers;

namespace GymTimer.Models;

public sealed partial class Chronometer : ObservableRecipient
{
	public delegate void TimerEvent();
	public TimerEvent OnRunningOut { get; set; }
	public TimerEvent OnOver { get; set; }

	public Chronometer(Settings appSettings)
	{
		_appSettings = appSettings;

		timer = new System.Timers.Timer(1000);
		timer.Elapsed += (_, _) => Tick();
		;
	}

	public void Tick()
	{
		if (TimerValue <= 0 && RestState) {
			if (_appSettings.AutoStartSet) {
				BeginSet();
			} else {
				timer.Stop();
				return;
			}
		}


		switch (TimerValue) {
			case > 0 and <= 3:
				OnRunningOut?.BeginInvoke(null, null);
				break;
			case 0:
				OnOver?.BeginInvoke(null, null);
				break;
			default: break;
		}

		TimerValue--;
	}

	readonly System.Timers.Timer timer;
	readonly Settings _appSettings;

	public bool IsResting => RestState && timer.Enabled;

	public int RestDuration { get; set; } = 60;

	[ObservableProperty]
	[NotifyPropertyChangedRecipients]
	bool restState = true;

	[ObservableProperty]
	[NotifyPropertyChangedRecipients]
	int timerValue;

	[ObservableProperty]
	int setsCompleted;

	public void BeginSet()
	{
		RestState = false;
		TimerValue = 0;
		timer.Start();
	}

	public void BeginRest()
	{
		TimerValue = RestDuration;
		RestState = true;
		SetsCompleted++;
	}

	public void Reset()
	{
		timer.Stop();
		TimerValue = 0;
		SetsCompleted = 0;
		RestState = true;
	}
}
