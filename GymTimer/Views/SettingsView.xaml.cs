using GymTimer.Models;
using GymTimer.ViewModels;

namespace GymTimer.Views;

public partial class SettingsView : ContentView
{
	public SettingsView(Settings settings)
	{
		InitializeComponent();
		BindingContext = new SettingsViewModel(settings);
	}

	public void Close(object sender, EventArgs e)
	{
		Navigation.PopModalAsync();
	}
}