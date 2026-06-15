using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class MaskControl : CustomControl
{
    public string? MaskText { get; set; }
    public string? ButtonText { get; set; }
    public ImageSource? ButtonImage { get; set; }

    private bool _showButton;
    public bool ShowButton
    {
        get => _showButton;
        set
        {
            _showButton = value;

            var element = GetTemplateChild("PART_BUTTON");
            if (element is Button btn == false) return;

            btn.Visibility = _showButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public static readonly DependencyProperty MaskVisibilityProperty =
        DependencyProperty.Register(nameof(MaskVisibility), typeof(Visibility), typeof(MaskControl), new PropertyMetadata(Visibility.Visible, null));

    public Visibility MaskVisibility
    {
        get => (Visibility)GetValue(MaskVisibilityProperty);
        set => SetValue(MaskVisibilityProperty, value);
    }

    static MaskControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskControl), new FrameworkPropertyMetadata(typeof(MaskControl)));
    }
}
