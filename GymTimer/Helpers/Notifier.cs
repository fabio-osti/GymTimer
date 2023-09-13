using JetBrains.Annotations;
using Plugin.LocalNotification;

namespace GymTimer.Helpers;

public sealed class Notifier
{
    private readonly Task<bool> _notificationRequest = LocalNotificationCenter.Current.RequestNotificationPermission();
    private readonly Settings _settings;
    
    public Notifier(Settings settings)
    {
        _settings = settings;
    }
    
    public bool IsAppForeground { get; set; }

    public async void NotifyRestIsOver()
    {
        if (!IsAppForeground) return;
        if (!_settings.ShowNotification) return;
        if (!await _notificationRequest) return;
        
        await new NotificationRequest {
            Title = "Rest Over",
            Description = "The rest is now over and the set has begun.",
            Silent = _settings.PlaySounds // So the app sound and the notification sound don't overlap 
        }.Show();
    }
}
