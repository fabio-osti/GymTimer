using CommunityToolkit.Mvvm.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GymTimer.Services;

public sealed partial class Chronometer : ObservableRecipient
{
    public delegate void TimerEvent();

    private readonly Settings _settings;

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

    public Chronometer(Settings settings)
    {
        _settings = settings;

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
            if (_settings.AutoStartSet) {
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
        var events = TimerSeconds switch {
            > 0 when TimerSeconds <= _settings.RunningOutThreshold
                => OnFinishing?.GetInvocationList().Cast<TimerEvent>(),
            0 => OnFinished?.GetInvocationList().Cast<TimerEvent>(),
            _ => null
        };
        if (events is null) return;

        foreach (var @event in events) {
            new Task(@event.Invoke).Start();
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
