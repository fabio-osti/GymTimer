using CommunityToolkit.Maui;
using GymTimer.Services;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Chronometer = GymTimer.Services.Chronometer;
using Settings = GymTimer.Services.Settings;

namespace GymTimer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseLocalNotification()
            .ConfigureFonts(
                fonts => {
                    fonts.AddFont("MaterialIcons-Regular", "MaterialRegular");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            );

        builder
            .Services
            .AddSingleton<Settings>()
            .AddSingletonWithShellRoute<SettingsView, SettingsViewModel>("Settings")
            .AddSingleton<Chronometer>()
            .AddSingleton<Ringer>()
            .AddSingleton<Notifier>()
            .AddSingletonWithShellRoute<TimerView, TimerViewModel>("MainPage");

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
