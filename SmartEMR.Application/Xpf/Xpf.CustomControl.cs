using System.Reflection;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public class CustomControl : UserControl
{
    public CustomControl()
    {
        var method = this.GetType().GetMethod("InitializeComponent",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        method?.Invoke(this, null);
    }
}
