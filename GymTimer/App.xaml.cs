using Chronometer = GymTimer.Services.Chronometer;

namespace GymTimer;

public partial class App : Application
{
    private readonly Notifier _notifier;

    public App(Chronometer chronometer, Ringer ringer, Notifier notifier)
    {
        InitializeComponent();

        _notifier = notifier;

        chronometer.OnFinishing += ringer.RingFinishingBell;
        chronometer.OnFinished += ringer.RingFinishedBell;

        chronometer.OnFinishing += _notifier.NotifyRestIsFinishing;
        chronometer.OnFinished += _notifier.NotifyRestIsFinished;

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
