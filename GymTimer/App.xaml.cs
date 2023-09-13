namespace GymTimer;

public partial class App : Application
{
    private readonly Notifier _notifier;

    public App(Chronometer chronometer, Ringer ringer, Notifier notifier)
    {
        InitializeComponent();

        _notifier = notifier;

        chronometer.OnRunningOut += ringer.RingFinishingBell;
        chronometer.OnOver += ringer.RingFinishedBell;
        chronometer.OnOver += notifier.NotifyRestIsOver;

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Resumed += (_, _) => _notifier.IsAppForeground = false;

        window.Stopped += (_, _) => _notifier.IsAppForeground = true;

        return window;
    }
}
