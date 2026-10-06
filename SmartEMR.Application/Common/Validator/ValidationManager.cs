using DevExpress.Xpf.Editors;
using SmartEMR.Application.Core;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SmartEMR.Application.Common.Validator;

public class ValidationManager
{
    private static readonly Brush ErrorBorderBrush = Brushes.IndianRed;
    private static readonly Brush NormalBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDE5EF"));

    private readonly Dictionary<SignUpField, Control> _validationElements = new();
    
    public void SetValidationElements(Dictionary<SignUpField, Control> dictValidation)
    {
        _validationElements.Clear();

        foreach (var item in dictValidation)
        {
            var element = item.Value;
            if (element != null)
            {
                element.LostFocus += (s, e) =>
                {
                    ClearValidation(element);
                };
            }

            _validationElements.Add(item.Key, item.Value);
        }
    }

    public void ShowValidation(SignUpField field)
    {
        if (!_validationElements.TryGetValue(field, out var element))
        {
            return;
        }

        element.BorderBrush = ErrorBorderBrush;

        SmartUI.BeginInvoke(() =>
        {
            element.Focus();
            Shake(element);

        }, System.Windows.Threading.DispatcherPriority.Input);
    }

    public void ClearValidation(Control element)
    {
        if (element is BaseEdit editor && editor.EditValue != null)
        {
            element.BorderBrush = NormalBorderBrush;
        }
    }

    private void Shake(Control element)
    {
        if (element.RenderTransform is not TranslateTransform transform)
        {
            transform = new TranslateTransform();
            element.RenderTransform = transform;
        }

        var animation = new DoubleAnimationUsingKeyFrames
        {
            Duration = TimeSpan.FromMilliseconds(250)
        };

        animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.Zero)));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(-5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(40))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(80))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(-4, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(120))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(4, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(160))));
        animation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(220))));

        transform.BeginAnimation(TranslateTransform.XProperty, animation);
    }
}
