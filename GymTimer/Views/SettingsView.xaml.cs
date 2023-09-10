namespace GymTimer.Views;

public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsViewModel settings)
    {
        InitializeComponent();
        BindingContext = settings;
    }
}
