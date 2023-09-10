namespace GymTimer.Helpers;

[ContentProperty("Then")]
public sealed class IfView : ContentView
{
    public static readonly BindableProperty ConditionProperty =
        BindableProperty.Create(
            nameof(Condition),
            typeof(bool),
            typeof(IfView),
            false,
            BindingMode.TwoWay,
            propertyChanged: (a, _, c) => {
                (a as IfView).Condition = (bool)c;
            }
        );

    public static readonly BindableProperty ThenProperty =
        BindableProperty.Create(
            nameof(Then),
            typeof(View),
            typeof(IfView)
        );

    public static readonly BindableProperty ElseProperty =
        BindableProperty.Create(
            nameof(Else),
            typeof(View),
            typeof(IfView)
        );

    public IfView()
    {
        Application.Current.Dispatcher.Dispatch(UpdateContent);
    }

    public bool Condition
    {
        get => (bool)GetValue(ConditionProperty);
        set {
            Application.Current.Dispatcher.Dispatch(UpdateContent);
            SetValue(ConditionProperty, value);
        }
    }

    public View Then { get => (View)GetValue(ThenProperty); set => SetValue(ThenProperty, value); }

    public View Else { get => (View)GetValue(ElseProperty); set => SetValue(ElseProperty, value); }

    private void UpdateContent() => Content = Condition ? Then : Else;
}
