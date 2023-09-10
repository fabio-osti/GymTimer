﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GymTimer.Models;

public sealed partial class Chronometer : ObservableRecipient
{
    public delegate void TimerEvent();

    private readonly Settings _appSettings;

    private readonly Timer _timer;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsResting))]
    [NotifyPropertyChangedRecipients]
    private bool _restState = true;

    [ObservableProperty]
    private int _setsCompleted;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private int _timerValue;

    public Chronometer(Settings appSettings)
    {
        _appSettings = appSettings;

        _timer = new Timer(1000);
        _timer.Elapsed += Tick;
    }

    public TimerEvent OnRunningOut { get; set; }
    public TimerEvent OnOver { get; set; }

    public bool IsResting => RestState && _timer.Enabled;

    public int RestDuration { get; set; } = 60;

    private void Tick(object o, ElapsedEventArgs e)
    {
        if (TimerValue <= 0 && RestState) {
            if (_appSettings.AutoStartSet) {
                BeginSet();
            } else {
                _timer.Stop();
                return;
            }
        }

        new Thread(
            () => {
                switch (TimerValue) {
                    case > 0 and <= 3:
                        OnRunningOut?.Invoke();
                        break;
                    case 0:
                        OnOver?.Invoke();
                        break;
                }
            }
        ).Start();

        TimerValue--;
    }

    public void BeginSet()
    {
        RestState = false;
        TimerValue = 0;
        _timer.Start();
    }

    public void BeginRest()
    {
        TimerValue = RestDuration;
        RestState = true;
        SetsCompleted++;
    }

    public void Reset()
    {
        _timer.Stop();
        TimerValue = 0;
        SetsCompleted = 0;
        RestState = true;
    }
}
