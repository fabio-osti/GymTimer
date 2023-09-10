namespace GymTimer.Views;

public partial class TimerView
{
    public TimerView(TimerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
