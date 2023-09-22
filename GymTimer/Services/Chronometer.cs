using CommunityToolkit.Mvvm.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GymTimer.Services;

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
    private int _timerSeconds;

    public Chronometer(Settings appSettings)
    {
        _appSettings = appSettings;

        _timer = new Timer(1000);
        _timer.Elapsed += Tick;
    }

    public TimerEvent OnFinishing { get; set; }
    public TimerEvent OnFinished { get; set; }

    public bool IsResting => RestState && _timer.Enabled;

    public int RestDuration { get; set; } = 60;

    private void Tick(object o, ElapsedEventArgs e)
    {
        if (TimerSeconds <= 0 && RestState) {
            if (_appSettings.AutoStartSet) {
                BeginSet();
            } else {
                _timer.Stop();
                return;
            }
        }

        new Thread(Dispatch).Start();

        TimerSeconds--;
    }

    private void Dispatch()
    {
        switch (TimerSeconds) {
            case > 0 when TimerSeconds <= _appSettings.RunningOutThreshold:
                OnFinishing?.Invoke();
                break;
            case 0:
                OnFinished?.Invoke();
                break;
        }
    }

    public void BeginSet()
    {
        RestState = false;
        TimerSeconds = 0;
        _timer.Start();
    }

    public void BeginRest()
    {
        TimerSeconds = RestDuration;
        RestState = true;
        SetsCompleted++;
    }

    public void Reset()
    {
        _timer.Stop();
        TimerSeconds = 0;
        SetsCompleted = 0;
        RestState = true;
    }
}
