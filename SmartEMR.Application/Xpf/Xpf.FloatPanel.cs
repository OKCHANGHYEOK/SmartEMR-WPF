using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SmartEMR.Application.Xpf;

public partial class FloatPanel : CustomControl
{
    public static DependencyProperty IsTopMostPopupProperty =
        DependencyProperty.Register(nameof(IsTopMostPopup), typeof(bool), typeof(FloatPanel), new PropertyMetadata(true));

    public bool IsTopMostPopup
    {
        get => (bool)GetValue(IsTopMostPopupProperty);
        set => SetValue(IsTopMostPopupProperty, value);
    }

    static FloatPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatPanel), new FrameworkPropertyMetadata(typeof(FloatPanel)));
        
    }

    public FloatPanel() : base()
    {
        this.Focusable = true;
        this.IsTabStop = true;

        this.KeyDown += OnKeyDown_FloatPanel;

        this.Loaded += (s, e) =>
        {
            SmartUI.BeginInvoke(() =>
            {
                TextFocusBehavior.SetFocusToFirstTextElement(this);
            }, DispatcherPriority.Background);
        };
    }

    public virtual bool ClosingFloatPanel() { return true; }

    [RelayCommand]
    public void Close()
    {
        SmartUI.CloseFloatPanel(this);
    }

    private void OnKeyDown_FloatPanel(object sender, KeyEventArgs e)
    {
        var element = sender as FloatPanel;
        if (element == null) return;

        if (e.Key == Key.Escape)
        {
            SmartUI.CloseFloatPanel(element);
        }
    }
}
 