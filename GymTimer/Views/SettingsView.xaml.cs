using GymTimer.Models;
using GymTimer.ViewModels;

namespace GymTimer.Views;

public partial class SettingsView : ContentPage
{
    public SettingsView(Settings settings)
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel(settings);
    }
}
