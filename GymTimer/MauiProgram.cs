using CommunityToolkit.Maui;
using GymTimer.Helpers;
using GymTimer.Models;
using GymTimer.ViewModels;
using GymTimer.Views;
using Microsoft.Extensions.Logging;

namespace GymTimer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("MaterialIcons-Regular", "MaterialRegular");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services
			.AddSingleton<Settings>()
			.AddSingletonWithShellRoute<SettingsView, SettingsViewModel>("Settings")
			.AddSingleton<Ringer>()
			.AddSingletonWithShellRoute<TimerView, TimerViewModel>("MainPage");

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
