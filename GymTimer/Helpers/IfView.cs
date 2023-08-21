namespace GymTimer.Helpers;

[ContentProperty("Then")]
public class IfView : ContentView
{
	public static readonly BindableProperty ConditionProperty =
		BindableProperty.Create(
			nameof(Condition),
			typeof(bool),
			typeof(IfView),
			false,
			defaultBindingMode: BindingMode.TwoWay,
			propertyChanged: (a, b, c) =>
			{
				(a as IfView).Condition = (bool)c;
			});

	public static readonly BindableProperty ThenProperty =
		BindableProperty.Create(
			nameof(Then),
			typeof(View),
			typeof(IfView),
			null);

	public static readonly BindableProperty ElseProperty =
		BindableProperty.Create(
			nameof(Else),
			typeof(View),
			typeof(IfView),
			null);

	public bool Condition
	{
		get { return (bool)GetValue(ConditionProperty); }
		set
		{
			Application.Current.Dispatcher.Dispatch(UpdateContent);
			SetValue(ConditionProperty, value);
		}
	}

	public View Then
	{
		get { return (View)GetValue(ThenProperty); }
		set { SetValue(ThenProperty, value); }
	}

	public View Else
	{
		get { return (View)GetValue(ElseProperty); }
		set { SetValue(ElseProperty, value); }
	}

	public IfView()
	{
		Application.Current.Dispatcher.Dispatch(UpdateContent);
	}

	void UpdateContent() => Content = Condition ? Then : Else;
}