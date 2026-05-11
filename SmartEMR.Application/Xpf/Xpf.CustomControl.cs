using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public class CustomControl : UserControl
{
    public CustomControl()
    {
        this.GetType().GetMethod("InitializeComponent")?.Invoke(this, null);
    }
}
