using Avalonia;
using Avalonia.Labs.Lottie;

namespace CShroudApp.Desktop.Resources.Panels.Auth.Behaviors;

public static class LottieBehavior
{
    public static readonly AttachedProperty<bool> PlayAnimationOnTrueProperty =
        AvaloniaProperty.RegisterAttached<Lottie, bool>("PlayAnimationOnTrue", typeof(LottieBehavior));

    static LottieBehavior()
    {
        PlayAnimationOnTrueProperty.Changed.AddClassHandler<Lottie>((lottie, e) =>
        {
            if (e.NewValue is true)
            {
                lottie.Start();
            }
        });
    }

    public static void SetPlayAnimationOnTrue(AvaloniaObject element, bool value) =>
        element.SetValue(PlayAnimationOnTrueProperty, value);

    public static bool GetPlayAnimationOnTrue(AvaloniaObject element) =>
        element.GetValue(PlayAnimationOnTrueProperty);
}
