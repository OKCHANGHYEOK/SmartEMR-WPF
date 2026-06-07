using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using System.Windows;
using System.Windows.Input;

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

        this.PreviewKeyDown += OnPreviewKeyDown_FloatPanel;
        this.Loaded += (s, e) =>
        {
            this.Focus();
        };
    }

    public virtual bool ClosingFloatPanel() { return true; }

    [RelayCommand]
    public void Close()
    {
        SmartUI.CloseFloatPanel(this);
    }

    private void OnPreviewKeyDown_FloatPanel(object sender, KeyEventArgs e)
    {
        var element = sender as FloatPanel;
        if (element == null) return;

        SmartUI.CloseFloatPanel(element);
    }
}
 