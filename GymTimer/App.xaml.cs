using GymTimer.Services;
using Chronometer = GymTimer.Services.Chronometer;

namespace GymTimer;

public partial class App : Application
{
    private readonly Notifier _notifier;

    public App(Chronometer chronometer, Ringer ringer, Notifier notifier)
    {
        InitializeComponent();

        chronometer.OnFinishing += ringer.RingFinishingBell;
        chronometer.OnFinished += ringer.RingFinishedBell;

        _notifier = notifier;
        chronometer.OnFinishing += notifier.NotifyRestIsFinishing;
        chronometer.OnFinished += notifier.NotifyRestIsFinished;

        MainPage = new AppShell();
    }
    
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Created += (_, _) => _ = _notifier.RequestPermission();
        window.Resumed += (_, _) => _notifier.IsAppForeground = false;
        window.Stopped += (_, _) => _notifier.IsAppForeground = true;

        return window;
    }
}
