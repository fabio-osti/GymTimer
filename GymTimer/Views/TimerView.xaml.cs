using GymTimer.ViewModels;

namespace GymTimer.Views;

public partial class TimerView : ContentPage
{
	public TimerView(TimerViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}