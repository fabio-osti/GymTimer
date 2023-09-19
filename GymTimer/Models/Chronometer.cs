using CommunityToolkit.Mvvm.ComponentModel;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GymTimer.Models;

public sealed partial class Chronometer : ObservableRecipient
{
    public delegate void TimerEvent();

    private readonly Settings appSettings;

    private readonly Timer timer;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsResting))]
    [NotifyPropertyChangedRecipients]
    private bool restState = true;

    [ObservableProperty]
    private int setsCompleted;

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private int timerValue;

    public Chronometer(Settings appSettings)
    {
        this.appSettings = appSettings;

        timer = new Timer(1000);
        timer.Elapsed += Tick;
    }

    public TimerEvent OnRunningOut { get; set; }
    public TimerEvent OnOver { get; set; }

    public bool IsResting => RestState && timer.Enabled;

    public int RestDuration { get; set; } = 60;

    private void Tick(object o, ElapsedEventArgs e)
    {
        if (TimerValue <= 0 && RestState) {
            if (appSettings.AutoStartSet) {
                BeginSet();
            } else {
                timer.Stop();
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
