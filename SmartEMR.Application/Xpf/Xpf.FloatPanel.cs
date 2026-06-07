using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using System.Windows;

namespace SmartEMR.Application.Xpf;

public partial class FloatPanel : CustomControl
{
    static FloatPanel()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatPanel), new FrameworkPropertyMetadata(typeof(FloatPanel)));
        
    }

    public FloatPanel() : base()
    {
        this.Focusable = true;
        this.IsTabStop = true;

        this.Loaded += (s, e) =>
        {
            this.Focus();
        };
    }

    [RelayCommand]
    public void Close()
    {
        SmartUI.CloseFloatPanel(this);
    }
}
 