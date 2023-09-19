using Plugin.LocalNotification;

namespace GymTimer.Services;

public sealed class Notifier
{
    private Task<bool> _notificationPermission;
    private readonly Settings _settings;

    public Notifier(Settings settings)
    {
        _settings = settings;
    }

    public bool IsAppForeground { get; set; }

    public async Task RequestPermission()
    {
        if (_notificationPermission is not null) return;
        var notificationEnabled = LocalNotificationCenter.Current.AreNotificationsEnabled();
        if (await notificationEnabled) {
            _notificationPermission = notificationEnabled;
        } else {
            _notificationPermission = LocalNotificationCenter.Current.RequestNotificationPermission();
        }
    }

    public async void NotifyRestIsOver()
    {
        if (!IsAppForeground) return;
        if (!_settings.ShowNotification) return;
        if (!await _notificationPermission) return;

        await new NotificationRequest {
            Title = "Rest Over",
            Description = "The rest is now over and the set has begun.",
            Silent = _settings.PlaySounds // So the app sound and the notification sound don't overlap 
        }.Show();
    }
}
