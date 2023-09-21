using Plugin.LocalNotification;

namespace GymTimer.Services;

public sealed class Notifier
{
    private readonly Settings _settings;

    private int _restId;
    private bool _isAppForeground;
    private Task<bool> _notificationPermission;
    private NotificationRequest _request;

    public Notifier(Settings settings)
    {
        _settings = settings;
    }

    public bool IsAppForeground
    {
        get => _isAppForeground;
        set {
            _isAppForeground = value;
            if (!value) _request?.Cancel();
        }
    }

    public async Task RequestPermission()
    {
        if (_notificationPermission is not null) return;

        var notificationEnabled = LocalNotificationCenter.Current.AreNotificationsEnabled();
        _notificationPermission =
            await notificationEnabled
                ? notificationEnabled
                : LocalNotificationCenter.Current.RequestNotificationPermission();
    }

    public void NotifyRestIsFinished()
    {
        _request.Title = "Rest is Over";
        _request.Description = _settings.AutoStartSet ?
            "The rest is over, a new set have begun."
            : "The rest is over, begin a new set.";
        _request.Show();
        
        _restId++;
    }

    public async void NotifyRestIsFinishing()
    {
        if (_request?.NotificationId == _restId) return;
        if (!IsAppForeground) return;
        if (!_settings.ShowNotification) return;
        if (!await _notificationPermission) return;

        _request = new NotificationRequest {
            NotificationId = _restId,
            Title = "Rest is Almost Over",
            Description = "Get ready for your new set.",
            Silent = _settings.PlaySounds // So the app sound and the notification sound don't overlap 
        };
        await _request.Show();
    }
}
