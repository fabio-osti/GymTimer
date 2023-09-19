using GymTimer.Services;
using Chronometer = GymTimer.Services.Chronometer;

namespace GymTimer;

public partial class App : Application
{
    private readonly Notifier _notifier;

    public App(Chronometer chronometer, Ringer ringer, Notifier notifier)
    {
        InitializeComponent();

        chronometer.OnRunningOut += ringer.RingFinishingBell;
        chronometer.OnOver += ringer.RingFinishedBell;

        _notifier = notifier;
        chronometer.OnOver += notifier.NotifyRestIsOver;

        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        _ = _notifier.RequestPermission();
        base.OnStart();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Resumed += (_, _) => _notifier.IsAppForeground = false;
        window.Stopped += (_, _) => _notifier.IsAppForeground = true;

        return window;
    }
}
