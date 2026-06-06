using System.Windows;

namespace SmartEMR.Application.Xpf;

public class FloatPanel : CustomControl
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
}
 