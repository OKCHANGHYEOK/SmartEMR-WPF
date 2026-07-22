using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class MaskControl : CustomControl
{
    public string? ButtonText { get; set; }
    public ImageSource? ButtonImage { get; set; }
    
    public static readonly DependencyProperty MaskTextProperty =
        DependencyProperty.Register(nameof(MaskText), typeof(string), typeof(MaskControl), new PropertyMetadata(string.Empty, null));

    public string? MaskText 
    {
        get => (string)GetValue(MaskTextProperty);
        set => SetValue(MaskTextProperty, value);
    }

    public static readonly DependencyProperty ButtonCommandProperty =
        DependencyProperty.Register(nameof(ButtonCommand), typeof(IRelayCommand), typeof(MaskControl), new PropertyMetadata(null, null));

    public IRelayCommand ButtonCommand
    {
        get => (IRelayCommand)GetValue(ButtonCommandProperty);
        set => SetValue(ButtonCommandProperty, value);
    }

    public static readonly DependencyProperty ShowButtonProperty =
        DependencyProperty.Register(nameof(ShowButton), typeof(bool), typeof(MaskControl), new PropertyMetadata(true));

    public bool ShowButton
    {
        get => (bool)GetValue(ShowButtonProperty);
        set => SetValue(ShowButtonProperty, value);
    }

    static MaskControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskControl), new FrameworkPropertyMetadata(typeof(MaskControl)));
    }
}
